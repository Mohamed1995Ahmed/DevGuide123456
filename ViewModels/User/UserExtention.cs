using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevGuide.Models.Models;
using Models.Models;



namespace ViewModels
{
    public static class  UserExtention
    {
        public static DevGuide.Models.Models.User ToModel(this UserRegisterViewModel model)
        {
            return new DevGuide.Models.Models.User()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                //LastName = model.LastName,
            };

        }
        public static User ToUpdateProfile(this Profile userProfile, User user)
        {
            if (!string.IsNullOrEmpty(userProfile.FirstName))
                user.FirstName = userProfile.FirstName;
            if (!string.IsNullOrEmpty(userProfile.LastName))
                user.LastName = userProfile.LastName;
            if (!string.IsNullOrEmpty(userProfile.CvPath))
                user.CV = userProfile.CvPath;
            if (!string.IsNullOrEmpty(userProfile.Country))
                user.Country = userProfile.Country;
            if (userProfile.YearsOfExperience == 0)
                user.YearsOfExperience = userProfile.YearsOfExperience;
            if (!string.IsNullOrEmpty(userProfile.Level))
                user.Level = userProfile.Level;
            if (!string.IsNullOrEmpty(userProfile.ImagePath))
                user.Image = userProfile.ImagePath;
            if (!string.IsNullOrEmpty(userProfile.About))
                user.About = userProfile.About;
            if (!string.IsNullOrEmpty(userProfile.Title))
                user.Title = userProfile.Title;
            if (!string.IsNullOrEmpty(userProfile.PhoneNumber))
                user.PhoneNumber = userProfile.PhoneNumber;
            return user;



        }

