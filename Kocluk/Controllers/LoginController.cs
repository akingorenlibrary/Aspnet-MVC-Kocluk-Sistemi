using Humanizer;
using Kocluk.Data;
using Kocluk.Dto;
using Kocluk.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Kocluk.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly DatabaseContext _databaseContext;
        public LoginController(ILogger<LoginController> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public IActionResult Index()
        {  
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Login(KullaniciLoginRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var kullanici = await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Eposta == dto.Eposta);

                    if (kullanici != null && kullanici.Sifre == dto.Sifre && !kullanici.KullaniciRolu.Equals("admin"))
                    {
                        _logger.LogInformation("kullanıcı giriş başarılı");
                        
                        List<Claim> claims=new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, kullanici.Id.ToString()));
                        claims.Add(new Claim(ClaimTypes.Name, kullanici.Adi ?? string.Empty));
                        claims.Add(new Claim("Name",kullanici.Adi));
                        claims.Add(new Claim("Rol", kullanici.KullaniciRolu));
                        claims.Add(new Claim("Id", kullanici.Id.ToString()));

                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme); 
                        ClaimsPrincipal principal= new ClaimsPrincipal(claimsIdentity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        _logger.LogInformation("kullanıcı giriş başarısız");
                        TempData["KullaniciLoginMessage"] = "Giriş başarısız. Lütfen tekrar deneyin.";
                        return View("Index", dto);
                    }
                }
                catch (Exception ex)
                {
                    TempData["KullaniciLoginMessage"] = $"Sql Server Connection Error: {ex.Message}";
                    return View("Index");
                }
            }
            return View("Index", dto);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Login");
        }

        public IActionResult ForgetPassword()
        {
            return View("ForgetPassword");
        }


        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto dto)
        {
            try
            {
                var kullanici = await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Eposta == dto.Eposta);
                if (kullanici != null && dto.SifreSifirlamaMetni.Equals(kullanici.SifreSifirlamaMetni))
                {
                    ForgetPasswordSetPasswordDto forgetPasswordSetPasswordDto = new ForgetPasswordSetPasswordDto()
                    {
                        id = kullanici.Id
                    };
                    return View("ForgetPasswordSetPassword", forgetPasswordSetPasswordDto);
                }
                else
                {
                    TempData["ForgetPasswordErrorMessage"] = "Böyle bir eposta yok";
                    return View("ForgetPassword");
                }
            }
            catch (SqlException ex)
            {
                TempData["ForgetPasswordErrorMessage"] = $"Sql Server Connection Error: {ex.Message}";
                return View("ForgetPassword");
            }
        }



        [HttpPost]
        public async Task<IActionResult> PasswordUpdate(ForgetPasswordSetPasswordDto dto)
        {

            try
            {
                var user = await _databaseContext.Kullanicilar.FindAsync(dto.id);

                if (user != null)
                {
                    user.Sifre = dto.Sifre;
                    _databaseContext.Update(user);
                    await _databaseContext.SaveChangesAsync();

                    TempData["ForgetSetPasswordSuccessMessage"] = "İşlem başarılı";
                    return View("ForgetPasswordSetPassword");
                }
                else
                {
                    TempData["ForgetSetPasswordErrorMessage"] = "Hata oluştu";
                    return View("ForgetPasswordSetPassword");
                }
            }
            catch (SqlException ex)
            {
                TempData["ForgetSetPasswordErrorMessage"] = $"Sql Server Connection Error: {ex.Message}";
                return View("ForgetPasswordSetPassword");
            }

        }
        
    }
}
