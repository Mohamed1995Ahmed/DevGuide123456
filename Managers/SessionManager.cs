using DevGuide.Models;
using DevGuide.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Managers
{
    public class SessionManager : BaseManager<Session>
    {
        public SessionManager(ProjectContext context) : base(context) { }

        public async Task<IEnumerable<Session>> GetAllSessionsAsync()
        {
            return await GetAllAsync();
        }





        public bool AddFeedback(int sessionId, string feedback)
        {
            var session = context.Session.FirstOrDefault(s => s.Id == sessionId);
            if (session == null) return false;

            session.Feedback = feedback;
            context.SaveChanges();
            return true;
        }
    }
}

