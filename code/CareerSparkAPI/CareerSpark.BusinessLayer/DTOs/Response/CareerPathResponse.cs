using CareerSpark.BusinessLayer.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerSpark.BusinessLayer.DTOs.Response
{
    public class CareerPathResponse
    {
        public string CareerField { get; set; }
        public List<CareerPathDto> Paths { get; set; }

    }
}
