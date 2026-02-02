namespace AutoGrader.Domain.Models;

public class Student
{
    public int Id { get; set; }
    public int XmlId { get; set; }
    public List<Exam> Exams { get; set; } = new();
}
