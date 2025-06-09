using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizTest.Web.Models;
using QuizTest.Web.Models.Entities;
using QuizTest.Web.ViewData;
using Microsoft.AspNetCore.Identity;

namespace QuizTest.Web.controllers
{
    public class QuizTestController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public QuizTestController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult AddQuiz()
        {
            return View();
        }
        // [HttpGet]
        // public IActionResult AddOption()
        // {
        //     return View();
        // }

        [HttpPost]
        public async Task<IActionResult> AddQuiz(AddQuizzViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var quizz = new Quizz
            {
                Id = Guid.NewGuid(),
                Ques = viewModel.Ques,
            };
            await dbContext.Quizzes.AddAsync(quizz);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("AddQuiz", "QuizTest");
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var quizzes = await dbContext.Quizzes.ToListAsync();
            var options = await dbContext.Options.ToListAsync();

            var viewModel = new QuizListViewModel
            {
                Quizzes = quizzes,
                AllOptions = options
            };

            return View(viewModel);
        }


        // [HttpPost]
        // public async Task<IActionResult> AddOption(AddOptionViewModel viewModel)
        // {
        //     if (!ModelState.IsValid)
        //         return View(viewModel);

        //     var option = new Options
        //     {
        //         Id = Guid.NewGuid(),
        //         Ans = viewModel.Ans,
        //         IsCorrect = viewModel.IsCorrect,
        //         QuizzId = viewModel.QuizzId
        //     };
        //     await dbContext.Options.AddAsync(option);
        //     await dbContext.SaveChangesAsync();

        //     return RedirectToAction("AddOption", "QuizTest");
        // }
        [HttpGet]
        public async Task<IActionResult> AddOption()
        {
            // Get all quizzes
            var quizzes = await dbContext.Quizzes
                .Include(q => q.Options)
                .ToListAsync();

            // Filter: only quizzes with less than 4 options
            var eligibleQuizzes = quizzes
                .Where(q => q.Options == null || q.Options.Count < 4)
                .ToList();

            ViewBag.Quizzes = eligibleQuizzes;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOption(AddOptionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Quizzes = await dbContext.Quizzes.ToListAsync();
                return View(viewModel);
            }

            var option = new Options
            {
                Id = Guid.NewGuid(),
                Ans = viewModel.Ans,
                IsCorrect = viewModel.IsCorrect,
                QuizzId = viewModel.QuizzId
            };
            await dbContext.Options.AddAsync(option);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("AddOption", "QuizTest");
        }

        [HttpGet]
        public async Task<IActionResult> Question()
        {
            var quizzes = await dbContext.Quizzes.ToListAsync();
            var options = await dbContext.Options.ToListAsync();

            var viewModel = new QuizListViewModel
            {
                Quizzes = quizzes,
                AllOptions = options
            };

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Question(Dictionary<Guid, Guid> selectedOptions)
        {
            int correctCount = 0;

            foreach (var entry in selectedOptions)
            {
                var selectedOptionId = entry.Value;

                var option = dbContext.Options.FirstOrDefault(o => o.Id == selectedOptionId);
                if (option != null && option.IsCorrect)
                {
                    correctCount++;
                }
            }

            TempData["Score"] = correctCount;
            return RedirectToAction("QuizResult", "QuizTest");
        }

        [HttpGet]
        
        public IActionResult QuizResult()
        {
            if (TempData.ContainsKey("Score"))
            {
                int score = (int)TempData["Score"];
                ViewBag.Score = score;
            }
            else
            {
                ViewBag.Score = 0; // Default value if no score is found
            }

            return View();
        }



    }
}