using System;
using System.Collections.Generic;

namespace CareerSpark.DataAccessLayer.Entities;

public partial class TestHistory
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TestSessionId { get; set; }
    public int TestAnswerId { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public TestSession TestSession { get; set; } = null!;
    public TestAnswer TestAnswer { get; set; } = null!;
}
