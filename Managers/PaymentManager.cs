using DevGuide.Models.Models;
using DevGuide.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Managers
{
    public class PaymentManager : BaseManager<Payment>
    {
        private readonly SessionManager sessionManager;

        public PaymentManager(ProjectContext context,SessionManager sessionManager) : base(context)
        {
            this.sessionManager = sessionManager;
        }
        public async Task<(decimal totalAllTime, decimal totalLastMonth, decimal totalCurrentMonth)> GetPaymentTotalsForInstructorAsync1(string instructorId)
        {
            if (string.IsNullOrEmpty(instructorId))
            {
                throw new ArgumentException("Instructor ID cannot be null or empty.", nameof(instructorId));
            }

            // Fetch all payments related to the instructor
            var payments = await GetAll().Where(p => p.Session.User_Instructor_Id == instructorId).ToListAsync();

            // Calculate total payments for all time
            var totalAllTime = payments.Sum(p => p.Total);

            // Get the first day of last month and the first day of the current month
            var firstDayOfCurrentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var firstDayOfLastMonth = firstDayOfCurrentMonth.AddMonths(-1);

            // Calculate total payments for last month
            var totalLastMonth = payments
                .Where(p => p.Session.DateTime >= firstDayOfLastMonth && p.Session.DateTime < firstDayOfCurrentMonth)
                .Sum(p => p.Total);

            // Calculate total payments for the current month
            var totalCurrentMonth = payments
                .Where(p => p.Session.DateTime >= firstDayOfCurrentMonth)
                .Sum(p => p.Total);

            return (totalAllTime, totalLastMonth, totalCurrentMonth);
        }



        public async Task<(decimal totalAllTime, decimal totalLastMonth, decimal totalCurrentMonth)> GetPaymentTotalsForInstructorAsync(string instructorId)
            {
                if (string.IsNullOrEmpty(instructorId))
                {
                    throw new ArgumentException("Instructor ID cannot be null or empty.", nameof(instructorId));
                }

                // Retrieve all payments for sessions conducted by the specified instructor
                var payments = await GetAll()
                    .Where(p => p.Session.User_Instructor_Id == instructorId)
                    .ToListAsync();

                // Calculate total payments for all time
                var totalAllTime = payments.Sum(p => p.Total);

                // Get the first day of last month and the first day of the current month
                var firstDayOfCurrentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var firstDayOfLastMonth = firstDayOfCurrentMonth.AddMonths(-1);

                // Calculate total payments for last month
                var totalLastMonth = payments
                    .Where(p => p.Session.DateTime >= firstDayOfLastMonth && p.Session.DateTime < firstDayOfCurrentMonth)
                    .Sum(p => p.Total);

                // Calculate total payments for the current month
                var totalCurrentMonth = payments
                    .Where(p => p.Session.DateTime >= firstDayOfCurrentMonth)
                    .Sum(p => p.Total);

                return (totalAllTime, totalLastMonth, totalCurrentMonth);
            }
        }

    
}