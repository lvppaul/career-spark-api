-- ========================================
-- SEED DATA FOR CareerSparkDB (PostgreSQL)
-- ========================================

-- Roles
INSERT INTO "Role" ("RoleName") VALUES 
('Admin'), ('Moderator'), ('User');

INSERT INTO "public"."User" ("Id", "Name", "Phone", "Email", "Password", "avatarURL","avatarPublicId", "RefreshToken", "ExpiredRefreshTokenAt", "IsActive", "IsVerified","SecurityStamp", "CreatedAt", "RoleId")
VALUES ('1', 'Phát Lê Vĩnh', null, 'levinhphat790@gmail.com', null, null, null, null, null, 'true', 'false', '1478b4c6-b518-42d4-a14e-054db39b36r6', '2025-10-06 17:39:57.748498', '2');

-- Subscription Plans
INSERT INTO "SubscriptionPlan" ("Name", "Price", "DurationDays", "Description", "Benefits", "Level", "IsActive") VALUES
('Monthly Plan', 30000, 30, 'Gói đăng ký theo tháng – linh hoạt, tiết kiệm cho người dùng ngắn hạn', 'Hỗ trợ qua email, Không quảng cáo, Truy cập nội dung tiêu chuẩn' , 1, TRUE),
('Quarterly Plan', 99000, 90, 'Gói đăng ký 3 tháng – tối ưu cho người dùng thường xuyên, nhiều tiện ích hơn', 'Hỗ trợ qua email và chat trực tiếp, Không quảng cáo, ChatAI(giới hạn lượt chat)', 2, TRUE),
('Yearly Plan', 299000, 365, 'Gói đăng ký 1 năm – đầy đủ quyền lợi cao cấp và trải nghiệm tối đa', 'Hỗ trợ 24/7 qua email, chat và điện thoại, Không quảng cáo, chatAI, Ưu tiên trải nghiệm tính năng beta, Xem nội dung 4K Ultra HD', 3, TRUE);

-- Career Fields
INSERT INTO "CareerField" ("Name", "Description") VALUES
('Công nghệ thông tin', 'Lĩnh vực phần mềm, hệ thống, trí tuệ nhân tạo'),
('Kinh tế', 'Tài chính, kế toán, quản trị kinh doanh'),
('Y tế & Sức khỏe', 'Ngành y, điều dưỡng, chăm sóc sức khỏe'),
('Giáo dục', 'Dạy học, đào tạo, nghiên cứu'),
('Nghệ thuật & Sáng tạo', 'Mỹ thuật, âm nhạc, thiết kế'),
('Kỹ thuật & Cơ khí', 'Ngành điện, cơ khí, xây dựng');

-- Career Paths
-- ========== 1️ Công nghệ thông tin ==========
INSERT INTO "CareerPath" ("Title", "Description", "CareerFieldId") VALUES
('Lập trình viên Backend', 'Phát triển logic server-side, API, và kết nối cơ sở dữ liệu.', 1),
('Lập trình viên Frontend', 'Xây dựng giao diện web tương tác với người dùng.', 1),
('Kỹ sư DevOps', 'Tự động hóa quy trình triển khai và quản lý hạ tầng.', 1),
('Chuyên viên Kiểm thử phần mềm (QA)', 'Thực hiện kiểm thử phần mềm đảm bảo chất lượng.', 1),
('Kỹ sư Dữ liệu', 'Quản lý, xử lý và phân tích dữ liệu lớn.', 1),
('Chuyên gia Trí tuệ nhân tạo (AI/ML)', 'Nghiên cứu và ứng dụng các thuật toán học máy.', 1),
('Chuyên viên An ninh mạng', 'Phát hiện và phòng chống tấn công mạng.', 1);

-- ========== 2️ Kinh tế ==========
INSERT INTO "CareerPath" ("Title", "Description", "CareerFieldId") VALUES
('Kế toán viên', 'Ghi chép, tổng hợp và phân tích thông tin tài chính doanh nghiệp.', 2),
('Chuyên viên Phân tích tài chính', 'Tư vấn và phân tích dữ liệu tài chính để ra quyết định đầu tư.', 2),
('Chuyên viên Marketing', 'Lên kế hoạch và triển khai chiến dịch quảng bá sản phẩm.', 2),
('Chuyên viên Nhân sự', 'Quản lý tuyển dụng, đào tạo và chế độ nhân sự.', 2),
('Chuyên viên Quản trị kinh doanh', 'Xây dựng chiến lược và quản lý hoạt động kinh doanh.', 2),
('Nhân viên Bán hàng (Sales Executive)', 'Tiếp cận và chăm sóc khách hàng để đạt mục tiêu doanh số.', 2),
('Chuyên viên Đầu tư chứng khoán', 'Phân tích thị trường và đưa ra quyết định đầu tư.', 2);

-- ========== 3️ Y tế & Sức khỏe ==========
INSERT INTO "CareerPath" ("Title", "Description", "CareerFieldId") VALUES
('Bác sĩ đa khoa', 'Khám, chẩn đoán và điều trị bệnh thông thường.', 3),
('Y tá/Điều dưỡng', 'Hỗ trợ bác sĩ trong chăm sóc và điều trị bệnh nhân.', 3),
('Kỹ thuật viên xét nghiệm y học', 'Thực hiện xét nghiệm và phân tích mẫu bệnh phẩm.', 3),
('Dược sĩ', 'Nghiên cứu, pha chế và tư vấn sử dụng thuốc.', 3),
('Bác sĩ chuyên khoa', 'Đi sâu vào lĩnh vực cụ thể như tim mạch, nhi, thần kinh, v.v.', 3),
('Chuyên viên dinh dưỡng', 'Tư vấn chế độ ăn uống và sức khỏe.', 3),
('Nhà nghiên cứu y sinh', 'Nghiên cứu khoa học trong lĩnh vực sinh học và y học.', 3);

