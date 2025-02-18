using DAl.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.IRepository
{
    public interface IUnitOfWork<T> : IDisposable where T : class
    {


         IBaseRepositrory<Category> Category { get; }
        UserManager<ApplicationUser> UserManager { get; }
        Task<bool> SaveChangesAsync();
        bool SaveChanges();

        Task<IDbContextTransaction> BeginTransactionAsync();

    }
}
