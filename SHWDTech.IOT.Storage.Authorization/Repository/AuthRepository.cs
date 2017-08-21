using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SHWDTech.IOT.Storage.Authorization.Entities;
using SHWDTech.IOT.Storage.Authorization.Models;

namespace SHWDTech.IOT.Storage.Authorization.Repository
{
    public class AuthRepository : IDisposable
    {
        private readonly AuthorizationDbContext _ctx;

        private readonly UserManager<SHWDIdentityUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthRepository()
        {
            _ctx = new AuthorizationDbContext();
            _userManager = new UserManager<SHWDIdentityUser>(new UserStore<SHWDIdentityUser>(_ctx));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_ctx));
        }

        public async Task<IdentityResult> RegisterUser(UserRegisterModel userModel)
        {
            var user = new SHWDIdentityUser
            {
                UserName = userModel.UserName,
                PhoneNumber = userModel.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<SHWDIdentityUser> FindUser(string userName, string password)
        {
            var user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public Audience FindAudience(string clientId)
        {
            return _ctx.Audiences.FirstOrDefault(a => a.Id == clientId);
        }

        public async Task<SHWDIdentityUser> FindById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return user;
        }

        public IdentityRole FindDustRoleById(string id)
        {
            return _ctx.Roles.First(r => r.Id == id);
        }

        public async Task<IList<string>> GetUserRoles(SHWDIdentityUser user)
        {
            var roles = await _userManager.GetRolesAsync(user.Id);

            return roles;
        }

        public List<IdentityRole> GetDustRoles(Expression<Func<IdentityRole, bool>> exp)
        {
            var query = _ctx.Roles.AsQueryable();
            if (exp != null)
            {
                query = query.Where(exp);
            }

            return query.ToList();
        }

        public IdentityResult DeleteUser(string userId)
        {
            var user = _userManager.FindById(userId);
            return _userManager.Delete(user);
        }

        public IdentityRole GetDustRole(SHWDIdentityUser user)
        {
            if (user.Roles == null || user.Roles.Count <= 0)
            {
                return new IdentityRole();
            }

            var dustRole = _ctx.Roles.First(r => r.Id == user.Roles.First().RoleId);
            return dustRole;
        }

        public async Task<IList<Claim>> GetUserClaims(SHWDIdentityUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user.Id);

            return claims;
        }

        public IdentityResult RemoveClaim(string userId, Claim claim)
        {
            return _userManager.RemoveClaim(userId, claim);
        }

        public IdentityResult ChangePassword(string userId, string currentPassword, string newPassword)
        {
            return _userManager.ChangePassword(userId, currentPassword, newPassword);
        }

        public async Task<SHWDIdentityUser> FindAsync(UserLoginInfo loginInfo)
        {
            var user = await _userManager.FindAsync(loginInfo);

            return user;
        }

        public async Task<IdentityResult> CreateAsync(SHWDIdentityUser user)
        {
            var result = await _userManager.CreateAsync(user);

            return result;
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await _userManager.AddLoginAsync(userId, login);

            return result;
        }

        public bool UserInRole(string userId, string roleName)
        {
            return _userManager.IsInRoleAsync(userId, roleName).Result;
        }

        public SHWDIdentityUser FindByName(string name)
        {
            return _userManager.FindByName(name);
        }

        public Guid FindVendorId(SHWDIdentityUser usr)
        {
            var claims = _userManager.GetClaimsAsync(usr.Id).Result;
            return Guid.Parse(claims.First(c => c.Type == "VendorId").Value);
        }

        public int GetUserCount(Expression<Func<SHWDIdentityUser, bool>> exp)
        {
            return exp == null ? _userManager.Users.Count() : _userManager.Users.Count(exp);
        }

        public int GetRoleCount(Expression<Func<IdentityRole, bool>> exp)
        {
            return exp == null ? _roleManager.Roles.Count() : _roleManager.Roles.Count(exp);
        }

        public List<SHWDIdentityUser> GetUserTable(int offset, int limit)
        {
            return _userManager.Users.OrderBy(u => u.Id).Skip(offset).Take(limit).ToList();
        }

        public async Task<IdentityResult> UpdateAsync(SHWDIdentityUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public IdentityResult Update(SHWDIdentityUser user)
        {
            return _userManager.Update(user);
        }

        public void UserAddRole(SHWDIdentityUser user, string roleId)
        {
            var role = _roleManager.FindById(roleId);
            _userManager.AddToRole(user.Id, role.Name);
        }

        public IdentityRole FindRoleById(string roleId)
        {
            return _roleManager.FindById(roleId);
        }

        public void UserRemoveFromRoles(SHWDIdentityUser user, string[] roleIds)
        {
            var roles = roleIds.Select(r => _roleManager.FindById(r).Name).ToArray();
            _userManager.RemoveFromRoles(user.Id, roles);
        }

        public ServiceSchema FindServiceSchema(string schemaName)
        {
            return _ctx.ServiceSchemas.FirstOrDefault(s => s.SchemaName == schemaName);
        }

        public HmacAuthenticationService FindHmacAuthenticationServiceByAppId(string authenticationName, string appId)
        {
            return _ctx.HmacAuthenticationServices.FirstOrDefault(s => s.AuthenticationName == authenticationName && s.AppId == appId);
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();
        }
    }
}
