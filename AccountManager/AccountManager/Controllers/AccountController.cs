using AccountManager.Models;
using Auth.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace AccountManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly LoginsDbContext _context;

        public AccountController(LoginsDbContext context)
        {
            _context = context;
        }

        public IActionResult Account() //Try to get to account, otherwise sign in page
        {
            if (User.Identity!.IsAuthenticated)
            {
                var currentLogin = _context.Logins.SingleOrDefault(login => login.Login == User.Identity.Name); //try to get entity with current login
                if (currentLogin != null)
                {
                    AccountModel model = new AccountModel();
                    model.Name = currentLogin.Name;
                    model.Email = currentLogin.Email;
                    model.Surname = currentLogin.Surname;
                    model.Phone = currentLogin.Phone;
                    return View("Account", model);
                }
            }
            return View("SignIn");
        }

        [HttpPost]
        public IActionResult Register(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                if (NoDuplicates(model)) //check if no duplicate of unique keys
                {
                    AddAccount(model);
                    return Account();
                }
            }
            return View("Registration");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                if (CredentialsFine(model))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, model.Login!) //put login in cookies(claims)
                    };
                    ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType); //make claims identity
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id)); //saving cookies and claims and sign in
                    return RedirectToAction("Account");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Account");
        }

        public IActionResult ChangeAccountInformation(AccountModel model)
        {
            if (ModelState.IsValid)
            {
                var accountInfo = _context.Logins.SingleOrDefault(login => login.Login == User.Identity!.Name); //find entity with needed login
                if (accountInfo == null)
                {
                    return Account();
                }
                var phone = _context.Logins.SingleOrDefault(login => login.Phone == model.Phone); //может быть самим собой
                if (phone != null && phone != accountInfo) //if there is another same phone in any other entity
                {
                    ModelState.AddModelError("Phone", "This phone number already exist");
                    return Account();
                }
                var email = _context.Logins.SingleOrDefault(login => login.Email == model.Email);
                if (email != null && email != accountInfo) //if there is another same email in any other entity
                {
                    ModelState.AddModelError("Email", "This email already exist");
                    return Account();
                }
                accountInfo.Surname = model.Surname;
                accountInfo.Email = model.Email;
                accountInfo.Phone = model.Phone;
                accountInfo.Name = model.Name;
                _context.SaveChanges();
            }
            return Account();
        }

        private bool CredentialsFine(SignInModel model)
        {
            var login = _context.Logins.SingleOrDefault(login => login.Login == model.Login); //try to find account with this login
            if (login != null)
            {
                if (login.Password == HashPassword(model.Password!))
                {
                    return true;
                }
            }
            ModelState.AddModelError("Login", "Wrong Login or Password");
            ModelState.AddModelError("Password", "Wrong Login or Password");

            return false;
        }

        public IActionResult Registration()
        { 
            return View("Registration"); 
        }

        private bool NoDuplicates(RegistrationModel model) //checks all unique keys
        {
            var login = _context.Logins.FirstOrDefault(login => login.Login == model.Login);
            if (login != null)
            {
                ModelState.AddModelError("Login", "Login already exists");
                return false;
            }
            var phone = _context.Logins.FirstOrDefault(login => login.Phone == model.Phone);
            if (phone != null)
            {

                ModelState.AddModelError("Phone", "Phone already exists");
                return false;
            }
            var email = _context.Logins.FirstOrDefault(login => login.Email == model.Email);
            if (phone != null)
            {

                ModelState.AddModelError("Email", "Email already exists");
                return false;
            }
            return true;
        }

        private void AddAccount(RegistrationModel model)
        {
            string hashedPassword = HashPassword(model.Password!);
            AccountInfoModel infoModel = new AccountInfoModel(model.Login, hashedPassword, model.Name, model.Surname, model.Phone, model.Email);
            _context.Logins.Add(infoModel);
            _context.SaveChanges();
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hash;
            }
        }
    }
}