-- ========== 4️ Giáo dục ==========
INSERT INTO "CareerPath" ("Title", "Description", "CareerFieldId") VALUES
('Giáo viên Toán', 'Dạy học môn Toán ở các cấp học.', 4),
('Giáo viên Tiếng Anh', 'Giảng dạy tiếng Anh và kỹ năng giao tiếp.', 4),
('Giảng viên Đại học', 'Nghiên cứu và giảng dạy tại các trường đại học.', 4),
('Chuyên viên Tư vấn học đường', 'Hỗ trợ định hướng học tập và tâm lý cho học sinh.', 4),
('Nhà nghiên cứu giáo dục', 'Nghiên cứu phương pháp giảng dạy và cải tiến chương trình học.', 4),
('Giáo viên Mầm non', 'Chăm sóc và giáo dục trẻ nhỏ ở độ tuổi mầm non.', 4),
('Quản lý giáo dục', 'Triển khai, giám sát và đánh giá hoạt động đào tạo.', 4);

-- ========== 5️ Nghệ thuật & Sáng tạo ==========
INSERT INTO "CareerPath" ("Title", "Description", "CareerFieldId") VALUES
('Họa sĩ minh họa', 'Sáng tạo tranh, hình ảnh minh họa cho sách và truyền thông.', 5),
('Nhạc sĩ/Composer', 'Sáng tác và hòa âm phối khí các tác phẩm âm nhạc.', 5),
('Nhà thiết kế đồ họa (Graphic Designer)', 'Thiết kế hình ảnh thương hiệu và truyền thông.', 5),
('Nhiếp ảnh gia', 'Chụp và chỉnh sửa ảnh nghệ thuật hoặc thương mại.', 5),
('Nhà thiết kế thời trang', 'Tạo ra mẫu thiết kế quần áo, phụ kiện.', 5),
('Đạo diễn/Producer', 'Thực hiện và quản lý sản xuất phim, video.', 5),
('Nhà văn/Copywriter', 'Sáng tạo nội dung và câu chuyện truyền cảm hứng.', 5);

-- ========== 6️ Kỹ thuật & Cơ khí ==========
INSERT INTO "CareerPath" ("Title", "Description", "CareerFieldId") VALUES
('Kỹ sư Cơ khí', 'Thiết kế, chế tạo và vận hành máy móc.', 6),
('Kỹ sư Điện', 'Lắp đặt, vận hành và bảo trì hệ thống điện.', 6),
('Kỹ sư Xây dựng', 'Thiết kế và giám sát các công trình dân dụng.', 6),
('Kỹ sư Ô tô', 'Nghiên cứu và phát triển phương tiện giao thông.', 6),
('Kỹ sư Tự động hóa', 'Thiết kế hệ thống điều khiển tự động cho dây chuyền sản xuất.', 6),
('Kỹ sư Robot', 'Phát triển robot phục vụ công nghiệp và đời sống.', 6),
('Kỹ sư Môi trường', 'Nghiên cứu và cải thiện các vấn đề môi trường.', 6);

-- ========================================
-- CAREER ROADMAP: CÔNG NGHỆ THÔNG TIN
-- ========================================

-- Backend Developer
INSERT INTO "CareerRoadmap" 
("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl") VALUES
(1,1,'Học C# cơ bản','Nắm vững cú pháp, OOP và cấu trúc dữ liệu trong C#.','C#, OOP, Data Structures','Beginner',4,'https://learn.microsoft.com/dotnet/csharp'),
(1,2,'Học ASP.NET Core','Xây dựng RESTful API và hiểu middleware.','ASP.NET Core, REST API, Middleware','Intermediate',5,'https://dotnet.microsoft.com/learn/aspnet'),
(1,3,'Sử dụng Entity Framework Core','ORM, migration và mô hình database-first.','EF Core, LINQ, SQL','Intermediate',4,'https://learn.microsoft.com/ef/core'),
(1,4,'Tích hợp Authentication & Authorization','Cấu hình JWT, Identity, và phân quyền API.','JWT, ASP.NET Identity, Claims','Advanced',3,'https://learn.microsoft.com/aspnet/core/security'),
(1,5,'Triển khai với Docker & CI/CD','Tạo pipeline CI/CD và build container.','Docker, GitHub Actions, DevOps','Advanced',4,'https://docs.docker.com');

-- Frontend Developer
INSERT INTO "CareerRoadmap" 
("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl") VALUES
(2,1,'Học HTML, CSS, JavaScript','Nắm vững nền tảng web cơ bản.','HTML, CSS, JavaScript','Beginner',4,'https://developer.mozilla.org'),
(2,2,'Làm quen với React và TypeScript','Phát triển SPA với React và TS.','React, TypeScript, Hooks','Intermediate',6,'https://react.dev/learn'),
(2,3,'Sử dụng UI framework','Tối ưu UI với Tailwind hoặc MUI.','TailwindCSS, MUI, Responsive Design','Intermediate',3,'https://mui.com'),
(2,4,'Kết nối API & Quản lý State','Sử dụng Axios, Redux hoặc Context.','Axios, Redux, ContextAPI','Intermediate',4,'https://redux-toolkit.js.org'),
(2,5,'Tối ưu SEO & Hiệu suất','Học Next.js, lazy loading, caching.','Next.js, SEO, Optimization','Advanced',4,'https://nextjs.org');

-- DevOps Engineer
INSERT INTO "CareerRoadmap" 
("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl") VALUES
(3,1,'Hiểu cơ bản về hệ điều hành & mạng','Nắm kiến thức Linux và mạng máy tính.','Linux, Networking, Shell','Beginner',5,'https://linuxjourney.com'),
(3,2,'Tìm hiểu CI/CD','Tự động hóa build và deploy.','CI/CD, Jenkins, GitHub Actions','Intermediate',4,'https://www.jenkins.io'),
(3,3,'Docker & Containerization','Đóng gói và chạy ứng dụng bằng container.','Docker, Images, Compose','Intermediate',4,'https://docs.docker.com'),
(3,4,'Học Kubernetes','Quản lý container và scaling ứng dụng.','Kubernetes, Pods, Services','Advanced',5,'https://kubernetes.io'),
(3,5,'Monitoring & Logging','Theo dõi ứng dụng bằng Prometheus, Grafana.','Prometheus, Grafana, Logs','Advanced',4,'https://prometheus.io');

