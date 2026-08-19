namespace RotaPlanner.Models;

public class RotaPattern
{
    // E.g., [4, -2, 5, -2] -> 4 on, 2 off, 5 on, 2 off
    public List<int> Sequence { get; set; } = new();

    // E.g., [7, 8, 9] for 7 AM, 8 AM, 9 AM shift rotations
    public List<int> StartTimesPool { get; set; } = new();
}
