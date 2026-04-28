namespace DevelopTest.Models;

public class SportArea
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public AreaType? Type { get; set; }
    public TimeSpan? OpeningTime { get; set; }
    public TimeSpan? ClosingTime { get; set; }
    public int? Capacity { get; set; }
}

public enum AreaType 
{ 
    Soccer, 
    BasketBall, 
    Pool,
    Tennis,
    Volleyball,
    Gym,
    Athletics,
    SkatePark
}
