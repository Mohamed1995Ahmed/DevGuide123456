using DevGuide.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public static class SessionExtention
    {
        public static SessionListViewModel ToSessionListViewModel(this Session model)
        {
            if (model.Payment == null)
            {
                return new SessionListViewModel
                {
                    Id = model.Id,
                    Topic = model.Topic,
                    DateTime = model.DateTime,
                    Status = model.Status,
                    Total = 0,
                    User_FirstName = model.User.FirstName,
                    User_LastName = model.User.LastName,
                    User_Instructor_Id = model.User_Instructor_Id,

                };
            }
            else
            {
                return new SessionListViewModel
                {
                    Id = model.Id,
                    Topic = model.Topic,
                    DateTime = model.DateTime,
                    Status = model.Status,
                    Total = model.Payment.Total,
                    User_FirstName = model.User.FirstName,
                    User_LastName = model.User.LastName,
                    User_Instructor_Id = model.User_Instructor_Id,

                };
            }
        }


        public static SessionListViewModel ToDeveloperSessionListViewModel(this Session model)
        {
            if (model.Payment == null)
            {
                return new SessionListViewModel
                {
                    Id = model.Id,
                    Topic = model.Topic,
                    DateTime = model.DateTime,
                    Status = model.Status,
                    Total = 0,
                    User_FirstName = model.User_Instructor.FirstName,
                    User_LastName = model.User_Instructor.LastName,
                    User_Instructor_Id = model.User_Id,

                };
            }
            else
            {
                return new SessionListViewModel
                {
                    Id = model.Id,
                    Topic = model.Topic,
                    DateTime = model.DateTime,
                    Status = model.Status,
                    Total = model.Payment.Total,
                    User_FirstName = model.User_Instructor.FirstName,
                    User_LastName = model.User_Instructor.LastName,
                    User_Instructor_Id = model.User_Id,

                };
            }
        }
    }
}
