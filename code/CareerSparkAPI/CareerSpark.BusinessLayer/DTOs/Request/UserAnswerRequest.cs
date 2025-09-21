using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerSpark.BusinessLayer.DTOs.Request
{
    public class UserAnswerRequest
    {
        public int QuestionId { get; set; }
        public bool IsSelected { get; set; }
    }
}