-- QA Engineer
INSERT INTO "CareerRoadmap" 
("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl") VALUES
(4,1,'Hiểu quy trình kiểm thử phần mềm','Phân biệt Unit, Integration, System testing.','Testing, SDLC, QA Concepts','Beginner',3,'https://www.guru99.com/software-testing.html'),
(4,2,'Học viết Test Case và Bug Report','Thực hành test thực tế trên dự án.','TestCase, Bug Tracking, Jira','Intermediate',4,'https://www.atlassian.com/software/jira'),
(4,3,'Tự động hóa kiểm thử','Dùng Selenium hoặc Cypress.','Selenium, Cypress, Automation','Intermediate',6,'https://www.selenium.dev'),
(4,4,'Kiểm thử API','Dùng Postman, Newman, và REST Assured.','Postman, API Testing','Advanced',3,'https://www.postman.com'),
(4,5,'Tích hợp kiểm thử CI/CD','Chạy test pipeline trong GitHub Actions.','CI/CD, QA Integration','Advanced',4,'https://docs.github.com/actions');

-- Data Engineer
INSERT INTO "CareerRoadmap" 
("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl") VALUES
(5,1,'Hiểu SQL và xử lý dữ liệu','Làm việc với PostgreSQL, joins, indexes.','SQL, PostgreSQL','Beginner',4,'https://www.sqltutorial.org'),
(5,2,'Sử dụng Python cho xử lý dữ liệu','Dùng Pandas, Numpy để xử lý tập dữ liệu.','Python, Pandas, Numpy','Intermediate',4,'https://pandas.pydata.org'),
(5,3,'ETL Pipeline','Xây dựng luồng dữ liệu với Airflow hoặc Prefect.','ETL, Airflow, Data Flow','Intermediate',5,'https://airflow.apache.org'),
(5,4,'Kho dữ liệu & BI','Dùng Snowflake hoặc Power BI.','Data Warehouse, BI Tools','Advanced',4,'https://powerbi.microsoft.com'),
(5,5,'Data Lake & Big Data','Dùng Spark, Kafka, Hadoop.','Big Data, Spark, Kafka','Advanced',6,'https://spark.apache.org');

-- AI/ML Engineer
INSERT INTO "CareerRoadmap" 
("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl") VALUES
(6,1,'Học Python & Toán cơ bản','Nắm vững NumPy, thống kê, xác suất.','Python, Statistics, Math','Beginner',5,'https://www.kaggle.com/learn/python'),
(6,2,'Học Machine Learning cơ bản','Phân loại, hồi quy, clustering.','Scikit-learn, ML Models','Intermediate',5,'https://scikit-learn.org'),
(6,3,'Deep Learning & Neural Network','Học TensorFlow hoặc PyTorch.','Deep Learning, CNN, RNN','Intermediate',6,'https://www.tensorflow.org'),
(6,4,'Xử lý ngôn ngữ tự nhiên (NLP)','Huấn luyện mô hình text.','NLP, Transformers','Advanced',5,'https://huggingface.co'),
(6,5,'Triển khai mô hình AI','Đưa model lên production.','ML Ops, Docker, API','Advanced',4,'https://mlflow.org');


-- ========================================
-- CAREER ROADMAP: KINH TẾ & QUẢN TRỊ
-- ========================================

-- Kế toán viên
INSERT INTO "CareerRoadmap" 
("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl") VALUES
(8,1,'Học nguyên lý kế toán','Hiểu về tài sản, nợ phải trả, vốn chủ sở hữu.','Accounting Principles, Balance Sheet','Beginner',4,'https://www.coursera.org/learn/financial-accounting-fundamentals'),
(8,2,'Kế toán tài chính','Học quy trình ghi sổ và lập báo cáo.','Financial Accounting, Journal Entries','Intermediate',5,'https://www.edx.org/course/financial-accounting'),
(8,3,'Kế toán quản trị','Phân tích chi phí và lập kế hoạch tài chính.','Managerial Accounting, Cost Control','Intermediate',5,'https://www.coursera.org/learn/managerial-accounting'),
(8,4,'Chuẩn mực kế toán Việt Nam & Quốc tế','Hiểu VAS và IFRS.','VAS, IFRS, Reporting Standards','Advanced',4,'https://ifrs.org'),
(8,5,'Ứng dụng phần mềm kế toán','Sử dụng MISA, Excel nâng cao.','Excel, MISA, Accounting Software','Advanced',3,'https://support.misa.vn');

-- Chuyên viên Phân tích tài chính
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(9,1,'Hiểu nguyên lý tài chính doanh nghiệp','Phân tích dòng tiền, vốn, lợi nhuận.','Corporate Finance, Cash Flow','Beginner',4,'https://www.coursera.org/learn/corporate-finance'),
(9,2,'Học Excel và Google Sheets nâng cao','Phân tích dữ liệu và lập mô hình tài chính.','Excel Modeling, Data Analysis','Intermediate',4,'https://exceljet.net'),
(9,3,'Phân tích báo cáo tài chính','Hiểu các chỉ số tài chính quan trọng.','Financial Ratios, Statements','Intermediate',5,'https://www.investopedia.com'),
(9,4,'Định giá doanh nghiệp','Sử dụng DCF và phương pháp so sánh.','Valuation, DCF, Comparable Analysis','Advanced',5,'https://corporatefinanceinstitute.com'),
(9,5,'Trình bày & tư vấn đầu tư','Viết báo cáo tài chính chuyên nghiệp.','Presentation, Investment Report','Advanced',3,'https://www.linkedin.com/learning/');

-- Chuyên viên Marketing
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(10,1,'Hiểu về Marketing cơ bản','Nắm khái niệm 4P, STP.','Marketing Fundamentals, STP','Beginner',3,'https://www.coursera.org/learn/marketing-introduction'),
(10,2,'Phân tích thị trường & hành vi khách hàng','Thu thập và xử lý dữ liệu thị trường.','Market Research, Consumer Insight','Intermediate',4,'https://www.udemy.com/course/market-research/'),
(10,3,'Digital Marketing','Triển khai quảng cáo online và SEO.','SEO, Ads, Analytics','Intermediate',5,'https://www.google.com/digitalgarage'),
(10,4,'Content & Branding','Xây dựng nội dung và thương hiệu.','Branding, Storytelling, Content','Advanced',4,'https://www.hubspot.com/resources'),
(10,5,'Đo lường hiệu quả chiến dịch','Dùng GA4, KPI, ROI để đánh giá.','Analytics, KPI, ROI','Advanced',3,'https://analytics.google.com');

