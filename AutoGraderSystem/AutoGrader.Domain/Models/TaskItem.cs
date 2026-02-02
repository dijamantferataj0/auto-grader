namespace AutoGrader.Domain.Models;

public class TaskItem
{
    public int Id { get; set; }
    public int XmlId { get; set; }
    public string Expression { get; set; } = string.Empty;
    public int ExamId { get; set; }
    public Exam Exam { get; set; } = null!;
}
