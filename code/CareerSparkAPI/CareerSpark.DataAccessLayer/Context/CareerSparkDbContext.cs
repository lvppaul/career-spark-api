using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Enums;
using Microsoft.EntityFrameworkCore;

namespace CareerSpark.DataAccessLayer.Context;

public partial class CareerSparkDbContext : DbContext
{


    public CareerSparkDbContext(DbContextOptions<CareerSparkDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<CareerField> CareerFields { get; set; }

    public virtual DbSet<CareerMapping> CareerMappings { get; set; }

    public virtual DbSet<CareerRoadmap> CareerRoadmaps { get; set; }

    public virtual DbSet<CareerPath> CareerPaths { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<QuestionTest> QuestionTests { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }

    public virtual DbSet<TestAnswer> TestAnswers { get; set; }

    public virtual DbSet<TestHistory> TestHistories { get; set; }

    public virtual DbSet<TestSession> TestSessions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSubscription> UserSubscriptions { get; set; }

    public virtual DbSet<News> News { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Blogs");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.CreateAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsPublished).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp with time zone");
            entity.Property(e => e.Tag).HasMaxLength(100);
        });

        modelBuilder.Entity<CareerField>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CareerField");
            entity.ToTable("CareerField");
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_News");
            entity.ToTable("News");
            entity.Property(e => e.Title).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Tag).HasMaxLength(100);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.avatarPublicId).HasMaxLength(200);
        });

        modelBuilder.Entity<CareerMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CareerMapping");
            entity.ToTable("CareerMapping");
            entity.Property(e => e.RiasecType).HasMaxLength(20);
            entity.HasOne(d => d.CareerField)
                .WithMany(p => p.CareerMappings)
                .HasForeignKey(d => d.CareerFieldId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CareerMapping_CareerField");
        });

        modelBuilder.Entity<CareerRoadmap>(entity =>
        {
            entity.ToTable("CareerRoadmap");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.SkillFocus).HasMaxLength(200);
            entity.Property(e => e.DifficultyLevel).HasMaxLength(50);
            entity.Property(e => e.SuggestedCourseUrl).HasMaxLength(255);
            entity.HasOne(e => e.CareerPath)
                  .WithMany(p => p.CareerRoadmaps)
                  .HasForeignKey(e => e.CareerPathId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CareerPath>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CareerPath");
            entity.ToTable("CareerPath");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.HasOne(d => d.CareerField)
                .WithMany(p => p.CareerPaths)
                .HasForeignKey(d => d.CareerFieldId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CareerPath_CareerField");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Comments");
            entity.Property(e => e.CreateAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("timestamp with time zone");

            entity.HasOne(d => d.Blog)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comments_Blog");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comments_User");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Orders");
            entity.ToTable("Orders");

            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasConversion(
                    v => v.ToString(),
                    v => (OrderStatus)System.Enum.Parse(typeof(OrderStatus), v, true))
                .HasDefaultValue(OrderStatus.Pending);

            entity.Property(e => e.VnPayTransactionId).HasMaxLength(255);
            entity.Property(e => e.VnPayOrderInfo).HasMaxLength(500);
            entity.Property(e => e.VnPayResponseCode).HasMaxLength(10);

            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.PaidAt).HasColumnType("timestamp with time zone");
            entity.Property(e => e.ExpiredAt).HasColumnType("timestamp with time zone");

            entity.HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_User");

            entity.HasOne(d => d.SubscriptionPlan)
                .WithMany()
                .HasForeignKey(d => d.SubscriptionPlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_SubscriptionPlan");
        });

        modelBuilder.Entity<QuestionTest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_QuestionTest");
            entity.ToTable("QuestionTest");
            entity.Property(e => e.QuestionType).HasMaxLength(50);
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Result");
            entity.ToTable("Result");
            entity.HasOne(d => d.TestSession)
                .WithMany(p => p.Results)
                .HasForeignKey(d => d.TestSessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Result_TestSession");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Role");
            entity.ToTable("Role");
            entity.Property(e => e.RoleName).HasMaxLength(100);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
        });

        modelBuilder.Entity<SubscriptionPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SubscriptionPlan");
            entity.ToTable("SubscriptionPlan");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Level).IsRequired();
        });

        modelBuilder.Entity<TestAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TestAnswer");
            entity.ToTable("TestAnswer");
            entity.Property(e => e.IsSelected).HasDefaultValue(false);
            entity.HasOne(d => d.Question)
                .WithMany(p => p.TestAnswers)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestAnswer_Question");
            entity.HasOne(d => d.TestSession)
                .WithMany(p => p.TestAnswers)
                .HasForeignKey(d => d.TestSessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestAnswer_TestSession");
        });

        modelBuilder.Entity<TestHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TestHistory");
            entity.ToTable("TestHistory");
            entity.HasOne(d => d.TestAnswer)
                .WithMany(p => p.TestHistories)
                .HasForeignKey(d => d.TestAnswerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestHistory_TestAnswer");
            entity.HasOne(d => d.TestSession)
                .WithMany(p => p.TestHistories)
                .HasForeignKey(d => d.TestSessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestHistory_TestSession");
            entity.HasOne(d => d.User)
                .WithMany(p => p.TestHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestHistory_User");
        });

        modelBuilder.Entity<TestSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TestSession");
            entity.ToTable("TestSession");
            entity.Property(e => e.StartAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasOne(d => d.User)
                .WithMany(p => p.TestSessions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestSession_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_User");
            entity.ToTable("User");
            entity.HasIndex(e => e.Email, "UQ_User_Email").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.ExpiredRefreshTokenAt)
                .HasColumnType("timestamp with time zone");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Password).HasMaxLength(255).IsRequired(false);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.RefreshToken).HasMaxLength(500);
            entity.Property(e => e.avatarURL).HasMaxLength(255);
            entity.Property(e => e.avatarPublicId).HasMaxLength(200);

            entity.HasOne(d => d.Role)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        modelBuilder.Entity<UserSubscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserSubscription");
            entity.ToTable("UserSubscription");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasOne(d => d.Plan)
                .WithMany(p => p.UserSubscriptions)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserSubscription_Plan");
            entity.HasOne(d => d.User)
                .WithMany(p => p.UserSubscriptions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserSubscription_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
