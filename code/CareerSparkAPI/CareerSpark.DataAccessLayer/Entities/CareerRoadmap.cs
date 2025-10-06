using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerSpark.DataAccessLayer.Entities;

public partial class CareerRoadmap
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

   
    [Required]
    [ForeignKey(nameof(CareerPath))]
    public int CareerPathId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;


    [MaxLength(500)]
    public string? Description { get; set; }

 
    [Required]
    public int StepOrder { get; set; }


    [MaxLength(200)]
    public string? SkillFocus { get; set; }

  
    [MaxLength(50)]
    public string? DifficultyLevel { get; set; }

 
    public int? DurationWeeks { get; set; }

  
    [MaxLength(255)]
    public string? SuggestedCourseUrl { get; set; }


    public virtual CareerPath CareerPath { get; set; } = null!;
}
