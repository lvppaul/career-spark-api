-- ========================================
-- SEED DATA FOR CareerSparkDB (PostgreSQL)
-- ========================================

-- Roles
INSERT INTO "Role" ("RoleName") VALUES 
('Admin'), ('User'), ('Moderator');

INSERT INTO "public"."User" ("Id", "Name", "Phone", "Email", "Password", "avatarURL", "RefreshToken", "ExpiredRefreshTokenAt", "IsActive", "CreatedAt", "RoleId")
VALUES ('1', 'Phát Lê Vĩnh', null, 'levinhphat790@gmail.com', null, null, null, null, 'true', '2025-10-06 17:39:57.748498', '2');

-- Subscription Plans
INSERT INTO "SubscriptionPlan" ("Name", "Price", "DurationDays", "Description", "Level", "IsActive") VALUES
('Free Plan', 0, 30, 'Gói miễn phí cơ bản', 1, TRUE),
('Standard Plan', 99.99, 90, 'Gói tiêu chuẩn 3 tháng', 2, TRUE),
('Premium Plan', 299.99, 365, 'Gói cao cấp 1 năm', 3, TRUE);

-- Career Fields
INSERT INTO "CareerField" ("Name", "Description") VALUES
('Công nghệ thông tin', 'Lĩnh vực phần mềm, hệ thống, trí tuệ nhân tạo'),
('Kinh tế', 'Tài chính, kế toán, quản trị kinh doanh'),
('Y tế & Sức khỏe', 'Ngành y, điều dưỡng, chăm sóc sức khỏe'),
('Giáo dục', 'Dạy học, đào tạo, nghiên cứu'),
('Nghệ thuật & Sáng tạo', 'Mỹ thuật, âm nhạc, thiết kế'),
('Kỹ thuật & Cơ khí', 'Ngành điện, cơ khí, xây dựng');

-- Career Paths
-- ========== 1️⃣ Công nghệ thông tin ==========
INSERT INTO "CareerPath" ("Title", "Description", "CareerFieldId") VALUES
('Lập trình viên Backend', 'Phát triển logic server-side, API, và kết nối cơ sở dữ liệu.', 1),
('Lập trình viên Frontend', 'Xây dựng giao diện web tương tác với người dùng.', 1),
('Kỹ sư DevOps', 'Tự động hóa quy trình triển khai và quản lý hạ tầng.', 1),
('Chuyên viên Kiểm thử phần mềm (QA)', 'Thực hiện kiểm thử phần mềm đảm bảo chất lượng.', 1),
('Kỹ sư Dữ liệu', 'Quản lý, xử lý và phân tích dữ liệu lớn.', 1),
('Chuyên gia Trí tuệ nhân tạo (AI/ML)', 'Nghiên cứu và ứng dụng các thuật toán học máy.', 1),
('Chuyên viên An ninh mạng', 'Phát hiện và phòng chống tấn công mạng.', 1);

-- ========== 2️⃣ Kinh tế ==========
INSERT INTO "CareerPath" ("Title", "Description", "CareerFieldId") VALUES
('Kế toán viên', 'Ghi chép, tổng hợp và phân tích thông tin tài chính doanh nghiệp.', 2),
('Chuyên viên Phân tích tài chính', 'Tư vấn và phân tích dữ liệu tài chính để ra quyết định đầu tư.', 2),
('Chuyên viên Marketing', 'Lên kế hoạch và triển khai chiến dịch quảng bá sản phẩm.', 2),
('Chuyên viên Nhân sự', 'Quản lý tuyển dụng, đào tạo và chế độ nhân sự.', 2),
('Chuyên viên Quản trị kinh doanh', 'Xây dựng chiến lược và quản lý hoạt động kinh doanh.', 2),
('Nhân viên Bán hàng (Sales Executive)', 'Tiếp cận và chăm sóc khách hàng để đạt mục tiêu doanh số.', 2),
('Chuyên viên Đầu tư chứng khoán', 'Phân tích thị trường và đưa ra quyết định đầu tư.', 2);

