namespace QuizTest.Web.Models.Entities
{
    public class Quizz
    {
        public Guid Id { get; set; }
        public string Ques { get; set; }
        public ICollection<Options> Options { get; set; }
    }
}