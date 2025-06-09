namespace QuizTest.Web.Models.Entities
{
    public class AddOptionViewModel
    {
        public string Ans { get; set; }
        public bool IsCorrect { get; set; }
        public Guid QuizzId { get; set; } // Foreign key to the Quizz entity
    }
}