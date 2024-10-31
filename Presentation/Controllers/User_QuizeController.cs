using DevGuide.Models.Models;
using Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class User_QuizeController : ControllerBase
    {
        //private readonly User_QuizeMAnager _userQuizManager;

        //public User_QuizeController(User_QuizeMAnager userQuizManager)
        //{
        //    _userQuizManager = userQuizManager;
        //}

        //[HttpPost("submit")]
        //public IActionResult SubmitQuiz(User_Quiz userQuiz)
        //{
        //    // Assuming `userQuiz` contains User_Id, Quiz_Id, and a list of User_Answers
        //    _userQuizManager.CalculateScore(userQuiz);
        //   _userQuizManager.Add(userQuiz);
            
        //    return Ok(new { Score = userQuiz.Score, Result = userQuiz.Result });
        //}
    }
}