-- ========== 3️⃣ Y tế & Sức khỏe ==========
INSERT INTO "CareerPath" ("Title", "Description", "CareerFieldId") VALUES
('Bác sĩ đa khoa', 'Khám, chẩn đoán và điều trị bệnh thông thường.', 3),
('Y tá/Điều dưỡng', 'Hỗ trợ bác sĩ trong chăm sóc và điều trị bệnh nhân.', 3),
('Kỹ thuật viên xét nghiệm y học', 'Thực hiện xét nghiệm và phân tích mẫu bệnh phẩm.', 3),
('Dược sĩ', 'Nghiên cứu, pha chế và tư vấn sử dụng thuốc.', 3),
('Bác sĩ chuyên khoa', 'Đi sâu vào lĩnh vực cụ thể như tim mạch, nhi, thần kinh, v.v.', 3),
('Chuyên viên dinh dưỡng', 'Tư vấn chế độ ăn uống và sức khỏe.', 3),
('Nhà nghiên cứu y sinh', 'Nghiên cứu khoa học trong lĩnh vực sinh học và y học.', 3);

-- ========== 4️⃣ Giáo dục ==========
INSERT INTO "CareerPath" ("Title", "Description", "CareerFieldId") VALUES
('Giáo viên Toán', 'Dạy học môn Toán ở các cấp học.', 4),
('Giáo viên Tiếng Anh', 'Giảng dạy tiếng Anh và kỹ năng giao tiếp.', 4),
('Giảng viên Đại học', 'Nghiên cứu và giảng dạy tại các trường đại học.', 4),
('Chuyên viên Tư vấn học đường', 'Hỗ trợ định hướng học tập và tâm lý cho học sinh.', 4),
('Nhà nghiên cứu giáo dục', 'Nghiên cứu phương pháp giảng dạy và cải tiến chương trình học.', 4),
('Giáo viên Mầm non', 'Chăm sóc và giáo dục trẻ nhỏ ở độ tuổi mầm non.', 4),
('Quản lý giáo dục', 'Triển khai, giám sát và đánh giá hoạt động đào tạo.', 4);

-- ========== 5️⃣ Nghệ thuật & Sáng tạo ==========
INSERT INTO "CareerPath" ("Title", "Description", "CareerFieldId") VALUES
('Họa sĩ minh họa', 'Sáng tạo tranh, hình ảnh minh họa cho sách và truyền thông.', 5),
('Nhạc sĩ/Composer', 'Sáng tác và hòa âm phối khí các tác phẩm âm nhạc.', 5),
('Nhà thiết kế đồ họa (Graphic Designer)', 'Thiết kế hình ảnh thương hiệu và truyền thông.', 5),
('Nhiếp ảnh gia', 'Chụp và chỉnh sửa ảnh nghệ thuật hoặc thương mại.', 5),
('Nhà thiết kế thời trang', 'Tạo ra mẫu thiết kế quần áo, phụ kiện.', 5),
('Đạo diễn/Producer', 'Thực hiện và quản lý sản xuất phim, video.', 5),
('Nhà văn/Copywriter', 'Sáng tạo nội dung và câu chuyện truyền cảm hứng.', 5);

-- ========== 6️⃣ Kỹ thuật & Cơ khí ==========
INSERT INTO "CareerPath" ("Title", "Description", "CareerFieldId") VALUES
('Kỹ sư Cơ khí', 'Thiết kế, chế tạo và vận hành máy móc.', 6),
('Kỹ sư Điện', 'Lắp đặt, vận hành và bảo trì hệ thống điện.', 6),
('Kỹ sư Xây dựng', 'Thiết kế và giám sát các công trình dân dụng.', 6),
('Kỹ sư Ô tô', 'Nghiên cứu và phát triển phương tiện giao thông.', 6),
('Kỹ sư Tự động hóa', 'Thiết kế hệ thống điều khiển tự động cho dây chuyền sản xuất.', 6),
('Kỹ sư Robot', 'Phát triển robot phục vụ công nghiệp và đời sống.', 6),
('Kỹ sư Môi trường', 'Nghiên cứu và cải thiện các vấn đề môi trường.', 6);

-- ========================================
-- CAREER ROADMAP (chỉ ví dụ nhóm CNTT)
-- ========================================

