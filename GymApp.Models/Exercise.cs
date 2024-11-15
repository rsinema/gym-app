namespace GymApp.Models;

public class Exercise(int userId, string name, ExerciseType type)
{
    public int Id { get; } = new Random().Next(100_000_000, 1_000_000_000);
    public int UserId { get; } = userId;
    public string Name { get; } = name;
    public DateTime Date { get; } = DateTime.Now;
    public ExerciseType Type { get; } = type;
    public int? Weight { get; set; }
    public int? Reps { get; set; }
    public string? Time { get; set; }
    public string? Distance { get; set; }
}