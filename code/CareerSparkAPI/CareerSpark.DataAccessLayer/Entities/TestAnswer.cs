using System;
using System.Collections.Generic;

namespace CareerSpark.DataAccessLayer.Entities;

public partial class TestAnswer
{
    public int Id { get; set; }
    public bool IsSelected { get; set; } = false;
    public int QuestionId { get; set; }
    public int TestSessionId { get; set; }

    // Navigation
    public QuestionTest Question { get; set; } = null!;
    public TestSession TestSession { get; set; } = null!;
    public ICollection<TestHistory> TestHistories { get; set; } = new List<TestHistory>();
}