-- Chuyên viên Nhân sự
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(11,1,'Nắm quy trình tuyển dụng','Tìm hiểu Job Description, sourcing.','Recruitment, Interviewing','Beginner',3,'https://www.linkedin.com/learning/'),
(11,2,'Đào tạo và phát triển nhân viên','Thiết kế chương trình đào tạo.','Learning & Development','Intermediate',4,'https://www.coursera.org/learn/hr-fundamentals'),
(11,3,'Quản lý lương thưởng & phúc lợi','Thiết kế hệ thống C&B.','Compensation & Benefits','Intermediate',4,'https://www.shrm.org/'),
(11,4,'Xử lý quan hệ lao động','Giải quyết tranh chấp & hợp đồng.','Labor Law, Employee Relations','Advanced',3,'https://moj.gov.vn'),
(11,5,'Chiến lược HR tổng thể','Tối ưu nguồn nhân lực.','HR Strategy, KPI Alignment','Advanced',5,'https://www.cipd.org');

-- Quản trị kinh doanh
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(12,1,'Tìm hiểu mô hình kinh doanh','Phân tích Canvas và chuỗi giá trị.','Business Model, Value Chain','Beginner',3,'https://www.strategyzer.com'),
(12,2,'Kỹ năng lãnh đạo & giao tiếp','Phát triển kỹ năng mềm.','Leadership, Communication','Intermediate',4,'https://www.coursera.org/learn/leadership'),
(12,3,'Phân tích SWOT & chiến lược','Lập kế hoạch kinh doanh dài hạn.','SWOT, Strategic Planning','Intermediate',5,'https://hbr.org'),
(12,4,'Quản lý vận hành & chuỗi cung ứng','Tối ưu hóa hiệu quả sản xuất.','Operations, Supply Chain','Advanced',4,'https://www.edx.org/course/supply-chain-management'),
(12,5,'Khởi nghiệp & đổi mới sáng tạo','Thực hành lập business plan.','Entrepreneurship, Innovation','Advanced',4,'https://www.startupschool.org');

-- Chuyên viên Đầu tư chứng khoán
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(13,1,'Hiểu thị trường chứng khoán','Nắm quy tắc hoạt động và thuật ngữ.','Stock Basics, Trading Rules','Beginner',3,'https://www.investopedia.com/terms/s/stockmarket.asp'),
(13,2,'Phân tích kỹ thuật','Học đọc biểu đồ, trendline, RSI.','Technical Analysis, Indicators','Intermediate',5,'https://www.tradingview.com'),
(13,3,'Phân tích cơ bản','Đọc báo cáo tài chính và định giá cổ phiếu.','Fundamental Analysis, Valuation','Intermediate',4,'https://corporatefinanceinstitute.com'),
(13,4,'Tâm lý thị trường & quản lý rủi ro','Kiểm soát cảm xúc đầu tư.','Risk Management, Trading Psychology','Advanced',4,'https://www.udemy.com/course/trading-psychology/'),
(13,5,'Chiến lược đầu tư dài hạn','Xây danh mục và tái cân bằng tài sản.','Portfolio Management, Strategy','Advanced',4,'https://www.morningstar.com');

-- ========================================
-- CAREER ROADMAP: Y TẾ & SỨC KHỎE
-- ========================================

-- Bác sĩ đa khoa
INSERT INTO "CareerRoadmap"
("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl") VALUES
(15,1,'Nắm kiến thức y học cơ bản','Học về giải phẫu, sinh lý, hóa sinh, bệnh học.','Anatomy, Physiology, Biochemistry','Beginner',8,'https://www.khanacademy.org/science/health-and-medicine'),
(15,2,'Học bệnh học và dược lý','Hiểu cơ chế bệnh và thuốc điều trị.','Pathology, Pharmacology','Intermediate',6,'https://www.medscape.com/'),
(15,3,'Chẩn đoán lâm sàng','Học hỏi kỹ năng thăm khám và đặt chẩn đoán.','Clinical Diagnosis, Patient Interview','Intermediate',6,'https://openstax.org/books/anatomy-and-physiology'),
(15,4,'Thực hành y khoa','Tham gia thực tập tại bệnh viện.','Clinical Skills, Procedures','Advanced',12,'https://www.coursera.org/learn/clinical-skills'),
(15,5,'Đạo đức nghề nghiệp và y học cộng đồng','Hiểu vai trò và trách nhiệm xã hội.','Medical Ethics, Public Health','Advanced',4,'https://www.who.int/health-topics');

-- Điều dưỡng viên / Y tá
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(16,1,'Kiến thức nền về điều dưỡng','Tìm hiểu vai trò của điều dưỡng.','Nursing Fundamentals, Anatomy','Beginner',6,'https://www.coursera.org/learn/nursing-fundamentals'),
(16,2,'Kỹ năng chăm sóc bệnh nhân','Học chăm sóc, vệ sinh và hỗ trợ bệnh nhân.','Patient Care, Hygiene, Communication','Intermediate',5,'https://nurse.org/resources/'),
(16,3,'Sơ cứu và cấp cứu cơ bản','Xử lý các tình huống khẩn cấp.','First Aid, CPR, Emergency Care','Intermediate',4,'https://www.redcross.org/take-a-class'),
(16,4,'Sử dụng thiết bị y tế','Thực hành đo huyết áp, ECG, tiêm truyền.','Medical Equipment, Vital Signs','Advanced',5,'https://www.open.edu/openlearn'),
(16,5,'Làm việc nhóm & báo cáo','Giao tiếp và lập hồ sơ y tế.','Teamwork, Nursing Report','Advanced',3,'https://www.nursingworld.org');

-- Kỹ thuật viên xét nghiệm y học
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(17,1,'Cơ sở sinh học & hóa sinh','Nắm nguyên lý phản ứng sinh học.','Biochemistry, Biology','Beginner',5,'https://openstax.org/books/biology-2e'),
(17,2,'Làm việc trong phòng thí nghiệm','Học quy trình thu thập và bảo quản mẫu.','Lab Safety, Sample Collection','Intermediate',4,'https://www.labmanager.com'),
(17,3,'Phân tích mẫu bệnh phẩm','Sử dụng máy xét nghiệm tự động.','Lab Techniques, Automation','Intermediate',5,'https://www.abbott.com'),
(17,4,'Đọc kết quả xét nghiệm','Phân tích kết quả và hỗ trợ chẩn đoán.','Data Interpretation, Quality Control','Advanced',5,'https://www.cdc.gov/lab'),
(17,5,'Kiểm định chất lượng xét nghiệm','Tham gia kiểm chuẩn ISO, QA.','ISO Standards, QA, Calibration','Advanced',4,'https://www.iso.org');

