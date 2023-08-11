using EShop.Models;
using EShop.Models.ViewModels;
using EShop.Repositories;
using EShop.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace EShop.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> PasswordSingIn(LoginViewModel model, HttpContext httpContext)
        {
            var user = await _userRepository.GetSingleByCondition(u => u.Email == model.UserEmail && u.Password == model.Password);
            
            if(user != null)
            {
                SingIn(user, httpContext);
            }

            return user;
        }

        public async Task SingIn(User user, HttpContext httpContext)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var authProperties = new AuthenticationProperties { };
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);
        }

        public async Task<User> GetUser(ClaimsPrincipal principal)
        {
            int userId = Int32.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _userRepository.GetSingleById(userId);
            return user;
        }

        public async Task<int> GetUserId(ClaimsPrincipal principal)
        {
            return Int32.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public async Task<User> GetSingleById(int id)
        {
            return await _userRepository.GetSingleById(id);
        }

        public async Task<PaginatedList<User>> GetPaginatedList(string? searchString, string? sortOrder = null, int pageIndex = 1, int pageSize = 5)
        {
            var query = await _userRepository.GetAll();
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(u => u.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    query = query.OrderBy(u => u.Name);
                    break;
                default:
                    query = query.OrderByDescending(u => u.Name); 
                    break;
            }
            return await PaginatedList<User>.CreateAsync(query, pageIndex, pageSize);
        }

        public async Task<bool> IsEmailExists(string email)
        {
            var query = await _userRepository.GetAll();
            bool isEmailExists = query.Any(u => u.Email == email);
            return isEmailExists;
        }

        public async Task<User> Add(User user)
        {
            return await _userRepository.Add(user);
        }

        public async Task<User> Update(User user)
        {
            return await _userRepository.Update(user);
        }

        public async Task Delete(int id)
        {
            await _userRepository.Delete(id);
        }
    }
}
