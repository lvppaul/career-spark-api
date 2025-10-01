using System;
using System.Collections.Generic;

namespace CareerSpark.DataAccessLayer.Entities;

public partial class TestAnswer
{
    public int Id { get; set; }

    public bool? IsSelected { get; set; }

    public int QuestionId { get; set; }

    public int TestSessionId { get; set; }

    public virtual QuestionTest Question { get; set; } = null!;

    public virtual ICollection<TestHistory> TestHistories { get; set; } = new List<TestHistory>();

    public virtual TestSession TestSession { get; set; } = null!;
}