-- Backend Developer
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")
VALUES
(1,1,'Học C# cơ bản','Nắm vững cú pháp, OOP và cấu trúc dữ liệu trong C#.','C#, OOP, Data Structures','Beginner',4,'https://learn.microsoft.com/dotnet/csharp'),
(1,2,'Tìm hiểu ASP.NET Core','Xây dựng RESTful API và hiểu middleware.','ASP.NET Core, REST API','Intermediate',5,'https://dotnet.microsoft.com/learn/aspnet'),
(1,3,'Học Entity Framework Core','Thiết kế database và truy vấn bằng ORM.','EF Core, Database Design','Intermediate',4,'https://learn.microsoft.com/ef/core'),
(1,4,'Học triển khai Docker & CI/CD','Triển khai ứng dụng bằng Docker và GitHub Actions.','Docker, CI/CD, DevOps','Advanced',4,'https://docs.docker.com');

-- Frontend Developer
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")
VALUES
(2,1,'Học HTML, CSS, JavaScript','Nắm vững nền tảng web cơ bản.','HTML, CSS, JavaScript','Beginner',4,'https://developer.mozilla.org'),
(2,2,'Học React và TypeScript','Phát triển SPA với React và TS.','React, TypeScript, Hooks','Intermediate',6,'https://react.dev'),
(2,3,'Học Tailwind hoặc MUI','Thiết kế giao diện đẹp, responsive.','TailwindCSS, MUI, UI Design','Intermediate',3,'https://mui.com'),
(2,4,'Học Next.js & tối ưu SEO','Xây dựng SSR app và tối ưu hiệu suất.','Next.js, SEO, Routing','Advanced',4,'https://nextjs.org');

-- Career Mapping (RIASEC)
INSERT INTO "CareerMapping" ("RiasecType", "CareerFieldId") VALUES
('R',6),('I',1),('A',5),('S',4),('E',2),('C',2);

-- ========================================
-- QUESTION TEST (58 câu)
-- ========================================

-- Realistic
INSERT INTO "QuestionTest" ("Content","QuestionType") VALUES
('Tôi tự thấy mình là người khá về các môn thể thao', 'Realistic'),
('Tôi là người yêu thích thiên nhiên', 'Realistic'),
('Tôi là người độc lập', 'Realistic'),
('Tôi thích sửa chữa đồ vật, vật dụng xung quanh tôi', 'Realistic'),
('Tôi thích làm việc sử dụng tay chân (làm vườn, sửa chữa nhà cửa)', 'Realistic'),
('Tôi thích tập thể dục', 'Realistic'),
('Tôi thích làm việc cho đến khi công việc hoàn thành', 'Realistic'),
('Tôi thích làm việc một mình', 'Realistic'),
('Tôi chơi các môn thể thao có tính đồng đội', 'Realistic'),
('Tôi thích mạo hiểm và tham gia phiêu lưu', 'Realistic');

-- Investigative
INSERT INTO "QuestionTest" ("Content","QuestionType") VALUES
('Tôi tò mò về thế giới xung quanh', 'Investigative'),
('Tôi rất hay để ý tới chi tiết và cẩn thận', 'Investigative'),
('Tôi có thể tính toán phức tạp', 'Investigative'),
('Tôi thích giải toán', 'Investigative'),
('Tôi thích sử dụng máy tính', 'Investigative'),
('Tôi thích các môn khoa học', 'Investigative'),
('Tôi thích thách thức', 'Investigative'),
('Tôi thích đọc sách', 'Investigative'),
('Tôi thích sưu tầm đồ vật', 'Investigative'),
('Tôi thích trò ô chữ', 'Investigative'),
('Tôi thích nghiên cứu', 'Investigative');

-- Artistic
INSERT INTO "QuestionTest" ("Content","QuestionType") VALUES
('Tôi rất sáng tạo', 'Artistic'),
('Tôi thích vẽ, tô màu và sơn', 'Artistic'),
('Tôi có thể chơi nhạc cụ', 'Artistic'),
('Tôi thích thời trang độc đáo', 'Artistic'),
('Tôi thích đọc truyện, kịch và thơ ca', 'Artistic'),
('Tôi thích mỹ thuật và thủ công', 'Artistic'),
('Tôi xem nhiều phim', 'Artistic'),
('Tôi thích chụp ảnh', 'Artistic'),
('Tôi thích học ngoại ngữ', 'Artistic'),
('Tôi thích hát, đóng kịch, khiêu vũ', 'Artistic');

