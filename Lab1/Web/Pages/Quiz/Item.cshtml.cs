using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace BackendLab01.Pages
{
    
    public class QuizModel : PageModel
    {
        private readonly IQuizUserService _userService;

        private readonly ILogger _logger;
        public QuizModel(IQuizUserService userService, ILogger<QuizModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        [BindProperty]
        public string Question { get; set; } = string.Empty;
        [BindProperty]
        public List<string> Answers { get; set; } = new List<string>();
        [BindProperty]
        public string UserAnswer { get; set; } = string.Empty;
        [BindProperty]
        public int QuizId { get; set; }
        [BindProperty]
        public int ItemId { get; set; }

        public void OnGet(int quizId, int itemId)
        {
            QuizId = quizId;
            ItemId = itemId;

            var quiz = _userService.FindQuizById(quizId);

            if (quiz?.Items != null && quiz.Items.Count >= itemId)
            {
                var quizItem = quiz.Items[itemId - 1]; 
                Question = quizItem.Question;
                Answers.AddRange(quizItem.IncorrectAnswers);
                Answers.Add(quizItem.CorrectAnswer);
                Answers = Answers.OrderBy(a => Guid.NewGuid()).ToList();
            }
            else
            {
                _logger.LogWarning($"Quiz lub pytanie nie istnieje: QuizID = {quizId}, ItemID = {itemId}");
            }
        }

        public IActionResult OnPost()
        {
            var quiz = _userService.FindQuizById(QuizId);
            _userService.SaveUserAnswerForQuiz(QuizId, 0, ItemId, UserAnswer);

            if (quiz != null && ItemId >= quiz.Items.Count)
            {
                return RedirectToPage("Summary", new { quizId = QuizId });
            }
            else
            {
                return RedirectToPage("Item", new { quizId = QuizId, itemId = ItemId + 1 });
            }
        }
    }
}
