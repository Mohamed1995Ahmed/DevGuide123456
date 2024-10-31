
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using DevGuide.Models;
namespace Managers
{
    public class BaseManager<T> where T : class
    {


        protected ProjectContext context;
        private DbSet<T> DbSet;

        public BaseManager(ProjectContext context)
        {
            this.context = context;
            DbSet = context.Set<T>();
        }

        public T GetById(object id)
        {
            return context.Set<T>().Find(id);
        }
        public IQueryable<T> GetAll()
        {
            return context.Set<T>().AsQueryable();
        }

        public IEnumerable<T> GetAll(params string[] eager)
        {
            IQueryable<T> query = context.Set<T>().AsQueryable();

            if (eager.Length > 0)
            {
                foreach (var eger in eager)
                {
                    query = query.Include(eger);
                }
            }
            return query.ToList();

        }

        public async Task<IEnumerable<T>> GetAllAsync(params string[] eager)
        {
            IQueryable<T> query = context.Set<T>().AsQueryable();

            if (eager.Length > 0)
            {
                foreach (var eger in eager)
                {
                    query = query.Include(eger);
                }
            }
            return await query.ToListAsync();

        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }


        //public T Search(Expression<Func<T, bool>> search)
        //{
        //    return this.DbSet.SingleOrDefault(search);
        //}
        public IQueryable<T> Search(Expression<Func<T, bool>> search)
        {
            IQueryable<T> query = context.Set<T>().AsQueryable();
            if (search != null)
            {
                query = query.Where(search);
            }
            return query;
        }

        public IQueryable<T> Sort(IQueryable<T> query, string columnName = "Id", bool isAscending = true)
        {
            if (!string.IsNullOrEmpty(columnName))
            {
                query = query.OrderBy(columnName, isAscending);  // Apply dynamic sorting
            }
            return query;
        }

        public IQueryable<T> Filter(Expression<Func<T, bool>> expression,
           string columnName = "Id", bool IsAscending = false,
           int PageSize = 5, int PageNumber = 1)
        {
            IQueryable<T> query = this.context.Set<T>().AsQueryable();

            //Filter .........
            if (expression != null)
                query = query.Where(expression);


            //sort .........
            if (!string.IsNullOrEmpty(columnName))
                query = query.OrderBy(columnName, IsAscending);


            //pagination PageSize , PageNumber
            if (PageNumber < 0)
            {
                PageNumber = 1;
            }
            if (PageSize < 0)
            {
                PageSize = 5;
            }
            int RowCount = query.Count();
            if (RowCount < PageSize)
            {
                PageSize = RowCount;
                PageNumber = 1;
            }
            //todo : 40 last 40
            int toSkip = (PageNumber - 1) * PageSize;
            query = query.Skip(toSkip).Take(PageSize);

            return query;
        }
  //      public IQueryable<T> Filter(
  //Expression<Func<T, bool>> expression,
  //string columnName = "Id",
  //bool IsAscending = false,
  //int PageSize = 5,
  //int PageNumber = 1)
  //      {
  //          IQueryable<T> query = this.context.Set<T>().AsQueryable();

  //          // Filtering logic
  //          if (expression != null)
  //          {
  //              query = query.Where(expression);
  //          }

  //          // Sorting logic
  //          if (!string.IsNullOrEmpty(columnName))
  //          {
  //              query = query.OrderBy(columnName, IsAscending);
  //          }

  //          // Total count before pagination (for the Pagination object)
  //          int totalCount = query.Count();

  //          // Pagination logic
  //          if (PageNumber < 1)
  //          {
  //              PageNumber = 1;
  //          }

  //          if (PageSize < 1)
  //          {
  //              PageSize = 5;
  //          }

  //          int toSkip = (PageNumber - 1) * PageSize;
  //          query = query.Skip(toSkip).Take(PageSize);

  //          // Returning Pagination object
  //          return new Pagination<IQueryable<T>>
  //          {
  //              PageSize = PageSize,
  //              PageNumber = PageNumber,
  //              TotalCount = totalCount,
  //              Data = query
  //          };
  //      }


        //    public Pagination<IQueryable<T>> Filter(
        //Expression<Func<T, bool>> expression,
        //string columnName = "Id",
        //bool IsAscending = false,
        //int PageSize = 5,
        //int PageNumber = 1)
        //    {
        //        IQueryable<T> query = this.context.Set<T>().AsQueryable();

