using DAl.IRepository;
using DAl.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using Twilio.Rest.Accounts.V1.Credential;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ThirdParty.Utilities
{
    public class SeederData
    {
        private readonly IUnitOfWork<CommissionScheme> _unitOfWork;
        private readonly IUnitOfWork<PricesToshipping> _unitOfWorkshipping;
        private readonly IUnitOfWork<Account> _unitOfWorksaccount;
        private readonly IUnitOfWork<ApplicationUser> _unitOfWorkuser;
        private readonly IAuthRepository _aurhrep;
        private readonly IOptions<AdminData> _options;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeederData(
       IUnitOfWork<CommissionScheme> unitOfWork,
       IUnitOfWork<PricesToshipping> unitOfWorkshipping,
       IUnitOfWork<Account> unitOfWorksaccount,
       IUnitOfWork<ApplicationUser> unitOfWorkuser,
       IAuthRepository aurhrep,
       IOptions<AdminData> options,
       RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _unitOfWorkshipping = unitOfWorkshipping;
            _unitOfWorksaccount = unitOfWorksaccount;
            _unitOfWorkuser = unitOfWorkuser;
            _aurhrep = aurhrep;
            _options = options;
            _roleManager = roleManager;
        }
    
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "Company", "Markter", "Normal" };

            foreach (var roleName in roleNames)
            {


                if (!await roleManager.RoleExistsAsync(roleName))
                {

                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public static async Task SeedDataInCommissionScheme(IUnitOfWork<CommissionScheme> unitOfWork)
        {


            if (unitOfWork.CommissionScheme.GetItems().Count() == 0)
            {
                List<CommissionScheme> listCommissionScheme = new()
                            {
                                new CommissionScheme
                                {
                                    LowerLimit = 0,
                                    UpperLimit = 1000,
                                    CommissionRate = 0.12,
                                    StartDate = DateTime.UtcNow,
                                    EndDate = DateTime.UtcNow.AddYears(1),
                                    IsActive = true,
                                    UserType = UserType.Normal
                                },
                                new CommissionScheme
                                {
                                    LowerLimit = 0,
                                    UpperLimit = 1000,
                                    CommissionRate = 0.05,
                                    StartDate = DateTime.UtcNow,
                                    EndDate = DateTime.UtcNow.AddYears(1),
                                    IsActive = true,
                                    UserType = UserType.Markter
                                },
                            };

                foreach (var commission in listCommissionScheme)
                {
                    if (commission.LowerLimit >= commission.UpperLimit)
                    {
                        throw new ArgumentException($"LowerLimit must be less than UpperLimit for the commission scheme with UserType: {commission.UserType}");
                    }
                }

                await unitOfWork.CommissionScheme.CreateRange(listCommissionScheme);


                await unitOfWork.SaveChangesAsync();
            }
        }


        public static async Task SeedDataShiping(IUnitOfWork<PricesToshipping> unitOfWork)
        {


            var data = unitOfWork.PricesToshipping.GetItems();

            if (!data.Any())
            {


                var PricesToshippings = new List<PricesToshipping>
                                {
                                    new PricesToshipping
                                    {
                                        TypeToShiping = TypeToShiping.Weight,
                                        LowerLimit = 1,
                                        UpperLimit = 5,
                                        Price = 23,
                                        CreateAt = DateTime.UtcNow,
                                        UpdateAt = DateTime.UtcNow,
                                        IsActived = true
                                    },
                                    new PricesToshipping
                                    {
                                        TypeToShiping = TypeToShiping.Weight,
                                        LowerLimit = 5,
                                        UpperLimit = 15,
                                        Price = 18,
                                        CreateAt = DateTime.UtcNow,
                                        UpdateAt = DateTime.UtcNow,
                                        IsActived = true
                                    },
                                    new PricesToshipping
                                    {
                                        TypeToShiping = TypeToShiping.Weight,
                                        LowerLimit = 15,
                                        UpperLimit = 30,
                                        Price = 16,
                                        CreateAt = DateTime.UtcNow,
                                        UpdateAt = DateTime.UtcNow,
                                        IsActived = true
                                    },
                                    new PricesToshipping
                                    {
                                        TypeToShiping = TypeToShiping.Weight,
                                        LowerLimit = 30,
                                        UpperLimit = 70,
                                        Price = 14,
                                        CreateAt = DateTime.UtcNow,
                                        UpdateAt = DateTime.UtcNow,
                                        IsActived = true
                                    },
                                    new PricesToshipping
                                    {
                                        TypeToShiping = TypeToShiping.Weight,
                                        LowerLimit = 70,
                                        UpperLimit = 100,
                                        Price = 13,
                                        CreateAt = DateTime.UtcNow,
                                        UpdateAt = DateTime.UtcNow,
                                        IsActived = true
                                    },
                                    new PricesToshipping
                                    {
                                        TypeToShiping = TypeToShiping.Weight,
                                        LowerLimit = 100,
                                        UpperLimit = 175,
                                        Price = 12,
                                        CreateAt = DateTime.UtcNow,
                                        UpdateAt = DateTime.UtcNow,
                                        IsActived = true
                                    },
                                    new PricesToshipping
                                    {
                                        TypeToShiping = TypeToShiping.Weight,
                                        LowerLimit = 175,
                                        UpperLimit = double.MaxValue,
                                        Price = 11,
                                        CreateAt = DateTime.UtcNow,
                                        UpdateAt = DateTime.UtcNow,
                                        IsActived = true
                                    },
                                    new PricesToshipping  {
                                        TypeToShiping = TypeToShiping.Volume,
                                        LowerLimit = 1,
                                        UpperLimit = 1,
                                        Price = 220,
                                        CreateAt = DateTime.UtcNow,
                                        UpdateAt = DateTime.UtcNow,
                                        IsActived = true
                                    }


                };


                await unitOfWork.PricesToshipping.CreateRange(PricesToshippings);


                await unitOfWork.SaveChangesAsync();



            }



        }


        public static async Task SeedDataAccounts(IUnitOfWork<Account> unitOfWork)
        {


            var data = unitOfWork.Accounts.GetItems();

            if (!data.Any())
            {


                var accounts = new List<Account>
                {
                        new Account
                        {
                            NameofAccount = "شركة زي مابدك للتجارة والاستيراد",
                            NameOfBank = "شركة العمقي للصرافة",
                            NumberOfAccount = "254132645",
                            IsActive = true

                        },
                        new Account
                        {
                            NameofAccount = "شركة زي مابدك للتجارة والاستيراد",
                            NameOfBank = "شركة البسيري للصرافة",
                            NumberOfAccount = "23251495",
                            IsActive = true
                        },
                        new Account
                        {
                            NameofAccount = "شركة زي مابدك للتجارة والاستيراد",
                            NameOfBank = "شركة بن دول للصرافة",
                            NumberOfAccount = "2322662",
                            IsActive = true
                        },
                        new Account
                        {
                            NameofAccount = "شركة زي مابدك للتجارة والاستيراد",
                            NameOfBank = "بنك التضامن",
                            NumberOfAccount = "00114385",
                            IsActive = true
                        }

                };

                await unitOfWork.Accounts.CreateRange(accounts);
                await unitOfWork.SaveChangesAsync();



            }



        }


        public static async Task SeedAdminInfo(IUnitOfWork<ApplicationUser> unitOfWork, IAuthRepository authRepository, IOptions<AdminData> options)
        {
            var admindata = options.Value;

            var existingUser = await unitOfWork.User.GetItemWithFunc(e => e.Email == admindata.Email);

            if (existingUser == null)
            {
                var user = new ApplicationUser()
                {
                    Email = admindata.Email,
                    FullName = $"{admindata.FirstName} {admindata.LastName}",
                    IsComapny = false,
                    IsMarketer = false,
                    PhoneNumber = admindata.Phone,
                    UserName = new MailAddress(admindata.Email).User,
                    CreatedAt = DateTime.UtcNow,
                    UpdateAt = DateTime.UtcNow,
                };

                var result = await authRepository.RegisterMethode(user, admindata.Password, "Admin");

                if (result.Succeeded)
                {
                    await unitOfWork.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Failed to register admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }


        public async Task SeedDataAsync()
        {
            await SeederData.SeedDataInCommissionScheme(_unitOfWork);
            await SeederData.SeedDataShiping(_unitOfWorkshipping);
            await SeederData.SeedRolesAsync(_roleManager);
            await SeederData.SeedDataAccounts(_unitOfWorksaccount);
            await SeederData.SeedAdminInfo(_unitOfWorkuser, _aurhrep, _options);
        }

    }
}
