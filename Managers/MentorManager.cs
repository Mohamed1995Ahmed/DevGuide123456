using DevGuide.Models;
using DevGuide.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Managers
{
    public class InstructorManager : BaseManager<User>
    {
        private readonly UserManager<User> userManager;
        private ProjectContext _context;
        public InstructorManager( UserManager<User> _userManager, ProjectContext context) : base(context)
        {
            this.userManager = _userManager;
            this._context = context;
        }








        public async Task<UserListViewModel> GetMentorByIdAsync(string mentorId)
        {
            //var mentor = await GetAllAsync(u => u.Id == mentorId, "Skills"); 
            // Include any related entities
            var mentor = await GetAllAsync(u => u.Id == mentorId);

            // Assuming only one mentor can be returned based on ID
            var user = mentor.FirstOrDefault();
            if (user == null) return null;

            // Map to ViewModel
            return new UserListViewModel
            {
                //ID = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Title = user.Title,
                Image = user.Image,
                Skills = user.Skills != null && user.Skills.Any()
                        ? user.Skills.Select(s => s.Skill).Select(s => s.Name).ToList()
                        : new List<string>(),
                SocialAccounts = user.SocialAccounts != null && user.SocialAccounts.Any()
    ? user.SocialAccounts.Select(l => new SocialLinkViewModel
    {
        SocialName = l.SocialName,
        SocialLink = l.SocialLink
    }).ToList()
 : new List<SocialLinkViewModel>()
                //Skills = user.Skills?.Select(s => s.Skill.Name).ToList()
                // Assuming Skills is a collection of User_Skills
            };
        }







        public async Task<List<User>> GetTopRatedHRs()
        {
            // Retrieve all users
            var users = userManager.Users.ToList();

            // Filter HRs by role
            var HRs = new List<User>();
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Contains("HR"))
                {
                    HRs.Add(user);
                }
            }

            // Sort HRs by average review rate and take the top 4
            var topRatedHRs = HRs
                .Where(u => u.Reviews.Any()) // Ensure they have reviews
                .OrderByDescending(u => u.Reviews.Average(r => r.Rate))
                .Take(4)
                .ToList();

            return topRatedHRs;
        }

        public async Task<List<User>> GetTopRatedMentors()
        {
            // Retrieve all users
            var users = userManager.Users.ToList();

            // Filter Mentors by role
            var mentors = new List<User>();
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Contains("Mentor"))
                {
                    mentors.Add(user);
                }
            }

            // Sort Mentors by average review rate and take the top 4
            var topRatedMentors = mentors
                .Where(u => u.Reviews.Any()) // Ensure they have reviews
                .OrderByDescending(u => u.Reviews.Average(r => r.Rate))
                .Take(4)
                .ToList();

            return topRatedMentors;
        }


        //public async Task<List<User>> GetTopRatedHRs()
        //{
        //    // Retrieve all users
        //    var users = userManager.Users.ToList();

        //    // Filter HRs by role
        //    var HRs = new List<User>();
        //    foreach (var user in users)
        //    {
        //        var roles = await userManager.GetRolesAsync(user);
        //        if (roles.Contains("HR"))
        //        {
        //            HRs.Add(user);
        //        }
        //    }

        //    // Sort HRs by average review rate
        //    var topRatedHRs = HRs
        //        .Where(u => u.Reviews.Any()) // Make sure they have reviews
        //        .OrderByDescending(u => u.Reviews.Average(r => r.Rate))
        //        .ToList();

        //    return topRatedHRs;
        //}

        //public async Task<List<User>> GetTopRatedMentors()
        //{
        //    // Retrieve all users
        //    var users = userManager.Users.ToList();

        //    // Filter Mentors by role (those who have instructed sessions)
        //    var mentors = new List<User>();
        //    foreach (var user in users)
        //    {
        //        var roles = await userManager.GetRolesAsync(user);
        //        if (roles.Contains("Mentor"))
        //        {
        //            mentors.Add(user);
        //        }
        //    }

        //    // Sort Mentors by average review rate
        //    var topRatedMentors = mentors
        //        .Where(u => u.Reviews.Any()) // Make sure they have reviews
        //        .OrderByDescending(u => u.Reviews.Average(r => r.Rate))
        //        .ToList();

        //    return topRatedMentors;
        //}

    }
}
    
