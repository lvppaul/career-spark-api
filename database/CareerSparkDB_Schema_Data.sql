-- Nếu DB đã tồn tại thì xóa đi
DROP DATABASE IF EXISTS CareerSparkDB;
CREATE DATABASE CareerSparkDB;
GO


USE CareerSparkDB;
GO

/* ========================================
   CREATE TABLES
   ======================================== */

-- Role
CREATE TABLE Role (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(100) NOT NULL
);

-- User
CREATE TABLE [User] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Phone NVARCHAR(20),
    Email NVARCHAR(200) UNIQUE,
    Password NVARCHAR(255) NULL, 
    RefreshToken NVARCHAR(500),
    ExpiredRefreshTokenAt DATETIME, 
	avatarURL NVARCHAR(255),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    RoleId INT NOT NULL,
    FOREIGN KEY (RoleId) REFERENCES Role(Id)
);

-- Blogs
CREATE TABLE Blogs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    CreateAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME
);

-- Comments
CREATE TABLE Comments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Content NVARCHAR(MAX) NOT NULL,
    UserId INT NOT NULL,
    BlogId INT NOT NULL,
    CreateAt DATETIME DEFAULT GETDATE(),
    UpdateAt DATETIME,
    FOREIGN KEY (UserId) REFERENCES [User](Id),
    FOREIGN KEY (BlogId) REFERENCES Blogs(Id)
);

-- QuestionTest
CREATE TABLE QuestionTest (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Content NVARCHAR(MAX) NOT NULL,
    QuestionType NVARCHAR(50) NOT NULL
);

-- TestSession
CREATE TABLE TestSession (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    StartAt DATETIME DEFAULT GETDATE(),
    EndAt DATETIME,
    FOREIGN KEY (UserId) REFERENCES [User](Id)
);

-- TestAnswer
CREATE TABLE TestAnswer (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IsSelected BIT DEFAULT 0,
    QuestionId INT NOT NULL,
    TestSessionId INT NOT NULL,
    FOREIGN KEY (QuestionId) REFERENCES QuestionTest(Id),
    FOREIGN KEY (TestSessionId) REFERENCES TestSession(Id)
);

-- Result
CREATE TABLE Result (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Content NVARCHAR(MAX),
    R INT,
    I INT,
    A INT,
    S INT,
    E INT,
    C INT,
    TestSessionId INT NOT NULL,
    FOREIGN KEY (TestSessionId) REFERENCES TestSession(Id)
);

-- TestHistory
CREATE TABLE TestHistory (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    TestSessionId INT NOT NULL,
    TestAnswerId INT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES [User](Id),
    FOREIGN KEY (TestSessionId) REFERENCES TestSession(Id),
    FOREIGN KEY (TestAnswerId) REFERENCES TestAnswer(Id)
);

-- CareerField
CREATE TABLE CareerField (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX)
);

-- CareerPath
CREATE TABLE CareerPath (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    CareerFieldId INT NOT NULL,
    FOREIGN KEY (CareerFieldId) REFERENCES CareerField(Id)
);

-- CareerMilestone
CREATE TABLE CareerMilestone (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CareerPathId INT NOT NULL,
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    StepOrder INT NOT NULL,
    SuggestedCourseUrl NVARCHAR(500),
    FOREIGN KEY (CareerPathId) REFERENCES CareerPath(Id)
);

-- Tạo bảng SubscriptionPlan
CREATE TABLE SubscriptionPlan (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    DurationDays INT NOT NULL,
    Description NVARCHAR(MAX),
    Level INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1
);
-- UserSubscription
CREATE TABLE UserSubscription (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    PlanId INT NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    IsActive BIT DEFAULT 1,
    FOREIGN KEY (UserId) REFERENCES [User](Id),
    FOREIGN KEY (PlanId) REFERENCES SubscriptionPlan(Id)
);

-- CareerMapping
CREATE TABLE CareerMapping (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RiasecType NVARCHAR(20) NOT NULL,
    CareerFieldId INT NOT NULL,
    FOREIGN KEY (CareerFieldId) REFERENCES CareerField(Id)
);

CREATE TABLE Orders
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserId INT NOT NULL,
    SubscriptionPlanId INT NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pending', -- Enum được lưu dạng string

    VnPayTransactionId NVARCHAR(255) NULL,
    VnPayOrderInfo NVARCHAR(500) NULL,
    VnPayResponseCode NVARCHAR(10) NULL,

    CreatedAt DATETIME NOT NULL DEFAULT (GETUTCDATE()),
    PaidAt DATETIME NULL,
    ExpiredAt DATETIME NULL,

    -- Foreign Keys
    FOREIGN KEY (UserId) REFERENCES [User](Id),
    FOREIGN KEY (SubscriptionPlanId) REFERENCES SubscriptionPlan(Id)
);

PRINT 'Database schema created successfully.';

/* ========================================
   SEED DATA
   ======================================== */

