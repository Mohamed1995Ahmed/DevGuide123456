using DevGuide.Models;
using DevGuide.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Managers
{
    public class QuizeManager:BaseManager<Quiz>
    {
        public QuizeManager(ProjectContext context) : base(context)
        {

        }
        //public async Task<List<Quiz>> GetQuizzesBySkillsAsync(List<int> skillIds)
        //{
           
        //}


        // Fetch a quiz by ID, including related questions and options
        public async Task<Quiz> GetQuizWithDetailsAsync(int quizId)
        {
            return await context.Quiz
                .Include(q => q.Questions)
                .ThenInclude(question => question.Options)
                .FirstOrDefaultAsync(q => q.Id == quizId);
        }






    }
}
