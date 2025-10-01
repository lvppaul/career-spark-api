using System;
using System.Collections.Generic;
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

    public virtual DbSet<CareerMilestone> CareerMilestones { get; set; }

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

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Blogs__3214EC071B8C9BFE");

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<CareerField>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CareerFi__3214EC071B6D6078");

            entity.ToTable("CareerField");

            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<CareerMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CareerMa__3214EC072E68D511");

            entity.ToTable("CareerMapping");

            entity.Property(e => e.RiasecType).HasMaxLength(20);

            entity.HasOne(d => d.CareerField).WithMany(p => p.CareerMappings)
                .HasForeignKey(d => d.CareerFieldId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CareerMap__Caree__6754599E");
        });

        modelBuilder.Entity<CareerMilestone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CareerMi__3214EC076007CB29");

            entity.ToTable("CareerMilestone");

            entity.Property(e => e.SuggestedCourseUrl).HasMaxLength(500);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.CareerPath).WithMany(p => p.CareerMilestones)
                .HasForeignKey(d => d.CareerPathId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CareerMil__Caree__5DCAEF64");
        });

        modelBuilder.Entity<CareerPath>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CareerPa__3214EC07045D85B9");

            entity.ToTable("CareerPath");

            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.CareerField).WithMany(p => p.CareerPaths)
                .HasForeignKey(d => d.CareerFieldId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CareerPat__Caree__5AEE82B9");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comments__3214EC07EECE1EAD");

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Blog).WithMany(p => p.Comments)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comments__BlogId__440B1D61");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comments__UserId__4316F928");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC0736814C31");

            entity.ToTable("Orders");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasConversion( //Value Conversion: Enum <-> String
                    v => v.ToString(), // khi lưu vào database thì chuyển enum thành string
                    v => (OrderStatus)System.Enum.Parse(typeof(OrderStatus), v, true) // khi lấy từ database về thì chuyển string thành enum
                    )
                .HasDefaultValue(OrderStatus.Pending);

            entity.Property(e => e.VnPayTransactionId).HasMaxLength(255);
            entity.Property(e => e.VnPayOrderInfo).HasMaxLength(500);
            entity.Property(e => e.VnPayResponseCode).HasMaxLength(10);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaidAt).HasColumnType("datetime");
            entity.Property(e => e.ExpiredAt).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__UserId");

            entity.HasOne(d => d.SubscriptionPlan).WithMany()
                .HasForeignKey(d => d.SubscriptionPlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__SubscriptionPlanId");
        });

        modelBuilder.Entity<QuestionTest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC07050C3ED2");

            entity.ToTable("QuestionTest");

            entity.Property(e => e.QuestionType).HasMaxLength(50);
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Result__3214EC077DFCACFA");

            entity.ToTable("Result");

            entity.HasOne(d => d.TestSession).WithMany(p => p.Results)
                .HasForeignKey(d => d.TestSessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Result__TestSess__5165187F");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC075D73DB95");

            entity.ToTable("Role");

            entity.Property(e => e.RoleName).HasMaxLength(100);
        });

        modelBuilder.Entity<SubscriptionPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subscrip__3214EC070AFA0287");

            entity.ToTable("SubscriptionPlan");

            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Level).IsRequired();
        });

        modelBuilder.Entity<TestAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestAnsw__3214EC07C7CEEEE5");

            entity.ToTable("TestAnswer");

            entity.Property(e => e.IsSelected).HasDefaultValue(false);

            entity.HasOne(d => d.Question).WithMany(p => p.TestAnswers)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TestAnswe__Quest__4D94879B");

            entity.HasOne(d => d.TestSession).WithMany(p => p.TestAnswers)
                .HasForeignKey(d => d.TestSessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TestAnswe__TestS__4E88ABD4");
        });

        modelBuilder.Entity<TestHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestHist__3214EC0724C44584");

            entity.ToTable("TestHistory");

            entity.HasOne(d => d.TestAnswer).WithMany(p => p.TestHistories)
                .HasForeignKey(d => d.TestAnswerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TestHisto__TestA__5629CD9C");

            entity.HasOne(d => d.TestSession).WithMany(p => p.TestHistories)
                .HasForeignKey(d => d.TestSessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TestHisto__TestS__5535A963");

            entity.HasOne(d => d.User).WithMany(p => p.TestHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TestHisto__UserI__5441852A");
        });

        modelBuilder.Entity<TestSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestSess__3214EC078E28310F");

            entity.ToTable("TestSession");

            entity.Property(e => e.EndAt).HasColumnType("datetime");
            entity.Property(e => e.StartAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.TestSessions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TestSessi__UserI__49C3F6B7");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07F9B336B1");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534D307CD4A").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.ExpiredRefreshTokenAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Password).HasMaxLength(255).IsRequired(false);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.RefreshToken).HasMaxLength(500);
            entity.Property(e => e.avatarURL).HasMaxLength(255);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__RoleId__3C69FB99");
        });

        modelBuilder.Entity<UserSubscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserSubs__3214EC07E55F3ECF");

            entity.ToTable("UserSubscription");

            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Plan).WithMany(p => p.UserSubscriptions)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserSubsc__PlanI__6477ECF3");

            entity.HasOne(d => d.User).WithMany(p => p.UserSubscriptions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserSubsc__UserI__6383C8BA");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
