using System;
using System.Collections.Generic;

namespace CareerSpark.DataAccessLayer.Entities;

public partial class TestSession
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime? StartAt { get; set; }


    public virtual ICollection<Result> Results { get; set; } = new List<Result>();

    public virtual ICollection<TestAnswer> TestAnswers { get; set; } = new List<TestAnswer>();

    public virtual ICollection<TestHistory> TestHistories { get; set; } = new List<TestHistory>();

    public virtual User User { get; set; } = null!;
}
