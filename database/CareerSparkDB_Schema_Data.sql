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
    RoleName NVARCHAR(100) NOT NULL,
	IsDeleted BIT NOT NULL DEFAULT 0
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
CREATE TABLE CareerRoadmap (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CareerPathId INT NOT NULL,
    StepOrder INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(500),
    SkillFocus NVARCHAR(200),
    DifficultyLevel NVARCHAR(50),
    DurationWeeks INT,
    SuggestedCourseUrl NVARCHAR(255),
    FOREIGN KEY (CareerPathId) REFERENCES CareerPath(Id) ON DELETE CASCADE
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
-- ========== 1️⃣ Công nghệ thông tin ==========
INSERT INTO CareerPath (Title, Description, CareerFieldId) VALUES
(N'Lập trình viên Backend', N'Phát triển logic server-side, API, và kết nối cơ sở dữ liệu.', 1),
(N'Lập trình viên Frontend', N'Xây dựng giao diện web tương tác với người dùng.', 1),
(N'Kỹ sư DevOps', N'Tự động hóa quy trình triển khai và quản lý hạ tầng.', 1),
(N'Chuyên viên Kiểm thử phần mềm (QA)', N'Thực hiện kiểm thử phần mềm đảm bảo chất lượng.', 1),
(N'Kỹ sư Dữ liệu', N'Quản lý, xử lý và phân tích dữ liệu lớn.', 1),
(N'Chuyên gia Trí tuệ nhân tạo (AI/ML)', N'Nghiên cứu và ứng dụng các thuật toán học máy.', 1),
(N'Chuyên viên An ninh mạng', N'Phát hiện và phòng chống tấn công mạng.', 1);

-- ========== 2️⃣ Kinh tế ==========
INSERT INTO CareerPath (Title, Description, CareerFieldId) VALUES
(N'Kế toán viên', N'Ghi chép, tổng hợp và phân tích thông tin tài chính doanh nghiệp.', 2),
(N'Chuyên viên Phân tích tài chính', N'Tư vấn và phân tích dữ liệu tài chính để ra quyết định đầu tư.', 2),
(N'Chuyên viên Marketing', N'Lên kế hoạch và triển khai chiến dịch quảng bá sản phẩm.', 2),
(N'Chuyên viên Nhân sự', N'Quản lý tuyển dụng, đào tạo và chế độ nhân sự.', 2),
(N'Chuyên viên Quản trị kinh doanh', N'Xây dựng chiến lược và quản lý hoạt động kinh doanh.', 2),
(N'Nhân viên Bán hàng (Sales Executive)', N'Tiếp cận và chăm sóc khách hàng để đạt mục tiêu doanh số.', 2),
(N'Chuyên viên Đầu tư chứng khoán', N'Phân tích thị trường và đưa ra quyết định đầu tư.', 2);

-- ========== 3️⃣ Y tế & Sức khỏe ==========
INSERT INTO CareerPath (Title, Description, CareerFieldId) VALUES
(N'Bác sĩ đa khoa', N'Khám, chẩn đoán và điều trị bệnh thông thường.', 3),
(N'Y tá/Điều dưỡng', N'Hỗ trợ bác sĩ trong chăm sóc và điều trị bệnh nhân.', 3),
(N'Kỹ thuật viên xét nghiệm y học', N'Thực hiện xét nghiệm và phân tích mẫu bệnh phẩm.', 3),
(N'Dược sĩ', N'Nghiên cứu, pha chế và tư vấn sử dụng thuốc.', 3),
(N'Bác sĩ chuyên khoa', N'Đi sâu vào lĩnh vực cụ thể như tim mạch, nhi, thần kinh, v.v.', 3),
(N'Chuyên viên dinh dưỡng', N'Tư vấn chế độ ăn uống và sức khỏe.', 3),
(N'Nhà nghiên cứu y sinh', N'Nghiên cứu khoa học trong lĩnh vực sinh học và y học.', 3);

-- ========== 4️⃣ Giáo dục ==========
INSERT INTO CareerPath (Title, Description, CareerFieldId) VALUES
(N'Giáo viên Toán', N'Dạy học môn Toán ở các cấp học.', 4),
(N'Giáo viên Tiếng Anh', N'Giảng dạy tiếng Anh và kỹ năng giao tiếp.', 4),
(N'Giảng viên Đại học', N'Nghiên cứu và giảng dạy tại các trường đại học.', 4),
(N'Chuyên viên Tư vấn học đường', N'Hỗ trợ định hướng học tập và tâm lý cho học sinh.', 4),
(N'Nhà nghiên cứu giáo dục', N'Nghiên cứu phương pháp giảng dạy và cải tiến chương trình học.', 4),
(N'Giáo viên Mầm non', N'Chăm sóc và giáo dục trẻ nhỏ ở độ tuổi mầm non.', 4),
(N'Quản lý giáo dục', N'Triển khai, giám sát và đánh giá hoạt động đào tạo.', 4);

-- ========== 5️⃣ Nghệ thuật & Sáng tạo ==========
INSERT INTO CareerPath (Title, Description, CareerFieldId) VALUES
(N'Họa sĩ minh họa', N'Sáng tạo tranh, hình ảnh minh họa cho sách và truyền thông.', 5),
(N'Nhạc sĩ/Composer', N'Sáng tác và hòa âm phối khí các tác phẩm âm nhạc.', 5),
(N'Nhà thiết kế đồ họa (Graphic Designer)', N'Thiết kế hình ảnh thương hiệu và truyền thông.', 5),
(N'Nhiếp ảnh gia', N'Chụp và chỉnh sửa ảnh nghệ thuật hoặc thương mại.', 5),
(N'Nhà thiết kế thời trang', N'Tạo ra mẫu thiết kế quần áo, phụ kiện.', 5),
(N'Đạo diễn/Producer', N'Thực hiện và quản lý sản xuất phim, video.', 5),
(N'Nhà văn/Copywriter', N'Sáng tạo nội dung và câu chuyện truyền cảm hứng.', 5);

-- ========== 6️⃣ Kỹ thuật & Cơ khí ==========
INSERT INTO CareerPath (Title, Description, CareerFieldId) VALUES
(N'Kỹ sư Cơ khí', N'Thiết kế, chế tạo và vận hành máy móc.', 6),
(N'Kỹ sư Điện', N'Lắp đặt, vận hành và bảo trì hệ thống điện.', 6),
(N'Kỹ sư Xây dựng', N'Thiết kế và giám sát các công trình dân dụng.', 6),
(N'Kỹ sư Ô tô', N'Nghiên cứu và phát triển phương tiện giao thông.', 6),
(N'Kỹ sư Tự động hóa', N'Thiết kế hệ thống điều khiển tự động cho dây chuyền sản xuất.', 6),
(N'Kỹ sư Robot', N'Phát triển robot phục vụ công nghiệp và đời sống.', 6),
(N'Kỹ sư Môi trường', N'Nghiên cứu và cải thiện các vấn đề môi trường.', 6);

go
-- CAREER ROADMAP
-- CareerFieldId = 1 — Công nghệ thông tin
-- Lập trình viên Backend (CareerPathId = 1)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl)
VALUES
(1, 1, N'Học C# cơ bản', N'Nắm vững cú pháp, OOP và cấu trúc dữ liệu trong C#.', N'C#, OOP, Data Structures', N'Beginner', 4, N'https://learn.microsoft.com/dotnet/csharp'),
(1, 2, N'Tìm hiểu ASP.NET Core', N'Xây dựng RESTful API và hiểu middleware.', N'ASP.NET Core, REST API', N'Intermediate', 5, N'https://dotnet.microsoft.com/learn/aspnet'),
(1, 3, N'Học Entity Framework Core', N'Thiết kế database và truy vấn bằng ORM.', N'EF Core, Database Design', N'Intermediate', 4, N'https://learn.microsoft.com/ef/core'),
(1, 4, N'Học triển khai Docker & CI/CD', N'Triển khai ứng dụng bằng Docker và GitHub Actions.', N'Docker, CI/CD, DevOps', N'Advanced', 4, N'https://docs.docker.com');

