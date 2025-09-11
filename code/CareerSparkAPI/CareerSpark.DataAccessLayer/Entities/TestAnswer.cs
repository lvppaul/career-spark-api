using System;
using System.Collections.Generic;

namespace CareerSpark.DataAccessLayer.Entities;

public partial class TestAnswer
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public bool? IsSelected { get; set; }

    public int QuestionId { get; set; }

    public virtual QuestionTest Question { get; set; } = null!;

    public virtual ICollection<TestHistory> TestHistories { get; set; } = new List<TestHistory>();
}
