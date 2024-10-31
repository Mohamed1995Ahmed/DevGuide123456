using DevGuide.Models.Models;
using Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;
using System.Security.Claims;
using ViewModels;
using ViewModels.review1;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewManager reviewManager;
        private readonly SessionManager sessionManager;

        public ReviewController(ReviewManager reviewManager, SessionManager sessionManager)
        {
            this.reviewManager = reviewManager;
            this.sessionManager = sessionManager;
        }

   //     [HttpPost("Add Review")]
   //     [Authorize]
   //     public async Task<IActionResult> CreateReview([FromForm] view123 review1, int session_Id, int sessionId)
   //     {
   //         // Check if the Session_Id exists in the database
   //         string USer_Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
   //         // Fetch the session asynchronously based on User_Id and session_Id
   //         var session = await sessionManager.GetAll()
   //.Where(s => s.Id == session_Id && s.User_Id == USer_Id) // Verify session ID and developer's ID
   //.Select(s => new { s.User_Instructor_Id, s.User_Id }) // Only select User_Instructor_Id
   //.FirstOrDefaultAsync();
   //         string Mentor_id = session.User_Instructor_Id;
   //         string user_id = session.User_Id;

   //         //      var session1 = await sessionManager.GetAll()
   //         //.Include(s => s.User).Where(s => s.Id == session_Id && s.User_Id == USer_Id) // Include the user who booked the session
   //         //.FirstOrDefaultAsync();
   //         //       var session = await sessionManager.GetAll()
   //         //.Include(s => s.User)  // Include the user who booked the session
   //         //.FirstOrDefaultAsync(s => s.Id == session_Id && s.User_Id == user_Id);

   //         //return session;
   //         // Check if the session was found
   //         if (session == null)
   //         {
   //             return BadRequest("Invalid Session_Id: Session does not exist.");
   //         }

   //         // Retrieve the Mentor_id

   //         //        [HttpPost("Add Review")]
   //         //        [Authorize]
   //         //        public async Task<IActionResult> CreateReview([FromForm] view123 review1, int session_Id)
   //         //        {
   //         //            // Check if the Session_Id exists in the database
   //         //            string USer_Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
   //         //            // Fetch the session asynchronously based on User_Id and session_Id
   //         //            var session = await sessionManager.GetAll()
   //         //      .Where(s => s.Id == session_Id && s.User_Id == USer_Id) // Verify session ID and developer's ID
   //         //      .Select(s => new { s.User_Instructor_Id, s.User_Id }) // Only select User_Instructor_Id
   //         //      .FirstOrDefaultAsync();

   //         //            // Check if the session was found
   //         //            if (session == null)
   //         //            {
   //         //                return BadRequest("Invalid Session_Id: Session does not exist.");
   //         //            }

   //         //            // Retrieve the Mentor_id
   //         //            string Mentor_id = session.User_Instructor_Id;
   //         //            string user_id = session.User_Id;


   //         //  string user_id = session1.User_Id;


   //         //            //var session1 = sessionManager.GetByIdAsync(review.Session_Id);


   //         if (USer_Id == user_id)
   //         {
   //             Review review = new Review()
   //             {
   //                 Rate = review1.Rate,
   //                 ReviewDate = review1.ReviewDate,
   //                 Description = review1.Description,
   //                 Session_Id = session_Id,
   //                 UserId = session.User_Instructor_Id,
   //             };
   //             return Ok(review);
   //         }





   //         //            if (USer_Id == user_id)
   //         //            {
   //         //                Review review = new Review()
   //         //                {
   //         //                    Rate = review1.Rate,
   //         //                    ReviewDate = review1.ReviewDate,
   //         //                    Description = review1.Description,
   //         //                    Session_Id = session_Id,
   //         //                    UserId = session.User_Instructor_Id,






   //         //                };
   //         //                reviewManager.Add(review);
   //         //                return Ok(review);


   //         //            }
   //         //            return BadRequest("not add");



   //         return Ok("not create");

            
   //     }
        [HttpGet("GetREVIEWS")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSessions()
        {
            var reviews1 = await reviewManager.GetAllReviewsAsync();

            var reviews = reviews1.Select(s => new ReviewGet
            {
                Description = s.Description,
                Rating = s.Rate,
                DeveloperName = s.Session?.User?.UserName ?? "N/A",                  // Handle potential null values
                MentorName = s.Session?.User_Instructor?.UserName ?? "N/A"
            }).ToList();

            return Ok(reviews);
        }




        //        }


        //    }
        //}







        [HttpPost("AddReview/{session_Id}")]
        [Authorize]
        public async Task<IActionResult> CreateReview([FromBody] view123 review1, [FromRoute] int session_Id)
        {
            // Retrieve User_Id from claims
            string user_Id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fetch the session based on User_Id and session_Id
            var session = await sessionManager.GetAll()
                .Where(s => s.Id == session_Id && s.User_Id == user_Id) // Verify session ID and developer's ID
                .Select(s => new { s.User_Instructor_Id, s.User_Id }) // Only select User_Instructor_Id
                .FirstOrDefaultAsync();

            // Check if the session was found
            if (session == null)
            {
                return new JsonResult(new { Message = "Invalid Session_Id: Session does not exist." }) { StatusCode = 400 };
            }

            // Retrieve Mentor_id
            string mentor_Id = session.User_Instructor_Id;

            // Verify if the current user is authorized to add a review
            if (user_Id == session.User_Id)
            {
                // Create the Review object
                var review = new Review
                {
                    Rate = review1.Rate,
                    ReviewDate = review1.ReviewDate,
                    Description = review1.Description,
                    Session_Id = session_Id,
                    UserId = mentor_Id
                };

                // Add the review using the review manager
                reviewManager.Add(review);
                return new JsonResult(new { Message = "Review added successfully", Review = review }) { StatusCode = 200 };
            }

            return new JsonResult(new { Message = "Review not added" }) { StatusCode = 400 };
        }

    }
}