-- Social
INSERT INTO "QuestionTest" ("Content","QuestionType") VALUES
('Tôi rất thân thiện', 'Social'),
('Tôi thích chỉ dẫn hoặc dạy người khác', 'Social'),
('Tôi thích nói chuyện trước đám đông', 'Social'),
('Tôi làm việc tốt trong nhóm', 'Social'),
('Tôi thích điều hành thảo luận', 'Social'),
('Tôi thích giúp đỡ người khó khăn', 'Social'),
('Tôi thích dự tiệc', 'Social'),
('Tôi thích làm quen bạn mới', 'Social'),
('Tôi thích tham gia nhóm cộng đồng', 'Social');

-- Enterprising
INSERT INTO "QuestionTest" ("Content","QuestionType") VALUES
('Tôi thích học hỏi về tài chính', 'Enterprising'),
('Tôi thích bán hàng', 'Enterprising'),
('Tôi khá nổi tiếng ở trường', 'Enterprising'),
('Tôi thích lãnh đạo nhóm', 'Enterprising'),
('Tôi thích được bầu vị trí quan trọng', 'Enterprising'),
('Tôi thích có quyền lực', 'Enterprising'),
('Tôi muốn sở hữu doanh nghiệp nhỏ', 'Enterprising');

-- Conventional
INSERT INTO "QuestionTest" ("Content","QuestionType") VALUES
('Tôi thích dành dụm tiền', 'Conventional'),
('Tôi thích gọn gàng', 'Conventional'),
('Tôi thích phòng ngăn nắp', 'Conventional'),
('Tôi thích sưu tầm báo', 'Conventional'),
('Tôi thích lập danh sách công việc', 'Conventional'),
('Tôi thích sử dụng máy tính', 'Conventional'),
('Tôi cân nhắc chi phí kỹ lưỡng', 'Conventional'),
('Tôi thích đánh máy hơn viết tay', 'Conventional'),
('Tôi thích làm thư ký', 'Conventional'),
('Khi làm toán tôi kiểm tra lại nhiều lần', 'Conventional'),
('Tôi thích viết thư', 'Conventional');

--Blogs
INSERT INTO "Blogs" ("AuthorId", "Title", "Content", "UpdatedAt")
VALUES
(1, 'Cách chăm sóc cá Koi mùa mưa', 
 'Mùa mưa là thời điểm cá Koi dễ bị stress do thay đổi nhiệt độ và chất lượng nước. Hãy kiểm tra pH, nhiệt độ và oxy thường xuyên để đảm bảo môi trường ổn định.', NULL),
(1, 'Top 5 loại thức ăn tốt nhất cho cá Koi', 
 'Thức ăn có hàm lượng protein cao, dễ tiêu hóa và chứa đầy đủ vitamin là lựa chọn lý tưởng cho cá Koi. Một số thương hiệu nổi bật gồm Hikari, Aqua Master, và JPD.', NULL),
(1, 'Cách nhận biết cá Koi bị bệnh nấm', 
 'Dấu hiệu thường gặp là xuất hiện các đốm trắng, tróc vảy, hoặc bơi lờ đờ. Khi phát hiện, cần cách ly cá bệnh và sử dụng thuốc diệt nấm chuyên dụng.', NULL),
(1, 'Hướng dẫn thiết kế hồ Koi đúng chuẩn phong thủy', 
 'Một hồ Koi đạt chuẩn nên có hình dạng uốn lượn tự nhiên, không có góc nhọn, và hướng nước chảy nhẹ nhàng để mang lại may mắn và thịnh vượng.', NULL),
(1, 'Cách duy trì màu sắc rực rỡ cho cá Koi', 
 'Để cá Koi luôn có màu sắc đẹp, cần cung cấp ánh sáng tự nhiên đủ, chế độ ăn phù hợp và thay nước định kỳ để loại bỏ tạp chất trong hồ.', NULL);
