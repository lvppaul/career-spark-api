USE CareerSparkDB;
GO

-- =========================================
-- SEED ROLE
-- =========================================
INSERT INTO Role (RoleName) VALUES 
(N'Admin'),
(N'User'),
(N'Moderator');

-- =========================================
-- SEED USER
-- =========================================
INSERT INTO [User] (Name, Phone, Email, Password, RoleId) VALUES
(N'Nguyen Van A', '0912345678', 'a@example.com', '123456', 1),
(N'Tran Thi B', '0987654321', 'b@example.com', '123456', 2),
(N'Le Van C', '0905123456', 'c@example.com', '123456', 3);

-- =========================================
-- SEED SUBSCRIPTION PLAN
-- =========================================
INSERT INTO SubscriptionPlan (Name, Price, DurationDays, Description) VALUES
(N'Free Plan', 0, 30, N'Gói miễn phí cơ bản'),
(N'Standard Plan', 99.99, 90, N'Gói tiêu chuẩn 3 tháng'),
(N'Premium Plan', 299.99, 365, N'Gói cao cấp 1 năm');

-- =========================================
-- SEED CAREER FIELD
-- =========================================
INSERT INTO CareerField (Name, Description) VALUES
(N'Công nghệ thông tin', N'Lĩnh vực phần mềm, hệ thống, trí tuệ nhân tạo'),
(N'Kinh tế', N'Tài chính, kế toán, quản trị kinh doanh');

-- =========================================
-- SEED CAREER PATH
-- =========================================
INSERT INTO CareerPath (Title, Description, CareerFieldId) VALUES
(N'Lập trình viên Backend', N'Tập trung phát triển server-side', 1),
(N'Phân tích tài chính', N'Tập trung phân tích và dự báo tài chính', 2);

-- =========================================
-- SEED CAREER MILESTONE
-- =========================================
INSERT INTO CareerMileStone (CareerPathId, Title, Description, [Index], SuggestedCourseUrl) VALUES
(1, N'Học C# cơ bản', N'Nắm vững C# để lập trình backend', 1, N'https://learn.microsoft.com/dotnet/csharp'),
(1, N'Tìm hiểu ASP.NET Core', N'Xây dựng API với ASP.NET Core', 2, N'https://dotnet.microsoft.com/learn/aspnet'),
(2, N'Học kế toán cơ bản', N'Nắm vững các nguyên tắc kế toán', 1, N'https://example.com/accounting-course');

-- =========================================
-- SEED BLOGS
-- =========================================
INSERT INTO Blogs (Title, Content) VALUES
(N'Chào mừng đến Career Spark', N'Bài viết giới thiệu dự án Career Spark.'),
(N'Mẹo chọn nghề nghiệp', N'5 mẹo giúp bạn định hướng nghề nghiệp.');

-- =========================================
-- SEED COMMENTS
-- =========================================
INSERT INTO Comments (Content, UserId, BlogId) VALUES
(N'Bài viết rất hay!', 2, 1),
(N'Cảm ơn thông tin bổ ích.', 1, 2);

-- =========================================
-- SEED QUESTION TEST (RIASEC)
-- =========================================

-- Realistic (R – Thực tế)
INSERT INTO QuestionTest (Content, QuestionType) VALUES
(N'Tôi tự thấy mình là người khá về các môn thể thao', 'Realistic'),
(N'Tôi là người yêu thích thiên nhiên', 'Realistic'),
(N'Tôi là người độc lập', 'Realistic'),
(N'Tôi thích sửa chữa đồ vật, vật dụng xung quanh tôi', 'Realistic'),
(N'Tôi thích làm việc sử dụng tay chân (làm vườn, sửa chữa nhà cửa)', 'Realistic'),
(N'Tôi thích tập thể dục', 'Realistic'),
(N'Tôi thích làm việc cho đến khi công việc hoàn thành (không thích bỏ dở việc)', 'Realistic'),
(N'Tôi thích làm việc một mình', 'Realistic'),
(N'Tôi chơi các môn thể thao có tính đồng đội', 'Realistic'),
(N'Tôi thích mạo hiểm và tham gia các cuộc phiêu lưu mới', 'Realistic');

-- Investigative (I – Nghiên cứu, Khoa học)
INSERT INTO QuestionTest (Content, QuestionType) VALUES
(N'Tôi là người hay tò mò về thế giới xung quanh mình (thiên nhiên, không gian, những sinh vật sống)', 'Investigative'),
(N'Tôi là người rất hay để ý tới chi tiết và cẩn thận', 'Investigative'),
(N'Tôi tò mò về mọi thứ', 'Investigative'),
(N'Tôi có thể tính những bài toán phức tạp', 'Investigative'),
(N'Tôi thích giải các bài tập toán', 'Investigative'),
(N'Tôi thích sử dụng máy tính', 'Investigative'),
(N'Tôi thích các môn khoa học', 'Investigative'),
(N'Tôi thích những thách thức', 'Investigative'),
(N'Tôi thích đọc sách', 'Investigative'),
(N'Tôi thích sưu tầm (đá, tem, tiền đồng)', 'Investigative'),
(N'Tôi thích trò ô chữ', 'Investigative');