-- Lập trình viên Frontend (CareerPathId = 2)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl)
VALUES
(2, 1, N'Học HTML, CSS, JavaScript', N'Nắm vững nền tảng web cơ bản.', N'HTML, CSS, JavaScript', N'Beginner', 4, N'https://developer.mozilla.org'),
(2, 2, N'Học React và TypeScript', N'Phát triển SPA với React và TS.', N'React, TypeScript, Hooks', N'Intermediate', 6, N'https://react.dev'),
(2, 3, N'Học Tailwind hoặc MUI', N'Thiết kế giao diện đẹp, responsive.', N'TailwindCSS, MUI, UI Design', N'Intermediate', 3, N'https://mui.com'),
(2, 4, N'Học Next.js & tối ưu SEO', N'Xây dựng SSR app và tối ưu hiệu suất.', N'Next.js, SEO, Routing', N'Advanced', 4, N'https://nextjs.org');

-- Kỹ sư DevOps (CareerPathId = 3)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl)
VALUES
(3, 1, N'Tìm hiểu Linux và Networking', N'Nắm cơ bản về hệ thống, SSH, IP, DNS.', N'Linux, Networking, CLI', N'Beginner', 4, N'https://ubuntu.com/tutorials'),
(3, 2, N'Học Docker & Kubernetes', N'Tạo container và triển khai cluster.', N'Docker, Kubernetes', N'Intermediate', 6, N'https://kubernetes.io'),
(3, 3, N'Tích hợp CI/CD Pipeline', N'Sử dụng Jenkins hoặc GitHub Actions.', N'CI/CD, Automation', N'Intermediate', 4, N'https://docs.github.com/actions'),
(3, 4, N'Triển khai Cloud', N'Deploy ứng dụng lên AWS hoặc Azure.', N'AWS, Azure, Deployment', N'Advanced', 4, N'https://aws.amazon.com/getting-started');

