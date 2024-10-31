using DevGuide.Models;
using DevGuide.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers
{
    public class User_QuizeManager:BaseManager<User_Quiz>
    {
        public User_QuizeManager(ProjectContext context) : base(context)
        {

        }

        public async Task<User_Quiz> GetUserQuizWithDetailsAsync(int userQuizId)
        {
            return await context.User_Quiz
                .Include(uq => uq.Quiz)
                .ThenInclude(q => q.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(uq => uq.Id == userQuizId);
        }

        public async Task<List<int>> GetUserSkillsAsync(string userId)
        {
            return await context.User_Skills
                .Where(us => us.User_Id == userId)
                .Select(us => us.Skill_Id)
                .ToListAsync();
        }


        //public decimal CalculateScore(User_Quiz userQuiz)
        //{
        //    decimal score = 0;
        //    foreach (var answer in userQuiz.Answers)
        //    {
        //        var correctOption = answer.Question.Options.FirstOrDefault(o => o.IsCorrect);
        //        if (correctOption != null && correctOption.Id == answer.Option_Id)
        //        {
        //            score += 1; // Increment score for each correct answer
        //        }
        //    }
        //    userQuiz.Score = score; // Assign calculated score to User_Quiz
        //    userQuiz.Result = score >= (userQuiz.Quiz.Number_Of_Questions / 2m); // Example: pass if >= 50%
        //    return score;
        //}
        //public void SubmitQuiz(User_Quiz userQuiz)
        //{
        //    // Ensure userQuiz is not null and contains valid data
        //    if (userQuiz == null || userQuiz.Quiz_Id <= 0)
        //    {
        //        throw new ArgumentException("Invalid User_Quiz data.");
        //    }

        //    // Fetch the Quiz entity from the database using the Quiz_Id
        //    var quiz = context.Quiz.Find(userQuiz.Quiz_Id);
        //    if (quiz == null)
        //    {
        //        throw new ArgumentException($"Quiz with ID {userQuiz.Quiz_Id} not found.");
        //    }

        //    // Associate the fetched quiz with the userQuiz
        //    userQuiz.Quiz = quiz;

        //    // Calculate the score based on user answers
        //    CalculateScore(userQuiz);

        //    // Add the userQuiz to the database
        //    Add(userQuiz); // This method should save userQuiz with the calculated score
        //}
    }
}
