using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerSpark.DataAccessLayer.Entities
{
    public class TestSession
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime StartAt { get; set; } = DateTime.Now;
        public DateTime? EndAt { get; set; }

        // Navigation
        public User User { get; set; } = null!;
        public ICollection<TestAnswer> TestAnswers { get; set; } = new List<TestAnswer>();
        public ICollection<Result> Results { get; set; } = new List<Result>();
        public ICollection<TestHistory> TestHistories { get; set; } = new List<TestHistory>();
    }
}
