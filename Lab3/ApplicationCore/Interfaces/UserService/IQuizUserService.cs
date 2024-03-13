using ApplicationCore.Interfaces.Repository;

namespace BackendLab01;

public interface IQuizUserService
{
    Quiz CreateAndGetQuizRandom(int count);

    Quiz? FindQuizById(int id);
    IEnumerable<Quiz> FindAllQuizzes();

    QuizItemUserAnswer SaveUserAnswerForQuiz(int quizId, int quizItemId, int userId, string answer);

    List<QuizItemUserAnswer> GetUserAnswersForQuiz(int quizId, int userId);

    int CountCorrectAnswersForQuizFilledByUser(int quizId, int userId)
    {
        return GetUserAnswersForQuiz(quizId, userId)
            .Count(e => e.IsCorrect());
    }
}