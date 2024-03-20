using ApplicationCore.Interfaces.AdminService;
using ApplicationCore.Interfaces.UserService;
using ApplicationCore.Models;
using ApplicationCore.Models.QuizAggregate;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Dto;
using WebApi.Validators;

namespace WebApi.Controllers;

[ApiController]
[Route("/api/v1/user/quizzes")]
public class ApiQuizUserController : ControllerBase
{
    private readonly IQuizUserService _service;
    private readonly IQuizAdminService _adminService;
    private readonly IMapper _mapper;
    private readonly IValidator<QuizItem> _quizItemValidator;
    private readonly IValidator<NewQuizItemDto> _newQuizItemDtoValidator;

    public ApiQuizUserController(IQuizUserService service, IQuizAdminService adminService, IMapper mapper,
        IValidator<QuizItem> quizItemValidator, IValidator<NewQuizItemDto> validator)
    {
        _service = service;
        _adminService = adminService;
        _mapper = mapper;
        _quizItemValidator = quizItemValidator;
        _newQuizItemDtoValidator = validator;
    }

    [HttpPost]
    public ActionResult<object> AddQuiz(LinkGenerator link, NewQuizDto dto)
    {
        var quiz = _adminService.AddQuiz(_mapper.Map<Quiz>(dto));
        return Created(
            link.GetPathByAction(HttpContext, nameof(GetQuiz), null, new { quiId = quiz.Id }),
            quiz
        );
    }

    [HttpGet]
    [Route("{quizId}")]
    public ActionResult<Quiz> GetQuiz(int quizId)
    {
        var quiz = _adminService.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId);
        return quiz is null ? NotFound() : quiz;
    }

    [HttpPatch]
    [Route("{quizId}")]
    [Consumes("application/json-patch+json")]
    public ActionResult<Quiz> AddQuizItem(int quizId, JsonPatchDocument<Quiz>? patchDoc)
    {
        var quiz = _adminService.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId);
        if (quiz is null || patchDoc is null)
        {
            return NotFound(new
            {
                error = $"Quiz width id {quizId} not found"
            });
        }
        int previousCount = quiz.Items.Count;
        patchDoc.ApplyTo(quiz, ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (previousCount < quiz.Items.Count)
        {
            QuizItem item = quiz.Items[^1];
            quiz.Items.RemoveAt(quiz.Items.Count - 1);

            var validationResult = _quizItemValidator.Validate(item);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            _adminService.AddQuizItemToQuiz(quizId, item);
        }
        return Ok(_adminService.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId));
    }

    [Route("{quizId}/items/{itemId}/answers/{userId}")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public ActionResult<object> SaveAnswer(
        int quizId,
        int itemId,
        int userId,
        QuizItemUserAnswerDto dto,
        LinkGenerator linkGenerator
    )
    {
        _service.SaveUserAnswerForQuiz(quizId, userId, itemId, dto.Answer ?? "");
        return Created(
            linkGenerator.GetUriByAction(HttpContext, nameof(GetQuizFeedback), null,
                new { quizId = quizId, userId = 1 }),
            new
            {
                answer = dto.Answer,
            });
    }

    [Route("{quizId}/answers/{userId}")]
    [HttpGet]
    public ActionResult<object> GetQuizFeedback(int quizId, int userId)
    {
        var feedback = _service.GetUserAnswersForQuiz(quizId, userId).ToList();

        return new FeedbackDto
        {
            QuizId = quizId,
            UserId = userId,
            TotalQuestion = _service.FindQuizById(quizId)?.Items.Count??0,
            QuizItemsAnswers = feedback.Select(x => _mapper.Map<QuizItemUserAnswer, FeedbackQuizItemDto>(x)).ToList()
        };
    }

    [Route("test")]
    [HttpPost]
    public ActionResult Test(NewQuizItemDto dto)
    {
        var result = _newQuizItemDtoValidator.Validate(dto);

        if (result.IsValid) { return Ok(); }
        else { return  BadRequest(); }
    }
}