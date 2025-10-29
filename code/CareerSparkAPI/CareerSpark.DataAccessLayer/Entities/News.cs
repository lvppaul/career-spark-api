using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerSpark.DataAccessLayer.Entities
{
    public partial class News
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Tag { get; set; } 
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }
        public string? avatarPublicId { get; set; }
    }
}
