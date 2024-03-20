namespace WebApi.Dto;

public class NewQuizItemDto
{
    public string Question { get; set; }
    public IEnumerable<string> Options { get; set; }
    public int CorrectOptionIndex { get; set; }
}
