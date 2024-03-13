using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BackendLab01.Pages;

public class All : PageModel
{
    private readonly IQuizAdminService _quizAdminService;

    public List<BackendLab01.Quiz> Quizzes { get; set; } = new List<BackendLab01.Quiz>();

    public All(IQuizAdminService quizAdminService)
    {
        _quizAdminService = quizAdminService;
    }

    public void OnGet()
    {
        Quizzes = _quizAdminService.FindAllQuizzes();
    }
}