-- Dược sĩ
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(18,1,'Học hóa dược & dược lý','Nắm nguyên lý cấu tạo và tác dụng thuốc.','Pharmacology, Medicinal Chemistry','Beginner',6,'https://www.khanacademy.org/science/health-and-medicine'),
(18,2,'Quy trình sản xuất thuốc','Học GMP và kiểm nghiệm.','GMP, Drug Manufacturing','Intermediate',5,'https://www.who.int/medicines/areas/quality_safety/quality_assurance'),
(18,3,'Phân phối và bảo quản thuốc','Nắm quy định GSP, GDP.','Storage, Supply Chain','Intermediate',4,'https://www.fda.gov'),
(18,4,'Tư vấn sử dụng thuốc','Hướng dẫn bệnh nhân về liều và tác dụng phụ.','Counseling, Pharmacovigilance','Advanced',4,'https://www.ncbi.nlm.nih.gov/books/NBK557408/'),
(18,5,'Cập nhật thuốc mới & nghiên cứu lâm sàng','Theo dõi xu hướng thuốc và thử nghiệm.','Clinical Research, New Drugs','Advanced',5,'https://clinicaltrials.gov');

-- Chuyên viên Dinh dưỡng
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(19,1,'Cơ bản về dinh dưỡng','Hiểu vai trò các nhóm chất dinh dưỡng.','Nutrition, Macronutrients','Beginner',3,'https://www.coursera.org/learn/nutrition'),
(19,2,'Dinh dưỡng cho từng độ tuổi','Thiết kế chế độ ăn theo nhóm tuổi.','Diet Planning, Health Stages','Intermediate',4,'https://www.who.int/nutrition'),
(19,3,'Phân tích tình trạng dinh dưỡng','Đánh giá BMI, cân nặng, thiếu chất.','Health Metrics, Diet Analysis','Intermediate',4,'https://www.nutrition.org'),
(19,4,'Dinh dưỡng điều trị bệnh','Chế độ ăn cho bệnh tim mạch, tiểu đường.','Medical Nutrition Therapy','Advanced',5,'https://www.ncbi.nlm.nih.gov/books/NBK482514/'),
(19,5,'Nghiên cứu và phát triển sản phẩm','Phát triển thực phẩm chức năng.','Food Science, R&D','Advanced',5,'https://ifst.onlinelibrary.wiley.com');

-- Nhà nghiên cứu y sinh
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(20,1,'Kiến thức nền sinh học phân tử','Hiểu DNA, RNA, protein.','Molecular Biology, Genetics','Beginner',6,'https://www.khanacademy.org/science/biology'),
(20,2,'Thực hành phòng thí nghiệm','Thao tác PCR, Western blot.','Lab Techniques, Experiment Design','Intermediate',5,'https://www.addgene.org'),
(20,3,'Phân tích dữ liệu sinh học','Sử dụng công cụ Bioinformatics.','Bioinformatics, R, Python','Intermediate',6,'https://www.edx.org/course/bioinformatics'),
(20,4,'Nghiên cứu và công bố khoa học','Học viết bài báo, thiết kế nghiên cứu.','Research Paper, Statistics','Advanced',5,'https://pubmed.ncbi.nlm.nih.gov'),
(20,5,'Đạo đức và quản lý nghiên cứu','Tuân thủ quy định IRB, đạo đức sinh học.','Research Ethics, IRB','Advanced',4,'https://www.nih.gov');

-- ========================================
-- CAREER ROADMAP: GIÁO DỤC & ĐÀO TẠO
-- ========================================

-- Giáo viên Toán
INSERT INTO "CareerRoadmap"
("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl") VALUES
(21,1,'Ôn tập kiến thức Toán học cơ bản','Nắm vững đại số, hình học, giải tích.','Mathematics, Algebra, Geometry','Beginner',4,'https://www.khanacademy.org/math'),
(21,2,'Kỹ năng sư phạm Toán','Tìm hiểu phương pháp dạy học hiện đại.','Pedagogy, Teaching Methods','Intermediate',4,'https://www.coursera.org/learn/teach-math'),
(21,3,'Ứng dụng công nghệ trong giảng dạy','Sử dụng GeoGebra, Desmos, PowerPoint.','EdTech, GeoGebra, Digital Tools','Intermediate',3,'https://www.geogebra.org/learn'),
(21,4,'Thiết kế đề kiểm tra và đánh giá','Biên soạn bài thi phù hợp năng lực học sinh.','Assessment, Rubric Design','Advanced',4,'https://www.edutopia.org'),
(21,5,'Phát triển năng lực nghiên cứu giáo dục Toán','Học cách phân tích và cải tiến phương pháp dạy.','Research, Education Analysis','Advanced',4,'https://eric.ed.gov');

-- Giáo viên Tiếng Anh
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(22,1,'Củng cố kiến thức ngữ pháp & từ vựng','Nâng cao khả năng ngôn ngữ.','Grammar, Vocabulary','Beginner',4,'https://www.cambridgeenglish.org/learning-english'),
(22,2,'Kỹ năng nghe - nói - đọc - viết','Rèn luyện 4 kỹ năng ngôn ngữ.','Listening, Speaking, Reading, Writing','Intermediate',6,'https://www.bbc.co.uk/learningenglish'),
(22,3,'Phương pháp giảng dạy ESL','Áp dụng kỹ thuật CLT, TBL, PPP.','ESL Teaching, Classroom Management','Intermediate',5,'https://www.tesol.org'),
(22,4,'Đánh giá năng lực người học','Học thiết kế bài kiểm tra & feedback.','Assessment, Evaluation, Feedback','Advanced',4,'https://www.cambridgeenglish.org'),
(22,5,'Luyện chứng chỉ giảng dạy quốc tế','Đạt chứng chỉ TESOL hoặc CELTA.','TESOL, CELTA, Certification','Advanced',6,'https://www.tefl.org');

