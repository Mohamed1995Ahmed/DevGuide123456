using DevGuide.Models.Models;
using Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ViewModels;

namespace Presentation.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        private SessionManager _sessionManager;
        public SessionController(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        [HttpGet("GetAllSessionForOneUser")]
        [Authorize]
        public List<SessionListViewModel> GetAllSessionForOneUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return new List<SessionListViewModel>();
            }
            //if (Statues.HasValue)
            //{
            //    var sessions = _sessionManager.GetAll().Where(s => s.User_Instructor_Id == userId && s.Status == Statues).ToList();
            //    var sessionsviews = new List<SessionListViewModel>();
            //    foreach (var session in sessions)
            //    {
            //        sessionsviews.Add(session.ToSessionListViewModel());
            //    }

            //    return sessionsviews;
            //}
            //else
            //{
                var sessions = _sessionManager.GetAll().Where(s => s.User_Instructor_Id == userId).ToList();
                var sessionsviews = new List<SessionListViewModel>();
                foreach (var session in sessions)
                {
                    sessionsviews.Add(session.ToSessionListViewModel());
                }

                return sessionsviews;
            //}
        }


        [HttpGet("GetAllSessionForOneDeveloper")]
        [Authorize]
        public List<SessionListViewModel> GetAllSessionForOneDeveloper()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return new List<SessionListViewModel>();
            }
        

                var sessions = _sessionManager.GetAll().Where(s => s.User_Id == userId).ToList();
                var sessionsviews = new List<SessionListViewModel>();
                foreach (var session in sessions)
                {
                    sessionsviews.Add(session.ToDeveloperSessionListViewModel());
                }

                return sessionsviews;
        }


        //[HttpGet("GetSessionById/{id}")]
        //[Authorize]
        //public async Task<IActionResult> GetSessionById(int id)
        //{
        //    var session = await _sessionManager.GetByIdAsync(id);

        //    if (session == null)
        //    {
        //        return new JsonResult(new { Message = "Session not found." });
        //    }

        //    // Map to SessionDetailViewModel
        //    var sessionViewModel = new SessionDetailViewModel
        //    {
        //        Id = session.Id,
        //        Topic = session.Topic,
        //        Description = session.Description,
        //        DateTime = session.DateTime,
        //        Duration = session.Duration,
        //        MeetingLink = session.MeetingLink,
        //        Feedback = session.Feedback,
        //        Notes = session.Notes,
        //        Status = session.Status,
        //        UserId = session.User_Id,
        //        UserImage = session.User.Image,
        //        UserTitle = session.User.Title,
        //        UserEmail = session.User.Email,
        //        InstructorId = session.User_Instructor_Id,
        //        Price = session.Payment.Total,
        //        UserFirstName = session.User.FirstName,
        //        UserLastName = session.User.LastName,
        //    };

        //    return new JsonResult(sessionViewModel);
        //}


        [HttpGet("GetSessionById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetSessionById(int id)
        {
            var session = await _sessionManager.GetByIdAsync(id);

            if (session == null)
            {
                return new JsonResult(new { Message = "Session not found." });
            }

            // Map to SessionDetailViewModel
            var sessionViewModel = new SessionDetailViewModel
            {
                Id = session.Id,
                Topic = session.Topic,
                Description = session.Description,
                DateTime = session.DateTime,
                Duration = session.Duration,
                MeetingLink = session.MeetingLink,
                Feedback = session.Feedback,
                Notes = session.Notes,
                Status = session.Status,
                UserId = session.User_Id,
                UserImage = session.User.Image,
                UserTitle = session.User.Title,
                UserEmail = session.User.Email,
                InstructorId = session.User_Instructor_Id,
                Price = session.Payment.Total,
                UserFirstName = session.User.FirstName,
                UserLastName = session.User.LastName,
                MentorFirstName= session.User_Instructor.FirstName,
                MentorLastName= session.User_Instructor.LastName,
                MentorImage= session.User_Instructor.Image,
                InstructorEmail=session.User_Instructor.Email,
                MentorTitle= session.User_Instructor.Title,
            };

            return new JsonResult(sessionViewModel);
        }

        [HttpGet("GetSessions")]
        [Authorize(Roles = "Admin, Mentor, Developer")]
        public async Task<IActionResult> GetSessions()
        {
            var sessions = await _sessionManager.GetAllSessionsAsync();

            var sessionResults = sessions.Select(s => new SessionViewModel
            {
                Topic = s.Topic,
                DateTime = s.DateTime,
                UserName = s.User?.UserName ?? "N/A",                  // Handle potential null values
                InstructorName = s.User_Instructor?.UserName ?? "N/A"
            }).ToList();

            return Ok(sessionResults);


            //[HttpPost("AddFeedback")]
            //[Authorize]
            //public IActionResult AddFeedback(int sessionId, [FromBody] string feedback)
            //{
            //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //    // Ensure that the user owns the session or has the right permissions
            //    var session = _sessionManager.GetAll().FirstOrDefault(s => s.Id == sessionId && s.User_Id == userId);

            //    if (session == null)
            //    {
            //        return new JsonResult(new { Success = false, Message = "Session not found or access denied." });
            //    }

            //    var success = _sessionManager.AddFeedback(sessionId, feedback);
            //    if (success)
            //    {
            //        return new JsonResult(new { Success = true, Message = "Feedback added successfully." });
            //    }

            //    return new JsonResult(new { Success = false, Message = "Failed to add feedback." });
            //}

            //[HttpPut("UpdateFeedback/{sessionId}")]
            //[Authorize]
            //public async Task<IActionResult> UpdateFeedback(int sessionId, string feedback)
            //{
            //    if (string.IsNullOrWhiteSpace(feedback))
            //    {
            //        return new JsonResult(new { message = "Feedback cannot be empty." });
            //    }

            //    // Retrieve the session
            //    var session = await _sessionManager.GetByIdAsync(sessionId);

            //    if (session == null)
            //    {
            //        return new JsonResult(new { message = "Session not found." });
            //    }

            //    // Update the feedback field
            //    session.Feedback = feedback;

            //    var result = _sessionManager.Update(session);

            //    if (result)
            //    {
            //        return new JsonResult(new { message = "Feedback updated successfully." });
            //    }
            //    else
            //    {
            //        return new JsonResult(new { message = "Failed to update feedback." });
            //    }
            //}
        }

        [HttpPut("UpdateFeedback/{sessionId}")]
        [Authorize]
        public async Task<IActionResult> UpdateFeedback(int sessionId, [FromBody] AddFeedbackViewModel feedbackDto)
        {
            if (feedbackDto == null || string.IsNullOrWhiteSpace(feedbackDto.Feedback))
            {
                return new JsonResult(new { message = "Feedback cannot be empty." });
            }

            var session = await _sessionManager.GetByIdAsync(sessionId);

            if (session == null)
            {
                return new JsonResult(new { message = "Session not found." });
            }

            // Update the feedback field
            session.Feedback = feedbackDto.Feedback;

            var result = _sessionManager.Update(session);

            if (result)
            {

                session.Status = BookingStatus.Completed;
                result = _sessionManager.Update(session);
                return new JsonResult(new { message = "Feedback Send successfully." });
            }
            else
            {
                return new JsonResult(new { message = "Failed to Send feedback." });
            }

        }
  
        
        [HttpPut("UpdateFeedbackForCanceled/{sessionId}")]
[Authorize]
public async Task<IActionResult> UpdateFeedbackForCanceled(int sessionId, [FromBody] AddFeedbackViewModel feedbackDto)
{
    if (feedbackDto == null || string.IsNullOrWhiteSpace(feedbackDto.Feedback))
    {
        return new JsonResult(new { message = "Feedback cannot be empty." });
    }

    var session = await _sessionManager.GetByIdAsync(sessionId);

    if (session == null)
    {
        return new JsonResult(new { message = "Session not found." });
    }

    // Update the feedback field
    session.Feedback = feedbackDto.Feedback;

    var result = _sessionManager.Update(session);

    if (result)
    {

        
       
        return new JsonResult(new { message = "Feedback Send successfully." });
    }
    else
    {
        return new JsonResult(new { message = "Failed to Send feedback." });
    }

}

       [HttpDelete("CancelSession/{id}")]
 [Authorize]
 public async Task<IActionResult> CancelSession(int id)
 {
    var UserId=User.FindFirstValue(ClaimTypes.NameIdentifier);
     if (string.IsNullOrEmpty(UserId))
     {
         return Unauthorized(new { message = "User is not authenticated." });
     }
     // Locate the session by ID
     var session =  _sessionManager.GetById(id);
     if (session == null)
     {
         return NotFound(new { message = "Session not found." });
     }
     if (session.User_Id != UserId)
     {
         return Ok( "You are not authorized to cancel this session.");
     }

     // Delete the session
     var sessionDeleted =  _sessionManager.Delete(session);
     if (!sessionDeleted)
     {
         return BadRequest(new { message = "Failed to cancel and delete the session." });
     }

     return Ok(new { message = "Session canceled and deleted successfully!" });
 }






        [HttpPut("UpdateMeetingLink/{sessionId}")]
        [Authorize]
        public async Task<IActionResult> UpdateMeetingLink(int sessionId, [FromBody] AddFeedbackViewModel MeetingLink)
        {
            if (MeetingLink == null || string.IsNullOrWhiteSpace(MeetingLink.Feedback))
            {
                return new JsonResult(new { message = "Feedback cannot be empty." });
            }

            var session = await _sessionManager.GetByIdAsync(sessionId);

            if (session == null)
            {
                return new JsonResult(new { message = "Session not found." });
            }

            // Update the feedback field
            session.MeetingLink = MeetingLink.Feedback;

            var result = _sessionManager.Update(session);

            if (result)
            {
                return new JsonResult(new { message = "MeetingLink Send successfully." });
            }
            else
            {
                return new JsonResult(new { message = "Failed to Send MeetingLink." });
            }

        }

    }
}