using DevGuide.Models;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers
{
    public class EducationManager:BaseManager<Education>
    {
        public EducationManager(ProjectContext context) : base(context)
        {

        }

        public IEnumerable<Education> GetEducationsByUserId(string userId)
        {
            // Filter educations by userId
            return GetAll(e => e.User_Id==userId);
        }

        // Custom async method to get educations by user ID (asynchronous)
        public async Task<IEnumerable<Education>> GetEducationsByUserIdAsync(string userId)
        {
            // Filter educations by userId asynchronously
            return await GetAllAsync(e => e.User_Id == userId);
        }

    }
}
