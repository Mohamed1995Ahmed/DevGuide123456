using DevGuide.Models;
using DevGuide.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers
{
    public class QueryManager : BaseManager<Query>
    {
        public QueryManager(ProjectContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Query>> GetUserQueriesWithAnswersAsync(string userId)
        {
            return await GetAllAsync(q => q.User_Id == userId);
        }

        public async Task<IEnumerable<Query>> GetMentorQueriesAsync(string mentorId)
        {
            return await GetAllAsync(q => q.User_Instructor_Id == mentorId);
        }

    }
}