-- Roles
INSERT INTO Role (RoleName) VALUES 
(N'Admin'),(N'User'),(N'Moderator');

-- Users
INSERT INTO [User] (Name, Phone, Email, Password, RoleId) VALUES
(N'Nguyen Van A','0912345678','a@example.com','123456',1),
(N'Tran Thi B','0987654321','b@example.com','123456',2),
(N'Le Van C','0905123456','c@example.com','123456',3);


-- Subscription Plans
INSERT INTO SubscriptionPlan (Name, Price, DurationDays, Description, Level,IsActive) VALUES
(N'Free Plan', 0, 30, N'Gói miễn phí cơ bản',1,1),
(N'Standard Plan', 99.99, 90, N'Gói tiêu chuẩn 3 tháng',2,1),
(N'Premium Plan', 299.99, 365, N'Gói cao cấp 1 năm',3,1);

-- Career Fields
INSERT INTO CareerField (Name, Description) VALUES
(N'Công nghệ thông tin',N'Lĩnh vực phần mềm, hệ thống, trí tuệ nhân tạo'),
(N'Kinh tế',N'Tài chính, kế toán, quản trị kinh doanh'),
(N'Y tế & Sức khỏe',N'Ngành y, điều dưỡng, chăm sóc sức khỏe'),
(N'Giáo dục',N'Dạy học, đào tạo, nghiên cứu'),
(N'Nghệ thuật & Sáng tạo',N'Mỹ thuật, âm nhạc, thiết kế'),
(N'Kỹ thuật & Cơ khí',N'Ngành điện, cơ khí, xây dựng');

-- Career Paths
INSERT INTO CareerPath (Title, Description, CareerFieldId) VALUES
(N'Lập trình viên Backend',N'Tập trung phát triển server-side',1),
(N'Chuyên gia AI/ML',N'Làm việc với dữ liệu và trí tuệ nhân tạo',1),
(N'Phân tích tài chính',N'Tập trung phân tích và dự báo tài chính',2),
(N'Bác sĩ đa khoa',N'Chẩn đoán và điều trị bệnh thông thường',3),
(N'Giáo viên Toán',N'Dạy học môn Toán',4),
(N'Nhạc sĩ/Composer',N'Sáng tác âm nhạc và biểu diễn',5),
(N'Kỹ sư Cơ khí',N'Thiết kế, chế tạo và bảo trì máy móc',6);

-- Career Milestones
INSERT INTO CareerMilestone (CareerPathId, Title, Description, StepOrder, SuggestedCourseUrl) VALUES
(1,N'Học C# cơ bản',N'Nắm vững C# để lập trình backend',1,N'https://learn.microsoft.com/dotnet/csharp'),
(1,N'Tìm hiểu ASP.NET Core',N'Xây dựng API với ASP.NET Core',2,N'https://dotnet.microsoft.com/learn/aspnet'),
(2,N'Học Python cho ML',N'Nắm vững Python và thư viện ML',1,N'https://www.tensorflow.org'),
(3,N'Học kế toán cơ bản',N'Nắm vững các nguyên tắc kế toán',1,N'https://example.com/accounting-course'),
(4,N'Tốt nghiệp Y đa khoa',N'Hoàn thành chương trình đại học ngành Y',1,N'https://example.com/medical'),
(5,N'Chứng chỉ Sư phạm',N'Hoàn thành chứng chỉ nghiệp vụ sư phạm',1,N'https://example.com/teacher'),
(6,N'Học sáng tác nhạc',N'Tìm hiểu lý thuyết âm nhạc và sáng tác',1,N'https://example.com/music'),
(7,N'Kỹ thuật cơ khí cơ bản',N'Học nền tảng cơ khí và chế tạo',1,N'https://example.com/mechanical');

-- Career Mapping
INSERT INTO CareerMapping (RiasecType, CareerFieldId) VALUES
('R',6),('I',1),('A',5),('S',4),('E',2),('C',2);

-- Blogs & Comments
INSERT INTO Blogs (Title, Content) VALUES
(N'Chào mừng đến Career Spark',N'Bài viết giới thiệu dự án Career Spark.'),
(N'Mẹo chọn nghề nghiệp',N'5 mẹo giúp bạn định hướng nghề nghiệp.');

INSERT INTO Comments (Content, UserId, BlogId) VALUES
(N'Bài viết rất hay!',2,1),
(N'Cảm ơn thông tin bổ ích.',1,2);

/* ==============================
   SEED QUESTION TEST (58 câu)
   ============================== */
-- Realistic (10)
INSERT INTO QuestionTest (Content, QuestionType) VALUES
(N'Tôi tự thấy mình là người khá về các môn thể thao', 'Realistic'),
(N'Tôi là người yêu thích thiên nhiên', 'Realistic'),
(N'Tôi là người độc lập', 'Realistic'),
(N'Tôi thích sửa chữa đồ vật, vật dụng xung quanh tôi', 'Realistic'),
(N'Tôi thích làm việc sử dụng tay chân (làm vườn, sửa chữa nhà cửa)', 'Realistic'),
(N'Tôi thích tập thể dục', 'Realistic'),
(N'Tôi thích làm việc cho đến khi công việc hoàn thành', 'Realistic'),
(N'Tôi thích làm việc một mình', 'Realistic'),
(N'Tôi chơi các môn thể thao có tính đồng đội', 'Realistic'),
(N'Tôi thích mạo hiểm và tham gia phiêu lưu', 'Realistic');

