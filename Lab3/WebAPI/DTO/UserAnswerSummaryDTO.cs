namespace WebAPI.DTO;

public class UserAnswerSummaryDTO
{
    public int UserId { get; set; }
    public int QuizId { get; set; }
    public int CorrectAnswers { get; set; }
    public QuizDTO Quiz { get; set; }
    public List<QuizItemUserAnswerDTO> UserAnswers { get; set; }
}
