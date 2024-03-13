using BackendLab01;

namespace WebAPI.DTO;

public class QuizItemDTO
{
    public int Id { get; set; }
    public string Question { get; set; }
    public List<string> Options { get; set; }

    public static QuizItemDTO of(QuizItem item)
    {
        if (item is null)
        {
            return null;
        }
        List<string> options = new List<string>();
        options.Add(item.CorrectAnswer);
        options.AddRange(item.IncorrectAnswers);
        return new QuizItemDTO()
        {
            Id = item.Id,
            Question = item.Question,
            Options = options
        };
    }
}