-- Artistic (A – Nghệ thuật, Sáng tạo)
INSERT INTO QuestionTest (Content, QuestionType) VALUES
(N'Tôi rất sáng tạo', 'Artistic'),
(N'Tôi thích vẽ, tô màu và sơn', 'Artistic'),
(N'Tôi có thể chơi một nhạc cụ', 'Artistic'),
(N'Tôi thích tự thiết kế quần áo cho mình hoặc mặc những thời trang lạ và thú vị', 'Artistic'),
(N'Tôi thích đọc truyện viễn tưởng, kịch và thơ ca', 'Artistic'),
(N'Tôi thích mĩ thuật và thủ công', 'Artistic'),
(N'Tôi xem rất nhiều phim', 'Artistic'),
(N'Tôi thích chụp ảnh (chim, người, cảnh đẹp)', 'Artistic'),
(N'Tôi thích học một ngoại ngữ', 'Artistic'),
(N'Tôi thích hát, đóng kịch và khiêu vũ', 'Artistic');

-- Social (S – Xã hội, Hỗ trợ, Giao tiếp)
INSERT INTO QuestionTest (Content, QuestionType) VALUES
(N'Tôi rất thân thiện', 'Social'),
(N'Tôi thích chỉ dẫn hoặc dạy người khác', 'Social'),
(N'Tôi thích nói chuyện trước đám đông', 'Social'),
(N'Tôi làm việc rất tốt trong nhóm', 'Social'),
(N'Tôi thích điều hành các cuộc thảo luận', 'Social'),
(N'Tôi thích giúp đỡ những người khó khăn', 'Social'),
(N'Tôi thích dự tiệc', 'Social'),
(N'Tôi thích làm quen với bạn mới', 'Social'),
(N'Tôi thích làm việc với các nhóm hoạt động xã hội tại trường học, nhà thờ, chùa, phường, xóm, hay cộng đồng', 'Social');

-- Enterprising (E – Quản lý, Kinh doanh, Lãnh đạo)
INSERT INTO QuestionTest (Content, QuestionType) VALUES
(N'Tôi thích học hỏi về tài chính (tiền bạc)', 'Enterprising'),
(N'Tôi thích bán các sản phẩm, bán hàng online', 'Enterprising'),
(N'Tôi nghĩ mình thuộc dạng nổi tiếng ở trường', 'Enterprising'),
(N'Tôi thích lãnh đạo nhóm và các cuộc thảo luận', 'Enterprising'),
(N'Tôi thích được bầu vào các vị trí quan trọng trong nhóm hoặc câu lạc bộ trong và ngoài nhà trường', 'Enterprising'),
(N'Tôi thích có quyền và thích ở vị trí lãnh đạo', 'Enterprising'),
(N'Tôi muốn sở hữu một cửa hàng, quán ăn/uống, một doanh nghiệp nhỏ', 'Enterprising');

-- Conventional (C – Nghiệp vụ, Tổ chức, Ngăn nắp)
INSERT INTO QuestionTest (Content, QuestionType) VALUES
(N'Tôi thích dành dụm tiền', 'Conventional'),
(N'Tôi thích gọn gàng và ngăn nắp', 'Conventional'),
(N'Tôi thích phòng của tôi thường xuyên gọn gàng và ngăn nắp', 'Conventional'),
(N'Tôi thích sưu tầm các bài báo về các sự kiện nổi tiếng', 'Conventional'),
(N'Tôi thích lập những danh sách các việc cần làm', 'Conventional'),
(N'Tôi thích sử dụng máy tính', 'Conventional'),
(N'Tôi rất thực tế và cân nhắc mọi chi phí trước khi mua một thứ gì đó', 'Conventional'),
(N'Tôi thích đánh máy bài tập của trường lớp hơn là viết tay', 'Conventional'),
(N'Tôi thích đảm nhận công việc thư ký trong một câu lạc bộ hay nhóm', 'Conventional'),
(N'Khi làm toán, tôi hay kiểm tra lại bài làm nhiều lần', 'Conventional'),
(N'Tôi thích viết thư', 'Conventional');

-- =========================================
-- DEMO FLOW TEST RIASEC
-- =========================================

-- Tạo session demo cho userId=2
INSERT INTO TestSession (UserId, StartAt) VALUES (2, GETDATE());

-- Giả sử session vừa tạo có Id = 1
-- SELECT SCOPE_IDENTITY();

-- Thêm câu trả lời demo
INSERT INTO TestAnswer (IsSelected, QuestionId, TestSessionId) VALUES
(1, 1, 1),  -- chọn Realistic câu 1
(0, 2, 1),  -- bỏ Realistic câu 2
(1, 12, 1); -- chọn Investigative câu 2

-- Lưu kết quả tính toán
INSERT INTO Result (Content, R, I, A, S, E, C, TestSessionId)
VALUES (N'Kết quả test demo', 1, 1, 0, 0, 0, 0, 1);

-- Log lại lịch sử
INSERT INTO TestHistory (UserId, TestSessionId, TestAnswerId) VALUES
(2, 1, 1),
(2, 1, 2),
(2, 1, 3);
