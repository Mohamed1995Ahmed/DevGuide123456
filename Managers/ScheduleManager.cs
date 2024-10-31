using DevGuide.Models;
using DevGuide.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Managers
{
    public class ScheduleManager : BaseManager<Schedule>
    {
        private readonly SessionManager sessionManager;

        public ScheduleManager(ProjectContext context,SessionManager sessionManager) : base(context)
        {
            this.sessionManager = sessionManager;
        }

        public bool IsMentorAvailable(int mentorId, Day day, TimeSpan startTime, TimeSpan endTime)
        {
            // Retrieve all schedules for the given mentor and date
            var schedules = GetAll()
                .Where(s => ParseUserId(s.User_Id) == mentorId && s.Day == day)
                .ToList();

            // Check if any schedule conflicts with the provided time range
            return !schedules.Any(s =>
                (s.StartTime >= startTime && s.StartTime < endTime) ||  // Overlaps start time
                (s.EndTime > startTime && s.EndTime <= endTime));        // Overlaps end time
        }

        // Helper method to safely parse User_Id to int
        public int? ParseUserId(string userId)
        {
            if (int.TryParse(userId, out int parsedId))
            {
                return parsedId;
            }
            return null; // Return null if parsing fails
        }
        public async Task<bool> Remove(int scheduleId)
        {
            var schedule =await GetByIdAsync(scheduleId);
            if (schedule != null)
            {
                return Delete(schedule);
            }
            return false;
        }


        // Method to get unbooked schedules for a mentor

        public List<Schedule> GetUnbookedSchedulesForMentor(string mentorId)
        {
            // Get all schedules for the mentor
            var allSchedulesForMentor = GetAll()
                .Where(s => s.User_Id == mentorId)
                .ToList();

            // Get all booked sessions for the mentor
            var bookedSessions = sessionManager.GetAll()
                .Where(sess => sess.User_Instructor_Id == mentorId)
                .ToList();

            // Return schedules that do not overlap with booked sessions
            var availableSchedules = allSchedulesForMentor
                .Where(schedule => !bookedSessions.Any(sess =>
                    sess.DateTime.Date == ConvertDayToDate(schedule.Day, schedule.StartTime).Date && // Same day
                    (
                        (sess.DateTime.TimeOfDay >= schedule.StartTime && sess.DateTime.TimeOfDay < schedule.EndTime) || // Session starts during the schedule
                        (sess.DateTime.AddMinutes(sess.Duration).TimeOfDay > schedule.StartTime && sess.DateTime.AddMinutes(sess.Duration).TimeOfDay <= schedule.EndTime) || // Session ends during the schedule
                        (sess.DateTime.TimeOfDay <= schedule.StartTime && sess.DateTime.AddMinutes(sess.Duration).TimeOfDay >= schedule.EndTime) // Session fully overlaps the schedule
                    )
                ))
                .ToList();

            return availableSchedules;
        }




        public DateTime ConvertDayToDate(Day day, TimeSpan startTime)
        {
            // Assuming you have a method to get today's date, or use a specific date context
            DateTime today = DateTime.Today;

            // Find the next occurrence of the specified day of the week
            DateTime nextDay = today.AddDays((7 + (int)day - (int)today.DayOfWeek) % 7);

            // Combine the date with the start time
            return new DateTime(nextDay.Year, nextDay.Month, nextDay.Day, startTime.Hours, startTime.Minutes, 0);
        }
        public Day ConvertDayOfWeekToDayEnum(DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Sunday => Day.Sunday,
                DayOfWeek.Monday => Day.Monday,
                DayOfWeek.Tuesday => Day.Tuesday,
                DayOfWeek.Wednesday => Day.Wednesday,
                DayOfWeek.Thursday => Day.Thursday,
                DayOfWeek.Friday => Day.Friday,
                DayOfWeek.Saturday => Day.Saturday,
                _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek), "Invalid day of the week"),
            };
        }




    }

}








