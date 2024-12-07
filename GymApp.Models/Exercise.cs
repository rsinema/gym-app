namespace GymApp.Models;

public class Exercise(int userId, string name, ExerciseType type, int? weight = null, int? reps = null, int? time = null, double? distance = null)
{
    public int Id { get; set; } = new Random().Next(100_000_000, 1_000_000_000);
    public int UserId { get; set; } = userId;
    public string Name { get; set; } = name;
    public DateTime Date { get; set; } = DateTime.Now;
    public ExerciseType Type { get; set; } = type;
    public int? Weight { get; set; } = weight;
    public int? Reps { get; set; } = reps;
    public int? Time { get; set; } = time;
    public double? Distance { get; set; } = distance;
}