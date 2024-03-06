using BackendLab01;

namespace WebAPI.DTO;

public class QuizDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<QuizItemDTO> Items { get; set; }

    public static QuizDTO of(Quiz quiz)
    {
        if (quiz is null)
        {
            return null;
        }
        return new QuizDTO()
        {
            Id = quiz.Id,
            Title = quiz.Title,
            Items = quiz.Items.Select(QuizItemDTO.of).ToList()
        };
    }
}