-- QA Engineer (CareerPathId = 4)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl)
VALUES
(4, 1, N'Học kiểm thử cơ bản', N'Hiểu test case, bug report, regression.', N'Manual Testing, QA Process', N'Beginner', 3, N'https://www.guru99.com/software-testing.html'),
(4, 2, N'Học Postman & API Testing', N'Thực hành test API, viết test script.', N'Postman, REST API', N'Intermediate', 3, N'https://learning.postman.com'),
(4, 3, N'Học Selenium hoặc Cypress', N'Tự động hóa UI testing.', N'Selenium, Cypress', N'Intermediate', 4, N'https://www.selenium.dev'),
(4, 4, N'Học Agile & Scrum', N'Áp dụng kiểm thử trong Agile.', N'Agile, Scrum, QA Collaboration', N'Advanced', 2, N'https://www.scrum.org');

-- Kỹ sư Dữ liệu (CareerPathId = 5)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl)
VALUES
(5, 1, N'Học Python & SQL', N'Xử lý dữ liệu và truy vấn cơ bản.', N'Python, SQL, Data Cleaning', N'Beginner', 4, N'https://mode.com/sql-tutorial'),
(5, 2, N'Học Data Visualization', N'Trực quan hóa dữ liệu với Pandas, Matplotlib.', N'Pandas, Matplotlib, EDA', N'Intermediate', 4, N'https://pandas.pydata.org'),
(5, 3, N'Học ETL và Data Pipeline', N'Thiết kế pipeline dữ liệu với Airflow.', N'ETL, Airflow, Automation', N'Intermediate', 5, N'https://airflow.apache.org'),
(5, 4, N'Học Big Data & Cloud', N'Làm việc với Spark và AWS S3.', N'Spark, AWS, Cloud Storage', N'Advanced', 6, N'https://spark.apache.org');

