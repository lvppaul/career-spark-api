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
        private IQuestionTestRepository _questionTestRepository;
        private ITestAnswerRepository _testAnswerRepository;
        private ITestHistoryRepository _testHistoryRepository;
        private IResultRepository _resultRepository;
        private ITestSessionRepository _testSessionRepository;
        private IBlogRepository _blogRepository;
        private ICareerFieldRepository _careerFieldRepository;
        private ICareerPathRepository _careerPathRepository;
        private ICareerMileStoneRepository _careerMileStoneRepository;
        private ICommentRepository _commentRepository;
        private ISubscriptionPlanRepository _subscriptionPlanRepository;
        private IUserSubscriptionRepository _userSubscriptionRepository;
        private ICareerMappingRepository _careerMappingRepository;

        // Constructor to initialize the context
        public UnitOfWork(CareerSparkDbContext context)
        {
            _context = context;
        }

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

        public IResultRepository ResultRepository
        {
            get { return _resultRepository ??= new ResultRepository(_context); }
        }

        public ITestSessionRepository TestSessionRepository
        {
            get { return _testSessionRepository ??= new TestSessionRepository(_context); }
        }

        public IBlogRepository BlogRepository
        {
            get { return _blogRepository ??= new BlogRepository(_context); }
        }

        public ICareerFieldRepository CareerFieldRepository
        {
            get { return _careerFieldRepository ??= new CareerFieldRepository(_context); }
        }

        public ICareerPathRepository CareerPathRepository
        {
            get { return _careerPathRepository ??= new CareerPathRepository(_context); }
        }

        public ICareerMileStoneRepository CareerMilestoneRepository
        {
            get { return _careerMileStoneRepository ??= new CareerMileStoneRepository(_context); }
        }

        public ICommentRepository CommentRepository
        {
            get { return _commentRepository ??= new CommentRepository(_context); }
        }

        public ISubscriptionPlanRepository SubscriptionPlanRepository
        {
            get { return _subscriptionPlanRepository ??= new SubscriptionPlanRepository(_context); }
        }

        public IUserSubscriptionRepository UserSubscriptionRepository
        {
            get { return _userSubscriptionRepository ??= new UserSubscriptionRepository(_context); }
        }

        public ICareerMappingRepository CareerMappingRepository
        {
            get { return _careerMappingRepository ??= new CareerMappingRepository(_context); }
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
