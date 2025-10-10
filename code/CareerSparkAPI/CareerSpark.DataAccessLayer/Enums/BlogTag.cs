using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerSpark.DataAccessLayer.Enums
{
   
        /// <summary>
        /// Danh sách tag dùng để phân loại các bài viết Blog
        /// trong hệ thống CareerSpark (hướng nghiệp, kỹ năng, việc làm, phát triển bản thân,...)
        /// </summary>
    public enum BlogTag
    {
        //  Định hướng nghề nghiệp
        [Description("Định hướng nghề nghiệp")]
        HuongNghiep,

        [Description("Tổng quan ngành nghề")]
        TongQuanNganh,

        [Description("Lộ trình nghề nghiệp")]
        LoTrinhNgheNghiep,

        [Description("Xu hướng nghề nghiệp")]
        XuHuongNghe,

        [Description("Môi trường làm việc")]
        MoiTruongLamViec,

        //  Học tập & Kỹ năng
        [Description("Lộ trình học tập")]
        LoTrinhHocTap,

        [Description("Kỹ năng mềm")]
        KyNangMem,

        [Description("Kỹ năng chuyên môn")]
        KyNangChuyenMon,

        [Description("Giao tiếp")]
        GiaoTiep,

        [Description("Giải quyết vấn đề")]
        GiaiQuyetVanDe,

        //  Việc làm & Kinh nghiệm
        [Description("Tìm việc làm")]
        TimViec,

        [Description("Phỏng vấn")]
        PhongVan,

        [Description("Viết CV & Hồ sơ")]
        VietCV,

        [Description("Thực tập & Cơ hội nghề nghiệp")]
        ThucTap,

        [Description("Kinh nghiệm làm việc thực tế")]
        KinhNghiemLamViec,

        //  Phát triển bản thân
        [Description("Động lực & Cảm hứng")]
        DongLuc,

        [Description("Hiệu suất làm việc")]
        HieuSuat,

        [Description("Kỹ năng lãnh đạo")]
        LanhDao,

        [Description("Quản lý thời gian")]
        QuanLyThoiGian,

        [Description("Tư duy phát triển bản thân")]
        TuDuyPhatTrien,

        //  Công nghệ & Tương lai nghề nghiệp
        [Description("Công nghệ & Trí tuệ nhân tạo")]
        CongNgheAI,

        [Description("Dữ liệu & Phân tích")]
        DuLieuVaPhanTich,

        [Description("Làm việc từ xa")]
        LamViecTuXa,

        [Description("Freelancer & Nghề tự do")]
        Freelancer,

        [Description("Tương lai nghề nghiệp")]
        TuongLaiNgheNghiep
    }
}

