using DAl.IRepository;
using Infrastructure.Utlities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DAl.Repository
{
    public class BaseRepositrory<T> : IBaseRepositrory<T> where T : class
    {


        private readonly ApplicationDbContext _context;

        public BaseRepositrory(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public StatusOpertion Create(T item)
        {
            var status = new StatusOpertion();


            var result = _context.Set<T>().Add(item);

            if (result == null)
            {
                status.Status = false;
                status.StatusMessage = "Created Filed";

                return status;
            }

            status.Status = true;
            status.StatusMessage = "Created Sucessfully";


            return status;

        }

        public async Task<StatusOpertion> CreateRange(List<T> items)
        {

            var status = new StatusOpertion();
            try
            {
                await _context.Set<T>().AddRangeAsync(items);

                status.Status = true;
                status.StatusMessage = "Created Successfully";
            }
            catch (Exception ex)
            {
                status.Status = false;
                status.StatusMessage = ex.Message;
            }
            return status;


        }

        public List<T> GetItems(bool getRel = true)
        {

            var Items = new List<T>();
            if (getRel)
                Items = _context.Set<T>().ToList();
            else
                Items = _context.Set<T>().AsNoTracking().ToList();

            return Items;

        }

        public async Task<List<T>> UpdateAsync(List<T> values)
        {
            if (values == null || values.Count == 0)
                throw new ArgumentException("Values cannot be null or empty");

            try
            {
                _context.Set<T>().UpdateRange(values);

                return values;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database update failed: {ex.Message}");
                throw new Exception("An error occurred while updating entities.", ex);
            }
        }



        public async Task<T> GetItem(string itemId, string[] includes = null)
        {
            var data = await _context.Set<T>().FindAsync(itemId);

            if (data == null) return null;

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    var navigation = _context.Entry(data).Navigation(include);
                    if (navigation != null && !navigation.IsLoaded)
                    {
                        await navigation.LoadAsync();
                    }
                }
            }

            return data;
        }

        public bool UpdateItem(T value)
        {
            try
            {
                _context.Set<T>().Update(value);


                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating item: {ex.Message}");

                return false;
            }
        }

        public List<T> GetItemsWithFunc(Expression<Func<T, bool>> expression, string[] includes = null)
        {

            IQueryable<T> data = _context.Set<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    data = data.Include(include);
                }
            }


            var result = data.Where(expression).ToList();


            return result;
        }

        public async Task<T> GetItemWithFunc(Expression<Func<T, bool>> expression, string[] includes = null)
        {

            IQueryable<T> data = _context.Set<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    data = data.Include(include);

                }

            }

            var result= await data.FirstOrDefaultAsync(expression);
            return result;

        }
        public async Task<bool> DeleteItemWithFunc(Expression<Func<T, bool>> expression)
        {
            try
            {
                var data = await _context.Set<T>().Where(expression).FirstOrDefaultAsync();

                if (data == null)
                {
                    return false;
                }

                _context.Set<T>().Remove(data);

     

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> DeleteRangeAsync(Expression<Func<T, bool>> expression)
        {
            try
            {
                var items = await _context.Set<T>().Where(expression).ToListAsync();

                if (items == null || items.Count == 0)
                {
                    return false;
                }

                _context.Set<T>().RemoveRange(items);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}"); 
                return false;
            }
        }



    }
}

