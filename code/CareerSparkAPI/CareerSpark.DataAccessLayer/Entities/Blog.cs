using System;
using System.Collections.Generic;

namespace CareerSpark.DataAccessLayer.Entities;

public partial class Blog
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string Title { get; set; } = null!;
    public string Tag { get; set; } = null!;
    public string Content { get; set; } = null!;
    
    public DateTime? CreateAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public bool IsPublished { get; set; } = false;
    public bool IsDeleted { get; set; } = false;

    public virtual User Author { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
