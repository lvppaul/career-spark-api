using System;
using System.Collections.Generic;

namespace CareerSpark.DataAccessLayer.Entities;

public partial class CareerMilestone
{
    public int Id { get; set; }

    public int CareerPathId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int StepOrder { get; set; }

    public string? SuggestedCourseUrl { get; set; }

    public virtual CareerPath CareerPath { get; set; } = null!;
}
