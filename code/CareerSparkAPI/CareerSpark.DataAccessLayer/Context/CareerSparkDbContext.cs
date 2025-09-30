using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CareerSpark.DataAccessLayer.Context;

public partial class CareerSparkDbContext : DbContext
{
    public CareerSparkDbContext()
    {
    }

    public CareerSparkDbContext(DbContextOptions<CareerSparkDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<CareerField> CareerFields { get; set; }

    public virtual DbSet<CareerMileStone> CareerMileStones { get; set; }

    public virtual DbSet<CareerPath> CareerPaths { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<QuestionTest> QuestionTests { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }

    public virtual DbSet<TestAnswer> TestAnswers { get; set; }

    public virtual DbSet<TestHistory> TestHistories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSubscription> UserSubscriptions { get; set; }

    private string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
        return configuration["ConnectionStrings:DefaultConnection"];
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(GetConnectionString());
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Blogs__3214EC075D1ED2B1");

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<CareerField>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CareerFi__3214EC072100DF97");

            entity.ToTable("CareerField");

            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<CareerMileStone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CareerMi__3214EC079DCE204B");

            entity.ToTable("CareerMileStone");

            entity.Property(e => e.SuggestedCourseUrl).HasMaxLength(500);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.CareerPath).WithMany(p => p.CareerMileStones)
                .HasForeignKey(d => d.CareerPathId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CareerMil__Caree__571DF1D5");
        });

        modelBuilder.Entity<CareerPath>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CareerPa__3214EC076B8BF44D");

            entity.ToTable("CareerPath");

            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.CareerField).WithMany(p => p.CareerPaths)
                .HasForeignKey(d => d.CareerFieldId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CareerPat__Caree__5441852A");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comments__3214EC07648A7206");

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Blog).WithMany(p => p.Comments)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comments__BlogId__4222D4EF");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comments__UserId__412EB0B6");
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
                    v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v) // khi lấy từ database về thì chuyển string thành enum
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
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC0736814C31");

            entity.ToTable("QuestionTest");

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.QuestionType).HasMaxLength(50);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Result__3214EC079E3E8609");

            entity.ToTable("Result");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC07078FA551");

            entity.ToTable("Role");

            entity.Property(e => e.RoleName).HasMaxLength(100);
        });

        modelBuilder.Entity<SubscriptionPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subscrip__3214EC079DED5C3C");

            entity.ToTable("SubscriptionPlan");

            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Level).IsRequired();
        });

        modelBuilder.Entity<TestAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestAnsw__3214EC07133EA14C");

            entity.ToTable("TestAnswer");

            entity.Property(e => e.IsSelected).HasDefaultValue(false);

            entity.HasOne(d => d.Question).WithMany(p => p.TestAnswers)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TestAnswe__Quest__48CFD27E");
        });

        modelBuilder.Entity<TestHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestHist__3214EC074884C0E8");

            entity.ToTable("TestHistory");

            entity.HasOne(d => d.Result).WithMany(p => p.TestHistories)
                .HasForeignKey(d => d.ResultId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TestHisto__Resul__4E88ABD4");

            entity.HasOne(d => d.TestAnswer).WithMany(p => p.TestHistories)
                .HasForeignKey(d => d.TestAnswerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TestHisto__TestA__4F7CD00D");

            entity.HasOne(d => d.User).WithMany(p => p.TestHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TestHisto__UserI__4D94879B");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07ED5327BC");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534B9E8E376").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.avatarURL).HasMaxLength(255);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__RoleId__3A81B327");
        });

        modelBuilder.Entity<UserSubscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserSubs__3214EC07B240E5C7");

            entity.ToTable("UserSubscription");

            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Plan).WithMany(p => p.UserSubscriptions)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserSubsc__PlanI__5DCAEF64");

            entity.HasOne(d => d.User).WithMany(p => p.UserSubscriptions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserSubsc__UserI__5CD6CB2B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
