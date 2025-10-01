using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerSpark.BusinessLayer.DTOs.Response
{
    public class StartTestResponse
    {
        public int SessionId { get; set; }
        public DateTime StartAt { get; set; }
    }
}
