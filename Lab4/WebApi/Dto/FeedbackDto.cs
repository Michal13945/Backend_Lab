namespace WebApi.Dto;

public class FeedbackDto
{
    public int QuizId { get; init; }

    public int UserId { get; set; }
    public int TotalQuestion { get; set; }

    public List<FeedbackQuizItemDto> QuizItemsAnswers { get; init; }
}
