using DAl.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;

namespace DAl.IRepository
{
    public interface IUnitOfWork<T> : IDisposable where T : class
    {


        IBaseRepositrory<Category> Category { get; }
        IBaseRepositrory<ApplicationUser> User { get; }
        IBaseRepositrory<Favorite> Favorite { get; set; }
        IBaseRepositrory<Cart> Cart { get; set; }
        IBaseRepositrory<CartItem> CartItem { get; set; }
        UserManager<ApplicationUser> UserManager { get; }
        IBaseRepositrory<CommissionScheme> CommissionScheme { get; set; }
        IBaseRepositrory<Marketer> Marketer { get; set; }
        IBaseRepositrory<CodesToMarketer> CodeToMarketer { get; set; }
        IBaseRepositrory<PricesToshipping> PricesToshipping { get; set; }
        IBaseRepositrory<FavoriteItem> FavoriteItems { get; set; }
        IBaseRepositrory<FavoriteSaller> FavoriteSallers { get; set; }
        IBaseRepositrory<Order> Order { get; set; }
        IBaseRepositrory<FileUploads> FileUploads { get; set; }
        IBaseRepositrory<ComapnyAccount> ComapnyAccount { get; set; }
        IBaseRepositrory<PayMentManoul> PayMentManoul { get; set; }
        IBaseRepositrory<Account> Accounts { get; set; }
        IBaseRepositrory<MarketerAccount> MarketerAccount { get; set; }
        IBaseRepositrory<BillingToMarketr> BillingToMarketr { get; set; }
         IBaseRepositrory<Proplem> Proplem { get; set; }
         IBaseRepositrory<ChatMessage> Message { get; set; }
        Task<bool> SaveChangesAsync();
        bool SaveChanges();

        Task<IDbContextTransaction> BeginTransactionAsync();

    }
}
