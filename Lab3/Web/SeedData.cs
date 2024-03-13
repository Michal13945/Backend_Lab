using ApplicationCore.Interfaces.Repository;
using BackendLab01;
using System.Linq;

namespace Infrastructure.Memory;
public static class SeedData
{
    public static void Seed(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            var quizRepo = provider.GetService<IGenericRepository<Quiz, int>>();
            var quizItemRepo = provider.GetService<IGenericRepository<QuizItem, int>>();

            var quiz1Items = new List<QuizItem>
            {
                new QuizItem(1, "Jakie miasto jest stolicą Polski?", new List<string> { "Madryt", "Londyn", "Berlin" }, "Warszawa"),
                new QuizItem(2, "W którym roku odbyły się pierwsze igrzyska olimpijskie?", new List<string> { "1896", "1900", "1912" }, "1896"),
                new QuizItem(3, "Która marka samochodu posiada model o nazwie A4?", new List<string> { "Mercedes", "BMW", "Renault" }, "Audi")
            };

            foreach (var item in quiz1Items)
            {
                quizItemRepo?.Add(item);
            }

            var quiz1 = new Quiz(1, quiz1Items, "Quiz Ogólny #1");
            quizRepo?.Add(quiz1);

            var quiz2Items = new List<QuizItem>
            {
                new QuizItem(4, "Jaki kolor powstanie po zmieszaniu niebieskiego i żółtego?", new List<string> { "pomarańczowy", "czerwony", "różowy" }, "żółty"),
                new QuizItem(5, "Ile planet jest w Układzie Słonecznym?", new List<string> { "7", "9", "12" }, "8"),
                new QuizItem(6, "Jaka jest najdłuższa rzeka na świecie?", new List<string> { "Nil", "Missisipi", "Jangcy" }, "Amazonka")
            };

            foreach (var item in quiz2Items)
            {
                quizItemRepo?.Add(item);
            }

            var quiz2 = new Quiz(2, quiz2Items, "Quiz Naukowy #2");
            quizRepo?.Add(quiz2);
        }
    }
}