        //        // Filtering logic
        //        if (expression != null)
        //        {
        //            query = query.Where(expression);
        //        }

        //        // Sorting logic
        //        if (!string.IsNullOrEmpty(columnName))
        //        {
        //            query = query.OrderBy(columnName, IsAscending);
        //        }

        //        // Total count before pagination (for the Pagination object)
        //        int totalCount = query.Count();

        //        // Pagination logic
        //        if (PageNumber < 1)
        //        {
        //            PageNumber = 1;
        //        }

        //        if (PageSize < 1)
        //        {
        //            PageSize = 5;
        //        }

        //        int toSkip = (PageNumber - 1) * PageSize;
        //        query = query.Skip(toSkip).Take(PageSize);

        //        // Returning Pagination object
        //        return new Pagination<IQueryable<T>>
        //        {
        //            PageSize = PageSize,
        //            PageNumber = PageNumber,
        //            TotalCount = totalCount,
        //            Data = query
        //        };
        //    }


        public bool Add(T Data)
        {
            try
            {
                context.Set<T>().Add(Data);
                this.context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        public bool Update(T Data)
        {
            try
            {
                context.Set<T>().Update(Data);
                this.context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool Delete(T Data)
        {
            try
            {
                context.Set<T>().Remove(Data);
                this.context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }


        public void AddList(IEnumerable<T> myList)
        {
            context.Set<T>().AddRange(myList);
            context.SaveChanges();
        }

        public void UpdateList(IEnumerable<T> myList)
        {
            context.Set<T>().UpdateRange(myList);
            context.SaveChanges();
        }

        public void DeleteList(IEnumerable<T> myList)
        {
            context.Set<T>().RemoveRange(myList);
            context.SaveChanges();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter, params string[] eager)
        {
            IQueryable<T> query = context.Set<T>().Where(filter);  // apply the filter here

            if (eager.Length > 0)
            {
                foreach (var eger in eager)
                {
                    query = query.Include(eger);
                }
            }

            return query.ToList();
        }

        //Add filtering support for GetAllAsync
            public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter, params string[] eager)
        {
            IQueryable<T> query = context.Set<T>().Where(filter);  // Apply the filter here

            if (eager.Length > 0)
            {
                foreach (var eger in eager)
                {
                    query = query.Include(eger);
                }
            }

            return await query.ToListAsync();
        }









        //public class BaseManager<T> where T : class
        //{
        //    protected ProjectContext context;
        //    private DbSet<T> DbSet;

        //    public BaseManager(ProjectContext context)
        //    {
        //        this.context = context;
        //        DbSet = context.Set<T>();
        //    }

        //    public T GetById(int id)
        //    {
        //        return context.Set<T>().Find(id);
        //    }

        //    public IQueryable<T> GetAll()
        //    {
        //        return context.Set<T>().AsQueryable();
        //    }

        //    public IEnumerable<T> GetAll(params string[] eager)
        //    {
        //        IQueryable<T> query = context.Set<T>().AsQueryable();
        //        if (eager.Length > 0)
        //        {
        //            foreach (var eger in eager)
        //            {
        //                query = query.Include(eger);
        //            }
        //        }
        //        return query.ToList();
        //    }

        //    // Add filtering support for GetAll
        //    public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter, params string[] eager)
        //    {
        //        IQueryable<T> query = context.Set<T>().Where(filter);  // Apply the filter here

        //        if (eager.Length > 0)
        //        {
        //            foreach (var eger in eager)
        //            {
        //                query = query.Include(eger);
        //            }
        //        }

        //        return query.ToList();
        //    }

        //    public async Task<IEnumerable<T>> GetAllAsync(params string[] eager)
        //    {
        //        IQueryable<T> query = context.Set<T>().AsQueryable();
        //        if (eager.Length > 0)
        //        {
        //            foreach (var eger in eager)
        //            {
        //                query = query.Include(eger);
        //            }
        //        }
        //        return await query.ToListAsync();
        //    }

        //    // Add filtering support for GetAllAsync
        //    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter, params string[] eager)
        //    {
        //        IQueryable<T> query = context.Set<T>().Where(filter);  // Apply the filter here

        //        if (eager.Length > 0)
        //        {
        //            foreach (var eger in eager)
        //            {
        //                query = query.Include(eger);
        //            }
        //        }

        //        return await query.ToListAsync();
        //    }

        //    // The rest of your BaseManager methods remain unchanged...
        //}




    }
}