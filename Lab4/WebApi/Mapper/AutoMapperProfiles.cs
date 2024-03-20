using ApplicationCore.Models;
using ApplicationCore.Models.QuizAggregate;
using AutoMapper;
using WebApi.Dto;

namespace WebApi.Mapper;

public class AutoMapperProfiles : Profile
{

    public AutoMapperProfiles()
    {
        CreateMap<QuizItem, QuizItemDto>()
            .ForMember(
                q => q.Options,
                op => op.MapFrom(i => new List<string>(i.IncorrectAnswers) { i.CorrectAnswer }));
        CreateMap<Quiz, QuizDto>()
            .ForMember(
                q => q.Items,
                op => op.MapFrom<List<QuizItem>>(i => i.Items)
            );
        CreateMap<NewQuizDto, Quiz>();

        CreateMap<QuizItemUserAnswer, FeedbackQuizItemDto>()
                .ForMember(
                    q => q.Question,
                    op => op.MapFrom(i => i.QuizItem.Question))
                .ForMember(
                    q => q.IsCorrect,
                    op => op.MapFrom(i => i.IsCorrect()))
                .ForMember(
                    q => q.QuizItemId,
                    op => op.MapFrom(i => i.QuizItem.Id));

    }
}
