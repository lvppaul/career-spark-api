using CareerSpark.DataAccessLayer.Interfaces;

namespace CareerSpark.DataAccessLayer.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        // Repository access
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        IQuestionTestRepository QuestionTestRepository { get; }
        ITestAnswerRepository TestAnswerRepository { get; }
        ITestHistoryRepository TestHistoryRepository { get; }
        IOrderRepository OrderRepository { get; }
        ISubscriptionPlanRepository SubscriptionPlanRepository { get; }
        IUserSubscriptionRepository UserSubscriptionRepository { get; }
        
        // Transaction management
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

        // Save changes
        Task<int> SaveAsync();
        //Không cần viết lại DisposeAsync() vì nó đã được kế thừa từ IAsyncDisposable.
    }
}
