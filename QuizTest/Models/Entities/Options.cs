namespace QuizTest.Web.Models.Entities
{
    public class Options
    {
        public Guid Id { get; set; }
        public string Ans { get; set; }
        public bool IsCorrect { get; set; }

        public Guid QuizzId { get; set; } 

        public Quizz Quizz { get; set; } 
        
    }
}