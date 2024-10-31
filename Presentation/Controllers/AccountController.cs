using DevGuide.Models.Models;

using DevGuide.Models.Models;

using Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


using Microsoft.EntityFrameworkCore;

using System.Text;
using ViewModels;
using DevGuide.Models;
using Models.Models;

using static System.Runtime.InteropServices.JavaScript.JSType;
using ViewModels;
using System.Data;


namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private AccountManager accountManager;
        private EducationManager educationManager;
        private ReviewManager reviewManager;
       
       

        private ProjectContext context;
        private InstructorManager istructorManager;
        private readonly User_QuizeManager user_QuizeManager;
        private UserManager<User> userManager;
        
        public AccountController(AccountManager _accountManager,
            EducationManager _educationManage, ReviewManager _reviewManager,
             ProjectContext context, InstructorManager _istructorManager,User_QuizeManager user_QuizeManager,
             UserManager<User> _userManager)
        {
            this.accountManager = _accountManager;
            this.educationManager = _educationManage;
            this.reviewManager = _reviewManager;
           
            this.context = context;
            this.istructorManager = _istructorManager;
            this.user_QuizeManager = user_QuizeManager;
            this.userManager = _userManager;

        }



        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePassword changePasswordModel)
        {
            if (changePasswordModel == null)
            {
                return BadRequest("Invalid change password request.");
            }

            // Assuming you have a way to get the current user's ID from the JWT token or context
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Get user ID from claims

            var result = await accountManager.ChangePasswordAsync(changePasswordModel, userId);
            if (result.Succeeded)
            {
                return new JsonResult("Password changed successfully.");
            }

            return BadRequest(result.Errors.Select(e => e.Description));
        }



       

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Check if the email or password already exists
                var existingUser = await accountManager.GetAll()
                    .FirstOrDefaultAsync(u => u.Email == viewModel.Email || u.UserName == viewModel.UserName);

                if (existingUser != null)
                {
                    // If email or password exists, return 403 Forbidden
                    return StatusCode(403, "Email or User Name already exists.");
                }

                // If email and password are unique, proceed with registration
                var result = await accountManager.Register(viewModel);
                if (result.Succeeded)
                {

                    var res = await accountManager.Login(new UserLoginViewModel { LoginMethod = viewModel.UserName, Password = viewModel.Password });
                    return new JsonResult(new APIResult<LoginResponse>()
                    {
                        Result = res,
                        Message = "Login Successfully",
                        StatusCode = 200,
                        Success = true

                    });
                    //return new JsonResult("You successfully created your account, login now.");

                }
                else
                {
                    var str = new StringBuilder();
                    foreach (var item in result.Errors)
                    {
                        str.Append(item.Description);
                    }
                    return new JsonResult(str.ToString());
                }
            }
            else
            {
                var str = new StringBuilder();
                foreach (var item in ModelState.Values)
                {
                    foreach (var item1 in item.Errors)
                    {
                        str.Append(item1.ErrorMessage);
                    }
                }

                return new ObjectResult(str.ToString());
            }
        }


       

           
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LogIn(UserLoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var loginResponse = await accountManager.Login(viewModel);

                if (!loginResponse.Success)
                {
                    return new JsonResult(new APIResult<string>
                    {
                        Result = string.Empty,
                        Message = "Invalid username or password. Please try again.",
                        StatusCode = 400,
                        Success = false
                    });
                }

                // If successful, return the JWT token and roles
                return new JsonResult(new APIResult<LoginResponse>
                {
                    Result = loginResponse,
                    Message = "Login successfully",
                    StatusCode = 200,
                    Success = true
                });
            }
            else
            {
                // Collect all validation errors
                var str = new StringBuilder();
                foreach (var item in ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        str.AppendLine(error.ErrorMessage);
                    }
                }

                return new JsonResult(new APIResult<string>
                {
                    Result = string.Empty,
                    Message = str.ToString(),
                    StatusCode = 400,
                    Success = false
                });
            }
        }



      

        [HttpGet]
        [Route("Logout")]
        public IActionResult LogOut()
        {
            accountManager.Signout();
            return Ok();
        }

        [HttpPut("CompleteProfile")]
        [Authorize]
        public async Task<IActionResult> CompleteProfile([FromForm] CompleteProfileViewModel model)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in");
            }

            var user = await accountManager.GetUserByID(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Update basic user info
            //user.FirstName = model.FirstName;
            //user.LastName = model.LastName;
            user.Country = model.Country ?? user.Country;
            user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;

            user.About = model.About ?? user.About;
            user.YearsOfExperience = model.YearsOfExperience ?? user.YearsOfExperience;
            user.Level = model.Level ?? user.Level;
            user.Title = model.Title ?? user.Title;
            if (model.CV != null)
            {
                user.CV = await SaveFileAsync(model.CV, "cv_files"); // SaveFileAsync needs to return the file path
            }

            if (model.Image != null)
            {
                user.Image = await SaveFileAsync(model.Image, "profile_images");
            }


            // Handle Skills
            if (model.Skills != null)
            {
                var userSkills = model.Skills.Select(skillId => new User_Skills { Skill_Id = skillId }).ToList();
                user.Skills = userSkills;  // Set new skills
            }


            var result = accountManager.Update(user);
            if (!result)
            {
                return BadRequest("Failed to update user profile");
            }


            var userRole = await accountManager.GetUserRole(user);  // Implement a method to get user's role
            return new JsonResult(new { Success = true, Role = userRole });
        }

            // Handle CV and Image
            

       

        [HttpPut("AddEducation")]
        [Authorize]
        public async Task<IActionResult> AddEducation(EducationViewModel education)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in");
            }

            var user = await accountManager.GetUserByID(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }
            if (user.Educations == null)
            {
                user.Educations = new List<Education>();
            }
            user.Educations.Add(new Education
            {
                Degree = education.Degree,
                FieldOfStudy = education.FieldOfStudy,
                University = education.University,
                Faculty = education.Faculty,
                CountryOfStudy = education.CountryOfStudy,
                StartDate = education.StartDate,
                EndDate = education.EndDate,
                TillNow = education.TillNow,
            });
            var result = accountManager.Update(user);
            if (!result)
            {
                return BadRequest("Failed to update user profile");
            }

            return new JsonResult("User profile updated successfully");
        }

        [HttpPut("AddExperience")]
        [Authorize]
        public async Task<IActionResult> AddExperience(ExperienceViewModel education)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in");
            }

            var user = await accountManager.GetUserByID(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }
            if (user.Experiences == null)
            {
                user.Experiences = new List<Experience>();
            }

            user.Experiences.Add(new Experience
            {
                Organization = education.Organization,
                FieldOfStudy = education.FieldOfStudy,
                StartDate = education.StartDate,
                EndDate = education.EndDate,
                TillNow = education.TillNow,
            });
            var result = accountManager.Update(user);
            if (!result)
            {
                return BadRequest("Failed to update user profile");
            }

            return new JsonResult("User profile updated successfully");
        }

        [HttpGet("GetAllExperiences")]
        [Authorize]
        public async Task<IActionResult> GellAllExperiencesForUser()
        {
            // Retrieve the user ID from the authenticated token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in");
            }

            // Fetch the user from the database, including their skills
            var user = await accountManager.GetAll()
                .Include(u => u.Experiences)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Ensure the user has skills
            if (user.Experiences == null || !user.Experiences.Any())
            {
                return NotFound("No Experiences found for this user");
            }

            // Select the skills to return
            var experiences = user.Experiences.Select(us => new
            {
                us.Id,
                us.FieldOfStudy,
                us.Organization,
                us.StartDate,
                us.EndDate,
                us.TillNow
            }).ToList();

            return Ok(experiences);
        }

        [HttpGet("GetAllEducations")]
        [Authorize]
        public async Task<IActionResult> GellAllEducationsForUser()
        {
            // Retrieve the user ID from the authenticated token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in");
            }

            // Fetch the user from the database, including their skills
            var user = await accountManager.GetAll()
                .Include(u => u.Educations)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Ensure the user has skills
            if (user.Educations == null || !user.Educations.Any())
            {
                return NotFound("No Educations found for this user");
            }

            // Select the skills to return
            var educations = user.Educations.Select(us => new
            {
                us.Id,
                us.Degree,
                us.FieldOfStudy,
                us.University,
                us.Faculty,
                us.CountryOfStudy,
                us.StartDate,
                us.EndDate,
                us.TillNow
            }).ToList();

            return Ok(educations);
        }








        // Helper method to save files (CV and Images)
        private async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            var filePath = Path.Combine("wwwroot", folderName, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/{folderName}/{file.FileName}";
        }



        


        [HttpPut("update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromForm] Profile updatedProfile)
        {
            // Assuming you're using ASP.NET Core Identity and have the user's ID stored in claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var profile = await accountManager.GetAll().FirstOrDefaultAsync(p => p.Id == userId);

            if (profile == null)
            {
                return NotFound("Profile not found");
            }


            // Update fields only if they are provided in the updatedProfile
            if (!string.IsNullOrEmpty(updatedProfile.FirstName))
            {
                profile.FirstName = updatedProfile.FirstName;
            }

            if (!string.IsNullOrEmpty(updatedProfile.LastName))
            {
                profile.LastName = updatedProfile.LastName;
            }

            if (!string.IsNullOrEmpty(updatedProfile.Title))
            {
                profile.Title = updatedProfile.Title;
            }

            // Handle CV file upload

            if (updatedProfile.Cv != null)
            {
                profile.CV = await SaveFileAsync(updatedProfile.Cv, "cv_files"); // Ensure SaveFileAsync returns the file path
            }

            // Handle Image file upload
            if (updatedProfile.Image != null)
            {
                profile.Image = await SaveFileAsync(updatedProfile.Image, "profile_images");

            }

            if (!string.IsNullOrEmpty(updatedProfile.Level))
            {
                profile.Level = updatedProfile.Level;
            }

            if (!string.IsNullOrEmpty(updatedProfile.Country))
            {
                profile.Country = updatedProfile.Country;
            }

            if (!string.IsNullOrEmpty(updatedProfile.PhoneNumber))
            {
                profile.PhoneNumber = updatedProfile.PhoneNumber;
            }

            if (updatedProfile.YearsOfExperience.HasValue)
            {
                profile.YearsOfExperience = updatedProfile.YearsOfExperience.Value;
            }

            if (!string.IsNullOrEmpty(updatedProfile.About))
            {
                profile.About = updatedProfile.About;

            }

            // Ensure accountManager.Update is an async method, if possible
            accountManager.Update(profile);

            return Ok(profile.toUserProfile());
        }





        [HttpGet("get-profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Retrieve authenticated user

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var profile = accountManager.GetById(userId);

            if (profile == null)
            {
                return NotFound("Profile not found");
            }

            return Ok(profile.toUserProfile()); // Return the profile data
        }


        [HttpPut("UpdateEducation")]
        [Authorize]
        public async Task<IActionResult> UpdateEducation(EducationViewModel educationUpdate)
        {
            // Retrieve the user ID from the authenticated token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in");
            }

            // Fetch the user from the database
            var user = await accountManager.GetAll().Include(u => u.Educations).FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Ensure the user has education records to update
            if (user.Educations == null)
            {
                return NotFound("No education records found for the user");
            }

            // var user = await accountManager.GetUserByID(userId);

            // Find the specific education record by EducationId
            var educationToUpdate = user.Educations.FirstOrDefault(e => e.Id == educationUpdate.Id);

            if (educationToUpdate == null)
            {
                return NotFound("Education record not found");
            }

            // Update the education fields if the user provided new values
            if (!string.IsNullOrEmpty(educationUpdate.Degree))
            {
                educationToUpdate.Degree = educationUpdate.Degree;
            }

            if (!string.IsNullOrEmpty(educationUpdate.FieldOfStudy))
            {
                educationToUpdate.FieldOfStudy = educationUpdate.FieldOfStudy;
            }

            if (!string.IsNullOrEmpty(educationUpdate.University))
            {
                educationToUpdate.University = educationUpdate.University;
            }

            if (!string.IsNullOrEmpty(educationUpdate.Faculty))
            {
                educationToUpdate.Faculty = educationUpdate.Faculty;
            }

            if (!string.IsNullOrEmpty(educationUpdate.CountryOfStudy))
            {
                educationToUpdate.CountryOfStudy = educationUpdate.CountryOfStudy;
            }

            if (educationUpdate.StartDate != default(DateTime))
            {
                educationToUpdate.StartDate = educationUpdate.StartDate;
            }

            if (educationUpdate.EndDate != default(DateTime?))
            {
                educationToUpdate.EndDate = educationUpdate.EndDate;
            }

            educationToUpdate.TillNow = educationUpdate.TillNow;

            // Update the user in the database
            var result = accountManager.Update(user);

            if (!result)
            {
                return BadRequest("Failed to update user education");
            }

            return Ok("User education updated successfully");
        }
        [HttpPut("UpdateExperience")]
        [Authorize]
        public async Task<IActionResult> UpdateExperience(ExperienceViewModel experience)
        {
            // Retrieve the user ID from the authenticated token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in");
            }

            // Fetch the user from the database
            var user = await accountManager.GetAll().Include(u => u.Experiences).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Ensure the user has experiences
            if (user.Experiences == null || !user.Experiences.Any())
            {
                return NotFound("No experiences found for this user");
            }

            // Find the existing experience by its Id
            var existingExperience = user.Experiences.FirstOrDefault(e => e.Id == experience.Id);

            if (existingExperience == null)
            {
                return NotFound("Experience not found");
            }

            // Update the experience fields
            existingExperience.Organization = experience.Organization;
            existingExperience.FieldOfStudy = experience.FieldOfStudy;
            existingExperience.StartDate = experience.StartDate;
            existingExperience.EndDate = experience.EndDate;
            existingExperience.TillNow = experience.TillNow;

            // Update the user in the database asynchronously
            var result = accountManager.Update(user);

            if (!result)
            {
                return BadRequest("Failed to update user profile");
            }

            return Ok("Experience updated successfully");
        }
        [HttpPost("AddSkill")]
        [Authorize]
        public async Task<IActionResult> AddSkill(SkillViewModel skillModel)
        {
            // Retrieve the user ID from the authenticated token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in");
            }

            // Fetch the user from the database
            var user = await accountManager.GetAll().Include(u => u.Skills).ThenInclude(us => us.Skill).FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Create a new skill
            var newSkill = new Skill
            {
                Name = skillModel.Name,
                Description = skillModel.Description
            };

            // Check if the UserSkills collection is initialized
            if (user.Skills == null)
            {
                user.Skills = new List<User_Skills>();
            }

            // Associate the new skill with the user
            var userSkill = new User_Skills
            {
                Skill = newSkill,
                User_Id = userId // Assuming User_Skills has a property User_Id
            };

            user.Skills.Add(userSkill);

            // Save changes to the user
            var result = accountManager.Update(user); // Ensure this method is asynchronous if needed

            if (!result)
            {
                return BadRequest("Failed to add skill");
            }

            return Ok("Skill added successfully");
        }
        [HttpGet("GetAllSkills")]
        [Authorize]
        public async Task<IActionResult> GetAllSkillsForUser()
        {
            // Retrieve the user ID from the authenticated token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in");
            }

            // Fetch the user from the database, including their skills
            var user = await accountManager.GetAll()
                .Include(u => u.Skills)
                .ThenInclude(us => us.Skill)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Ensure the user has skills
            if (user.Skills == null || !user.Skills.Any())
            {
                return NotFound("No skills found for this user");
            }

            // Select the skills to return
            var skills = user.Skills.Select(us => new
            {
                us.Skill.Id,
                us.Skill.Name,
                us.Skill.Description
            }).ToList();

            return Ok(skills);
        }
        [HttpDelete("DeleteSkill/{skillId}")]
        [Authorize]
        public async Task<IActionResult> DeleteSkillForUser(int skillId)
        {
            // Retrieve the user ID from the authenticated token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in");
            }

            // Fetch the user from the database, including their skills
            var user = await accountManager.GetAll()
                .Include(u => u.Skills)
                .ThenInclude(us => us.Skill)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Find the User_Skill entry for the specified skillId
            var userSkill = user.Skills.FirstOrDefault(us => us.Skill_Id == skillId);

            if (userSkill == null)
            {
                return NotFound("Skill not found for this user");
            }

            // Remove the User_Skill entry
            user.Skills.Remove(userSkill);

            // Update the user in the database
            var result = accountManager.Update(user);

            if (!result)
            {
                return BadRequest("Failed to delete skill");
            }

            return Ok("Skill deleted successfully");
        }
        

        [HttpGet("SocialAccounts")]
        [Authorize]
        public async Task<IActionResult> GetSocialAccounts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from claims

            if (string.IsNullOrEmpty(userId))
              {
                return Unauthorized("User not logged in");
             }

                // Get all social accounts (you can pass any eager loading parameters if needed)
                var user = await accountManager.GetAll().Include(u=>u.SocialAccounts).FirstOrDefaultAsync(u => u.Id == userId); ; // Include User if needed for details

            // Filter the accounts for the current user

            if (user == null)
            {
                return NotFound("user not found");
            }

            // ensure the user has skills
            if (user.SocialAccounts == null || !user.SocialAccounts.Any())
            {
                return NotFound("no socaial account found for this user");
            }

            var social = user.SocialAccounts.Select(s => new
            {
                s.Id,
                s.SocialName,
                s.SocialLink
            }
            ).ToList();

            return Ok(social); // Return the list wrapped in Ok() for proper IActionResult
        }
        [HttpPost("SocialAccounts")]
        [Authorize]
        public async Task<IActionResult> AddSocialAccount([FromBody] AddSocialAccountViewModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid social account data.");
            }

            // Get the user ID from claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in.");
            }

            // Create a new SocialAccount object
            var newSocialAccount = new SocialAccounts
            {
                SocialName = model.SocialName,
                SocialLink = model.SocialLink,
                User_Id = userId // Associate it with the current user
            };

            // Add the new social account to the database
            try
            {
                await accountManager.AddSocialAccountAsync(newSocialAccount); // Assuming you have this method in your AccountManager

                return CreatedAtAction(nameof(GetSocialAccounts), new { id = newSocialAccount.Id }, newSocialAccount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet("Filter")]
        public async Task<IActionResult> FilterUsers(
            string? name = null, string? role = null, string? title = null, decimal? minprice = null, decimal? maxprice = null, decimal? rate = null, string? skills = null, string? excludeMentorId = null, int page = 1, int pageSize = 5)

        {
            var result = await accountManager.FilterUsers("Id", false, pageSize, page, name, role, title, minprice, maxprice, rate,skills,excludeMentorId);
            //return new JsonResult(result.Data.ToList());
            return new JsonResult(result);

        }


        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {


            var res = accountManager.GetAll()
                           .Include(u => u.Reviews)  // Eagerly load Reviews
                           .Select(user => new
                           {

                               Id = user.Id,
                               FirstName = user.FirstName,
                               LastName = user.LastName,
                               Price = user.Price,
                               Image = user.Image,
                               YearsOfExperience = user.YearsOfExperience,
                               About = user.About,

                               Email = user.Email,
                               // Include other user fields as needed

                               AverageRate = user.Reviews.Any() ? user.Reviews.Average(r => r.Rate) : 0,
                               SocialAccounts = user.SocialAccounts.Any() ? user.SocialAccounts.Select(l => l.SocialLink) : null// Calculate average rate or return 0 if no reviews
                           })
                           .ToList();
            return new JsonResult(res);


        }


      
        [HttpGet("GetOneByID/{mentorId}")]

        public async Task<JsonResult> GetMentor(string mentorId)
        {
            var mentor = await istructorManager.GetMentorByIdAsync(mentorId);
            if (mentor == null)
            {
                return new JsonResult(new { Message = "Mentor not found" }) { StatusCode = StatusCodes.Status404NotFound };
            }
            return new JsonResult(mentor);
        }

        [HttpGet("GetUserByID/{userId}")]

        public async Task<JsonResult> GetUser(string userId)
        {
            var mentor =  istructorManager.GetById(userId);
            if (mentor == null)
            {
                return new JsonResult(new { Message = "Mentor not found" }) { StatusCode = StatusCodes.Status404NotFound };
            }
            return new JsonResult(mentor);
        }
    


        [HttpGet("GetOneUser/{id}")]
        public async Task<IActionResult> GetOneUser(string id)
        {
            var user = await accountManager.GetOneUser(id);
            if (user == null)
            {
                return new JsonResult(new { Message = "Mentor not found" }) { StatusCode = StatusCodes.Status404NotFound };
            }
            return new JsonResult(user);
        }
        [HttpGet("GetOneUserByClaim")]
        public async Task<IActionResult> GetOneUserByClaim()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { message = "User is not authenticated." });
            }
            var user = await accountManager.GetOneUser(userId);
            if (user == null)
            {
                return new JsonResult(new { Message = "Mentor not found" }) { StatusCode = StatusCodes.Status404NotFound };
            }
            return new JsonResult(user);
        }

        [HttpGet("GetReviewByClaim")]
        public async Task<IActionResult> GetReviewByClaim()
        {
            // Eagerly load related entities for reviews, including Session and User
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { message = "User is not authenticated." });
            }

            // Retrieve the logged-in user
           // var user = await userManager.FindByIdAsync(userId);
            //if (user == null)
            //{
            //    return NotFound(new { message = "User not found." });
            //}

            // Fetch the session IDs from reviews
            var user = await accountManager.GetReview(userId);
            if (user == null)
            {
                return new JsonResult(new { Message = "Mentor not found" }) { StatusCode = StatusCodes.Status404NotFound };
            }
            return new JsonResult(user);
        }



        [HttpGet("GetReview/{id}")]
        public async Task<IActionResult> GetReview(string id)
        {
            var user = await accountManager.GetReview(id);
            if (user == null)
            {
                return new JsonResult(new { Message = "Mentor not found" }) { StatusCode = StatusCodes.Status404NotFound };
            }
            return new JsonResult(user);
        }


        [HttpGet("GetSession/{id}")]
        public IActionResult GetSession(int id)
        {
            return new JsonResult(accountManager.GetSessionById(id));
        }


        [HttpGet("Mentors")]
        public async Task<IActionResult> GetMentors()
        {
            try
            {
                var mentors = await istructorManager.GetTopRatedMentors();
                if (mentors == null || !mentors.Any())
                {
                    return new JsonResult(new { Message = "No mentors found" }) { StatusCode = 404 };
                }

                var mentorInfo = mentors.Select(mentor => new
                {
                    mentor.Id,
                    mentor.FirstName,
                    mentor.LastName,
                    mentor.Title,
                    mentor.YearsOfExperience,
                    mentor.Image,
                    mentor.Price,
                    mentor.Country,
                    mentor.About,
                    mentor.Level
                });

                return new JsonResult(mentorInfo) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Message = "An error occurred", Error = ex.Message }) { StatusCode = 500 };
            }
        }




        [HttpGet("HRs")]
        public async Task<IActionResult> GetHRs()
        {
            try
            {
                var HRs = await istructorManager.GetTopRatedHRs();
                if (HRs == null || !HRs.Any())
                {
                    return new JsonResult(new { Message = "No HRs found" }) { StatusCode = 404 };
                }

                var hrInfo = HRs.Select(hr => new
                {
                    hr.Id,
                    hr.FirstName,
                    hr.LastName,
                    hr.Title,
                    hr.YearsOfExperience,
                    hr.Image,
                    hr.Price,
                    hr.Country,
                    hr.About,
                    hr.Level
                });

                return new JsonResult(hrInfo) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Message = "An error occurred", Error = ex.Message }) { StatusCode = 500 };
            }
        }






        //[HttpGet("HR")]
        //public async Task<IActionResult> GetHR()
        //{
        //    try
        //    {
        //        var HR = await istructorManager.GetHRsAsync();
        //        if (HR == null || !HR.Any())
        //        {
        //            return new JsonResult(new { Message = "No HR found" }) { StatusCode = 404 };
        //        }

        //        var HRInfo = HR.Select(HR => new
        //        {
        //            HR.Id,
        //            HR.FirstName,
        //            HR.LastName,
        //            HR.Title,
        //            HR.YearsOfExperience,
        //            HR.Image,
        //            HR.Price,
        //            HR.Country,
        //            HR.About,
        //            HR.Level
        //        });

        //        return new JsonResult(HRInfo) { StatusCode = 200 };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new JsonResult(new { Message = "An error occurred", Error = ex.Message }) { StatusCode = 500 };
        //    }
        //}



















        //[HttpGet("TopRated")]
        //public async Task<IActionResult> GetTopRatedUsers()
        //{
        //    try
        //    {
        //        var mentors = await istructorManager.GetMentorsAsync();
        //        var HRs = await istructorManager.GetHRsAsync();

        //        var topRated = new
        //        {
        //            Mentors = mentors.Select(mentor => new
        //            {
        //                mentor.Id,
        //                mentor.FirstName,
        //                mentor.LastName,
        //                mentor.Title,
        //                mentor.YearsOfExperience,
        //                mentor.Image,
        //                mentor.Price,
        //                mentor.Country,
        //                mentor.About,
        //                mentor.Level
        //            }),
        //            HRs = HRs.Select(hr => new
        //            {
        //                hr.Id,
        //                hr.FirstName,
        //                hr.LastName,
        //                hr.Title,
        //                hr.YearsOfExperience,
        //                hr.Image,
        //                hr.Price,
        //                hr.Country,
        //                hr.About,
        //                hr.Level
        //            })
        //        };

        //        return new JsonResult(topRated) { StatusCode = 200 };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new JsonResult(new { Message = "An error occurred", Error = ex.Message }) { StatusCode = 500 };
        //    }
        //}
        [HttpGet("GetUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await accountManager.GetAllAsync(); // Ensure this fetches all users asynchronously

            // Create a list to hold user results
            var userResults = new List<object>();

            foreach (var user in users)
            {
                var roles = await accountManager.GetUserRolesAsync(user.Id); // Await roles for each user sequentially
                userResults.Add(new
                {
                    user.UserName,
                    user.Email,
                    user.PhoneNumber,
                    user.CV,
                    user.Image,
                    Roles = string.Join(", ", roles) // Join roles into a single string
                });
            }

            return Ok(userResults);
        }
        [HttpPut("Chnange dev to mentor")]
        // [Authorize(Roles ="Admin")]
        //public async Task<bool> ConvertDeveloperToMentorByAdminAsync()
        //{
        //    User_Quiz userQuiz = new User_Quiz(); 
        //    var user = userQuiz.User;

        //    // Check if the user passed the quiz and is currently in the Developer role
        //    if (userQuiz.Result && await userManager.IsInRoleAsync(user, "Developer"))
        //    {
        //        await userManager.RemoveFromRoleAsync(user, "Developer");
        //        await userManager.AddToRoleAsync(user, "Mentor");
        //        return true;
        //    }

        //    return false; // No role change if conditions aren't met
        //}
        public async Task<IActionResult> ConvertDeveloperToMentor(int userQuizId)
        {
            // Fetch User_Quiz from database (this example assumes you have a service to get it)
            var userQuiz = await user_QuizeManager.GetUserQuizWithDetailsAsync(userQuizId);
            string userid1 = userQuiz.User_Id;

            if (userQuiz == null)
            {
                return NotFound("Quiz not found.");
            }

            var success = await accountManager.ConvertDeveloperToMentorByAdminAsync(userQuiz,userid1);

            if (success)
            {
                return Ok("User role updated to Mentor successfully.");
            }

            return BadRequest("User role update failed.");
        }
        [HttpPost("complete quiz")]
        public async Task<IActionResult> CompleteQuiz()
        {
            // Get the currently logged-in user
            var currentUser = await userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return Unauthorized("User not found or not logged in.");
            }

            // Process the quiz completion (assume this sets Result for the latest quiz)

            // Attempt to convert to mentor if the user passed the quiz
            var roleUpdated = await accountManager.ConvertDeveloperToMentorAsync(currentUser);

            if (roleUpdated)
            {
                return Ok("User role updated to Mentor successfully.");
            }

            return Ok("Quiz completed. No role update was necessary.");
        }
        [HttpGet("getallemntor")]
        public async Task<IActionResult> AllGetMentors()
        {
            var Success_to_Quiz = user_QuizeManager
            .GetAll()
            .Include(s => s.User).Where(s => s.Result == true)// Include User details
            .ToList();

            // Get users with the specified role (e.g., "Mentor") using UserManager
            var mentorUsers = await userManager.GetUsersInRoleAsync("Mentor");

            // Filter the quizzes to include only those associated with mentor users
            var filteredMentors = Success_to_Quiz
               .Where(q => mentorUsers.Any(u => u.Id == q.User.Id))
               .ToList();
            return new JsonResult(filteredMentors);

        }
        [HttpGet("getallHR")]
        public async Task<IActionResult> HRss()
        {
            var Success_to_Quiz = user_QuizeManager
            .GetAll()
            .Include(s => s.User).Where(s => s.Result == true)// Include User details
            .ToList();

            // Get users with the specified role (e.g., "Mentor") using UserManager
            var HRUsers = await userManager.GetUsersInRoleAsync("HR");

            // Filter the quizzes to include only those associated with mentor users
            var filteredHRs = Success_to_Quiz
               .Where(q => HRUsers.Any(u => u.Id == q.User.Id))
               .ToList();
            return new JsonResult(filteredHRs);

        }



    }




}



