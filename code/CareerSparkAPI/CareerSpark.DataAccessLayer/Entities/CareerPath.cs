using System;
using System.Collections.Generic;

namespace CareerSpark.DataAccessLayer.Entities;

public partial class CareerPath
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int CareerFieldId { get; set; }

    public virtual CareerField CareerField { get; set; } = null!;

    public virtual ICollection<CareerRoadmap> CareerRoadmaps { get; set; } = new List<CareerRoadmap>();
}
