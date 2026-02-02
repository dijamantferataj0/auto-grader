namespace AutoGrader.Domain.Models;

public class Teacher
{
    public int Id { get; set; }
    public int XmlId { get; set; }
    public List<Student> Students { get; set; } = new();
}
