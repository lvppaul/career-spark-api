using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerSpark.BusinessLayer.DTOs.Response
{
    public class TestResultResponse
    {
        public int R { get; set; }
        public int I { get; set; }
        public int A { get; set; }
        public int S { get; set; }
        public int E { get; set; }
        public int C { get; set; }

        public double R_Normalized { get; set; }
        public double I_Normalized { get; set; }
        public double A_Normalized { get; set; }
        public double S_Normalized { get; set; }
        public double E_Normalized { get; set; }
        public double C_Normalized { get; set; } 
    }
}