-- Giảng viên Đại học
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(23,1,'Nắm vững chuyên môn chuyên ngành','Củng cố kiến thức hàn lâm.','Domain Knowledge, Expertise','Beginner',8,'https://www.coursera.org/browse'),
(23,2,'Phát triển kỹ năng sư phạm đại học','Học phương pháp giảng dạy người lớn.','Higher Ed Pedagogy, Adult Learning','Intermediate',5,'https://www.edx.org/course/university-teaching'),
(23,3,'Tham gia nghiên cứu khoa học','Thiết kế nghiên cứu và công bố bài báo.','Research, Academic Writing','Intermediate',6,'https://www.ssrn.com'),
(23,4,'Quản lý lớp học và đánh giá sinh viên','Đánh giá theo năng lực và kết quả đầu ra.','Assessment, Outcome-based Education','Advanced',4,'https://www.qs.com'),
(23,5,'Tham gia hội thảo & phát triển học thuật','Chia sẻ kinh nghiệm nghiên cứu.','Conference, Networking, Mentoring','Advanced',4,'https://www.sciencedirect.com');

-- Chuyên viên Tư vấn học đường
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(24,1,'Tìm hiểu tâm lý học giáo dục','Hiểu hành vi, cảm xúc, động lực học sinh.','Educational Psychology, Counseling Basics','Beginner',5,'https://www.coursera.org/learn/child-development'),
(24,2,'Kỹ năng lắng nghe & giao tiếp','Phát triển kỹ năng tư vấn cá nhân.','Communication, Active Listening','Intermediate',4,'https://www.edx.org/course/counseling-skills'),
(24,3,'Phương pháp hỗ trợ học tập','Định hướng học tập và nghề nghiệp.','Career Guidance, Mentoring','Intermediate',5,'https://www.unesco.org/education'),
(24,4,'Xử lý tình huống và khủng hoảng tâm lý','Đưa ra giải pháp cho học sinh khó khăn.','Crisis Management, Mental Health','Advanced',4,'https://www.apa.org'),
(24,5,'Làm việc với phụ huynh và giáo viên','Phối hợp hỗ trợ học sinh toàn diện.','Parent Collaboration, Teamwork','Advanced',3,'https://positivepsychology.com');

-- Nhà nghiên cứu giáo dục
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(25,1,'Học phương pháp nghiên cứu giáo dục','Phân biệt định tính và định lượng.','Qualitative, Quantitative Methods','Beginner',5,'https://www.coursera.org/learn/educational-research'),
(25,2,'Phân tích dữ liệu khảo sát','Dùng SPSS hoặc R để xử lý dữ liệu.','SPSS, R, Statistics','Intermediate',5,'https://www.datacamp.com'),
(25,3,'Đánh giá chương trình học','Thiết kế thang đo và tiêu chí đánh giá.','Evaluation, Metrics','Intermediate',4,'https://www.edutopia.org'),
(25,4,'Viết bài báo khoa học','Tuân thủ chuẩn APA, trình bày kết quả.','Academic Writing, APA Format','Advanced',4,'https://owl.purdue.edu'),
(25,5,'Công bố và chia sẻ nghiên cứu','Xuất bản tạp chí, hội thảo.','Publication, Presentation','Advanced',4,'https://eric.ed.gov');

-- Quản lý giáo dục
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(26,1,'Hiểu hệ thống giáo dục Việt Nam','Cấu trúc và quy định quản lý.','Education System, Policy','Beginner',4,'https://moet.gov.vn'),
(26,2,'Kỹ năng lãnh đạo trường học','Phát triển tầm nhìn và sứ mệnh.','Leadership, Strategy','Intermediate',4,'https://www.edx.org/course/leadership-in-education'),
(26,3,'Quản lý nhân sự và tài chính giáo dục','Cân đối ngân sách và nhân sự.','Finance, HR Management','Intermediate',4,'https://www.unesco.org'),
(26,4,'Đánh giá chất lượng trường học','Xây dựng tiêu chuẩn và kiểm định.','Quality Assurance, Accreditation','Advanced',5,'https://www.qaa.ac.uk'),
(26,5,'Đổi mới và cải tiến chương trình','Ứng dụng mô hình giáo dục mới.','Curriculum Reform, Innovation','Advanced',4,'https://www.edutopia.org');

-- ========================================
-- CAREER ROADMAP: NGHỆ THUẬT & SÁNG TẠO
-- ========================================

-- Họa sĩ minh họa
INSERT INTO "CareerRoadmap"
("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl") VALUES
(27,1,'Nắm vững nền tảng mỹ thuật','Học bố cục, phối màu, và hình khối.','Art Fundamentals, Composition, Color Theory','Beginner',4,'https://drawabox.com/'),
(27,2,'Thực hành minh họa cơ bản','Tập vẽ nhân vật, phong cảnh, vật thể.','Sketching, Illustration','Intermediate',5,'https://www.skillshare.com/en/browse/illustration'),
(27,3,'Sử dụng phần mềm đồ họa','Làm quen với Photoshop, Procreate.','Photoshop, Digital Painting','Intermediate',4,'https://helpx.adobe.com/photoshop'),
(27,4,'Phát triển phong cách cá nhân','Tìm phong cách riêng, luyện portfolio.','Style Development, Creativity','Advanced',5,'https://artstation.com'),
(27,5,'Thương mại hóa tác phẩm','Bán tranh và nhận dự án freelance.','Freelancing, Art Business','Advanced',4,'https://www.behance.net');

-- Nhạc sĩ / Nhà soạn nhạc
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(28,1,'Học nhạc lý cơ bản','Hiểu hòa âm, giai điệu, nhịp điệu.','Music Theory, Harmony','Beginner',4,'https://www.musictheory.net'),
(28,2,'Học sáng tác và phối khí','Thực hành viết giai điệu, hợp âm.','Composition, Arrangement','Intermediate',5,'https://www.berklee.edu/online'),
(28,3,'Sử dụng phần mềm âm nhạc','Dùng FL Studio, Ableton, Logic Pro.','DAW, MIDI, Mixing','Intermediate',5,'https://www.coursera.org/learn/music-production'),
(28,4,'Thu âm và xử lý hậu kỳ','Học kỹ thuật thu âm chuyên nghiệp.','Recording, Mixing, Mastering','Advanced',5,'https://www.soundonsound.com'),
(28,5,'Phát hành và quảng bá âm nhạc','Đưa sản phẩm lên Spotify, YouTube.','Music Distribution, Marketing','Advanced',4,'https://distrokid.com');

