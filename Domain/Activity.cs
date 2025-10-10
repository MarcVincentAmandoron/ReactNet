using System;
using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Activity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Title { get; set; } 
    public DateTime? Date { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public bool? isCancelled { get; set; }

    // location props
    public string? City { get; set; }
    public string? Venue { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }


}