-- Investigative (11)
INSERT INTO QuestionTest (Content, QuestionType) VALUES
(N'Tôi tò mò về thế giới xung quanh', 'Investigative'),
(N'Tôi rất hay để ý tới chi tiết và cẩn thận', 'Investigative'),
(N'Tôi có thể tính toán phức tạp', 'Investigative'),
(N'Tôi thích giải toán', 'Investigative'),
(N'Tôi thích sử dụng máy tính', 'Investigative'),
(N'Tôi thích các môn khoa học', 'Investigative'),
(N'Tôi thích thách thức', 'Investigative'),
(N'Tôi thích đọc sách', 'Investigative'),
(N'Tôi thích sưu tầm đồ vật', 'Investigative'),
(N'Tôi thích trò ô chữ', 'Investigative'),
(N'Tôi thích nghiên cứu', 'Investigative');

-- Artistic (10)
INSERT INTO QuestionTest (Content, QuestionType) VALUES
(N'Tôi rất sáng tạo', 'Artistic'),
(N'Tôi thích vẽ, tô màu và sơn', 'Artistic'),
(N'Tôi có thể chơi nhạc cụ', 'Artistic'),
(N'Tôi thích thời trang độc đáo', 'Artistic'),
(N'Tôi thích đọc truyện, kịch và thơ ca', 'Artistic'),
(N'Tôi thích mỹ thuật và thủ công', 'Artistic'),
(N'Tôi xem nhiều phim', 'Artistic'),
(N'Tôi thích chụp ảnh', 'Artistic'),
(N'Tôi thích học ngoại ngữ', 'Artistic'),
(N'Tôi thích hát, đóng kịch, khiêu vũ', 'Artistic');

-- Social (9)
INSERT INTO QuestionTest (Content, QuestionType) VALUES
(N'Tôi rất thân thiện', 'Social'),
(N'Tôi thích chỉ dẫn hoặc dạy người khác', 'Social'),
(N'Tôi thích nói chuyện trước đám đông', 'Social'),
(N'Tôi làm việc tốt trong nhóm', 'Social'),
(N'Tôi thích điều hành thảo luận', 'Social'),
(N'Tôi thích giúp đỡ người khó khăn', 'Social'),
(N'Tôi thích dự tiệc', 'Social'),
(N'Tôi thích làm quen bạn mới', 'Social'),
(N'Tôi thích tham gia nhóm cộng đồng', 'Social');

-- Enterprising (7)
INSERT INTO QuestionTest (Content, QuestionType) VALUES
(N'Tôi thích học hỏi về tài chính', 'Enterprising'),
(N'Tôi thích bán hàng', 'Enterprising'),
(N'Tôi khá nổi tiếng ở trường', 'Enterprising'),
(N'Tôi thích lãnh đạo nhóm', 'Enterprising'),
(N'Tôi thích được bầu vị trí quan trọng', 'Enterprising'),
(N'Tôi thích có quyền lực', 'Enterprising'),
(N'Tôi muốn sở hữu doanh nghiệp nhỏ', 'Enterprising');

-- Conventional (11)
INSERT INTO QuestionTest (Content, QuestionType) VALUES
(N'Tôi thích dành dụm tiền', 'Conventional'),
(N'Tôi thích gọn gàng', 'Conventional'),
(N'Tôi thích phòng ngăn nắp', 'Conventional'),
(N'Tôi thích sưu tầm báo', 'Conventional'),
(N'Tôi thích lập danh sách công việc', 'Conventional'),
(N'Tôi thích sử dụng máy tính', 'Conventional'),
(N'Tôi cân nhắc chi phí kỹ lưỡng', 'Conventional'),
(N'Tôi thích đánh máy hơn viết tay', 'Conventional'),
(N'Tôi thích làm thư ký', 'Conventional'),
(N'Khi làm toán tôi kiểm tra lại nhiều lần', 'Conventional'),
(N'Tôi thích viết thư', 'Conventional');

-- Demo Test
INSERT INTO TestSession (UserId, StartAt) VALUES (2,GETDATE());
INSERT INTO TestAnswer (IsSelected,QuestionId,TestSessionId) VALUES (1,1,1),(0,2,1),(1,12,1);
INSERT INTO Result (Content,R,I,A,S,E,C,TestSessionId) VALUES (N'Kết quả test demo',1,1,0,0,0,0,1);
INSERT INTO TestHistory (UserId,TestSessionId,TestAnswerId) VALUES (2,1,1),(2,1,2),(2,1,3);

PRINT 'Database seeded successfully!';
