using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerSpark.BusinessLayer.DTOs.Response
{
    public class TestHistoryAnswerDto
    {
        public int QuestionId { get; set; }
        public string QuestionContent { get; set; }
        public string QuestionType { get; set; }
        public bool IsSelected { get; set; }
    }
}
