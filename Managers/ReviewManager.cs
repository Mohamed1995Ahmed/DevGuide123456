using DevGuide.Models;
using DevGuide.Models.Models;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers
{
    public class ReviewManager:BaseManager<Review>
    {
        public ReviewManager(ProjectContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await GetAllAsync();
        }
    }
}
