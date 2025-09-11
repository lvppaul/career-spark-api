using System;
using System.Collections.Generic;

namespace CareerSpark.DataAccessLayer.Entities;

public partial class TestHistory
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ResultId { get; set; }

    public int TestAnswerId { get; set; }

    public virtual Result Result { get; set; } = null!;

    public virtual TestAnswer TestAnswer { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
