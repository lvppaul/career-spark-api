using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerSpark.BusinessLayer.DTOs.Request
{
    public class SubmitTestRequest
    {
        public int UserId { get; set; }
        public List<UserAnswerRequest> Answers { get; set; } = new();
    }
}
