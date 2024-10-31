using DevGuide.Models.Models;
using Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Services;
using System.Security.Claims;
using System.Text.Json;


using ViewModels;


namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizeController : ControllerBase
    {

        private readonly ChatGptQuizService chatGptService;
        private QuizeManager quizManager;
        private User_QuizeManager user_QuizeManager;
        private SkillManager skillManager;

        public QuizeController(QuizeManager _quizManager, ChatGptQuizService _chatGptService,User_QuizeManager _user_QuizeManager,SkillManager _skillmanager)
        {
           quizManager = _quizManager ;
            user_QuizeManager = _user_QuizeManager ;
             skillManager = _skillmanager ;
            chatGptService = _chatGptService;
        }

        // Fetch quizzes based on the user's skills (Authorized)
        [HttpGet("quizzes")]
        [Authorize]
        //public async Task<IActionResult> GetQuizzesForUser()
        //{

        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return Unauthorized("User not found in claims.");
        //    }
        //    // Fetch the user's skills
        //    var userSkills = await user_QuizeManager.GetUserSkillsAsync(userId);

        //    // Fetch quizzes based on user skills
        //    var quizzes = await quizManager.GetQuizzesBySkillsAsync(userSkills);

        //    var quizViewModels = quizzes.Select(q => new CreateQuizDto
        //    {
        //        QuizId = q.Id,
        //        QuizName = q.QuizName,
        //        NumberOfQuestions = q.Number_Of_Questions,
        //        Duration = q.Duration,
        //        Questions = q.Questions.Select(qn => new QuestionDto
        //        {
        //            QuestionId = qn.Id,
        //            Text = qn.Text,
        //            Options = qn.Options.Select(o => new OptionDto
        //            {
        //                OptionId = o.Id,
        //                OptionText = o.Text
        //            }).ToList()
        //        }).ToList()
        //    }).ToList();

        //    return Ok(quizViewModels);
        //}

        // Generate a quiz based on a skill using ChatGPT (Authorized)
        [HttpPost("generate-quiz")]
        [Authorize]
        public async Task<IActionResult> GenerateQuiz([FromForm] string skillName)
        {
            try
            {
                var chatGptResponse = await chatGptService.GenerateQuiz(skillName);

                var quiz = new Quiz
                {
                    QuizName = $"{skillName} Quiz",
                    Number_Of_Questions = 5,
                    Duration = 15
                };

                var generatedQuestions = JsonSerializer.Deserialize<List<QuestionDto>>(chatGptResponse);
                quiz.Questions = generatedQuestions.Select(q => new Question
                {
                    Text = q.Text,
                    Options = q.Options.Select(o => new Option
                    {
                        Text = o.OptionText,
                        IsCorrect = false
                    }).ToList()
                }).ToList();

                quizManager.Add(quiz);

                return Ok(quiz);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine("Error generating quiz: " + ex.Message);
                return StatusCode(500, "Failed to generate quiz.");
            }
        }


        // Display a specific quiz to the user (Authorized)
        [HttpGet("quiz/{quizId}")]
        [Authorize]

        public async Task<IActionResult> GetQuiz(int quizId)
        {
            var quiz = await quizManager.GetQuizWithDetailsAsync(quizId);

            if (quiz == null)
            {
                return NotFound("Quiz not found.");
            }

            var quizViewModel = new CreateQuizDto
            {
                QuizId = quiz.Id,
                QuizName = quiz.QuizName,
                NumberOfQuestions = quiz.Number_Of_Questions,
                Duration = quiz.Duration,
                Questions = quiz.Questions.Select(q => new QuestionDto
                {
                    QuestionId = q.Id,
                    Text = q.Text,
                    Options = q.Options.Select(o => new OptionDto
                    {
                        OptionId = o.Id,
                        OptionText = o.Text
                    }).ToList()
                }).ToList()
            };

            return Ok(quizViewModel);
        }


        // Submit quiz and calculate score (Authorized)
        [HttpPost("submit-quiz")]
        [Authorize]
        public async Task<IActionResult> SubmitQuiz([FromForm] SubmitQuizViewModel model)
        {
            var userQuiz = await user_QuizeManager.GetUserQuizWithDetailsAsync(model.UserQuizId);

            if (userQuiz == null)
            {
                return NotFound("Quiz not found.");
            }

            // Calculate score
            int correctAnswers = 0;

            foreach (var question in userQuiz.Quiz.Questions)
            {
                var userAnswer = model.Answers.FirstOrDefault(a => a.QuestionId == question.Id);
                if (userAnswer != null)
                {
                    var correctOption = question.Options.FirstOrDefault(o => o.IsCorrect);
                    if (correctOption != null && correctOption.Id == userAnswer.SelectedOptionId)
                    {
                        correctAnswers++;
                    }
                }
            }

            decimal score = (decimal)correctAnswers / userQuiz.Quiz.Number_Of_Questions * 100;
            bool passed = score >= 50; // Example passing criteria (50%)

            // Save the result
            userQuiz.Score = score;
            userQuiz.Result = passed;
            user_QuizeManager.Update(userQuiz);

            var resultViewModel = new QuizResultViewModel
            {
                Score = score,
                Passed = passed
            };

            return Ok(resultViewModel);
        }
    }
}
