using ApplicationCore.Interfaces.Criteria;
using ApplicationCore.Interfaces.Repository;

namespace BackendLab01;

public class QuizUserService: IQuizUserService
{
    private readonly IGenericRepository<Quiz, int> quizRepository;
    private readonly IGenericRepository<QuizItem, int> itemRepository;
    private readonly IGenericRepository<QuizItemUserAnswer, string> answerRepository;

    public QuizUserService(IGenericRepository<Quiz, int> quizRepository, IGenericRepository<QuizItemUserAnswer, string> answerRepository, IGenericRepository<QuizItem, int> itemRepository)
    {
        this.quizRepository = quizRepository;
        this.answerRepository = answerRepository;
        this.itemRepository = itemRepository;
    }

    public Quiz CreateAndGetQuizRandom(int count)
    {
        throw new NotImplementedException();
    }

    public Quiz? FindQuizById(int id)
    {
        return quizRepository.FindById(id);
    }

    public IEnumerable<Quiz> FindAllQuizzes()
    {
        return quizRepository.FindAll();
    }

    public QuizItemUserAnswer SaveUserAnswerForQuiz(int quizId, int quizItemId, int userId, string answer)
    {
        var quiz = quizRepository.FindById(quizId);
        var item = itemRepository.FindById(quizItemId);
        if (quiz is null)
        {
            throw new Exception($"Quiz with id = {quizId} not found!");
        }

        if (item is null)
        {
            throw new Exception($"Quiz item with id = {quizItemId} not found!");
        }

        var userAnswer = new QuizItemUserAnswer(item, userId, quizId, answer);
        return answerRepository.Add(userAnswer);
    }


    public List<QuizItemUserAnswer> GetUserAnswersForQuiz(int quizId, int userId)
    {
        // return answerRepository.FindAll()
        //     .Where(x => x.QuizId == quizId)
        //     .Where(x => x. UserId == userId)
        //     .ToList();
        return answerRepository.FindBySpecification(new QuizItemsForQuizIdFilledByUser(quizId, userId)).ToList();
    }
}