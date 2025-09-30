using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.Interfaces;
using CareerSpark.BusinessLayer.Mappings;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Enums;
using CareerSpark.DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CareerSpark.BusinessLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVnPayService _vnPayService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IUnitOfWork unitOfWork, IVnPayService vnPayService, ILogger<OrderService> logger)
        {
            _unitOfWork = unitOfWork;
            _vnPayService = vnPayService;
            _logger = logger;
        }

        public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request, HttpContext httpContext)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Validate user exists
                var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return new CreateOrderResponse
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                if (user.RoleId != (int)UserRole.User) // Only allow orders for regular users
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return new CreateOrderResponse
                    {
                        Success = false,
                        Message = "Only regular users can create orders"
                    };
                }

                // Validate subscription plan exists
                var subscriptionPlanRequest = await _unitOfWork.SubscriptionPlanRepository.GetByIdAsync(request.SubscriptionPlanId);
                if (subscriptionPlanRequest == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return new CreateOrderResponse
                    {
                        Success = false,
                        Message = "Subscription plan not found"
                    };
                }

                var existingSubscription = await _unitOfWork.UserSubscriptionRepository
                    .GetActiveSubscriptionByUserIdAsync(request.UserId);
                if (existingSubscription != null && existingSubscription.Plan.Level > subscriptionPlanRequest.Level)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return new CreateOrderResponse
                    {
                        Success = false,
                        Message = "Cannot purchase a lower-level subscription plan while having an active higher-level subscription"
                    };
                }

                // Check if user already has a pending order for this plan
                var existingPendingOrder = await _unitOfWork.OrderRepository.GetPendingOrderByUserAndPlanAsync(request.UserId, request.SubscriptionPlanId);
                if (existingPendingOrder != null)
                {
                    // Return existing order with payment URL
                    var existingPaymentInfo = new PaymentInformationModel
                    {
                        Amount = (double)existingPendingOrder.Amount,
                        OrderDescription = $"Thanh toan goi {subscriptionPlanRequest.Name}",
                        OrderId = existingPendingOrder.Id,
                        Name = user.Name
                    };

                    var existingPaymentUrl = await _vnPayService.CreatePaymentUrl(existingPaymentInfo, httpContext);

                    await _unitOfWork.RollbackTransactionAsync();
                    return new CreateOrderResponse
                    {
                        Success = true,
                        Message = "Existing pending order found",
                        Order = OrderMapper.ToResponse(existingPendingOrder),
                        PaymentUrl = existingPaymentUrl
                    };
                }

                // Create new order
                var order = new Order
                {
                    UserId = request.UserId,
                    SubscriptionPlanId = request.SubscriptionPlanId,
                    Amount = subscriptionPlanRequest.Price,
                    Status = OrderStatus.Pending,
                    VnPayOrderInfo = $"Thanh toan goi {subscriptionPlanRequest.Name}",
                    CreatedAt = DateTime.UtcNow,
                    ExpiredAt = DateTime.UtcNow.AddMinutes(15) // Order expires in 15 minutes
                };

                _unitOfWork.OrderRepository.PrepareCreate(order);
                await _unitOfWork.SaveAsync();

                // Create payment URL
                var paymentInfo = new PaymentInformationModel
                {
                    Amount = (double)order.Amount,
                    OrderDescription = order.VnPayOrderInfo,
                    OrderId = order.Id,
                    Name = user.Name
                };

                var paymentUrl = await _vnPayService.CreatePaymentUrl(paymentInfo, httpContext);

                await _unitOfWork.CommitTransactionAsync();

                // Get order with details for response
                var orderWithDetails = await _unitOfWork.OrderRepository.GetOrderByIdWithDetailsAsync(order.Id);

                return new CreateOrderResponse
                {
                    Success = true,
                    Message = "Order created successfully",
                    Order = OrderMapper.ToResponse(orderWithDetails!),
                    PaymentUrl = paymentUrl
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error creating order for user {UserId} and plan {PlanId}", request.UserId, request.SubscriptionPlanId);

                return new CreateOrderResponse
                {
                    Success = false,
                    Message = $"Error creating order: {ex.Message}"
                };
            }
        }

        public async Task<OrderResponse?> GetOrderByIdAsync(int orderId)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetOrderByIdWithDetailsAsync(orderId);
                return order != null ? OrderMapper.ToResponse(order) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order by ID {OrderId}", orderId);
                return null;
            }
        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersByUserIdAsync(int userId)
        {
            try
            {
                var orders = await _unitOfWork.OrderRepository.GetOrdersByUserIdAsync(userId);
                return OrderMapper.ToResponseList(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders for user {UserId}", userId);
                return Enumerable.Empty<OrderResponse>();
            }
        }

        public async Task<bool> ProcessPaymentCallbackAsync(PaymentResponseModel paymentResponse)
        {
            try
            {
                if (!paymentResponse.Success || string.IsNullOrEmpty(paymentResponse.OrderId))
                {
                    _logger.LogWarning("Invalid payment response received");
                    return false;
                }

                if (!int.TryParse(paymentResponse.OrderId, out int orderId))
                {
                    _logger.LogWarning("Invalid order ID in payment response: {OrderId}", paymentResponse.OrderId);
                    return false;
                }

                await _unitOfWork.BeginTransactionAsync();

                var order = await _unitOfWork.OrderRepository.GetOrderByIdWithDetailsAsync(orderId);
                if (order == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    _logger.LogWarning("Order not found for ID: {OrderId}", orderId);
                    return false;
                }

                if (order.Status != OrderStatus.Pending)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    _logger.LogWarning("Order {OrderId} is not in pending status. Current status: {Status}", orderId, order.Status);
                    return false;
                }

                // Update order status based on VNPay response
                var newStatus = paymentResponse.VnPayResponseCode == "00" ? "Paid" : "Failed";

                await _unitOfWork.OrderRepository.UpdateOrderStatusAsync(
                    orderId,
                    newStatus,
                    paymentResponse.TransactionId,
                    paymentResponse.VnPayResponseCode
                );

                // If payment successful, handle user subscription
                if (newStatus == "Paid")
                {
                    var existingSubscription = await _unitOfWork.UserSubscriptionRepository
                        .GetActiveSubscriptionByUserIdAsync(order.UserId);

                    if (existingSubscription != null)
                    {
                        // User has an existing active subscription
                        if (existingSubscription.PlanId == order.SubscriptionPlanId)
                        {
                            // Gia hạn (cùng gói)
                            existingSubscription.EndDate = existingSubscription.EndDate
                                .AddDays(order.SubscriptionPlan.DurationDays);

                            _unitOfWork.UserSubscriptionRepository.PrepareUpdate(existingSubscription);

                            _logger.LogInformation("Subscription renewed for user {UserId} with plan {PlanId}", order.UserId, order.SubscriptionPlanId);
                        }
                        else
                        {
                            // Different plan - need to handle upgrade
                            var oldPlan = await _unitOfWork.SubscriptionPlanRepository.GetByIdAsync(existingSubscription.PlanId);
                            var newPlan = order.SubscriptionPlan;
                            if (newPlan.Level >= oldPlan.Level)
                            {
                                // Nâng cấp (gói mới cao hơn)

                                // Deactivate old subscription
                                existingSubscription.IsActive = false;
                                _unitOfWork.UserSubscriptionRepository.PrepareUpdate(existingSubscription);

                                // Create new subscription for the new plan
                                var newUserSubscription = new UserSubscription
                                {
                                    UserId = order.UserId,
                                    PlanId = newPlan.Id,
                                    StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                                    EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(newPlan.DurationDays)),
                                    IsActive = true
                                };

                                _unitOfWork.UserSubscriptionRepository.PrepareCreate(newUserSubscription);

                                _logger.LogInformation("Subscription changed for user {UserId} from plan {OldPlanId} (Level {OldLevel}) to plan {NewPlanId} (Level {NewLevel})",
                                    order.UserId, oldPlan.Id, oldPlan.Level, newPlan.Id, newPlan.Level);
                            }
                        }
                    }
                    else
                    {
                        // No existing subscription - create new subscription
                        var userSubscription = new UserSubscription
                        {
                            UserId = order.UserId,
                            PlanId = order.SubscriptionPlanId,
                            StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                            EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(order.SubscriptionPlan.DurationDays)),
                            IsActive = true
                        };

                        _unitOfWork.UserSubscriptionRepository.PrepareCreate(userSubscription);
                    }


                    _logger.LogInformation("New subscription created for user {UserId} with plan {PlanId}", order.UserId, order.SubscriptionPlanId);

                }

                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Payment processed successfully for order {OrderId} with status {Status}", orderId, newStatus);
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error processing payment callback for order {OrderId}", paymentResponse.OrderId);
                return false;
            }
        }

        public async Task<bool> CancelExpiredOrdersAsync()
        {
            try
            {
                var expiredOrders = await _unitOfWork.OrderRepository.GetAllAsync();
                var ordersToCancel = expiredOrders.Where(o =>
                    o.Status == OrderStatus.Pending &&
                    o.ExpiredAt.HasValue &&
                    o.ExpiredAt.Value < DateTime.UtcNow).ToList();

                if (!ordersToCancel.Any())
                    return true;

                await _unitOfWork.BeginTransactionAsync();

                foreach (var order in ordersToCancel)
                {
                    await _unitOfWork.OrderRepository.UpdateOrderStatusAsync(order.Id, "Cancelled");
                }

                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Cancelled {Count} expired orders", ordersToCancel.Count);
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error cancelling expired orders");
                return false;
            }
        }
    }
}