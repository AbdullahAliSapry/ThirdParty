using DAl.IRepository;
using DAl.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;

namespace DAl.Repository
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : class
    {

        private readonly ApplicationDbContext _context;
        public IBaseRepositrory<Category> Category { get;set;}
        public IBaseRepositrory<ApplicationUser> User { get;set;}
        public UserManager<ApplicationUser> UserManager { get; set; }
        public IBaseRepositrory<Favorite> Favorite { get; set; }
        public IBaseRepositrory<CommissionScheme> CommissionScheme { get; set; }
        public IBaseRepositrory<Marketer> Marketer { get; set; }
        public IBaseRepositrory<CodesToMarketer> CodeToMarketer { get; set; }
        public IBaseRepositrory<PricesToshipping> PricesToshipping { get; set; }
        public IBaseRepositrory<FavoriteItem> FavoriteItems { get; set; }
        public IBaseRepositrory<FavoriteSaller> FavoriteSallers { get; set; }
        public IBaseRepositrory<Cart> Cart { get; set; }
        public IBaseRepositrory<CartItem> CartItem { get; set; }
        public IBaseRepositrory<Order> Order { get; set; }
        public IBaseRepositrory<FileUploads> FileUploads { get; set; }
        public IBaseRepositrory<ComapnyAccount> ComapnyAccount { get; set; }
        public IBaseRepositrory<PayMentManoul> PayMentManoul { get; set; }
        public IBaseRepositrory<Account> Accounts { get; set; }
        public IBaseRepositrory<MarketerAccount> MarketerAccount { get; set; }
        public IBaseRepositrory<BillingToMarketr> BillingToMarketr { get; set; }
        public IBaseRepositrory<Proplem> Proplem { get; set; }
        public IBaseRepositrory<ChatMessage> Message { get; set; }
        public IBaseRepositrory<ImagesDynamic> ImagesDynamic { get; set; }
        public IBaseRepositrory<BannedIP> BannedIP { get; set; }

        public UnitOfWork(ApplicationDbContext context, IBaseRepositrory<Category> category, UserManager<ApplicationUser> userManager, IBaseRepositrory<ApplicationUser> user, IBaseRepositrory<Favorite> favorite, IBaseRepositrory<CommissionScheme> commissionScheme, IBaseRepositrory<Marketer> marketer, IBaseRepositrory<CodesToMarketer> codeToMarketer, IBaseRepositrory<PricesToshipping> pricesToshipping, IBaseRepositrory<Cart> cart, IBaseRepositrory<FavoriteItem> favoriteItems, IBaseRepositrory<FavoriteSaller> favoriteSallers, IBaseRepositrory<CartItem> cartItem, IBaseRepositrory<Order> order, IBaseRepositrory<FileUploads> fileUploads, IBaseRepositrory<ComapnyAccount> comapnyAccount, IBaseRepositrory<PayMentManoul> payMentManoul, IBaseRepositrory<Account> accounts, IBaseRepositrory<MarketerAccount> marketerAccount, IBaseRepositrory<BillingToMarketr> billingToMarketr, IBaseRepositrory<Proplem> proplem, IBaseRepositrory<ChatMessage> message, IBaseRepositrory<ImagesDynamic> imagesDynamic, IBaseRepositrory<BannedIP> bannedIP)
        {
            this._context = context;
            this.Category = category;
            UserManager = userManager;
            User = user;
            Favorite = favorite;
            CommissionScheme = commissionScheme;
            Marketer = marketer;
            CodeToMarketer = codeToMarketer;
            PricesToshipping = pricesToshipping;
            Cart = cart;
            FavoriteItems = favoriteItems;
            FavoriteSallers = favoriteSallers;
            CartItem = cartItem;
            Order = order;
            FileUploads = fileUploads;
            ComapnyAccount = comapnyAccount;
            PayMentManoul = payMentManoul;
            Accounts = accounts;
            MarketerAccount = marketerAccount;
            BillingToMarketr = billingToMarketr;
            Proplem = proplem;
            Message = message;
            ImagesDynamic = imagesDynamic;
            BannedIP = bannedIP;
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
