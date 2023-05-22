namespace Domain.Entities;

public class ScheduledTime
{
    public ScheduledTime()
    {
    }

    public ScheduledTime(int hour, int minute, int second)
    {
        Hour = hour;
        Minute = minute;
        Second = second;
    }

    public int Id { get; set; }
    public int Hour { get; set; }
    public int Minute { get; set; }
    public int Second { get; set; }
}