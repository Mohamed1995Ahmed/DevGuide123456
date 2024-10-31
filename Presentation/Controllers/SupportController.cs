using DevGuide.Models;
using DevGuide.Models.Models;
using Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ViewModels;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupportController : ControllerBase
    {
        private readonly SupportManager _supportManager;
        private readonly UserManager<User> userManager;

        public SupportController(SupportManager supportManager,UserManager<User> userManager)
        {
            _supportManager = supportManager;
            this.userManager = userManager;
        }


        [HttpPost("SubmitSupport")]
        [Authorize]
        public async Task<IActionResult> SubmitSupport([FromBody] SupportViewModel supportViewModel)
        {
            // Retrieve User_Id from claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return new JsonResult(new { message = "User not found" });
            }

            // Map the ViewModel to the Support model
            var support = new Support
            {
                Email = supportViewModel.Email,
                Username = supportViewModel.Username,
                PhoneNumber = supportViewModel.PhoneNumber,
                ObjectOfComplain = supportViewModel.ObjectOfComplain,
                Message = supportViewModel.Message,
                //Date = supportViewModel.Date,
                User_Id = userId
            };

            // Add to the database
            var result = await _supportManager.AddSupportAsync(support);

            // Return response
            return new JsonResult(result ? new { message = "Support submitted successfully" } : new { message = "Failed to submit support" });
        }
        [HttpGet("GetContactInfo")]
        public async Task<IActionResult> GetContactInfo()
        {
            // Retrieve user from the user manager or database by userId
            var user = await _supportManager.GetAllSessionsAsync();
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            // Map user data to the ContactViewModel
            var contactInfo = user.Select(s=> new ContactViewModel
            {
                Email= s.Email,
                Username= s.Username,
                PhoneNumber= s.PhoneNumber,
                ObjectOfComplain= s.ObjectOfComplain,
                Message= s.Message,
                
               
            });

            return Ok(contactInfo);
        }

    }
}