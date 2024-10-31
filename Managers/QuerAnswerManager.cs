using DevGuide.Models;
using DevGuide.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers
{
    public class QuerAnswerManager : BaseManager<QueryAnswer>
    {
        public QuerAnswerManager(ProjectContext context) : base(context)
        {

        }
        public async Task<IEnumerable<QueryAnswer>> GetMentorAnswersAsync(int QueryID)
        {
            return await GetAllAsync(qa => qa.Query_Id == QueryID);
        }


    }
}
