-- =========================================
-- INSERT DATA FOR QuestionTest (RIASEC Model)
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
GO

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
(N'Tôi thích sưu tập (đá, tem, tiền đồng)', 'Investigative'),
(N'Tôi thích trò ô chữ', 'Investigative');
GO

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
GO

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
GO

-- Enterprising (E – Quản lý, Kinh doanh, Lãnh đạo)
INSERT INTO QuestionTest (Content, QuestionType) VALUES
(N'Tôi thích học hỏi về tài chính (tiền bạc)', 'Enterprising'),
(N'Tôi thích bán các sản phẩm, bán hàng online', 'Enterprising'),
(N'Tôi nghĩ mình thuộc dạng nổi tiếng ở trường', 'Enterprising'),
(N'Tôi thích lãnh đạo nhóm và các cuộc thảo luận', 'Enterprising'),
(N'Tôi thích được bầu vào các vị trí quan trọng trong nhóm hoặc câu lạc bộ trong và ngoài nhà trường', 'Enterprising'),
(N'Tôi thích có quyền và thích ở vị trí lãnh đạo', 'Enterprising'),
(N'Tôi muốn sở hữu một cửa hàng, quán ăn/uống, một doanh nghiệp nhỏ', 'Enterprising');
GO

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
GO
