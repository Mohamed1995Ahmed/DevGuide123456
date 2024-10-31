using DevGuide.Models;
using DevGuide.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using System.Data;
using Models.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Http;
using ViewModels;
using Microsoft.AspNetCore.Authorization;



namespace Managers
{

    public class AccountManager : BaseManager<User>
    {
        private IConfiguration configuration;
        private UserManager<User> userManager;
        private SignInManager<User> signInManger;
        private EducationManager educationManager;
        private MyRoleManager roleManager;
        private ReviewManager reviewManager;
        private readonly User_QuizeManager user_QuizeManager;
        private QueryManager _queryManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountManager(ProjectContext context, UserManager<User> _userManager, SignInManager<User> _signInManager, IConfiguration _configuration, EducationManager _educationManager,
        MyRoleManager _roleManager, QueryManager queryManager,ReviewManager _reviewManager,User_QuizeManager user_QuizeManager) : base(context)
        {
            this.userManager = _userManager;
            this.signInManger = _signInManager;
            this.configuration = _configuration;
            this.educationManager = _educationManager;
            this.roleManager = _roleManager;

            this.reviewManager = _reviewManager;
            this.user_QuizeManager = user_QuizeManager;
            this._queryManager = queryManager;
        }

        public async Task<IdentityResult> Register(UserRegisterViewModel userRegisterViewModel)
        {
            User user = userRegisterViewModel.ToModel();

            var result = await userManager.CreateAsync(user, userRegisterViewModel.Password);
            //result= await userManager.AddToRoleAsync(user, "Admin");
            result = await userManager.AddToRoleAsync(user, userRegisterViewModel.Role);

            return result;
        }
        public async Task<IdentityResult> ChangePasswordAsync(ChangePassword changePasswordModel, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // User not found
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            // Verify the current password
            var isPasswordValid = await userManager.CheckPasswordAsync(user, changePasswordModel.CurrentPassword);
            if (!isPasswordValid)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Current password is incorrect." });
            }

            // Check if new password and confirm password match
            if (changePasswordModel.NewPassword != changePasswordModel.ConfirmPassword)
            {
                return IdentityResult.Failed(new IdentityError { Description = "New password and confirmation do not match." });
            }

            // Change the password
            var result = await userManager.ChangePasswordAsync(user, changePasswordModel.CurrentPassword, changePasswordModel.NewPassword);
            return result;
        }


        //public async Task<string> Login(UserLoginViewModel model)
        //{
        //    try 
        //    {

        //        var user = await userManager.FindByEmailAsync(model.LoginMethod);
        //        if (user == null)
        //        {
        //            user = await userManager.FindByNameAsync(model.LoginMethod);
        //            if (user == null)
        //            {

        //                return "Failed";

        //            }
        //        }


        //        var res = await signInManger.PasswordSignInAsync(user, model.Password, model.Rememberme, true);
        //        if (res.Succeeded)
        //        {
        //            List<Claim> claims = new List<Claim>();
        //            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
        //            var Roles = await userManager.GetRolesAsync(user);
        //            foreach (var role in Roles)
        //            {
        //                claims.Add(new Claim(ClaimTypes.Role, role));
        //            }

        //            //Roles.ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));
        //            SymmetricSecurityKey sskey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:Key"]));
        //            JwtSecurityToken token = new JwtSecurityToken(
        //                signingCredentials: new SigningCredentials(sskey, SecurityAlgorithms.HmacSha256),
        //                expires: DateTime.Now.AddDays(1),
        //                claims: claims
        //                );
        //            return new JwtSecurityTokenHandler().WriteToken(token);
        //        }
        //        else
        //        {
        //            return string.Empty;
        //        }
        //        //return
        //    }
        //    catch (Exception ex) 
        //    { 
        //        return ex.Message;
        //    }

        //}