-- Chuyên gia AI/ML (CareerPathId = 6)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl)
VALUES
(6, 1, N'Học Python cho ML', N'Nắm NumPy, Pandas, Scikit-learn.', N'Python, Scikit-learn', N'Beginner', 4, N'https://scikit-learn.org'),
(6, 2, N'Học Machine Learning cơ bản', N'Hiểu Regression, Classification, Clustering.', N'ML Algorithms, Math', N'Intermediate', 6, N'https://developers.google.com/machine-learning'),
(6, 3, N'Học Deep Learning', N'Sử dụng TensorFlow hoặc PyTorch.', N'TensorFlow, Neural Networks', N'Advanced', 8, N'https://www.tensorflow.org'),
(6, 4, N'Thực hiện dự án AI', N'Ứng dụng mô hình ML vào thực tế.', N'MLOps, Deployment', N'Advanced', 4, N'https://mlops.community');

-- Chuyên viên An ninh mạng (CareerPathId = 7)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl)
VALUES
(7, 1, N'Học Networking & OSI Model', N'Nắm vững mô hình mạng và giao thức.', N'OSI, TCP/IP, DNS', N'Beginner', 4, N'https://www.cisco.com'),
(7, 2, N'Học Ethical Hacking', N'Thực hành pentest và phòng thủ.', N'Hacking, Kali Linux', N'Intermediate', 6, N'https://www.hackthebox.com'),
(7, 3, N'Học OWASP Top 10', N'Hiểu các lỗ hổng phổ biến và cách phòng ngừa.', N'OWASP, Web Security', N'Intermediate', 4, N'https://owasp.org'),
(7, 4, N'Làm lab thực tế', N'Triển khai honeypot và IDS/IPS.', N'Honeypot, Security Tools', N'Advanced', 5, N'https://tryhackme.com');

-- CareerFieldId = 2 — Kinh tế
-- Kế toán viên (CareerPathId = 8)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl)
VALUES
(8, 1, N'Học nguyên lý kế toán', N'Hiểu cơ bản về tài sản, nợ và vốn.', N'Accounting, Balance Sheet', N'Beginner', 3, N'https://example.com/accounting-basics'),
(8, 2, N'Học Excel kế toán', N'Làm báo cáo tài chính bằng Excel.', N'Excel, Reporting', N'Intermediate', 3, N'https://exceljet.net'),
(8, 3, N'Học phần mềm kế toán (MISA)', N'Sử dụng phần mềm quản lý doanh nghiệp.', N'MISA, Fast Accounting', N'Intermediate', 4, N'https://misa.vn'),
(8, 4, N'Ôn chứng chỉ kế toán viên', N'Chuẩn bị thi ACCA hoặc CPA.', N'ACCA, CPA', N'Advanced', 6, N'https://acca-global.com');

-- Phân tích tài chính (CareerPathId = 9)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl)
VALUES
(9, 1, N'Học nguyên lý tài chính', N'Hiểu NPV, IRR, ROA, ROE.', N'Finance Fundamentals', N'Beginner', 3, N'https://corporatefinanceinstitute.com'),
(9, 2, N'Học Excel nâng cao', N'Tạo mô hình tài chính.', N'Excel, Modeling', N'Intermediate', 4, N'https://exceljet.net'),
(9, 3, N'Học phân tích báo cáo tài chính', N'Đọc hiểu báo cáo P&L và dòng tiền.', N'Financial Statements', N'Intermediate', 5, N'https://investopedia.com'),
(9, 4, N'Học Power BI', N'Trực quan hóa dữ liệu tài chính.', N'Power BI, Data Viz', N'Advanced', 4, N'https://powerbi.microsoft.com');

