using ApplicationCore.Interfaces;
using BackendLab01;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTO;

namespace WebAPI.Controllers;
[ApiController]
[Route("/api/v1/quizzes")]
public class QuizController : ControllerBase
{
    private readonly IQuizUserService _service;

    public QuizController(IQuizUserService service)
    {
        _service = service;
    }
    [HttpGet]
    [Route("{id}")]
    public ActionResult<QuizDTO> FindById(int id)
    {
        var result = QuizDTO.of(_service.FindQuizById(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet]
    public IEnumerable<QuizDTO> FindAll()
    {
        return _service.FindAllQuizzes().Select(QuizDTO.of).AsEnumerable();
    }

    [HttpPost]
    [Route("{quizId}/items/{itemId}/answers")]
    public ActionResult SaveAnswer([FromBody] QuizItemAnswerDTO DTO, int quizId, int itemId)
    {
        try
        {
            var answer = _service.SaveUserAnswerForQuiz(quizId, itemId, DTO.UserId, DTO.UserAnswer);
            return Created("", answer);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("{quizId}/answers/{userId}")]
    public UserAnswerSummaryDTO GetFeedback(int quizId, int userId)
    {
        var quizDTO = QuizDTO.of(_service.FindQuizById(quizId));
        var quizUserAnswers = _service.GetUserAnswersForQuiz(quizId, userId);
        var answers = _service.CountCorrectAnswersForQuizFilledByUser(quizId, userId);

        return new UserAnswerSummaryDTO()
        {
            QuizId = quizId,
            UserId = userId,
            CorrectAnswers = answers,
            Quiz = quizDTO,
            UserAnswers = quizUserAnswers.Select(x => new QuizItemUserAnswerDTO
            {
                QuizItemId = x.QuizItem.Id,
                Answer = x.Answer
            }).ToList()
        };
    }
}