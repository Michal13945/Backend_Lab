using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BackendLab01.Pages;

public class Summary : PageModel
{
    private readonly IQuizUserService _userService;

    public int QuizId { get; set; }
    public int CorrectAnswersCount { get; set; }
    public int TotalQuestionsCount { get; set; }
    private int UserId => 0;

    public Summary(IQuizUserService userService)
    {
        _userService = userService;
    }

    public void OnGet(int quizId)
    {
        QuizId = quizId;
        var quiz = _userService.FindQuizById(quizId);

        if (quiz != null)
        {
            CorrectAnswersCount = _userService.CountCorrectAnswersForQuizFilledByUser(quizId, UserId);
            TotalQuestionsCount = quiz.Items.Count;
        }
    }
}