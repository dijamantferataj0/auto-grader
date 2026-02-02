namespace AutoGrader.Domain.Models;

public class Exam
{
    public int Id { get; set; }
    public int XmlId { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public List<TaskItem> Tasks { get; set; } = new();
}
