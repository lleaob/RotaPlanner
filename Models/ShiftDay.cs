namespace RotaPlanner.Models;

public class ShiftDay
{
    public DateOnly Date { get; set; }
    public bool IsWorkDay { get; set; }
    public TimeOnly? StartTime { get; set; }
}