-- Chuyên viên Marketing (CareerPathId = 10)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl)
VALUES
(10, 1, N'Học nguyên lý Marketing', N'Nắm 4P và hành vi khách hàng.', N'Marketing 4P, STP', N'Beginner', 3, N'https://coursera.org/learn/marketing'),
(10, 2, N'Học Digital Marketing', N'Thực hành quảng cáo Google và Meta.', N'Ads, Analytics', N'Intermediate', 4, N'https://skillshop.exceedlms.com'),
(10, 3, N'Học Content & SEO', N'Tối ưu hóa nội dung web.', N'SEO, Copywriting', N'Intermediate', 4, N'https://ahrefs.com/blog/seo-basics'),
(10, 4, N'Phân tích dữ liệu Marketing', N'Theo dõi hiệu quả chiến dịch.', N'GA4, Data Analysis', N'Advanced', 4, N'https://analytics.google.com');

-- CareerFieldId = 3 — Y tế & Sức khỏe
-- Bác sĩ đa khoa (CareerPathId = 15)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl)
VALUES
(15, 1, N'Học kiến thức y học cơ bản', N'Nắm giải phẫu, sinh lý, hóa sinh.', N'Anatomy, Physiology, Biochemistry', N'Beginner', 8, N'https://example.com/medical-basics'),
(15, 2, N'Học chẩn đoán lâm sàng', N'Rèn kỹ năng khám, đọc kết quả xét nghiệm.', N'Diagnosis, Clinical Skills', N'Intermediate', 12, N'https://example.com/clinical'),
(15, 3, N'Thực tập tại bệnh viện', N'Thực hành điều trị, chăm sóc bệnh nhân.', N'Patient Care, Treatment', N'Advanced', 12, N'https://example.com/internship');

-- Điều dưỡng/Y tá (CareerPathId = 16)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl)  VALUES 
(16, 1, N'Học chăm sóc bệnh nhân cơ bản', N'Thực hành tiêm, đo sinh hiệu, vệ sinh y tế.', N'Nursing, Patient Care', N'Beginner', 8, N'https://example.com/nursing-basics'),
(16, 2, N'Học kỹ năng giao tiếp y tế', N'Tư vấn, giao tiếp với bệnh nhân và người nhà.', N'Communication, Empathy', N'Intermediate', 4, N'https://example.com/medical-communication'),
(16, 3, N'Học sơ cứu và cấp cứu', N'Xử lý các tình huống khẩn cấp.', N'First Aid, Emergency Response', N'Intermediate', 4, N'https://redcross.org');

-- Kỹ thuật viên xét nghiệm (CareerPathId = 17)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl) VALUES
(17, 1, N'Học kỹ thuật phòng thí nghiệm', N'Sử dụng thiết bị, chuẩn bị mẫu.', N'Lab Techniques, Equipment Handling', N'Beginner', 6, N'https://example.com/lab-basics'),
(17, 2, N'Học xét nghiệm sinh hóa & vi sinh', N'Phân tích máu, nước tiểu, mô tế bào.', N'Biochemistry, Microbiology', N'Intermediate', 8, N'https://example.com/biolab'),
(17, 3, N'Thực hành ghi chép và báo cáo', N'Lập báo cáo kết quả xét nghiệm.', N'Lab Report, Data Recording', N'Intermediate', 3, N'https://example.com/lab-report');

-- Dược sĩ (CareerPathId = 18)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl) VALUES
(18, 1, N'Học hóa dược và dược lý', N'Hiểu cơ chế tác dụng thuốc.', N'Pharmacology, Chemistry', N'Beginner', 6, N'https://example.com/pharma'),
(18, 2, N'Học công thức bào chế thuốc', N'Thực hành pha chế, bảo quản thuốc.', N'Formulation, Storage', N'Intermediate', 6, N'https://example.com/formulation'),
(18, 3, N'Học tư vấn sử dụng thuốc', N'Hướng dẫn liều lượng, tương tác thuốc.', N'Drug Counseling, Safety', N'Advanced', 4, N'https://example.com/drug-safety');

