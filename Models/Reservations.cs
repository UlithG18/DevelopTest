namespace DevelopTest.Models;

public class Reservation
{
    public int Id { get; set; }
    public DateOnly? Date { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public ReservationStatus Status { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }

    public int SportAreaId { get; set; }
    public SportArea SportArea { get; set; }
}

public enum ReservationStatus 
{ 
    Finished,
    Canceled,
    Pending
}