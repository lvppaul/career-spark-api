using CareerSpark.DataAccessLayer.Context;
using CareerSpark.DataAccessLayer.Interfaces;
using CareerSpark.DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace CareerSpark.DataAccessLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CareerSparkDbContext _context;
        private IDbContextTransaction _transaction;
        // Repositories
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private IBlogRepository _blogRepository;
        private IQuestionTestRepository _questionTestRepository;
        private ITestAnswerRepository _testAnswerRepository;
        private ITestHistoryRepository _testHistoryRepository;
        private IOrderRepository _orderRepository;
        private ISubscriptionPlanRepository _subscriptionPlanRepository;
        private IUserSubscriptionRepository _userSubscriptionRepository;

        // Constructor to initialize the context
        public UnitOfWork() => _context ??= new CareerSparkDbContext();

        //Lazy loading of repositories
        public IUserRepository UserRepository
        {
            get { return _userRepository ??= new UserRepository(_context); }
        }

        public IRoleRepository RoleRepository
        {
            get { return _roleRepository ??= new RoleRepository(_context); }
        }

        public IQuestionTestRepository QuestionTestRepository
        {
            get { return _questionTestRepository ??= new QuestionTestRepository(_context); }
        }

        public ITestAnswerRepository TestAnswerRepository
        {
            get { return _testAnswerRepository ??= new TestAnswerRepository(_context); }
        }

        public ITestHistoryRepository TestHistoryRepository
        {
            get { return _testHistoryRepository ??= new TestHistoryRepository(_context); }
        }

        public IBlogRepository BlogRepository
        {
            get { return _blogRepository ??= new BlogRepository(_context); }
        public IOrderRepository OrderRepository
        {
            get { return _orderRepository ??= new OrderRepository(_context); }
        }

        public ISubscriptionPlanRepository SubscriptionPlanRepository
        {
            get { return _subscriptionPlanRepository ??= new SubscriptionPlanRepository(_context); }
        }

        public IUserSubscriptionRepository UserSubscriptionRepository
        {
            get { return _userSubscriptionRepository ??= new UserSubscriptionRepository(_context); }
        }



        // Transaction Management
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        // Save Methods (sử dụng các method có sẵn trong GenericRepository)
        //public int Save()
        //{
        //    return _context.SaveChanges();
        //}

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Async Dispose
        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }

            await _context.DisposeAsync();
        }
    }
}
