using DevGuide.Models.Models;
using Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Security.Claims;
using ViewModels;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ScheduleManager _scheduleManager;
        private AccountManager _accountManager;
        private readonly SessionManager sessionManager;
        private readonly UserManager<User> usermanager;

        public ScheduleController(ScheduleManager scheduleManager, UserManager<User> _usermanager, AccountManager accountManager, SessionManager _sessionManager)
        {
            _scheduleManager = scheduleManager;
            usermanager = _usermanager;
            _accountManager = accountManager;
            sessionManager = _sessionManager;
        }




        [HttpPost("CheckAndHandleSchedule")]
        [Authorize]
        public async Task<IActionResult> CheckAndHandleSchedule([FromBody] AddScheduleViewModel model)
        {
            // Get the logged-in user's ID
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { message = "User is not authenticated." });
            }

            // Retrieve the logged-in user
            var user = await usermanager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            // Step 1: Retrieve existing schedules for the user
            var existingSchedules = _scheduleManager.GetAll()
                .Where(s => s.User_Id == userId)
                .ToList();

            // Step 2: Loop through the new schedules
            foreach (var schedule in model.Schedules)
            {
                // Check if the schedule is available
                if (schedule.Available == true)
                {
                    // If available, check if it exists in the existing schedules
                    var existingSchedule = existingSchedules.FirstOrDefault(s => s.Day == schedule.Day);

                    if (existingSchedule != null)
                    {
                        // If it exists, update the existing schedule
                        if ((schedule.StartTime.Minutes == 0 && schedule.StartTime.Seconds == 0) &&
                            (schedule.EndTime.Minutes == 0 && schedule.EndTime.Seconds == 0))
                        {
                            existingSchedule.StartTime = schedule.StartTime;
                            existingSchedule.EndTime = schedule.EndTime;

                            // Save the updated schedule
                            _scheduleManager.Update(existingSchedule);
                        }
                        else
                        {
                            return BadRequest(new { message = "Invalid schedule times. Start and End times should be on the hour." });
                        }
                    }
                    else
                    {
                        // If it doesn't exist, add a new schedule
                        if ((schedule.StartTime.Minutes == 0 && schedule.StartTime.Seconds == 0) &&
                            (schedule.EndTime.Minutes == 0 && schedule.EndTime.Seconds == 0))
                        {
                            Schedule newSchedule = new Schedule
                            {
                                Day = schedule.Day,
                                StartTime = schedule.StartTime,
                                EndTime = schedule.EndTime,
                                User_Id = userId
                            };

                            bool resultSchedule = _scheduleManager.Add(newSchedule);
                            if (!resultSchedule)
                            {
                                return BadRequest(new { message = "Failed to add schedule." });
                            }
                        }
                        else
                        {
                            return BadRequest(new { message = "Invalid schedule times. Start and End times should be on the hour." });
                        }
                    }
                }
                else
                {
                    // If the schedule is not available, check if it exists and delete it
                    var existingSchedule = existingSchedules.FirstOrDefault(s => s.Day == schedule.Day);
                    if (existingSchedule != null)
                    {
                        // Delete the schedule
                        _scheduleManager.Delete(existingSchedule); // Assuming you have a Delete method in _scheduleManager
                    }
                }
            }

            // Step 3: Update the user's price if provided
            if (model.Price.HasValue && model.Price > 0)
            {
                user.Price = model.Price.Value;
                _accountManager.Update(user);  // Assuming you have an Update method in _accountManager
            }

            return Ok(new { message = "Schedules updated successfully." });
        }


        private async Task<IActionResult> AddSchedule([FromForm] ScheduleViewModel model)
        {
            // Get the logged-in user's ID
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return BadRequest(new { message = "User is not authenticated." });
            }

            // Retrieve the logged-in user
            var user = await usermanager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }



            // Retrieve all schedules for the given mentor and date
            var schedules = _scheduleManager.GetAll()
                .Where(s => s.User_Id == userId && s.Day == model.Day)
                .ToList();

            // Check if any schedule conflicts with the provided time range
            bool isMentorAvailable = !schedules.Any(s =>
                (s.StartTime >= model.StartTime && s.StartTime < model.EndTime) ||  // Overlaps start time
                (s.EndTime > model.StartTime && s.EndTime <= model.EndTime));        // Overlaps end time

            if (!isMentorAvailable)
            {
                return BadRequest(new { message = "Mentor is not available at the given time." });
            }

            if ((model.StartTime.Minutes == 0 && model.StartTime.Seconds == 0) && (model.EndTime.Minutes == 0 && model.EndTime.Seconds == 0))
            {
                //return BadRequest(new { message = "The Minutes and Seconds must be 00 and 00" });
                Schedule newSchedule = new Schedule
                {
                    Day = model.Day,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    User_Id = userId
                };



                // Add the new schedule
                bool resultSchedule = _scheduleManager.Add(newSchedule);

                if (resultSchedule)
                {
                    return Ok(new { message = "Schedule added successfully" });
                }

            }

            return BadRequest(new { message = "Failed to add schedule." });
        }
        private async Task<IActionResult> EditScheduleInternal(ScheduleViewModel model, int scheduleId, string userId, User user)
        {
            // Find the existing schedule by ID and ensure it belongs to the logged-in user
            var existingSchedule = _scheduleManager.GetAll()
                .FirstOrDefault(s => s.Id == scheduleId && s.User_Id == userId);

            if (existingSchedule == null)
            {
                return NotFound(new { message = "Schedule not found for the given mentor." });
            }

            // Check if model fields are not null before updating the schedule
            if (model.Day != null)
            {
                existingSchedule.Day = model.Day;
            }
            if (model.StartTime != null)
            {
                existingSchedule.StartTime = model.StartTime;
            }
            if (model.EndTime != null)
            {
                existingSchedule.EndTime = model.EndTime;
            }

            var schedules = _scheduleManager.GetAll()
                .Where(s => s.User_Id == userId && s.Day == model.Day && s.Id != scheduleId)
                .ToList();

            bool isMentorAvailable = !schedules.Any(s =>
                (s.StartTime >= model.StartTime && s.StartTime < model.EndTime) ||
                (s.EndTime > model.StartTime && s.EndTime <= model.EndTime));

            if (!isMentorAvailable)
            {
                return BadRequest(new { message = "Mentor is not available at the given time." });
            }
            if ((model.StartTime.Minutes == 0 && model.StartTime.Seconds == 0) && (model.EndTime.Minutes == 0 && model.EndTime.Seconds == 0))
            {

                // Save the changes to the schedule
                bool updateResult = _scheduleManager.Update(existingSchedule);

                if (updateResult)
                {
                    return Ok(new { message = "Schedule updated successfully" });
                }
            }

            return BadRequest(new { message = "Failed to update schedule." });
        }

        [HttpGet("GetSchedulesWithPrice")]
        [Authorize]
        public IActionResult GetSchedulesWithPrice()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Validate if userId (mentorId) is provided
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { message = "User ID is required." });
            }

            // Retrieve user information
            var user = _accountManager.GetById(userId);

            // Get all days from the enum
            var allDays = Enum.GetValues(typeof(Day)).Cast<Day>().ToList();

            // Retrieve existing schedules for the user
            var userSchedules = _scheduleManager.GetAll()
                .Where(s => s.User_Id == userId)
                .ToList();

            // Build the schedules list, including all days
            var schedules = new AddScheduleViewModel
            {
                Schedules = allDays.Select(day =>
                {
                    // Check if this day exists in the user's schedule
                    var scheduleForDay = userSchedules.FirstOrDefault(s => s.Day == day);

                    if (scheduleForDay != null)
                    {
                        // If a schedule exists for this day, return it
                        return new ScheduleViewModel
                        {
                            Day = scheduleForDay.Day,
                            Available = true,
                            StartTime = scheduleForDay.StartTime,
                            EndTime = scheduleForDay.EndTime
                        };
                    }
                    else
                    {
                        // If no schedule exists for this day, return a default unavailable schedule
                        return new ScheduleViewModel
                        {
                            Day = day,
                            Available = false,
                            StartTime = TimeSpan.Zero, // Or use an empty string as you prefer
                            EndTime = TimeSpan.Zero    // Or use an empty string as you prefer
                        };
                    }
                }).ToList(),

                // Assign the user's price
                Price = user.Price
            };

            // If no schedules found, return a not found response
            if (schedules == null || schedules.Schedules.Count == 0)
            {
                return NotFound(new { message = "No schedules found for this user." });
            }

            // Return the schedules with price as a JSON response
            return new JsonResult(schedules);
        }


        //[HttpGet("GetSchedulesWithPrice")]
        //[Authorize]
        //public IActionResult GetSchedulesWithPrice()
        //{
        //    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    // Validate if mentorId is provided
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return BadRequest(new { message = "Mentor ID is required." });
        //    }
        //    //var user = usermanager.FindByIdAsync(userId);
        //    var user=_accountManager.GetById(userId);
        //    // Retrieve schedules for the given mentor ID
        //    var schedules =new AddScheduleViewModel {Schedules= _scheduleManager.GetAll()
        //        .Where(s => s.User_Id == userId)
        //        .Select(s => new ScheduleViewModel
        //        {
        //            Day = s.Day,
        //            Available = true,
        //            StartTime = s.StartTime,
        //            EndTime = s.EndTime,

        //        }).ToList(),
        //    Price=user.Price};


        //    if (schedules == null || schedules.Schedules.Count == 0)
        //    {
        //        return NotFound(new { message = "No schedules found for this mentor." });
        //    }

        //    return new JsonResult(schedules);
        //}






        [HttpGet("GetSchedules")]
        [Authorize]
        public IActionResult GetSchedulesForUser()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Validate if mentorId is provided
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { message = "Mentor ID is required." });
            }
            //var user = usermanager.FindByIdAsync(userId);
            // Retrieve schedules for the given mentor ID
            var schedules = _scheduleManager.GetAll()
                .Where(s => s.User_Id == userId)
                .Select(s => new ScheduleViewModel
                {
                    Day = s.Day,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    Available = true                    //Price=user.P
                    //MentorId = s.User_Id
                }).ToList();


            if (schedules == null || schedules.Count == 0)
            {
                return NotFound(new { message = "No schedules found for this mentor." });
            }

            return new JsonResult(schedules);
        }



        [HttpGet("GetSchedulesById")]
        [Authorize]
        public IActionResult GetSchedulesByUserId(string id) //i have mentor  id
        {
            // Validate if mentorId is provided
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { message = "Mentor ID is required." });
            }

            // Retrieve schedules for the given mentor ID
            var schedules = _scheduleManager.GetAll()
                .Where(s => s.User_Id == id)
                .Select(s => new ScheduleViewModel
                {
                    Day = s.Day,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime
                }).ToList();

            if (schedules == null || schedules.Count == 0)
            {
                return NotFound(new { message = "No schedules found for this mentor." });
            }

            // Initialize the result as a list of arrays
            var result = new List<object>();

            // Loop through each schedule and split into hourly chunks
            foreach (var schedule in schedules)
            {
                var startTime = schedule.StartTime;
                var endTime = schedule.EndTime;

                // Create a list to hold hourly chunks for this schedule
                var hourlyChunks = new List<string>();

                // Split the time into 1-hour intervals using TimeSpan
                TimeSpan oneHour = TimeSpan.FromHours(1);

                while (startTime < endTime)
                {
                    var nextHour = startTime + oneHour;

                    // Check if the next hour exceeds the end time
                    if (nextHour > endTime)
                    {
                        nextHour = endTime; // Limit to the end time
                    }

                    // Add the time chunk to the list
                    hourlyChunks.Add($"{startTime:hh\\:mm} - {nextHour:hh\\:mm}");

                    // Move the start time forward by 1 hour
                    startTime = nextHour;
                }

                // Add the day's hourly chunks to the result array
                result.Add(new
                {
                    Day = schedule.Day,
                    TimeSlots = hourlyChunks
                });
            }

            // Return the result as a JSON response
            return new JsonResult(result);
        }


        //[HttpPost("BookSession")]
        //[Authorize]
        //public async Task<IActionResult> BookSession([FromForm] BookSessionViewModel model)
        //{
        //    // Validate model state
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState); // Return validation errors
        //    }

        //    // Get the logged-in developer's ID
        //    string developerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (string.IsNullOrEmpty(developerId))
        //    {
        //        return BadRequest(new { message = "Developer is not authenticated." });
        //    }
        //}



        //[HttpGet("GetAll")]
        //public IActionResult GetSessionsByUserId(string id)
        //{
        //    return new je
        //}

        //public async Task<IActionResult> BookSession([FromForm] BookSessionViewModel model)
        //{
        //    // Get the logged-in developer's ID
        //    string developerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    if (developerId == null)
        //    {
        //        return BadRequest(new { message = "Developer is not authenticated." });
        //    }

        //    // Retrieve the developer
        //    var developer = await usermanager.FindByIdAsync(developerId);
        //    if (developer == null)
        //    {
        //        return NotFound(new { message = "Developer not found." });
        //    }

        //    // Check if the mentor's schedule exists and is available
        //    var schedule = _scheduleManager.GetAll()
        //        .FirstOrDefault(s => s.Id == model.ScheduleId && s.User_Id == model.MentorId);

        //    if (schedule == null)
        //    {
        //        return NotFound(new { message = "Schedule not found or already booked." });
        //    }

        //    // Create a new session
        //    Session newSession = new Session
        //    {
        //        Topic = model.Topic,
        //        Description = model.Description,
        //        DateTime = model.DateTime, // You can change this to the desired date
        //        Duration = model.Duration,
        //        MeetingLink = model.MeetingLink, // You can generate or provide a meeting link here
        //        Status = model.Status,


        //        // Relationships
        //        User_Id = developerId, // Developer (the one booking)
        //        User_Instructor_Id = model.MentorId, // Mentor (the one providing the session)
        //        Payment = new Payment
        //        {
        //            Amount = model.Amount,
        //            Tax = model.Tax,
        //            Total = model.Total,
        //            PaymentType = model.PaymentType
        //        }
        //    };

        //    // Add session to the database
        //    bool sessionAdded = sessionManager.Add(newSession);

        //    if (sessionAdded)
        //    {
        //        return Ok(new { message = "Session booked successfully!" });
        //    }
        //    //{
        //    //    // Remove or mark the schedule as booked/unavailable
        //    //    bool scheduleRemoved =await _scheduleManager.Remove(schedule.Id);

        //    //    if (scheduleRemoved)
        //    //    {
        //    //        return Ok(new { message = "Session booked successfully!" });
        //    //    }
        //    //    else
        //    //    {
        //    //        return BadRequest(new { message = "Failed to remove the schedule after booking." });
        //    //    }
        //    //}

        //    return BadRequest(new { message = "Failed to book session." });
        //}

        //[HttpPost]
        //[HttpPost]
        [HttpPost("BookSession")]
        [Authorize]
        //public async Task<IActionResult> BookSession([FromForm] BookSessionViewModel model)
        //{
        //    // Validate model state
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState); // Return validation errors
        //    }


        //    // Get the logged-in developer's ID
        //    string developerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (string.IsNullOrEmpty(developerId))
        //    {
        //        return BadRequest(new { message = "Developer is not authenticated." });
        //    }

        //    // Retrieve the developer
        //    var developer = await usermanager.FindByIdAsync(developerId);
        //    if (developer == null)
        //    {
        //        return NotFound(new { message = "Developer not found." });
        //    }

        //    // Check if the mentor's schedule exists and is available
        //    var schedule = _scheduleManager.GetAll()
        //        .FirstOrDefault(s => s.Id == model.ScheduleId && s.User_Id == model.MentorId);

        //    if (schedule == null)
        //    {
        //        return NotFound(new { message = "Schedule not found or already booked." });
        //    }

        //    // Check for time overlap
        //    var existingSession = sessionManager.GetAll()
        //        .Where(sess => sess.User_Instructor_Id == model.MentorId)
        //        .FirstOrDefault(sess => sess.DateTime < model.DateTime.AddMinutes(model.Duration) &&
        //                                model.DateTime < sess.DateTime.AddMinutes(sess.Duration));

        //    if (existingSession != null)
        //    {
        //        return BadRequest(new { message = "The session time overlaps with an existing booking." });
        //    }

        //    // Create a new session
        //    var newSession = new Session
        //    {
        //        Topic = model.Topic,
        //        Description = model.Description,
        //        DateTime = model.DateTime, // Ensure this is the correct DateTime (without overlap)
        //        Duration = model.Duration,
        //        MeetingLink = model.MeetingLink, // Add meeting link or generate one here
        //        Status = model.Status, // Set the session status (e.g., Pending, Confirmed, etc.)

        //        // Relationships
        //        User_Id = developerId, // Developer booking the session
        //        User_Instructor_Id = model.MentorId, // Mentor providing the session

        //        // Add payment details
        //        //Payment = new Payment
        //        //{
        //        //    Amount = model.Amount,
        //        //    Tax = model.Tax,
        //        //    Total = model.Total,
        //        //    PaymentType = model.PaymentType
        //        //}
        //    };

        //    // Add session to the database
        //    var sessionAdded = sessionManager.Add(newSession);
        //    if (sessionAdded)
        //    {
        //        return Ok(new { message = "Session booked successfully!" });
        //    }

        //    return BadRequest(new { message = "Failed to book session." });
        //}
        //[HttpPost("book-session1")]
        //[Authorize]


        //workedoeffdvgbhnjhbgvf
        //public async Task<IActionResult> BookSession1([FromForm] BookSessionViewModel model)
        //{
        //    // Validate model state
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // Get the logged-in developer's ID
        //    string developerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (string.IsNullOrEmpty(developerId))
        //    {
        //        return BadRequest(new { message = "Developer is not authenticated." });
        //    }

        //    // Retrieve the developer
        //    var developer = await usermanager.FindByIdAsync(developerId);
        //    if (developer == null)
        //    {
        //        return NotFound(new { message = "Developer not found." });
        //    }

        //    // Check if the mentor's schedule exists get  all session unbooked
        //    var schedule = _scheduleManager.GetAll()
        //        .FirstOrDefault(s => s.Id == model.ScheduleId && s.User_Id == model.MentorId);

        //    if (schedule == null)
        //    {
        //        return NotFound(new { message = "Schedule not found or already booked." });
        //    }
        //    //var datetime1 = model.DateTime;

        //    Day sessionDay = _scheduleManager.ConvertDayOfWeekToDayEnum(model.DateTime.DayOfWeek);
        //    // Check if the session day matches the schedule's day
        //    if (sessionDay != schedule.Day)
        //    {
        //        return BadRequest(new { message = "The session date doesn't match the mentor's available day." });
        //    }

        //    // Extract the time from the provided DateTime
        //    TimeSpan sessionStartTime = model.DateTime.TimeOfDay;

        //    // Check if the session start time is within the schedule's start and end time
        //    if (sessionStartTime <= schedule.StartTime && sessionStartTime >= schedule.EndTime)
        //    {
        //        return BadRequest(new { message = "Session time is not within the schedule's available hours." });
        //    }

        //    // Check for time overlap with existing sessions
        //    var existingSession = sessionManager.GetAll()
        //        .Where(sess => sess.User_Instructor_Id == model.MentorId)
        //        .FirstOrDefault(sess => sess.DateTime.Date == model.DateTime.Date && // Same day
        //                                sess.DateTime < model.DateTime.AddMinutes(model.Duration) && // No overlap
        //                                model.DateTime < sess.DateTime.AddMinutes(sess.Duration));

        //    if (existingSession != null)
        //    {
        //        return BadRequest(new { message = "The session time overlaps with an existing booking." });
        //    }

        //    // Ensure that the duration is exactly one hour and session starts at the hour
        //    if (model.DateTime.Minute == 0 && model.DateTime.Second == 0)
        //    {
        //        if (model.Duration != 1)
        //        {
        //            return BadRequest(new { message = "The Duration Must Be One Hour" });
        //        }

        //        var newSession = new Session
        //        {
        //            Topic = model.Topic,
        //            Description = model.Description,
        //            DateTime = model.DateTime,
        //            Duration = model.Duration,
        //            //MeetingLink = model.MeetingLink,
        //            //Status = model.Status,
        //            User_Id = developerId, // Developer booking the session
        //            User_Instructor_Id = model.MentorId, // Mentor providing the session
        //        };

        //        // Add session to the database
        //        var sessionAdded = sessionManager.Add(newSession);
        //        if (!sessionAdded)
        //        {
        //            return BadRequest(new { message = "Failed to book session." });
        //        }

        //        return Ok(new { message = "Session booked successfully!" });
        //    }

        //    return BadRequest(new { message = "The Minutes and Seconds Must Be 00 " });
        //}

        public async Task<IActionResult> BookSession1([FromBody] BookSessionViewModel model)
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the logged-in developer's ID
            string developerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(developerId))
            {
                return BadRequest(new { message = "Developer is not authenticated." });
            }

            // Retrieve the developer
            var developer = await usermanager.FindByIdAsync(developerId);
            if (developer == null)
            {
                return NotFound(new { message = "Developer not found." });
            }

            // Check if the mentor's schedule exists get  all session unbooked
            var schedule = _scheduleManager.GetAll()
                .Where(s => s.User_Id == model.MentorId);

            if (schedule == null)
            {
                return NotFound(new { message = "Schedule not found or already booked." });
            }
            //var datetime1 = model.DateTime;

            Day sessionDay = _scheduleManager.ConvertDayOfWeekToDayEnum(model.DateTime.DayOfWeek);
            // Check if the session day matches the schedule's day
            foreach (var item in schedule)
            {
                if (sessionDay != item.Day)
                {
                    // return BadRequest(new { message = "The session date doesn't match the mentor's available day." });
                    continue;

                }
                else if (sessionDay == item.Day)


                {
                    TimeSpan sessionStartTime = model.DateTime.TimeOfDay;

                    // Check if the session start time is within the schedule's start and end time
                    if (sessionStartTime <= item.StartTime && sessionStartTime >= item.EndTime)
                    {
                        return BadRequest(new { message = "Session time is not within the schedule's available hours." });
                    }
                    var existingSession = sessionManager.GetAll()
               .Where(sess => sess.User_Instructor_Id == model.MentorId)
               .FirstOrDefault(sess => sess.DateTime.Date == model.DateTime.Date && // Same day
                                       sess.DateTime < model.DateTime.AddMinutes(model.Duration) && // No overlap
                                       model.DateTime < sess.DateTime.AddMinutes(sess.Duration));

                    if (existingSession != null)
                    {
                        return BadRequest(new { message = "The session time overlaps with an existing booking." });
                    }

                    // Ensure that the duration is exactly one hour and session starts at the hour
                    if (model.DateTime.Minute == 0 && model.DateTime.Second == 0)
                    {
                        if (model.Duration != 1)
                        {
                            return BadRequest(new { message = "The Duration Must Be One Hour" });
                        }

                        var newSession = new Session
                        {
                            Topic = model.Topic,
                            Description = model.Description,
                            DateTime = model.DateTime,
                            Duration = model.Duration,
                            //MeetingLink = model.MeetingLink,
                            //Status = model.Status,
                            User_Id = developerId, // Developer booking the session
                            User_Instructor_Id = model.MentorId, // Mentor providing the session
                        };

                        // Add session to the database
                        var sessionAdded = sessionManager.Add(newSession);
                        if (!sessionAdded)
                        {
                            return BadRequest(new { message = "Failed to book session." });
                        }

                        return Ok(new
                        {
                            message = "Session booked successfully!",
                            Session = newSession
                        }
                        );
                    }
               
            }
            

            // Extract the time from the provided DateTime
           

            // Check for time overlap with existing sessions
           
            }

            return BadRequest(new { message = "The Minutes and Seconds Must Be 00 " });
        }



        //public async Task<IActionResult> BookSession1([FromForm] BookSessionViewModel model)
        //{
        //    // Validate model state
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // Get the logged-in developer's ID
        //    string developerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (string.IsNullOrEmpty(developerId))
        //    {
        //        return BadRequest(new { message = "Developer is not authenticated." });
        //    }

        //    // Retrieve the developer
        //    var developer = await usermanager.FindByIdAsync(developerId);
        //    if (developer == null)
        //    {
        //        return NotFound(new { message = "Developer not found." });
        //    }

        //    // Get all schedules for the mentor
        //    var schedules = _scheduleManager.GetAll()
        //        .Where(s => s.User_Id == model.MentorId)
        //        .ToList();

        //    if (!schedules.Any())
        //    {
        //        return NotFound(new { message = "Schedule not found or already booked." });
        //    }

        //    // Convert model DateTime day to Day enum
        //    Day sessionDay = _scheduleManager.ConvertDayOfWeekToDayEnum(model.DateTime.DayOfWeek);

        //    // Check if any schedule matches the provided day and time
        //    var matchingSchedule = schedules.FirstOrDefault(s => s.Day == sessionDay
        //                                                         && model.DateTime.TimeOfDay >= s.StartTime
        //                                                         && model.DateTime.TimeOfDay < s.EndTime);

        //    if (matchingSchedule == null)
        //    {
        //        return BadRequest(new { message = "The session time is not within the mentor's available schedule." });
        //    }

        //    // Check for time overlap with existing sessions
        //    var existingSession = sessionManager.GetAll()
        //        .Where(sess => sess.User_Instructor_Id == model.MentorId)
        //        .FirstOrDefault(sess => sess.DateTime.Date == model.DateTime.Date && // Same day
        //                                sess.DateTime < model.DateTime.AddMinutes(model.Duration) && // No overlap
        //                                model.DateTime < sess.DateTime.AddMinutes(sess.Duration));

        //    if (existingSession != null)
        //    {
        //        return BadRequest(new { message = "The session time overlaps with an existing booking." });
        //    }

        //    // Ensure that the duration is exactly one hour and session starts at the hour
        //    if (model.DateTime.Minute == 0 && model.DateTime.Second == 0)
        //    {
        //        if (model.Duration != 1)
        //        {
        //            return BadRequest(new { message = "The Duration Must Be One Hour" });
        //        }

        //        var newSession = new Session
        //        {
        //            Topic = model.Topic,
        //            Description = model.Description,
        //            DateTime = model.DateTime,
        //            Duration = model.Duration,
        //            User_Id = developerId, // Developer booking the session
        //            User_Instructor_Id = model.MentorId, // Mentor providing the session
        //        };

        //        // Add session to the database
        //        var sessionAdded = sessionManager.Add(newSession);
        //        if (!sessionAdded)
        //        {
        //            return BadRequest(new { message = "Failed to book session." });
        //        }

        //        return new JsonResult(new { message = "Session booked successfully!"
        //        ,Session=newSession});
        //    }

        //    return BadRequest(new { message = "The Minutes and Seconds Must Be 00 " });
        //}








        [HttpGet("unbooked-schedules/{mentorId}")]
        [Authorize]
        public async Task<IActionResult> GetUnbookedSessions(string mentorId)
        {
            // Step 1: Retrieve all schedules for the mentor
            var schedules = _scheduleManager.GetAll()
                .Where(s => s.User_Id == mentorId.ToString())
                .OrderBy(s => s.Day)
                .ToList(); // Fetch schedules into memory

            if (schedules == null || !schedules.Any())
            {
                return NotFound(new { message = "No schedules found for the mentor." });
            }

            // Step 2: Retrieve all booked sessions for this mentor from the database
            var bookedSessions = sessionManager.GetAll()
                .Where(sess => sess.User_Instructor_Id == mentorId.ToString())
                .OrderBy(sess => sess.DateTime)
                .ToList(); // Fetch sessions into memory

            // Step 3: Initialize a dictionary to hold available time slots for each schedule
            var unbookedSlotsBySchedule = new Dictionary<int, List<(TimeSpan Start, TimeSpan End)>>();
            var EmptySessions = new List<EmptySession>();
            int i = 0;
            while (i < 30)
            {
                DateTime current = DateTime.Now.AddDays(i);
                Schedule? schedule = null;
                foreach (var item in schedules)
                {
                    var test = (DayOfWeek)item.Day;
                    if (test == current.DayOfWeek) schedule = item;
                }
                if (schedule == null)
                {
                    i++;
                    continue;
                }
                else
                {
                    // Step 4: Filter booked sessions that match the same day as the current schedule
                    TimeSpan currentStartTime = schedule.StartTime;
                    TimeSpan endTime = schedule.EndTime;
                    var tet = (DayOfWeek)schedule.Day;
                    var thisDaySeesions = bookedSessions.Where(bs => bs.DateTime.DayOfWeek == tet && bs.DateTime.Date == current.Date);

                    while (endTime > currentStartTime)
                    {
                        // Check for unbooked slot before the booked session
                        var bookedSession = thisDaySeesions.Where(tt => (int)tt.DateTime.TimeOfDay.TotalHours == (int)currentStartTime.TotalHours).FirstOrDefault();

                        if (bookedSession == null)
                        {
                            // Create an unbooked slot from currentStartTime to one hour later
                            EmptySessions.Add(new EmptySession
                            {
                                Date = current.Date,
                                Day = schedule.Day,
                                StartTime = currentStartTime,
                                EndTime = currentStartTime.Add(TimeSpan.FromHours(1))
                            });
                        }

                        // Move currentStartTime to the end of the booked session
                        currentStartTime = currentStartTime.Add(TimeSpan.FromHours(1));

                    }

                    #region testing
                    // var filteredSessions = bookedSessions
                    //.Where(sess => sess.DateTime.DayOfWeek == (DayOfWeek)schedule.Day) // Convert schedule.Day (int) to DayOfWeek enum
                    //.ToList();

                    // Step 5: Initialize list of unbooked slots for this schedule
                    //List<(TimeSpan Start, TimeSpan End)> unbookedSlots = new List<(TimeSpan Start, TimeSpan End)>();

                    // Start from the beginning of the schedule's StartTime


                    // Step 6: Check for unbooked slots in the schedule
                    //foreach (var bookedSession in filteredSessions)
                    //{
                    //    //while (bookedSession.DateTime.TimeOfDay > currentStartTime)
                    //    //{
                    //    //    // Create an unbooked slot from currentStartTime to one hour later
                    //    //    unbookedSlots.Add((currentStartTime, currentStartTime.Add(TimeSpan.FromHours(1))));
                    //    //    currentStartTime = currentStartTime.Add(TimeSpan.FromHours(1));
                    //    //}




                    //    // Check for unbooked slot before the booked session
                    //    if (bookedSession.DateTime.TimeOfDay >= currentStartTime)
                    //    {
                    //        // Create an unbooked slot from currentStartTime to one hour later
                    //        unbookedSlots.Add((currentStartTime, currentStartTime.Add(TimeSpan.FromHours(1))));


                    //    }

                    //    // Move currentStartTime to the end of the booked session
                    //    currentStartTime = currentStartTime.Add(TimeSpan.FromHours(1));
                    //}

                    //    // If there's remaining time after the last booked session until the schedule's EndTime, add it as an unbooked slot
                    //if (currentStartTime < endTime)
                    //{
                    //    unbookedSlots.Add((currentStartTime, endTime));
                    //}

                    //    // Step 7: Store the unbooked slots for this schedule
                    //unbookedSlotsBySchedule[schedule.Id] = EmptySessions; 
                    #endregion
                }


                i++;
            }

            // Step 8: Return the unbooked slots for all schedules
            return Ok(new
            {
                message = "Available unbooked time slots for mentor's schedules",
                schedules = EmptySessions
            });
        }








    }

    public class EmptySession
    {
        public DateTime Date { get; set; }
        public Day Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}