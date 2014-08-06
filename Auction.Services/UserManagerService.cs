using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Auction.DAL.Models;
using Auction.DAL.UnitOfWork;
using Auction.Domain.Models;
using Auction.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Auction.Services
{
    public class UserManagerService : BaseService, IUserManagerService
    {
        public UserManagerService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            Uow.UserManager = InitUserManager(Uow.UserManager);
        }

        private UserManager<ApplicationUser> InitUserManager(UserManager<ApplicationUser> manager)
        {
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is: {0}"
            });
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is: {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var identityFactoryOptions = new IdentityFactoryOptions<UserManager<ApplicationUser>>();
            var dataProtectionProvider = identityFactoryOptions.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public ApplicationUserDomainModel FindByName(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            var user = Uow.UserManager.FindByName(name);
            var applicationUserDomainModel = Mapper.Map<ApplicationUser, ApplicationUserDomainModel>(user);
            return applicationUserDomainModel;
        }

        public async Task<ApplicationUserDomainModel> FindAsync(string email, string password)
        {
            if (email == null) throw new ArgumentNullException("email");
            if (password == null) throw new ArgumentNullException("password");
            var user = await Uow.UserManager.FindAsync(email, password);
            var applicationUserDomainModel = Mapper.Map<ApplicationUser, ApplicationUserDomainModel>(user);
            return applicationUserDomainModel;
        }

        public async Task<String> GenerateEmailConfirmationTokenAsync(string id)
        {
            if (id == null) throw new ArgumentNullException("id");
            string code = await Uow.UserManager.GenerateEmailConfirmationTokenAsync(id);
            return code;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUserDomainModel userDomainModel, string password)
        {
            if (userDomainModel == null) throw new ArgumentNullException("userDomainModel");
            if (password == null) throw new ArgumentNullException("password");
            var user = Mapper.Map<ApplicationUserDomainModel, ApplicationUser>(userDomainModel);
            IdentityResult result = await Uow.UserManager.CreateAsync(user, password);
            return result;
        }

        public async Task SendEmailAsync(string userId, string message, string body)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (message == null) throw new ArgumentNullException("message");
            if (body == null) throw new ArgumentNullException("body");
            await Uow.UserManager.SendEmailAsync(userId, message, body);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string code)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (code == null) throw new ArgumentNullException("code");
            IdentityResult result = await Uow.UserManager.ConfirmEmailAsync(userId, code);
            return result;
        }

        public async Task<ApplicationUserDomainModel> FindByNameAsync(string email)
        {
            if (email == null) throw new ArgumentNullException("email");
            var user = await Uow.UserManager.FindByNameAsync(email);
            var applicationUserDomainModel = Mapper.Map<ApplicationUser, ApplicationUserDomainModel>(user);
            return applicationUserDomainModel;
        }

        public async Task<bool> IsEmailConfirmedAsync(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            bool isEmailConfirmed = await Uow.UserManager.IsEmailConfirmedAsync(userId);
            return isEmailConfirmed;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            string code = await Uow.UserManager.GeneratePasswordResetTokenAsync(userId);
            return code;
        }

        public async Task<IdentityResult> ResetPasswordAsync(string userId, string code, string password)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (code == null) throw new ArgumentNullException("code");
            if (password == null) throw new ArgumentNullException("password");
            IdentityResult result = await Uow.UserManager.ResetPasswordAsync(userId, code, password);
            return result;
        }

        public async Task<IdentityResult> RemoveLoginAsync(string userId, UserLoginInfo userLoginInfo)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (userLoginInfo == null) throw new ArgumentNullException("userLoginInfo");
            IdentityResult result = await Uow.UserManager.RemoveLoginAsync(userId, userLoginInfo);
            return result;
        }

        public async Task<ApplicationUserDomainModel> FindByIdAsync(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            var user = await Uow.UserManager.FindByIdAsync(userId);
            var applicationUserDomainModel = Mapper.Map<ApplicationUser, ApplicationUserDomainModel>(user);
            return applicationUserDomainModel;
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (oldPassword == null) throw new ArgumentNullException("oldPassword");
            if (newPassword == null) throw new ArgumentNullException("newPassword");
            IdentityResult result = await Uow.UserManager.ChangePasswordAsync(userId, oldPassword, newPassword);
            return result;
        }

        public async Task<IdentityResult> AddPasswordAsync(string userId, string newPassword)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (newPassword == null) throw new ArgumentNullException("newPassword");
            IdentityResult result = await Uow.UserManager.AddPasswordAsync(userId, newPassword);
            return result;
        }

        public async Task<ApplicationUserDomainModel> FindAsync(UserLoginInfo userLoginInfo)
        {
            if (userLoginInfo == null) throw new ArgumentNullException("userLoginInfo");
            var user = await Uow.UserManager.FindAsync(userLoginInfo);
            var applicationUserDomainModel = Mapper.Map<ApplicationUser, ApplicationUserDomainModel>(user);
            return applicationUserDomainModel;
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo userLoginInfo)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (userLoginInfo == null) throw new ArgumentNullException("userLoginInfo");
            IdentityResult result = await Uow.UserManager.AddLoginAsync(userId, userLoginInfo);
            return result;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUserDomainModel userDomainModel)
        {
            if (userDomainModel == null) throw new ArgumentNullException("userDomainModel");
            var user = Mapper.Map<ApplicationUserDomainModel, ApplicationUser>(userDomainModel);
            IdentityResult result = await Uow.UserManager.CreateAsync(user);
            return result;
        }

        public IList<UserLoginInfo> GetLogins(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            var linkedAccounts = Uow.UserManager.GetLogins(userId);
            return linkedAccounts;
        }

        public ApplicationUserDomainModel FindById(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            var user = Uow.UserManager.FindById(userId);
            var applicationUserDomainModel = Mapper.Map<ApplicationUser, ApplicationUserDomainModel>(user);
            return applicationUserDomainModel;
        }

        public IEnumerable<ApplicationUserDomainModel> GetUsers()
        {

            var users = Uow.UserManager.Users;
            var usersDomainModel = Mapper.Map<IEnumerable<ApplicationUser>,
                IEnumerable<ApplicationUserDomainModel>>(users);
            return usersDomainModel;
        }

        public bool IsInRole(string userId, string role)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (role == null) throw new ArgumentNullException("role");
            bool isInRole = Uow.UserManager.IsInRole(userId, role);
            return isInRole;
        }

        public void RemoveFromRole(string userId, string role)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (role == null) throw new ArgumentNullException("role");
            Uow.UserManager.RemoveFromRole(userId, role);
        }

        public void AddToRole(string userId, string role)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (role == null) throw new ArgumentNullException("role");
            Uow.UserManager.AddToRole(userId, role);
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserDomainModel userDomainModel)
        {
            if (userDomainModel == null) throw new ArgumentNullException("userDomainModel");
            var user = Mapper.Map<ApplicationUserDomainModel, ApplicationUser>(userDomainModel);
            var claimsIdentity = await user.GenerateUserIdentityAsync(Uow.UserManager);
            return claimsIdentity;
        }

        public IList<string> GetRoles(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            var roles = Uow.UserManager.GetRoles(userId);
            return roles;
        }

        public void BanUser(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            var targetUser = Uow.UserManager.FindById(userId);
            if (targetUser == null)
            {
                return;
            }
            targetUser.IsBanned = true;
            Uow.UserManager.Update(targetUser);
        }

        public void UnbanUser(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            var targetUser = Uow.UserManager.FindById(userId);
            if (targetUser == null)
            {
                return;
            }
            targetUser.IsBanned = false;
            Uow.UserManager.Update(targetUser);
        }
    }
}