        public static Education ToEducationModel(this EducationViewModel model)
        {
            return new Education()
            {
                   Id =model.Id??0,
                   Degree =model.Degree,
                   FieldOfStudy=model.FieldOfStudy,
                   University =model.University,
                   Faculty =model.Faculty,
                   CountryOfStudy =model.CountryOfStudy,
                   StartDate =model.StartDate,
                   EndDate =model.EndDate,
                   TillNow =model.TillNow,
                   User_Id =model.User_Id
            };

        }
        public static UserListViewModel ToUserListViewModel(this User model)
        {
            // Check if model is null before proceeding (just in case)
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "User model cannot be null");
            }

            // Check if Reviews exist and contain data
            bool hasReviews = model.Reviews != null && model.Reviews.Any();
            if (model.Reviews != null && model.Reviews.Any()) {
                return new UserListViewModel()
            { 

                FirstName = model.FirstName,
               ID =model.Id,
                LastName = model.LastName,
                Title = model.Title,
                YearsOfExperience = model.YearsOfExperience,
                Image = model.Image,
                Price = model.Price,
                    // AverageRate = hasReviews ? (decimal)model.Reviews.Average(r => r.Rate) : 0,
                    AverageRate = model.Reviews.Any()
    ? Math.Round((decimal)model.Reviews.Average(r => r.Rate), 1)
    : 0,
                    NumOfReviews = (model.Reviews != null && model.Reviews.Any())
                ? model.Reviews.Count()
                : 0,

                    About = model.About,


                // Ensure Skills are not null before selecting
                Skills = model.Skills != null && model.Skills.Any()
                         ? model.Skills.Select(s => s.Skill).Select(s => s.Name).ToList()
                         : new List<string>(),

                    // Ensure SocialAccounts are not null before selecting
                    //SocialAccounts = model.SocialAccounts != null && model.SocialAccounts.Any()
                    //                 ? model.SocialAccounts.Select(l => l.SocialLink).ToList()
                    //                 : new List<string>()
                    //SocialAccounts = model.SocialAccounts != null && model.SocialAccounts.Any()
                    //                 ? model.SocialAccounts.ToList()
                    //                 : new List<SocialLinkViewModel>()
                    SocialAccounts = model.SocialAccounts != null && model.SocialAccounts.Any()
    ? model.SocialAccounts.Select(l => new SocialLinkViewModel
    {
        SocialName = l.SocialName,
        SocialLink = l.SocialLink
    }).ToList()
 : new List<SocialLinkViewModel>()

                };
            }
            else
            {
                return new UserListViewModel()
                {
                    FirstName = model.FirstName,
                    ID = model.Id,
                    LastName = model.LastName,
                    Title = model.Title,
                    YearsOfExperience = model.YearsOfExperience,
                    Image = model.Image,
                    Price = model.Price,
                    // AverageRate = hasReviews ? (decimal)model.Reviews.Average(r => r.Rate) : 0,
                    AverageRate =  0,
                    About = model.About,

                    // Ensure Skills are not null before selecting
                    Skills = model.Skills != null && model.Skills.Any()
                        ? model.Skills.Select(s => s.Skill).Select(s => s.Name).ToList()
                        : new List<string>(),
                    NumOfReviews = (model.Reviews != null && model.Reviews.Any())
                ? model.Reviews.Count()
                : 0,
                   
                    // Ensure SocialAccounts are not null before selecting
                    //SocialAccounts = model.SocialAccounts != null && model.SocialAccounts.Any()
                    //            ? model.SocialAccounts.Select(l => l.SocialLink).ToList()
                    //            : new List<string>()
                    SocialAccounts = model.SocialAccounts != null && model.SocialAccounts.Any()
    ? model.SocialAccounts.Select(l => new SocialLinkViewModel
    {
        SocialName = l.SocialName,
        SocialLink = l.SocialLink
    }).ToList()
    : new List<SocialLinkViewModel>()
                };
            }
        }

        //    public static UserListViewModel ToUserListViewModel(this User model) {

        //        if (model.Reviews != null && model.Reviews.Any())
        //        {

        //        return new UserListViewModel() {
        //            FirstName =model.FirstName,
        //            LastName=model.LastName,
        //            Title=model.Title,
        //            YearsOfExperience=model.YearsOfExperience,
        //            Image=model.Image,
        //            Price=model.Price,
        //            AverageRate= model.Reviews.Any() ?(decimal)model.Reviews.Average(r => r.Rate) : 0,
        //            About=model.About,
        //            Skills=model.Skills.Select(s=>s.Skill).Select(s=>s.Name).ToList(),
        //            SocialAccounts=model.SocialAccounts.Any() ? model.SocialAccounts.Select(l => l.SocialLink).ToList() : null

        //};


        //        }

        //        else
        //        {

        //            return new UserListViewModel()
        //            {
        //                FirstName = model.FirstName,
        //                LastName = model.LastName,
        //                Title = model.Title,
        //                YearsOfExperience = model.YearsOfExperience,
        //                Image = model.Image,
        //                Price = model.Price,
        //                AverageRate = 0,
        //                About = model.About,
        //                Skills = model.Skills.Select(s => s.Skill).Select(s => s.Name).ToList(),
        //                SocialAccounts = model.SocialAccounts.Any() ? model.SocialAccounts.Select(l => l.SocialLink).ToList() : null

        //            };
        //        }
        //    }

        public static UserProfile toUserProfile (this User user)
        {
            return new UserProfile() {
                FirstName = user.FirstName,
                LastName  = user.LastName,
                Title = user.Title,
                About = user.About,
                YearsOfExperience= user.YearsOfExperience,
                PhoneNumber = user.PhoneNumber,
                Country = user.Country,
                CVPath=user.CV,
                ImagePath=user.Image,
                Level = user.Level,
                

            };
        }
    }

}

    //public static class UserExtention
    //{
    //    public static DevGuide.Models.Models.User ToModel(this UserRegisterViewModel model)
    //    {
    //        return new DevGuide.Models.Models.User()
    //        {
    //            Email = model.Email,
    //            Name = model.Name,
    //            UserName = model.UserName,
    //             //= model.LastName,
    //        };

    //    }
    //}
//}


