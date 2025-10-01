using System;
using System.Collections.Generic;

namespace CareerSpark.DataAccessLayer.Entities;

public partial class CareerMapping
{
    public int Id { get; set; }

    public string RiasecType { get; set; } = null!;

    public int CareerFieldId { get; set; }

    public virtual CareerField CareerField { get; set; } = null!;
}
