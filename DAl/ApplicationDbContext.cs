using DAl.Configuration;
using DAl.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Category> Categories { get; set; }
        //public DbSet<Marketer> Marketers { get; set; }
        //public DbSet<CodesToMarketer> CodesToMarketers { get; set; }


        public DbSet<Proplem> Proplems { get; set; }    
        public DbSet<CodesToMarketer>  CodesToMarketers { get; set; }
        public DbSet<ImagesDynamic> ImagesDynamics { get; set; }

        public DbSet<Marketer> Marketers { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Cart> Carts{ get; set; }
        public DbSet<CartItem> CartItems{ get; set; }
        public DbSet<CommissionScheme> CommissionSchemes { get; set; }
        public DbSet<ReferralCodeUsage> ReferralCodeUsages {get;set;}
        public DbSet<PricesToshipping> PricesToshippings { get; set; }  
        public DbSet<Order> Orders { get; set; }  
        public DbSet<MarketerAccount> MarketerAccounts { get; set; }  
        public DbSet<FavoriteItem> FavoriteItems { get; set; }  
        public DbSet<FavoriteSaller> FavoriteSallers { get; set; }  
        public DbSet<AttributeItem> AttributeItems { get; set; }  
        public DbSet<ComapnyAccount> ComapnyAccounts { get; set; }  
        public DbSet<FileUploads> FileUploads { get; set; }  
        public DbSet<PayMentManoul> PayMentManouls { get; set; }  
        public DbSet<Account> Accounts { get; set; }  
        public DbSet<BillingToMarketr> BillingToMarketrs { get; set; }  
        public DbSet<ChatMessage> ChatMessages { get; set; }  


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            new CategoryConfiguration()
                .Configure(builder.Entity<Category>());

            new CodesToMarketerConfiguration()
               .Configure(builder.Entity<CodesToMarketer>());


            new MarketerConfiguration()
               .Configure(builder.Entity<Marketer>());


            new FavouritConfiguration()
                .Configure(builder.Entity<Favorite>());

            new CartConfiguration()
                .Configure(builder.Entity<Cart>());


            new CommissionSchemeConfiguration()
                .Configure(builder.Entity<CommissionScheme>());        
            
            new ReferralCodeUsageConfiguration()
                .Configure(builder.Entity<ReferralCodeUsage>()); 

            new ComapnyAccountCongiguration()
                .Configure(builder.Entity<ComapnyAccount>());
                        
            new MarketerAccountConfiguration()
                .Configure(builder.Entity<MarketerAccount>()); 
            
            new PayMentManoulConfiguration()
                .Configure(builder.Entity<PayMentManoul>());      
            
            
            new BillingToMarketrConfiguration()
                .Configure(builder.Entity<BillingToMarketr>());



        }

    }
}