-- Nhà thiết kế đồ họa (Graphic Designer)
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(29,1,'Cơ bản về thiết kế đồ họa','Hiểu nguyên lý thiết kế và bố cục.','Design Principles, Layout','Beginner',4,'https://www.coursera.org/learn/graphic-design'),
(29,2,'Học Photoshop và Illustrator','Thực hành thiết kế thương hiệu.','Adobe Photoshop, Illustrator','Intermediate',5,'https://helpx.adobe.com/illustrator'),
(29,3,'Thiết kế UI/UX','Tập trung trải nghiệm người dùng.','UI Design, UX Research','Intermediate',5,'https://www.figma.com/resources/learn-design/'),
(29,4,'Brand Identity & Marketing','Phát triển hệ thống nhận diện thương hiệu.','Branding, Visual Communication','Advanced',4,'https://www.behance.net'),
(29,5,'Portfolio & Freelance','Tạo hồ sơ cá nhân và chào khách hàng.','Portfolio, Freelance Business','Advanced',4,'https://www.dribbble.com');

-- Nhiếp ảnh gia
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(30,1,'Cơ bản về nhiếp ảnh','Hiểu khẩu độ, tốc độ, ISO.','Photography Basics, Exposure','Beginner',4,'https://photographycourse.net'),
(30,2,'Thực hành chụp ảnh chân dung và phong cảnh','Tập chụp bố cục và ánh sáng.','Portrait, Landscape, Lighting','Intermediate',5,'https://digital-photography-school.com'),
(30,3,'Chỉnh sửa ảnh chuyên nghiệp','Dùng Lightroom, Photoshop.','Photo Editing, Retouching','Intermediate',4,'https://helpx.adobe.com/lightroom'),
(30,4,'Nhiếp ảnh thương mại & sự kiện','Chụp sản phẩm, cưới, quảng cáo.','Commercial, Event Photography','Advanced',5,'https://www.creativelive.com'),
(30,5,'Xây dựng thương hiệu nhiếp ảnh','Tạo portfolio và quảng bá cá nhân.','Portfolio, Marketing','Advanced',4,'https://www.pixpa.com');

-- Nhà thiết kế thời trang
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(31,1,'Cơ bản về thời trang','Hiểu lịch sử và xu hướng thời trang.','Fashion Basics, History','Beginner',4,'https://www.coursera.org/learn/fashion-design'),
(31,2,'Vẽ phác thảo thời trang','Thực hành vẽ mẫu quần áo.','Sketching, Fashion Illustration','Intermediate',4,'https://www.skillshare.com/en/browse/fashion'),
(31,3,'Cắt may và dựng mẫu','Học pattern, draping, sewing.','Pattern Making, Sewing','Intermediate',6,'https://www.udemy.com/course/sewing-for-beginners/'),
(31,4,'Thiết kế bộ sưu tập','Lên ý tưởng, chọn chất liệu, màu sắc.','Collection Design, Fabric Selection','Advanced',5,'https://www.parsons.edu'),
(31,5,'Trình diễn và thương mại hóa','Tổ chức show và bán sản phẩm.','Fashion Show, Branding','Advanced',4,'https://www.vogue.com');

-- Đạo diễn / Nhà sản xuất
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(32,1,'Hiểu quy trình sản xuất phim','Từ kịch bản đến hậu kỳ.','Film Production, Script Writing','Beginner',4,'https://www.masterclass.com/film'),
(32,2,'Kịch bản và dựng cảnh','Phân cảnh, storyboard, casting.','Screenwriting, Storyboard','Intermediate',5,'https://www.futurelearn.com'),
(32,3,'Quay phim và ánh sáng','Học quay bằng máy DSLR hoặc Cinema Camera.','Cinematography, Lighting','Intermediate',5,'https://www.skillshare.com/en/browse/filmmaking'),
(32,4,'Dựng phim hậu kỳ','Sử dụng Premiere, DaVinci Resolve.','Video Editing, Color Grading','Advanced',5,'https://helpx.adobe.com/premiere-pro'),
(32,5,'Quản lý sản xuất & phát hành','Quản lý ngân sách, lịch quay, truyền thông.','Production Management, Distribution','Advanced',4,'https://www.studiobinder.com');

-- Nhà văn / Copywriter
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(33,1,'Kỹ năng viết sáng tạo','Hiểu cấu trúc câu chuyện và nhân vật.','Creative Writing, Storytelling','Beginner',4,'https://www.coursera.org/learn/creative-writing'),
(33,2,'Luyện viết blog và nội dung online','Học viết cho web, SEO.','Content Writing, SEO','Intermediate',4,'https://www.hubspot.com/resources'),
(33,3,'Copywriting cho quảng cáo','Tối ưu ngôn từ để bán hàng.','Copywriting, Branding','Intermediate',4,'https://www.copyblogger.com'),
(33,4,'Biên tập và xuất bản','Học chỉnh sửa và làm việc với nhà xuất bản.','Editing, Publishing','Advanced',4,'https://www.wattpad.com'),
(33,5,'Xây dựng thương hiệu cá nhân','Quảng bá bản thân như một tác giả.','Personal Branding, Marketing','Advanced',3,'https://www.linkedin.com/learning');

-- ========================================
-- CAREER ROADMAP: KỸ THUẬT & CƠ KHÍ
-- ========================================