-- Chuyên viên dinh dưỡng (CareerPathId = 19)	
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl) VALUES
(19, 1, N'Học kiến thức dinh dưỡng cơ bản', N'Phân biệt nhóm chất và nhu cầu cơ thể.', N'Nutrition, Health', N'Beginner', 4, N'https://example.com/nutrition-basics'),
(19, 2, N'Học xây dựng chế độ ăn', N'Lập thực đơn cho từng đối tượng.', N'Meal Planning, Calorie Counting', N'Intermediate', 6, N'https://example.com/diet-plan'),
(19, 3, N'Thực hành tư vấn dinh dưỡng', N'Hướng dẫn chế độ ăn cho bệnh nhân.', N'Consulting, Health Coaching', N'Advanced', 4, N'https://example.com/consulting');

-- CareerFieldId = 4 — Giáo dục
-- Giáo viên Toán (CareerPathId = 22)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl) VALUES
(22, 1, N'Nâng cao kiến thức Toán', N'Học sâu về Đại số, Giải tích, Hình học.', N'Algebra, Calculus, Geometry', N'Beginner', 8, N'https://khanacademy.org/math'),
(22, 2, N'Học phương pháp giảng dạy', N'Hiểu tâm lý học sinh và kỹ năng truyền đạt.', N'Teaching, Pedagogy', N'Intermediate', 4, N'https://example.com/teaching-methods'),
(22, 3, N'Chứng chỉ nghiệp vụ sư phạm', N'Hoàn thành chứng chỉ đào tạo nghề giáo.', N'Pedagogy Certificate', N'Intermediate', 6, N'https://example.com/teacher'),
(22, 4, N'Ứng dụng công nghệ dạy học', N'Sử dụng công cụ trực tuyến hiệu quả.', N'EdTech, Online Tools', N'Advanced', 3, N'https://example.com/edtech');

-- Giáo viên Tiếng Anh (CareerPathId = 23)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl) VALUES
(23, 1, N'Nâng cao ngữ pháp & phát âm', N'Luyện kỹ năng speaking, listening.', N'Grammar, Pronunciation', N'Beginner', 6, N'https://example.com/english'),
(23, 2, N'Học phương pháp ESL', N'Dạy tiếng Anh như ngôn ngữ thứ hai.', N'ESL Methodology', N'Intermediate', 4, N'https://example.com/esl'),
(23, 3, N'Chứng chỉ IELTS/TOEFL', N'Nâng cao kỹ năng ngôn ngữ chuẩn quốc tế.', N'IELTS, TOEFL', N'Advanced', 8, N'https://ielts.org');

-- Tư vấn học đường (CareerPathId = 25)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl) VALUES
(25, 1, N'Học tâm lý học cơ bản', N'Nắm tâm lý lứa tuổi học sinh.', N'Psychology, Counseling', N'Beginner', 4, N'https://example.com/psychology'),
(25, 2, N'Học kỹ năng tư vấn', N'Đặt câu hỏi, lắng nghe, hỗ trợ học sinh.', N'Counseling Skills', N'Intermediate', 4, N'https://example.com/counseling'),
(25, 3, N'Thực hành hỗ trợ học sinh', N'Tư vấn hướng nghiệp và học tập.', N'Career Guidance, Mentoring', N'Advanced', 4, N'https://example.com/mentorship');

-- CareerFieldId = 5 — Nghệ thuật & Sáng tạo
-- Họa sĩ minh họa (CareerPathId = 29)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl) VALUES
(29, 1, N'Học vẽ cơ bản', N'Làm quen hình khối, phối màu, bố cục.', N'Drawing, Composition', N'Beginner', 6, N'https://example.com/drawing'),
(29, 2, N'Học Digital Painting', N'Sử dụng phần mềm Procreate, Photoshop.', N'Digital Art, Photoshop', N'Intermediate', 5, N'https://adobe.com/photoshop'),
(29, 3, N'Xây dựng portfolio nghệ thuật', N'Tạo bộ sưu tập tác phẩm cá nhân.', N'Portfolio, Branding', N'Advanced', 4, N'https://example.com/portfolio');

