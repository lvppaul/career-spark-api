using System;
using System.Collections.Generic;

namespace CareerSpark.DataAccessLayer.Entities;

public partial class Result
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public int R { get; set; }
    public int I { get; set; }
    public int A { get; set; }
    public int S { get; set; }
    public int E { get; set; }
    public int C { get; set; }
    public int TestSessionId { get; set; }

    // Navigation
    public TestSession TestSession { get; set; } = null!;
    public ICollection<TestHistory> TestHistories { get; set; } = new List<TestHistory>();
}
