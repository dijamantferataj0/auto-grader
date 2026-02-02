using AutoGrader.Domain.Models;
using AutoGrader.Services.Interfaces;
using System.Xml;
using System.Xml.Linq;

namespace AutoGrader.Services.Services;

public class XmlParserService : IXmlParserService
{
    public async Task<Teacher> ParseXmlAsync(Stream xmlStream)
    {
        try
        {
            var doc = await XDocument.LoadAsync(xmlStream, LoadOptions.PreserveWhitespace, CancellationToken.None);

            var root = doc.Root;
            if (root == null || root.Name.LocalName != "Teacher")
            {
                throw new Exception("Invalid XML structure: Root element must be 'Teacher'");
            }

            var teacherIdAttr = root.Attribute("ID");
            if (teacherIdAttr == null)
            {
                throw new Exception("Teacher element must have an ID attribute");
            }

            if (!int.TryParse(teacherIdAttr.Value, out int teacherId))
            {
                throw new Exception("Teacher ID must be a valid integer");
            }

            var teacher = new Teacher
            {
                XmlId = teacherId,
                Students = new List<Student>()
            };

            var studentsElement = root.Element("Students");
            if (studentsElement != null)
            {
                foreach (var studentElement in studentsElement.Elements("Student"))
                {
                    var student = ParseStudent(studentElement);
                    teacher.Students.Add(student);
                }
            }

            return teacher;
        }
        catch (XmlException)
        {
            throw;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> ValidateXmlAsync(Stream xmlStream)
    {
        try
        {
            var doc = await XDocument.LoadAsync(xmlStream, LoadOptions.PreserveWhitespace, CancellationToken.None);

            var root = doc.Root;
            if (root == null || root.Name.LocalName != "Teacher")
            {
                throw new Exception("Invalid XML structure: Root element must be 'Teacher'");
            }

            var teacherIdAttr = root.Attribute("ID");
            if (teacherIdAttr == null)
            {
                throw new Exception("Teacher element must have an ID attribute");
            }

            return true;
        }
        catch
        {
            throw;
        }
    }

    private Student ParseStudent(XElement studentElement)
    {
        var studentIdAttr = studentElement.Attribute("ID");
        if (studentIdAttr == null)
        {
            throw new Exception("Student element must have an ID attribute");
        }

        if (!int.TryParse(studentIdAttr.Value, out int studentId))
        {
            throw new Exception("Student ID must be a valid integer");
        }

        var student = new Student
        {
            XmlId = studentId,
            Exams = new List<Exam>()
        };

        foreach (var examElement in studentElement.Elements("Exam"))
        {
            var exam = ParseExam(examElement);
            student.Exams.Add(exam);
        }

        return student;
    }

    private Exam ParseExam(XElement examElement)
    {
        var examIdAttr = examElement.Attribute("Id");
        if (examIdAttr == null)
        {
            throw new Exception("Exam element must have an Id attribute");
        }

        if (!int.TryParse(examIdAttr.Value, out int examId))
        {
            throw new Exception("Exam Id must be a valid integer");
        }

        var exam = new Exam
        {
            XmlId = examId,
            Tasks = new List<TaskItem>()
        };

        foreach (var taskElement in examElement.Elements("Task"))
        {
            var task = ParseTask(taskElement);
            exam.Tasks.Add(task);
        }

        return exam;
    }

    private TaskItem ParseTask(XElement taskElement)
    {
        var taskIdAttr = taskElement.Attribute("id");
        if (taskIdAttr == null)
        {
            throw new Exception("Task element must have an id attribute");
        }

        if (!int.TryParse(taskIdAttr.Value, out int taskId))
        {
            throw new Exception("Task id must be a valid integer");
        }

        var task = new TaskItem
        {
            XmlId = taskId,
            Expression = taskElement.Value
        };

        return task;
    }
}
