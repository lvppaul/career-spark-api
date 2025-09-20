USE CareerSparkDB;
GO

-- Thêm dữ liệu mẫu cho Role
INSERT INTO Role (RoleName) VALUES 
(N'Admin'),
(N'User'),
(N'Moderator');

-- Thêm dữ liệu mẫu cho User
INSERT INTO [User] (Name, Phone, Email, Password, RoleId) VALUES
(N'Nguyen Van A', '0912345678', 'a@example.com', '123456', 1),
(N'Tran Thi B', '0987654321', 'b@example.com', '123456', 2),
(N'Le Van C', '0905123456', 'c@example.com', '123456', 3);

-- Thêm dữ liệu mẫu cho SubscriptionPlan
INSERT INTO SubscriptionPlan (Name, Price, DurationDays, Description, Level,IsActive) VALUES
(N'Free Plan', 0, 30, N'Gói miễn phí cơ bản',1,1),
(N'Standard Plan', 99.99, 90, N'Gói tiêu chuẩn 3 tháng',2,1),
(N'Premium Plan', 299.99, 365, N'Gói cao cấp 1 năm',3,1);

-- Thêm dữ liệu mẫu cho CareerField
INSERT INTO CareerField (Name, Description) VALUES
(N'Công nghệ thông tin', N'Lĩnh vực phần mềm, hệ thống, trí tuệ nhân tạo'),
(N'Kinh tế', N'Tài chính, kế toán, quản trị kinh doanh');

-- Thêm dữ liệu mẫu cho CareerPath
INSERT INTO CareerPath (Title, Description, CareerFieldId) VALUES
(N'Lập trình viên Backend', N'Tập trung phát triển server-side', 1),
(N'Phân tích tài chính', N'Tập trung phân tích và dự báo tài chính', 2);

-- Thêm dữ liệu mẫu cho CareerMileStone
INSERT INTO CareerMileStone (CareerPathId, Title, Description, [Index], SuggestedCourseUrl) VALUES
(1, N'Học C# cơ bản', N'Nắm vững C# để lập trình backend', 1, N'https://learn.microsoft.com/dotnet/csharp'),
(1, N'Tìm hiểu ASP.NET Core', N'Xây dựng API với ASP.NET Core', 2, N'https://dotnet.microsoft.com/learn/aspnet'),
(2, N'Học kế toán cơ bản', N'Nắm vững các nguyên tắc kế toán', 1, N'https://example.com/accounting-course');

-- Thêm dữ liệu mẫu cho Blogs
INSERT INTO Blogs (Title, Content) VALUES
(N'Chào mừng đến Career Spark', N'Bài viết giới thiệu dự án Career Spark.'),
(N'Mẹo chọn nghề nghiệp', N'5 mẹo giúp bạn định hướng nghề nghiệp.');

-- Thêm dữ liệu mẫu cho Comments
INSERT INTO Comments (Content, UserId, BlogId) VALUES
(N'Bài viết rất hay!', 2, 1),
(N'Cảm ơn thông tin bổ ích.', 1, 2);