-- Kỹ sư Cơ khí
INSERT INTO "CareerRoadmap"
("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl") VALUES
(34,1,'Học cơ học và vật liệu cơ bản','Nắm vững tĩnh học, động lực học, sức bền vật liệu.','Statics, Dynamics, Material Strength','Beginner',5,'https://www.coursera.org/learn/mechanics'),
(34,2,'Thiết kế kỹ thuật với CAD','Sử dụng AutoCAD, SolidWorks.','CAD, SolidWorks, 3D Modeling','Intermediate',5,'https://www.solidworks.com/learn'),
(34,3,'Gia công và chế tạo cơ khí','Thực hành tiện, phay, hàn, in 3D.','Machining, Welding, 3D Printing','Intermediate',5,'https://www.udemy.com/course/machining-fundamentals/'),
(34,4,'Phân tích phần tử hữu hạn (FEA)','Sử dụng phần mềm ANSYS để mô phỏng.','FEA, Simulation, Stress Analysis','Advanced',4,'https://www.ansys.com'),
(34,5,'Tối ưu hóa sản xuất','Ứng dụng Lean Manufacturing.','Optimization, Lean, Six Sigma','Advanced',4,'https://www.edx.org/course/six-sigma');

-- Kỹ sư Điện
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(35,1,'Nắm kiến thức điện cơ bản','Hiểu dòng điện, mạch điện, Ohm, Kirchhoff.','Electrical Fundamentals, Circuits','Beginner',4,'https://www.khanacademy.org/science/electrical-engineering'),
(35,2,'Thiết kế mạch điện tử','Dùng Proteus, Multisim để mô phỏng.','Electronics, Simulation, PCB Design','Intermediate',5,'https://www.tinkercad.com'),
(35,3,'Học hệ thống điện công nghiệp','Vận hành máy biến áp, động cơ.','Power Systems, Motors','Intermediate',5,'https://www.coursera.org/learn/electrical-power-systems'),
(35,4,'Tự động hóa và điều khiển PLC','Lập trình PLC Siemens, Omron.','PLC, Control Systems','Advanced',4,'https://new.siemens.com/global/en/products/automation.html'),
(35,5,'Bảo trì và an toàn điện','Tuân thủ tiêu chuẩn IEC, OSHA.','Electrical Safety, Maintenance','Advanced',3,'https://www.osha.gov');

-- Kỹ sư Xây dựng
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(36,1,'Học vẽ kỹ thuật & AutoCAD','Tạo bản vẽ kỹ thuật công trình.','AutoCAD, Technical Drawing','Beginner',4,'https://www.autodesk.com/learn'),
(36,2,'Cấu trúc & vật liệu xây dựng','Hiểu các loại vật liệu và cấu kiện.','Construction Materials, Structural Engineering','Intermediate',4,'https://www.edx.org/course/construction-materials'),
(36,3,'Thiết kế công trình dân dụng','Dùng Revit, SAP2000, Etabs.','Civil Design, Revit, SAP2000','Intermediate',6,'https://www.autodesk.com/solutions/revit'),
(36,4,'Giám sát thi công & an toàn lao động','Theo dõi tiến độ, chất lượng.','Site Supervision, Safety','Advanced',5,'https://www.coursera.org/learn/construction-management'),
(36,5,'Quản lý dự án & chi phí xây dựng','Lập kế hoạch tài chính công trình.','Project Management, Cost Estimation','Advanced',4,'https://www.pmi.org');

-- Kỹ sư Ô tô
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(37,1,'Nguyên lý cơ khí và động cơ','Nắm cấu trúc động cơ đốt trong.','Mechanical Engineering, Engine Design','Beginner',5,'https://www.edx.org/course/introduction-to-mechanical-engineering'),
(37,2,'Hệ thống truyền động & phanh','Phân tích hệ thống truyền lực.','Transmission, Brake System','Intermediate',5,'https://www.coursera.org/learn/vehicle-dynamics'),
(37,3,'Điện - điện tử ô tô','Tìm hiểu cảm biến, ECU, CAN Bus.','Automotive Electronics, Sensors','Intermediate',5,'https://www.sae.org/learn'),
(37,4,'Chuẩn đoán và bảo trì','Thực hành kiểm tra xe bằng OBD-II.','Diagnostics, Maintenance','Advanced',4,'https://www.udemy.com/course/car-maintenance/'),
(37,5,'Xe điện & xu hướng tương lai','Nghiên cứu EV, Hybrid, tự lái.','EV, Battery, Autonomous Systems','Advanced',5,'https://www.tesla.com/');

-- Kỹ sư Tự động hóa
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(38,1,'Hiểu cơ bản điều khiển tự động','Nắm nguyên lý phản hồi, cảm biến.','Automation, Control Systems','Beginner',4,'https://www.coursera.org/learn/automatic-control'),
(38,2,'Lập trình PLC cơ bản','Học Ladder Diagram và HMI.','PLC, HMI, SCADA','Intermediate',5,'https://www.siemens.com/global/en/products/automation.html'),
(38,3,'Kỹ thuật đo lường & cảm biến','Sử dụng cảm biến đo áp suất, nhiệt độ.','Sensors, Measurement','Intermediate',4,'https://www.ni.com'),
(38,4,'Robot công nghiệp','Điều khiển cánh tay robot, băng chuyền.','Robotics, Motion Control','Advanced',6,'https://www.roboticseducation.org'),
(38,5,'Tích hợp IoT & SCADA','Giám sát hệ thống tự động từ xa.','IoT, SCADA, Networking','Advanced',4,'https://www.iotforall.com');

-- Kỹ sư Môi trường
INSERT INTO "CareerRoadmap" ("CareerPathId","StepOrder","Title","Description","SkillFocus","DifficultyLevel","DurationWeeks","SuggestedCourseUrl")  VALUES
(39,1,'Hiểu nền tảng môi trường','Học về sinh thái, nước, khí, đất.','Environmental Science, Ecology','Beginner',4,'https://www.coursera.org/learn/environmental-science'),
(39,2,'Quan trắc và thu thập dữ liệu','Thực hiện đo chất lượng nước và không khí.','Data Collection, Monitoring','Intermediate',4,'https://www.epa.gov'),
(39,3,'Xử lý nước và chất thải','Áp dụng công nghệ lọc và xử lý.','Water Treatment, Waste Management','Intermediate',5,'https://www.who.int/water_sanitation_health'),
(39,4,'Đánh giá tác động môi trường (ĐTM)','Phân tích và báo cáo rủi ro môi trường.','EIA, Sustainability, Risk Assessment','Advanced',5,'https://www.unep.org'),
(39,5,'Quản lý và phát triển bền vững','Lập kế hoạch bảo vệ môi trường.','Sustainability, Policy','Advanced',4,'https://sdgs.un.org');


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
