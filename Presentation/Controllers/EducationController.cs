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
    public class EducationController : ControllerBase
    {
        private EducationManager educationManager;

        public EducationController(EducationManager _educationManager)
        {
            this.educationManager = _educationManager;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<JsonResult> AddEducation([FromBody] EducationViewModel model)
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

            var education = new Education
            {
                Degree = model.Degree,
                FieldOfStudy = model.FieldOfStudy,
                University = model.University,
                Faculty = model.Faculty,
                CountryOfStudy = model.CountryOfStudy,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                TillNow = model.TillNow,
                User_Id = userId
            };

            try
            {
                // Add the education entry via service
                var result = educationManager.Add(education);

                if (result)
                {
                    return new JsonResult(new { Success = true, Message = "Education record added successfully" })
                    {
                        StatusCode = 201
                    };
                }
                else
                {
                    return new JsonResult(new { Success = false, Message = "Failed to save the education record" })
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
                    Message = "An internal error occurred while saving the education record",
                    Details = ex.Message
                })
                {
                    StatusCode = 500
                };
            }
        }

    }
}
