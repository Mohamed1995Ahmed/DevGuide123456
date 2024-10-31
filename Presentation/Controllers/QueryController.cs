using DevGuide.Models;
using DevGuide.Models.Models;
using Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ViewModels;
using ViewModels;

namespace Presentation.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class QueryController : ControllerBase
    {
    
         private QueryManager queryManager;
        private QuerAnswerManager querAnswerManager;
        private ProjectContext context ;

        public QueryController(QueryManager _queryManager, QuerAnswerManager _querAnswerManager, ProjectContext _context)
        {
            this.context = _context;
           this.queryManager = _queryManager;
           this.querAnswerManager = _querAnswerManager;
            
        }

        // POST: api/query
        [HttpPost("AddQuery")]
        [Authorize]
        public async Task<IActionResult> AddQuery([FromForm] AddQueryViewModel viewModel)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in");
            }
            viewModel.User_Id = userId;




            if (viewModel == null || string.IsNullOrWhiteSpace(viewModel.Question))
            {
                return new JsonResult(new { Question = "Query cannot be null and must have a message." }) { StatusCode = 400 };
            }

            // Create the query from the view model
            var query = viewModel.ToQuery(); // Pass the current user's ID/////////////

            // Handle file upload
            if (viewModel.File != null && viewModel.File.Length > 0)
            {
                var filePath = Path.Combine("uploads", viewModel.File.FileName); // Specify your upload directory
                if (!Directory.Exists("uploads"))
                {
                    Directory.CreateDirectory("uploads");
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.File.CopyToAsync(stream);
                }
                query.File = filePath; // Store the file path in the database
            }

            var result = queryManager.Add(query);
            if (result)
            {
                return new JsonResult(query) { StatusCode = 201 }; // Created
            }

            return new JsonResult(new { Question = "Internal server error while adding the query." }) { StatusCode = 500 };
        }





        //// POST: api/query
        //[HttpPost("AddAnswer")]
        //[Authorize]
        //public async Task<IActionResult> AddAnswer([FromForm] AddQueryViewModel viewModel)
        //{

        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return Unauthorized("User not logged in");
        //    }
        //    viewModel.User_Id = userId;




        //    if (viewModel == null || string.IsNullOrWhiteSpace(viewModel.Question))
        //    {
        //        return new JsonResult(new { Question = "Query cannot be null and must have a message." }) { StatusCode = 400 };
        //    }

        //    // Create the query from the view model
        //    var query = viewModel.ToQuery(); // Pass the current user's ID/////////////

        //    // Handle file upload
        //    if (viewModel.File != null && viewModel.File.Length > 0)
        //    {
        //        var filePath = Path.Combine("uploads", viewModel.File.FileName); // Specify your upload directory
        //        if (!Directory.Exists("uploads"))
        //        {
        //            Directory.CreateDirectory("uploads");
        //        }

        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await viewModel.File.CopyToAsync(stream);
        //        }
        //        query.File = filePath; // Store the file path in the database
        //    }

        //    var result = queryManager.Add(query);
        //    if (result)
        //    {
        //        return new JsonResult(query) { StatusCode = 201 }; // Created
        //    }

        //    return new JsonResult(new { Question = "Internal server error while adding the query." }) { StatusCode = 500 };
        //}



        //[HttpGet("UserQueries")]
        //public IActionResult GetUserQueries()
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return Unauthorized("User not logged in");
        //    }

        //    var userQueries = context.Query
        //        .Where(q => q.User_Id == userId)
        //        .Select(q => new QuaryAnswerViewModel
        //        {
        //         Id = q.Id,   
        //         FirstName = q.User_Instructor.FirstName,   
        //         LastName = q.User_Instructor.LastName,   
        //         MentorImageUrl = q.User_Instructor.Image,   
        //         MentorTitle = q.User_Instructor.Title,
        //         File = q.File,
        //         Question = q.Question,
        //         QueryAnswers = q.QueryAnswers.Select(i=>i.Answer).ToList()

        //        })
        //        .ToListAsync();

        //    return new JsonResult( new APIResult<object>
        //    {
        //        Message="List of Quary",
        //        StatusCode = 200,
        //        Success = true,
        //        Result = userQueries
        //    });
        //}
        [HttpGet("UserQueries")]
        public async Task<IActionResult> GetUserQueries()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in");
            }


            var userQueries = await queryManager.GetUserQueriesWithAnswersAsync(userId);

            var queryResult = userQueries.Select(q => new QuaryAnswerViewModel
            {
                Id = q.Id,
                FirstName = q.User_Instructor.FirstName,
                LastName = q.User_Instructor.LastName,
                Instructor_Id=q.User_Instructor_Id,
                User_Id = userId,
                DateTime = q.DateTime,
                MentorImageUrl = q.User_Instructor.Image,
                UserImageUrl=q.User.Image,
                MentorTitle = q.User_Instructor.Title,
                File = q.File,
                
                Question = q.Question,
                //QueryAnswers = querAnswerManager.GetAll().Where(qa => qa.Query_Id == q.Id).Select(qa => qa.Answer).ToList(),
                QueryAnswers = querAnswerManager.GetAll()
    .Where(qa => qa.Query_Id == q.Id)
    .Select(qa => new QueryAnswer
    {
        Answer = qa.Answer,
        File = qa.File,

    })
    .ToList()
            }).ToList();

            return new JsonResult(new APIResult<object>
            {
                Message = "List of Queries",
                StatusCode = 200,
                Success = true,
                Result = queryResult
            });
        }









        //    [HttpGet("UserQueries")]
        //    public async Task<IActionResult> GetAnswerofquery()
        //    {
        //        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //        if (string.IsNullOrEmpty(userId))
        //        {
        //            return Unauthorized("User not logged in");
        //        }


        //        var userQueries = await queryManager.GetUserQueriesWithAnswersAsync(userId);

        //        var queryResult = userQueries.Select(q => new QuaryAnswerViewModel
        //        {
        //            Id = q.Id,
        //            FirstName = q.User_Instructor.FirstName,
        //            LastName = q.User_Instructor.LastName,
        //            Instructor_Id = q.User_Instructor_Id,
        //            DateTime = q.DateTime,
        //            MentorImageUrl = q.User_Instructor.Image,
        //            UserImageUrl = q.User.Image,
        //            MentorTitle = q.User_Instructor.Title,
        //            File = q.File,

        //            Question = q.Question,
        //            //QueryAnswers = querAnswerManager.GetAll().Where(qa => qa.Query_Id == q.Id).Select(qa => qa.Answer).ToList(),
        //            QueryAnswers = querAnswerManager.GetAll()
        //.Where(qa => qa.Query_Id == q.Id)
        //.Select(qa => new QueryAnswer
        //{
        //    Answer = qa.Answer,
        //    File = qa.File,

        //})
        //.ToList()
        //        }).ToList();

        //        var mentorAnswer = await querAnswerManager.GetMentorAnswersAsync(Id);
        //        return new JsonResult(new APIResult<object>
        //        {
        //            Message = "List of Queries",
        //            StatusCode = 200,
        //            Success = true,
        //            Result = queryResult
        //        });
        //    }













        //[HttpGet("QueryAnswers/{queryId}")]
        //public async Task<IActionResult> GetAnswerOfQuery(int queryId)
        //{
        //    // Get the user ID from the claim
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return Unauthorized("User not logged in");
        //    }

        //    // Fetch the specific query based on queryId
        //    var query = await queryManager.GetByIdAsync(queryId);

        //    if (query == null)
        //    {
        //        return new JsonResult(new { Message = "Query not found." }) { StatusCode = 404 };
        //    }

        //    // Fetch all answers related to the specific queryId, ordered by DateTime descending
        //    var queryAnswers = querAnswerManager.GetAll()
        //        .Where(qa => qa.Query_Id == queryId)
        //        .OrderByDescending(qa => qa.DateTime)
        //        .Select(qa => new AddAnswerViewModel
        //        {
        //            Id = qa.Id,
        //            Answer = qa.Answer,
        //            FilePath = qa.File,
        //            DateTime = qa.DateTime
        //        })
        //        .ToList();

        //    // Prepare the full response including the instructor (mentor) information
        //    var queryResult = new QueryWithAnswersViewModel
        //    {
        //        Id = query.Id,
        //        Question = query.Question,
        //        File = query.File,
        //        DateTime = query.DateTime,

        //        // Instructor information
        //        MentorFirstName = query.User_Instructor.FirstName,
        //        MentorLastName = query.User_Instructor.LastName,
        //        MentorTitle = query.User_Instructor.Title,
        //        MentorImageUrl = query.User_Instructor.Image,

        //        // User information (who asked the query)
        //        UserImageUrl = query.User.Image,

        //        // List of answers
        //        QueryAnswers = queryAnswers
        //    };

        //    // Return the result in a JSON response
        //    return new JsonResult(new APIResult<object>
        //    {
        //        Message = "List of Answers",
        //        StatusCode = 200,
        //        Success = true,
        //        Result = queryResult
        //    });
        //}



        //[HttpGet("QueryAnswers/{queryId}")]
        //public async Task<IActionResult> GetAnswerOfQuery(int queryId)
        //{
        //    // Get the user ID from the claim
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return Unauthorized("User not logged in");
        //    }



        //    // Fetch the specific query based on queryId
        //    var query = await queryManager.GetByIdAsync(queryId);

        //    if (query == null)
        //    {
        //        return new JsonResult(new { Message = "Query not found." }) { StatusCode = 404 };
        //    }
        //    //var user_id=

        //    // Fetch all answers related to the specific queryId, ordered by DateTime ascending (oldest first)
        //    var queryAnswers = querAnswerManager.GetAll()
        //        .Where(qa => qa.Query_Id == queryId)
        //        .OrderBy(qa => qa.DateTime) // Changed from OrderByDescending to OrderBy
        //        .Select(qa => new AddAnswerViewModel
        //        {
        //            Id = qa.Id,
        //            Answer = qa.Answer,
        //            FilePath = qa.File,
        //            DateTime = qa.DateTime,
        //            //User_Id= context.Query.Where(r=>r.Id== qa.Query_Id).Select(r=>r.User_Id),
        //        })
        //        .ToList();

        //    // Prepare the full response including the instructor (mentor) information
        //    var queryResult = new QueryWithAnswersViewModel
        //    {
        //        Id = query.Id,
        //        Question = query.Question,
        //        File = query.File,
        //        DateTime = query.DateTime,

        //        // Instructor information
        //        MentorFirstName = query.User_Instructor.FirstName,
        //        MentorLastName = query.User_Instructor.LastName,
        //        MentorTitle = query.User_Instructor.Title,
        //        MentorImageUrl = query.User_Instructor.Image,
        //        UserId= userId,
        //        // User information (who asked the query)
        //        UserImageUrl = query.User.Image,

        //        // List of answers
        //        QueryAnswers = queryAnswers
        //    };

        //    // Return the result in a JSON response
        //    return new JsonResult(new APIResult<object>
        //    {
        //        Message = "List of Answers",
        //        StatusCode = 200,
        //        Success = true,
        //        Result = queryResult
        //    });
        //}



        [HttpGet("QueryAnswers/{queryId}")]
        public async Task<IActionResult> GetAnswerOfQuery(int queryId)
        {
            // Get the user ID from the claim
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in");
            }

            // Get the user's role from the claim and check if they are a developer
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var isDeveloper = userRole == "Developer"; // True if the user is a Developer

            // Fetch the specific query based on queryId
            var query = await queryManager.GetByIdAsync(queryId);

            if (query == null)
            {
                return new JsonResult(new { Message = "Query not found." }) { StatusCode = 404 };
            }

            // Fetch all answers related to the specific queryId, ordered by DateTime ascending (oldest first)
            var queryAnswers = querAnswerManager.GetAll()
                .Where(qa => qa.Query_Id == queryId)
                .OrderBy(qa => qa.DateTime)
                .Select(qa => new AddAnswerViewModel
                {
                    Id = qa.Id,
                    Answer = qa.Answer,
                    FilePath = qa.File,
                    DateTime = qa.DateTime,
                    Query_Id = queryId,
                    IsDeveloper = qa.IsDeveloper
                    // Set this based on the user's role
                })
                .ToList();

            // Prepare the full response including the instructor (mentor) information
            var queryResult = new QueryWithAnswersViewModel
            {
                Id = query.Id,
                Question = query.Question,
                File = query.File,
                DateTime = query.DateTime,

                // Instructor information
                MentorFirstName = query.User_Instructor.FirstName,
                MentorLastName = query.User_Instructor.LastName,
                MentorTitle = query.User_Instructor.Title,
                MentorImageUrl = query.User_Instructor.Image,
                UserId = userId,
               
                // User information (who asked the query)
                UserImageUrl = query.User.Image,

                // List of answers
                QueryAnswers = queryAnswers
            };

            // Return the result in a JSON response
            return new JsonResult(new APIResult<object>
            {
                Message = "List of Answers",
                StatusCode = 200,
                Success = true,
                Result = queryResult
            });
        }











        private async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            var filePath = Path.Combine("wwwroot", folderName, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return $"/{folderName}/{file.FileName}";
        }










        [HttpPost("AddAnswerQuery")]
        [Authorize]
        public async Task<IActionResult> AddAnswerQuery([FromForm] AddAnswerQueryViewModel viewModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in");
            }

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var isDeveloper = userRole == "Developer";

            if (viewModel == null || string.IsNullOrWhiteSpace(viewModel.Answer))
            {
                return new JsonResult(new { Answer = "Answer cannot be null and must have a message." }) { StatusCode = 400 };
            }

            // Create the answer query from the view model
            var answerQuery = new QueryAnswer
            {
                //Id=viewModel.id,
                Answer = viewModel.Answer,
                File=viewModel.FilePath,
                Query_Id = viewModel.Query_Id, // Foreign Key to the query
                DateTime = DateTime.Now,
                //userRole= userRole,
                IsDeveloper = isDeveloper,
            };

            // Handle file upload
            //if (viewModel.File != null && viewModel.File.Length > 0)
            //{
            //    var filePath = Path.Combine("uploads", viewModel.File.FileName); // Specify your upload directory
            //    if (!Directory.Exists("uploads"))
            //    {
            //        Directory.CreateDirectory("uploads");
            //    }

            //    using (var stream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await viewModel.File.CopyToAsync(stream);
            //    }
            //    answerQuery.File = filePath; // Store the file path in the database
            //}
            if (viewModel.File != null)
            {
                answerQuery.File = await SaveFileAsync(viewModel.File, "query_files"); // SaveFileAsync needs to return the file path
            }

            var result = querAnswerManager.Add(answerQuery);
            if (result)
            {
                return new JsonResult(answerQuery) { StatusCode = 201 }; // Created
            }

            return new JsonResult(new { Answer = "Internal server error while adding the answer query." }) { StatusCode = 500 };
        }











        [HttpGet("MentorQueries")]
        public async Task<IActionResult> GetMentorQueries()
        {
            // Check if the user is logged in and has the role of "mentor"
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userId) || userRole != "Mentor")
            {
                return Unauthorized("Access denied");
            }

            // Fetch the queries where the mentor is the instructor (User_Instructor_Id)
            var mentorQueries = await queryManager.GetMentorQueriesAsync(userId);

            // Map the result to the view model
            var queryResult = mentorQueries.Select(q => new QuaryAnswerViewModel
            {
                Id = q.Id,
                FirstName = q.User.FirstName,
                LastName = q.User.LastName,
                User_Id = q.User_Id,
                Instructor_Id = q.User_Instructor_Id,
                DateTime = q.DateTime,
                MentorImageUrl = q.User_Instructor.Image,
                UserImageUrl = q.User.Image,
                MentorTitle = q.User_Instructor.Title,
                File = q.File,
                Question = q.Question,
                QueryAnswers = q.QueryAnswers.Select(qa => new QueryAnswer
                {
                    Answer = qa.Answer,
                    File = qa.File
                }).ToList()
            }).ToList();

            return new JsonResult(new APIResult<object>
            {
                Message = "List of Queries for Mentor",
                StatusCode = 200,
                Success = true,
                Result = queryResult
            });
        }

    }
}
