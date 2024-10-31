using DevGuide.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Managers
{
    public static class Extensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string columnName, bool isAscending)
        {

            ParameterExpression parameter = Expression.Parameter(source.ElementType, "");

            MemberExpression property = Expression.Property(parameter, columnName);
            //b=> b.price
            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = isAscending ? "OrderBy" : "OrderByDescending";
            Expression methodCallExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { source.ElementType, property.Type },
                source.Expression,
                Expression.Quote(lambda)
            );

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var body = Expression.AndAlso(
                Expression.Invoke(expr1, parameter),
                Expression.Invoke(expr2, parameter)
            );

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
        public static Query ToQuery(this AddQueryViewModel viewModel)
        {

            return new Query
            {
                DateTime= DateTime.UtcNow,
                Question = viewModel.Question,
                File = viewModel.File?.FileName, // If you want to store the file name
                User_Id = viewModel.User_Id, // Assuming you want to associate the query with the user
                User_Instructor_Id=viewModel.User_Instructor_Id,
                //User_Instructor_Id = viewModel., 
                //QueryAnswers = new List<QueryAnswer>() 
            };
        }

        //public static QueryAnswer ToQueryAnswer(this AddQueryViewModel viewModel)
        //{

        //    return new QueryAnswer
        //    {
        //        DateTime = DateTime.UtcNow,
        //        Answer = viewModel.Question,
        //        File = viewModel.File?.FileName, // If you want to store the file name
        //        User_Id = viewModel.User_Id, // Assuming you want to associate the query with the user
        //        User_Instructor_Id = viewModel.User_Instructor_Id,
        //        //User_Instructor_Id = viewModel., 
        //        //QueryAnswers = new List<QueryAnswer>() 
        //    };
        //}

    }
}