        //public async Task<string> Login(UserLoginViewModel model)
        //{
        //    try
        //    {
        //        // Check if the user exists by email
        //        var user = await userManager.FindByEmailAsync(model.LoginMethod);
        //        if (user == null)
        //        {
        //            // If not found by email, check by username
        //            user = await userManager.FindByNameAsync(model.LoginMethod);
        //            if (user == null)
        //            {
        //                // If neither email nor username exists, return "Failed"
        //                return "Failed";
        //            }
        //        }

        //        // Perform password sign-in
        //        var res = await signInManger.PasswordSignInAsync(user, model.Password, model.Rememberme, true);
        //        if (res.Succeeded)
        //        {
        //            // Generate the JWT token on successful login
        //            List<Claim> claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, user.Id)
        //    };

        //            var roles = await userManager.GetRolesAsync(user);
        //            foreach (var role in roles)
        //            {
        //                claims.Add(new Claim(ClaimTypes.Role, role));
        //            }

        //            // Generate the JWT token
        //            var sskey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:Key"]));
        //            var token = new JwtSecurityToken(
        //                signingCredentials: new SigningCredentials(sskey, SecurityAlgorithms.HmacSha256),
        //                expires: DateTime.Now.AddDays(1),
        //                claims: claims
        //            );
        //            return new JwtSecurityTokenHandler().WriteToken(token);
        //        }
        //        else
        //        {
        //            // Return an empty string if login failed
        //            return string.Empty;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle the exception by returning the message
        //        return ex.Message;
        //    }
        //}

        //public async Task<string> Login(UserLoginViewModel model)
        //{
        //    try
        //    {

        //        var user = await userManager.FindByEmailAsync(model.LoginMethod);
        //        if (user == null)
        //        {
        //            user = await userManager.FindByNameAsync(model.LoginMethod);
        //            if (user == null)
        //            {

        //                return "Failed";

        //            }
        //        }


        //        var res = await signInManger.PasswordSignInAsync(user, model.Password, model.Rememberme, true);
        //        if (res.Succeeded)
        //        {
        //            List<Claim> claims = new List<Claim>();
        //            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
        //            var Roles = await userManager.GetRolesAsync(user);
        //            foreach (var role in Roles)
        //            {
        //                claims.Add(new Claim(ClaimTypes.Role, role));
        //            }

