using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerSpark.BusinessLayer.DTOs.Request
{
    public class CareerPathDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<CareerMilestoneDto> Milestones { get; set; }
    }
}
