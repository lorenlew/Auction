using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Auction.Domain.Models;
using Microsoft.AspNet.Identity;

namespace Auction.Services.Interfaces
{
    public interface IUserManagerService : IService
    {

        ApplicationUserDomainModel FindByName(string name);

        Task<ApplicationUserDomainModel> FindAsync(string email, string password);

        Task<String> GenerateEmailConfirmationTokenAsync(string id);

        Task<IdentityResult> CreateAsync(ApplicationUserDomainModel user, string password);

        Task SendEmailAsync(string userId, string message, string body);

        Task<IdentityResult> ConfirmEmailAsync(string userId, string code);

        Task<ApplicationUserDomainModel> FindByNameAsync(string email);

        Task<bool> IsEmailConfirmedAsync(string userId);

        Task<string> GeneratePasswordResetTokenAsync(string userId);

        Task<IdentityResult> ResetPasswordAsync(string userId, string code, string password);

        Task<IdentityResult> RemoveLoginAsync(string userId, UserLoginInfo userLoginInfo);

        Task<ApplicationUserDomainModel> FindByIdAsync(string userId);

        Task<IdentityResult> ChangePasswordAsync(string userId, string oldPassword, string newPassword);

        Task<IdentityResult> AddPasswordAsync(string userId, string newPassword);

        Task<ApplicationUserDomainModel> FindAsync(UserLoginInfo userLoginInfo);

        Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo userLoginInfo);

        Task<IdentityResult> CreateAsync(ApplicationUserDomainModel user);

        IList<UserLoginInfo> GetLogins(string userId);

        ApplicationUserDomainModel FindById(string userId);

        IEnumerable<ApplicationUserDomainModel> GetUsers();

        bool IsInRole(string userId, string role);

        void RemoveFromRole(string userId, string role);

        void AddToRole(string userId, string role);

        Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserDomainModel userDomainModel);

        IList<string> GetRoles(string userId);

        void BanUser(string userId);

        void UnbanUser(string userId);
    }
}
