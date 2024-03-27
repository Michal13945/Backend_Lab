using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Infrastructure.EF.Entities;
using Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF.Services;

public class QuizUserServiceEF : IQuizUserService
{
    private readonly QuizDbContext _context;

    public QuizUserServiceEF(QuizDbContext context)
    {
        _context = context;
    }

    public Quiz CreateAndGetQuizRandom(int count)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Quiz> FindAllQuizzes()
    {
        return _context
            .Quizzes
            .AsNoTracking()
            .Include(q => q.Items)
            .ThenInclude(i => i.IncorrectAnswers)
            .Select(QuizMapper.FromEntityToQuiz)
            .ToList();
    }

    public Quiz? FindQuizById(int id)
    {
        var entity = _context
            .Quizzes
            .AsNoTracking()
            .Include(q => q.Items)
            .ThenInclude(i => i.IncorrectAnswers)
            .FirstOrDefault(e => e.Id == id);
        return entity is null ? null : QuizMapper.FromEntityToQuiz(entity);
    }

    public List<QuizItemUserAnswer> GetUserAnswersForQuiz(int quizId, int userId)
    {
        var entities = _context
            .UserAnswers
            .AsNoTracking()
            .Include(x => x.QuizItem)
            .Where(x => x.QuizId == quizId && x.UserId == userId)
            .ToList();

        return entities.Select(QuizMapper.FromItemUserAnswerEntityToItemUserAnswer).ToList();
    }

    public QuizItemUserAnswer SaveUserAnswerForQuiz(int quizId, int quizItemId, int userId, string answer)
    {
        QuizItemUserAnswerEntity entity = new QuizItemUserAnswerEntity()
        {
            UserId = userId,
            QuizItemId = quizItemId,
            QuizId = quizId,
            UserAnswer = answer
        };
        try
        {
            var saved = _context.UserAnswers.Add(entity).Entity;
            _context.SaveChanges();
            return new QuizItemUserAnswer()
            {
                UserId = saved.UserId,
                QuizItem = QuizMapper.FromEntityToQuizItem(saved.QuizItem),
                QuizId = saved.QuizId,
                Answer = saved.UserAnswer
            };
        }
        catch (DbUpdateException e)
        {
            if (e.InnerException.Message.StartsWith("The INSERT"))
            {
                throw new QuizNotFoundException("Quiz, quiz item or user not found. Can't save!");
            }
            if (e.InnerException.Message.StartsWith("Violation of"))
            {
                throw new QuizAnswerItemAlreadyExistsException(quizId, quizItemId, userId);
            }
            throw new Exception(e.Message);
        }
    }
}
