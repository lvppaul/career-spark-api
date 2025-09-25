using System;
using System.Collections.Generic;

namespace CareerSpark.DataAccessLayer.Entities;

public partial class QuestionTest
{
    public int Id { get; set; }
    public string Content { get; set; } = null!;
    public string QuestionType { get; set; } = null!; // R, I, A, S, E, C

    // Navigation
    public ICollection<TestAnswer> TestAnswers { get; set; } = new List<TestAnswer>();
}
