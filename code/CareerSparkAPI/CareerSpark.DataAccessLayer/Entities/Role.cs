﻿using System;
using System.Collections.Generic;

namespace CareerSpark.DataAccessLayer.Entities;

public partial class Role
{
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public bool IsDeleted { get; set; } = true;
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