        //            //Roles.ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));
        //            SymmetricSecurityKey sskey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:Key"]));
        //            JwtSecurityToken token = new JwtSecurityToken(
        //                signingCredentials: new SigningCredentials(sskey, SecurityAlgorithms.HmacSha256),
        //                expires: DateTime.Now.AddDays(1),
        //                claims: claims
        //                );
        //            return new JwtSecurityTokenHandler().WriteToken(token);
        //        }
        //        else
        //        {
        //            return string.Empty;
        //        }
        //        //return
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }

        //}

        public async Task<LoginResponse> Login(UserLoginViewModel model)
        {
            var response = new LoginResponse();
            try
            {
                // Attempt to find the user by email or username
                var user = await userManager.FindByEmailAsync(model.LoginMethod)
                           ?? await userManager.FindByNameAsync(model.LoginMethod);

                if (user == null)
                {
                    response.Success = false;
                    return response; // User not found
                }

                // Sign in the user
                var result = await signInManger.PasswordSignInAsync(user, model.Password, model.Rememberme, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // Create claims for the JWT token
                    List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

                    var roles = await userManager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    // Generate JWT token
                    SymmetricSecurityKey sskey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:Key"]));
                    JwtSecurityToken token = new JwtSecurityToken(
                        signingCredentials: new SigningCredentials(sskey, SecurityAlgorithms.HmacSha256),
                        expires: DateTime.Now.AddDays(1),
                        claims: claims
                    );

                    response.Success = true;
                    response.Token = new JwtSecurityTokenHandler().WriteToken(token);
                    response.Roles = roles.ToList(); // Convert roles to list
                }
                else
                {
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                response.Success = false;
                response.Token = string.Empty; // No token on error
                                               // You might want to log ex.Message for debugging
            }

            return response;
        }


        //public async bool check(string email)
        //{

        //}
        public async void Signout()
        {
            await signInManger.SignOutAsync();
        }

        public async Task<User?> GetUserByID(string userId)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return null;
                }


                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task AddSocialAccountAsync(SocialAccounts socialAccount)
        {
            await context.User_SocialAccounts.AddAsync(socialAccount);
            await context.SaveChangesAsync();
        }



        //    public async Task<Pagination<List<UserListViewModel>>> FilterUsers(string columnName = "Id", bool IsAscending = false,
        //int PageSize = 5, int PageNumber = 1,
        //string? name = null, string? role = null,
        //string? title = null, decimal? minprice = null, decimal? maxprice = null,
        //decimal? rate = null)
        //    {
        //        // Build the filter expression
        //        var builder = PredicateBuilder.New<User>();
        //        var old = builder;

        //        // Filter by Name
        //        if (!string.IsNullOrEmpty(name))
        //        {
        //            builder = builder.And(user =>
        //                (user.FirstName + " " + user.LastName).Contains(name));
        //        }

        //        if (!string.IsNullOrEmpty(role))
        //        {
        //            var usersInRole = await userManager.GetUsersInRoleAsync(role); // Get users in the role
        //            var userIdsInRole =  usersInRole.Select(u => u.Id).ToList();
        //            builder = builder.And(user => userIdsInRole.Contains(user.Id));
        //        }

        //        // Filter by Title
        //        if (!string.IsNullOrEmpty(title))
        //        {
        //            builder = builder.And(user =>
        //                user.Title.Contains(title));
        //        }

        //        // Filter by Price
        //        if (minprice.HasValue && maxprice.HasValue)
        //        {
        //            builder = builder.And(user =>
        //                user.Price >= minprice.Value && user.Price <= maxprice.Value);
        //        }

        //        if (rate.HasValue)
        //        {
        //            builder = builder.And(user =>
        //                user.Reviews != null && user.Reviews.Any()
        //                ? (decimal)user.Reviews.Average(r => r.Rate) >= rate.Value && 
        //                (decimal)user.Reviews.Average(r => r.Rate) < (rate.Value+1)
        //                : false) ;
        //        }


        //        if (old == builder)
        //        {
        //            builder = null;
        //        }

        //        // Use the BaseManager filter function
        //        int total = (builder == null) ? base.GetAll().Count() : base.GetAll().Where(builder).Count();
        //        var query =  base.Filter(builder, columnName, IsAscending, PageSize, PageNumber);
        //        //return query.Select(b=>b.ToViewModel()).ToList();
        //        //var list= new List<UserListViewModel>();
        //        //foreach ( var item in query)
        //        //{
        //        //    var user=  item.ToUserListViewModel();
        //        //     list.Add(user);
        //        //}

        //       return new Pagination<List<UserListViewModel>>
        //        {
        //            PageNumber = PageNumber,
        //            PageSize = PageSize,
        //            TotalCount = total,
        //            Data = query.Select(b => b.ToUserListViewModel()).ToList(),
        //        };
        //    }



        //    public async Task<Pagination<List<UserListViewModel>>> FilterUsers(
        //string columnName = "Id", bool IsAscending = false,
        //int PageSize = 5, int PageNumber = 1,
        //string? name = null, string? role = null,
        //string? title = null, decimal? minprice = null, decimal? maxprice = null,
        //decimal? rate = null, string skills = null)
        //    {
        //        // Build the filter expression
        //        var builder = PredicateBuilder.New<User>();
        //        var old = builder;

        //        // Filter by Name
        //        if (!string.IsNullOrEmpty(name))
        //        {
        //            builder = builder.And(user =>
        //                (user.FirstName + " " + user.LastName).Contains(name));
        //        }

        //        // Filter by Role
        //        if (!string.IsNullOrEmpty(role))
        //        {
        //            var usersInRole = await userManager.GetUsersInRoleAsync(role); // Get users in the role
        //            var userIdsInRole = usersInRole.Select(u => u.Id).ToList();
        //            builder = builder.And(user => userIdsInRole.Contains(user.Id));
        //        }

        //        // Filter by Title
        //        if (!string.IsNullOrEmpty(title))
        //        {
        //            builder = builder.And(user =>
        //                user.Title.Contains(title));
        //        }

        //        // Filter by Price
        //        if (minprice.HasValue && maxprice.HasValue)
        //        {
        //            builder = builder.And(user =>
        //                user.Price >= minprice.Value && user.Price <= maxprice.Value);
        //        }

        //        // Filter by Rate
        //        if (rate.HasValue)
        //        {
        //            builder = builder.And(user =>
        //                user.Reviews != null && user.Reviews.Any()
        //                ? (decimal)user.Reviews.Average(r => r.Rate) >= rate.Value &&
        //                (decimal)user.Reviews.Average(r => r.Rate) < (rate.Value + 1)
        //                : false);
        //        }

        //        // Filter by Skills
        //        if (skills != null && skills.Any())
        //        {
        //            builder = builder.And(user =>
        //                user.Skills != null && user.Skills.Any(skill => skills.Contains(skill.Skill.Name)));
        //        }

        //        if (old == builder)
        //        {
        //            builder = null;
        //        }

        //        // Use the BaseManager filter function
        //        int total = (builder == null) ? base.GetAll().Count() : base.GetAll().Where(builder).Count();
        //        var query = base.Filter(builder, columnName, IsAscending, PageSize, PageNumber);

        //        return new Pagination<List<UserListViewModel>>
        //        {
        //            PageNumber = PageNumber,
        //            PageSize = PageSize,
        //            TotalCount = total,
        //            Data = query.Select(b => b.ToUserListViewModel()).ToList(),
        //        };
        //    }


        public async Task<Pagination<List<UserListViewModel>>> FilterUsers(
    string columnName = "Id", bool IsAscending = false,
    int PageSize = 5, int PageNumber = 1,
    string? name = null, string? role = null,
    string? title = null, decimal? minprice = null, decimal? maxprice = null,
    decimal? rate = null, string? skills = null, string? excludeMentorId = null)
        {
            var builder = PredicateBuilder.New<User>();
            var old = builder;

            // Exclude the mentor with the specified ID
            if (!string.IsNullOrEmpty(excludeMentorId))
            {
                builder = builder.And(user => user.Id != excludeMentorId);
            }

            // Filter by name
            if (!string.IsNullOrEmpty(name))
            {
                builder = builder.And(user =>
                    (user.FirstName + " " + user.LastName).Contains(name));
            }

            // Filter by Role
            if (!string.IsNullOrEmpty(role))
            {
                var usersInRole = await userManager.GetUsersInRoleAsync(role);
                var userIdsInRole = usersInRole.Select(u => u.Id).ToList();
                builder = builder.And(user => userIdsInRole.Contains(user.Id));
            }

            // Filter by Title
            if (!string.IsNullOrEmpty(title))
            {
                builder = builder.And(user =>
                    user.Title.Contains(title));
            }

            // Filter by Price
            if (minprice.HasValue && maxprice.HasValue)
            {
                builder = builder.And(user =>
                    user.Price >= minprice.Value && user.Price <= maxprice.Value);
            }

            // Filter by Rate
            if (rate.HasValue)
            {
                builder = builder.And(user =>
                    user.Reviews != null && user.Reviews.Any()
                    ? (decimal)user.Reviews.Average(r => r.Rate) >= rate.Value &&
                    (decimal)user.Reviews.Average(r => r.Rate) < (rate.Value + 1)
                    : false);
            }

            // Filter by Skills
            if (skills != null && skills.Any())
            {
                builder = builder.And(user =>
                    user.Skills != null && user.Skills.Any(skill => skills.Contains(skill.Skill.Name)));
            }

            if (old == builder)
            {
                builder = null;
            }

            // Use the BaseManager filter function
            int total = (builder == null) ? base.GetAll().Count() : base.GetAll().Where(builder).Count();
            var query = base.Filter(builder, columnName, IsAscending, PageSize, PageNumber);

            return new Pagination<List<UserListViewModel>>
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
                TotalCount = total,
                Data = query.Select(b => b.ToUserListViewModel()).ToList(),
            };
        }





        public List<UserListViewModel> GetAllUser()
        {

            var res = base.GetAll()
                           .Include(u => u.Reviews)  // Eagerly load Reviews
                           .Select(user => new UserListViewModel
                           {

                               ID = user.Id,
                               FirstName = user.FirstName,
                               LastName = user.LastName,
                               Price = user.Price,
                               Image = user.Image,
                               YearsOfExperience = user.YearsOfExperience,
                               About = user.About,

                          
                               // Include other user fields as needed

                               AverageRate = user.Reviews.Any() ? (decimal)user.Reviews.Average(r => r.Rate) : 0,
                               //SocialAccounts = user.SocialAccounts.Any() ? user.SocialAccounts.Select(l => l.SocialLink) : null// Calculate average rate or return 0 if no reviews
                           })
                           .ToList();
            return res;


        }


        public async Task<List<string>> GetUserRole(User user)
        {

            // Get the roles assigned to the user
            var roles = await userManager.GetRolesAsync(user);

            // Return the roles (for example, as a JSON response)
            return roles.ToList();
        }

        public Session GetSessionById(int id)
        {
            return context.Session.FirstOrDefault(r=>r.Id == id);
        }

        
        public async Task<List<ReviewViewModel>> GetReview(string id)
        {
            // Eagerly load related entities for reviews, including Session and User
            var user = await userManager.FindByIdAsync(id);
           
            // Fetch the session IDs from reviews
            var sessionIds = user.Reviews.Select(r => r.Session_Id).ToList();

            // Assuming GetSessionById is an async method that fetches the session by ID
            var sessions = new List<Session>();
            foreach (var sessionId in sessionIds)
            {
                var session = GetSessionById(sessionId);
                if (session != null)
                {
                    sessions.Add(session);  // Add the session if it exists
                }
            }

            // Return an empty list if the user or reviews don't exist
            if (user == null || user.Reviews == null || !user.Reviews.Any())
            {
                return new List<ReviewViewModel>();  // Return an empty list if no reviews exist
            }

            // Map the reviews to the ReviewViewModel
            List<ReviewViewModel> reviewList = user.Reviews.Select(r =>
            {
                // Find the corresponding session for this review by Session_Id
                var session = sessions.FirstOrDefault(s => s.Id == r.Session_Id);

                // Map to the ReviewViewModel
                return new ReviewViewModel
                {
                    Rate = r.Rate,
                    Description = r.Description,
                    ReviewDate = r.ReviewDate,
                    Reviewer = session?.User != null
                        ? new ReviewerViewModel
                        {
                            FirstName = session.User.FirstName,   // Assuming FirstName exists in User
                            LastName = session.User.LastName,     // Assuming LastName exists in User
                            Image = session.User.Image            // Assuming Image exists in User
                        }
                        : null
                };
            }).ToList();

            return reviewList;
        }

        public async Task<ProfileViewModel> GetOneUser(string id)
        {
            // Eager load the related entities, including Session and User for Reviews
            var user = await userManager.Users
                .Include(u => u.Skills)
                .Include(u => u.SocialAccounts)
                .Include(u => u.Reviews) // Include Reviews
                    .ThenInclude(r => r.Session) // Include Session in Reviews
                    .ThenInclude(s => s.User) // Include User in Session
                .Include(u => u.Educations)
                .Include(u => u.Experiences)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return null;

            // Map to ViewModel
            return new ProfileViewModel
            {
                ID = id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Title = user.Title,
                YearsOfExperience = user.YearsOfExperience,
                Image = user.Image,
                Price = user.Price,
                Country = user.Country,
                Level = user.Level,
                About = user.About,
                NumOfStudents = (context.Session != null && context.Session.Any(s => s.User_Instructor_Id == user.Id))
        ? context.Session.Count(s => s.User_Instructor_Id == user.Id)
        : 0,

                NumOfReviews = (user.Reviews != null && user.Reviews.Any())
                    ? user.Reviews.Count()
                    : 0,
                AverageRate = (user.Reviews != null && user.Reviews.Any())
    ? Math.Round((decimal)user.Reviews.Average(r => r.Rate), 1)
    : 0,

                Skills = user.Skills != null && user.Skills.Any()
                    ? user.Skills.Select(s => s.Skill).Select(s => s.Name).ToList()
                    : new List<string>(),
                SocialAccounts = user.SocialAccounts != null && user.SocialAccounts.Any()
                    ? user.SocialAccounts.Select(l => new SocialLinkViewModel
                    {
                        SocialName = l.SocialName,
                        SocialLink = l.SocialLink
                    }).ToList()
                    : new List<SocialLinkViewModel>(),
               
                Educations = user.Educations != null && user.Educations.Any()
                    ? user.Educations.Select(e => new EducationViewModel
                    {
                        Degree = e.Degree,
                        FieldOfStudy = e.FieldOfStudy,
                        University = e.University,
                        Faculty = e.Faculty,
                        CountryOfStudy = e.CountryOfStudy,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                        TillNow = e.TillNow
                    }).ToList()
                    : new List<EducationViewModel>(),
                Experiences = user.Experiences != null && user.Experiences.Any()
                    ? user.Experiences.Select(e => new ExperienceViewModel
                    {
                        FieldOfStudy = e.FieldOfStudy,
                        Organization = e.Organization,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                        TillNow = e.TillNow
                    }).ToList()
                    : new List<ExperienceViewModel>(),
            };
        }

        public  async Task <IList<string>> GetUserRolesAsync(string userId)
        {
            var user =await   userManager.FindByIdAsync(userId);
            return user == null ? new List<string>() :await  userManager.GetRolesAsync(user);
        }
        public async Task<bool> ConvertDeveloperToMentorByAdminAsync(User_Quiz userQuiz,string userid)
        {
             
            var user = userQuiz.User;

            // Check if the user passed the quiz and is currently in the Developer role
            if (userQuiz.Result==true && await userManager.IsInRoleAsync(user, "Developer")==true && userQuiz.User_Id==userid)
            {
                await userManager.RemoveFromRoleAsync(user, "Developer");
                await userManager.AddToRoleAsync(user, "Mentor");
                return true;
            }

            return false; // No role change if conditions aren't met
        }
        public async Task<bool> ConvertDeveloperToMentorAsync(User currentUser)
        {
          
            // Get the latest quiz attempt of the user
            var latestUserQuiz = await user_QuizeManager.GetAll()
                .Where(uq => uq.User_Id == currentUser.Id)
                .OrderByDescending(uq => uq.QuizCreated) // Assuming the most recent quiz is the one to check
                .FirstOrDefaultAsync();

            if (latestUserQuiz == null)
            {
                return false; // No quiz found, no conversion
            }

            // Check if the user passed the latest quiz and is currently in the Developer role
            if (latestUserQuiz.Result && await userManager.IsInRoleAsync(currentUser, "Developer"))
            {
                await userManager.RemoveFromRoleAsync(currentUser, "Developer");
                await userManager.AddToRoleAsync(currentUser, "Mentor");
                return true;
            }

            return false; // No role change if conditions aren't met
        }





    }
}
