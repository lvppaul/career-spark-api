using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerSpark.BusinessLayer.DTOs.Request
{
    public class CareerRoadmapDto
    {
        public int Id { get; set; }
        public int StepOrder { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? SkillFocus { get; set; }
        public string? DifficultyLevel { get; set; }
        public int? DurationWeeks { get; set; }
        public string? SuggestedCourseUrl { get; set; }
    }
}
