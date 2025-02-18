using DAl.IRepository;
using DAl.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Repository
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : class
    {

        private readonly ApplicationDbContext _context;
        public IBaseRepositrory<Category> Category { get;set;}
        public UserManager<ApplicationUser> UserManager { get; set; }
        public UnitOfWork(ApplicationDbContext context, IBaseRepositrory<Category> category, UserManager<ApplicationUser> userManager)
        {
            this._context = context;
            this.Category = category;
            UserManager = userManager;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public bool SaveChanges()
        {

            var reault = _context.SaveChanges();

            return reault > 0 ? true : false;
        }

        public async Task<bool> SaveChangesAsync()
        {

            var reault = await _context.SaveChangesAsync();

            return reault > 0 ? true : false;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
          
            return await _context.Database.BeginTransactionAsync();


        }
    }
}
