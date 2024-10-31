using Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System.Security.Claims;
using ViewModels;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExperienceController : ControllerBase
    {
        private ExperienceManager experienceManager;

        public ExperienceController(ExperienceManager _experienceManager)
        {
            this.experienceManager = _experienceManager;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<JsonResult> AddExperience([FromBody] ExperienceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Return a structured JSON response with validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return new JsonResult(new { Success = false, Message = "Validation failed", Errors = errors })
                {
                    StatusCode = 400
                };
            }

            // Get the user ID from the token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return new JsonResult(new { Success = false, Message = "Unauthorized access" })
                {
                    StatusCode = 401
                };
            }

            var experience = new Experience
            {
                
                FieldOfStudy = model.FieldOfStudy,
                Organization=model.Organization,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                TillNow = model.TillNow,
                User_Id = userId
            };

            try
            {
                // Add the education entry via service
                var result = experienceManager.Add(experience);

                if (result)
                {
                    return new JsonResult(new { Success = true, Message = "Experience record added successfully" })
                    {
                        StatusCode = 201
                    };
                }
                else
                {
                    return new JsonResult(new { Success = false, Message = "Failed to save the Experience record" })
                    {
                        StatusCode = 500
                    };
                }
            }
            catch (Exception ex)
            {
                // Log the error as necessary

                return new JsonResult(new
                {
                    Success = false,
                    Message = "An internal error occurred while saving the Experience record",
                    Details = ex.Message
                })
                {
                    StatusCode = 500
                };
            }
        }
    }
}
