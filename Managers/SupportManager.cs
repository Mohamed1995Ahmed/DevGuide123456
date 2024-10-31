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
    public class SupportManager : BaseManager<Support>
    {
       
        public SupportManager(ProjectContext context) : base(context)
        {
          
        }
        public async Task<bool> AddSupportAsync(Support support)
        {
            return await Task.Run(() => Add(support));
        }
        public async Task<IEnumerable<Support>> GetAllSessionsAsync()
        {
            return await GetAllAsync();
        }

    }
}
