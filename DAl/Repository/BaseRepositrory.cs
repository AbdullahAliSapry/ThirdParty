using DAl.IRepository;
using Infrastructure.Utlities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;

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




    }
}
