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
        IResultRepository ResultRepository { get; }
        ITestSessionRepository TestSessionRepository { get; }

        // Newly added repositories
        IBlogRepository BlogRepository { get; }
        ICareerFieldRepository CareerFieldRepository { get; }
        ICareerPathRepository CareerPathRepository { get; }
        ICareerRoadmapRepository CareerRoadmapRepository { get; }
        ICommentRepository CommentRepository { get; }
        ISubscriptionPlanRepository SubscriptionPlanRepository { get; }
        IUserSubscriptionRepository UserSubscriptionRepository { get; }
        ICareerMappingRepository CareerMappingRepository { get; }

    
        IOrderRepository OrderRepository { get; }
      
        
        // Transaction management
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

        // Save changes
        Task<int> SaveAsync();
        //Không cần viết lại DisposeAsync() vì nó đã được kế thừa từ IAsyncDisposable.
    }
}