-- Nhà thiết kế đồ họa (CareerPathId = 31)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl) VALUES
(31, 1, N'Học nguyên lý thiết kế', N'Hiểu về bố cục, màu sắc, typography.', N'Design, Color Theory', N'Beginner', 4, N'https://example.com/design-basics'),
(31, 2, N'Học Photoshop & Illustrator', N'Thành thạo công cụ thiết kế.', N'Photoshop, Illustrator', N'Intermediate', 6, N'https://adobe.com'),
(31, 3, N'Học thiết kế thương hiệu', N'Tạo logo, bộ nhận diện thương hiệu.', N'Branding, Logo Design', N'Advanced', 5, N'https://example.com/branding');

-- Nhạc sĩ/Composer (CareerPathId = 30)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl) VALUES
(30, 1, N'Học lý thuyết âm nhạc', N'Hiểu nhịp, hợp âm, giai điệu.', N'Music Theory', N'Beginner', 6, N'https://example.com/music-theory'),
(30, 2, N'Học sáng tác nhạc', N'Viết giai điệu, phối khí.', N'Composition, Arrangement', N'Intermediate', 6, N'https://example.com/composition'),
(30, 3, N'Sản xuất âm nhạc', N'Thực hành trên DAW như FL Studio.', N'Production, Mixing', N'Advanced', 6, N'https://flstudio.com');

-- CareerFieldId = 6 — Kỹ thuật & Cơ khí
-- Kỹ sư Cơ khí (CareerPathId = 36)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl) VALUES
(36, 1, N'Học vẽ kỹ thuật cơ khí', N'Đọc hiểu bản vẽ, tiêu chuẩn kỹ thuật.', N'Technical Drawing', N'Beginner', 4, N'https://example.com/mechanical-drawing'),
(36, 2, N'Học CAD/CAM', N'Thiết kế mô hình 3D, gia công CNC.', N'SolidWorks, AutoCAD', N'Intermediate', 6, N'https://autodesk.com'),
(36, 3, N'Thực hành chế tạo máy', N'Lắp ráp và kiểm tra sản phẩm.', N'Mechanics, Assembly', N'Advanced', 6, N'https://example.com/machine');

--Kỹ sư Xây dựng (CareerPathId = 38)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl) VALUES
(38, 1, N'Học vật liệu xây dựng', N'Nắm tính chất và ứng dụng vật liệu.', N'Materials, Civil Basics', N'Beginner', 4, N'https://example.com/materials'),
(38, 2, N'Học AutoCAD & Revit', N'Thiết kế bản vẽ và kết cấu công trình.', N'AutoCAD, BIM', N'Intermediate', 6, N'https://autodesk.com'),
(38, 3, N'Học quản lý dự án xây dựng', N'Lập tiến độ, kiểm soát chi phí.', N'Project Management', N'Advanced', 5, N'https://coursera.org/learn/project');

--Kỹ sư Môi trường (CareerPathId = 42)
INSERT INTO CareerRoadmap (CareerPathId, StepOrder, Title, Description, SkillFocus, DifficultyLevel, DurationWeeks, SuggestedCourseUrl) VALUES
(42, 1, N'Học cơ bản về môi trường', N'Nắm các khái niệm ô nhiễm, sinh thái.', N'Environment, Ecology', N'Beginner', 4, N'https://example.com/environment'),
(42, 2, N'Học công nghệ xử lý nước thải', N'Thiết kế hệ thống lọc, xử lý khí.', N'Water Treatment, Waste Management', N'Intermediate', 5, N'https://example.com/water-treatment'),
(42, 3, N'Học đánh giá tác động môi trường', N'Lập báo cáo EIA.', N'EIA, Sustainability', N'Advanced', 4, N'https://example.com/eia');

go


-- Career Mapping
INSERT INTO CareerMapping (RiasecType, CareerFieldId) VALUES
('R',6),('I',1),('A',5),('S',4),('E',2),('C',2);


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




