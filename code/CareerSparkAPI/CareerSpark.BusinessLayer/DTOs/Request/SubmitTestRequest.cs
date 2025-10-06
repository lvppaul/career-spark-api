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
        public int SessionId { get; set; }
        public List<SubmitAnswerDto> Answers { get; set; }
    }
}
