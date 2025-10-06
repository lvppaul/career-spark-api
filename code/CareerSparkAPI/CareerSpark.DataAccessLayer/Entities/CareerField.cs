using System;
using System.Collections.Generic;

namespace CareerSpark.DataAccessLayer.Entities;

public partial class CareerField
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<CareerMapping> CareerMappings { get; set; } = new List<CareerMapping>();

    public virtual ICollection<CareerPath> CareerPaths { get; set; } = new List<CareerPath>();
}
