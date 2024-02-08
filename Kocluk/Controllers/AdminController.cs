using Humanizer;
using Kocluk.Data;
using Kocluk.Dto;
using Kocluk.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Kocluk.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly DatabaseContext _databaseContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(ILogger<AdminController> logger, DatabaseContext databaseContext, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _databaseContext = databaseContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("Login")]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(AdminLoginRequestDto dto)
        {
          
                if (ModelState.IsValid)
                {
                    try
                    {
                        var admin = await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Eposta == dto.Eposta);

                        if (admin != null && admin.Sifre == dto.Sifre && admin.KullaniciRolu.Equals("admin"))
                        {
                        List<Claim> claims = new List<Claim>();

                        claims.Add(new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()));
                        claims.Add(new Claim(ClaimTypes.Name, admin.Adi ?? string.Empty));
                        claims.Add(new Claim("Name", admin.Adi));
                        claims.Add(new Claim("Rol", admin.KullaniciRolu));

                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        return RedirectToAction("Dashboard");
                        }
                        else
                        {
                            _logger.LogInformation("Admin giriş başarısız");
                            TempData["AdminLoginErrorMessage"] = "Giriş başarısız. Lütfen tekrar deneyin.";
                            return View("Index", dto);
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["AdminLoginErrorMessage"] = $"Sql Server Connection Error: {ex.Message}";
                        return View("Index", dto);
                    }
                }
                return View("Index", dto);
           
        }


        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("Dashboard");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }



        [HttpGet("Kullanicilar")]
        public IActionResult Kullanicilar()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<Kullanici> kullanicilar = _databaseContext.Kullanicilar
            .Where(k => k.KullaniciRolu == "kullanici")
            .ToList();
                return View("Kullanicilar", kullanicilar);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet("Antrenorler")]
        public IActionResult Antrenorler()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<Kullanici> antrenorler = _databaseContext.Kullanicilar
            .Where(k => k.KullaniciRolu == "antrenor")
            .ToList();
                return View("Antrenorler", antrenorler);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        

        [HttpGet("Account/Antrenor/Delete/{id}")]
        public IActionResult AntrenorDelete([FromRoute] int id)
        {
            try
            {
                var user = _databaseContext.Kullanicilar.Find(id);

                if (user == null)
                {
                    return NotFound();
                }

                _databaseContext.Kullanicilar.Remove(user);
                _databaseContext.SaveChanges();

                return RedirectToAction("Antrenorler");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("Account/Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var user = _databaseContext.Kullanicilar.Find(id);
                KullanicilarinAntrenorleri kullanicilarinAntrenorleri = await _databaseContext.KullanicilarinAntrenorleris.FirstOrDefaultAsync(x => x.KullaniciId==id);
      
                if (user == null)
                {
                    return NotFound();
                }
                if (kullanicilarinAntrenorleri != null)
                {
                    _databaseContext.KullanicilarinAntrenorleris.Remove(kullanicilarinAntrenorleri);
                    Kullanici antrenor=await _databaseContext.Kullanicilar.FirstOrDefaultAsync(x=>x.Id==kullanicilarinAntrenorleri.AntrenorId);
                    antrenor.DanisanSayisi = antrenor.DanisanSayisi - 1;
                    _databaseContext.Kullanicilar.Update(antrenor);
                }

                _databaseContext.Kullanicilar.Remove(user);
                _databaseContext.SaveChanges();

                return RedirectToAction("Kullanicilar");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpGet("Account/Antrenor/ChangeActive/{id}")]
        public IActionResult AntrenorChangeActive([FromRoute] int id)
        {
            try
            {
                var user = _databaseContext.Kullanicilar.Find(id);

                if (user == null)
                {
                    return NotFound();
                }

                if (user.isActive)
                {
                    user.isActive = false;
                }
                else
                {
                    user.isActive = true;
                }
                _databaseContext.Update(user);
                _databaseContext.SaveChanges();

                return RedirectToAction("Antrenorler");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("Account/ChangeActive/{id}")]
        public IActionResult ChangeActive([FromRoute] int id)
        {
            try
            {
                var user = _databaseContext.Kullanicilar.Find(id);

                if (user == null)
                {
                    return NotFound();
                }

                if (user.isActive)
                {
                    user.isActive = false;
                }
                else
                {
                    user.isActive = true;
                }
                _databaseContext.Update(user);
                _databaseContext.SaveChanges();

                return RedirectToAction("Kullanicilar");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpGet("Account/Add")]
        public IActionResult AddAccount()
        {
            return View("AddAccount");
        }

        [HttpGet("Account/Antrenor/Add")]
        public IActionResult AddAntrenorAccount()
        {
            return View("AddAntrenorAccount");
        }



        [HttpPost("AdminAddAntrenor")]
        public async Task<IActionResult> AdminAddAntrenor(AdminAddAntrenorDto adminAddAntrenorDto)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var epostaVarMi = await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Eposta.Trim() == adminAddAntrenorDto.Eposta.Trim());
                    if (epostaVarMi == null)
                    {
                        if (adminAddAntrenorDto.ProfilFotografDosyasi != null)
                        {
                            string folder = "images/";
                            string fileName = Guid.NewGuid().ToString() + "_" + adminAddAntrenorDto.ProfilFotografDosyasi.FileName;
                            folder += fileName;
                            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                            await adminAddAntrenorDto.ProfilFotografDosyasi.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

                            Kullanici antrenor = new Kullanici()
                            {
                                Adi = adminAddAntrenorDto.Adi,
                                Soyadi = adminAddAntrenorDto.Soyadi,
                                DogumTarihi = adminAddAntrenorDto.DogumTarihi,
                                Cinsiyet = adminAddAntrenorDto.Cinsiyet,
                                TelefonNumarasi = adminAddAntrenorDto.TelefonNumarasi,
                                ProfilFototagrafUrl = fileName,
                                Eposta = adminAddAntrenorDto.Eposta,
                                Sifre = adminAddAntrenorDto.Sifre,
                                KullaniciRolu = "antrenor",
                                UzmanlikAlanlari = adminAddAntrenorDto.UzmanlikAlanlari,
                                Deneyimleri = adminAddAntrenorDto.Deneyimleri,
                                Hedefleri="null",
                                SifreSifirlamaMetni=adminAddAntrenorDto.SifreSifirlamaMetni
                            };

                            _databaseContext.Kullanicilar.Add(antrenor);
                            _databaseContext.SaveChanges();
                            TempData["AdminAddUserSuccessMessage"] = "Antrenör Eklendi";
                            return RedirectToAction("AddAntrenorAccount");

                        }
                    }
                    else
                    {
                        TempData["AdminAddUserErrorMessage"] = "Bu eposta sistemde kayıtlı";
                        return View("AddAntrenorAccount", adminAddAntrenorDto);
                    }
                }
                catch (Exception ex)
                {
                    TempData["AdminAddUserErrorMessage"] = $"Sql Server Connection Error: {ex.Message}";
                    if (ex.InnerException != null)
                    {
                        _logger.LogInformation("Durum: " + ex.InnerException.Message);
                    }
                    return View("AddAntrenorAccount", adminAddAntrenorDto);
                }
            }
            TempData["AdminAddUserErrorMessage"] = "Valid değil";
            return View("AddAntrenorAccount", adminAddAntrenorDto);

        }






        [HttpPost("AdminAddUser")]
        public async Task<IActionResult> AdminAddUser(AdminAddKullaniciDto adminAddKullaniciDto)
        {
            _logger.LogInformation("geldi");
            if (ModelState.IsValid)
            {
                try
                {
                    var epostaVarMi = await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Eposta.Trim() == adminAddKullaniciDto.Eposta.Trim());
                    if (epostaVarMi == null)
                    {
                        if (adminAddKullaniciDto.ProfilFotografDosyasi != null)
                        {
                            string folder = "images/";
                            string fileName = Guid.NewGuid().ToString() + "_" + adminAddKullaniciDto.ProfilFotografDosyasi.FileName;
                            folder += fileName;
                            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                            await adminAddKullaniciDto.ProfilFotografDosyasi.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

                            Kullanici kullanici = new Kullanici()
                            {
                                Adi = adminAddKullaniciDto.Adi,
                                Soyadi = adminAddKullaniciDto.Soyadi,
                                DogumTarihi = adminAddKullaniciDto.DogumTarihi,
                                Cinsiyet = adminAddKullaniciDto.Cinsiyet,
                                TelefonNumarasi = adminAddKullaniciDto.TelefonNumarasi,
                                ProfilFototagrafUrl = fileName,
                                Eposta = adminAddKullaniciDto.Eposta,
                                Sifre = adminAddKullaniciDto.Sifre,
                                KullaniciRolu = "kullanici",
                                UzmanlikAlanlari="yok",
                                Deneyimleri="yok",
                                Hedefleri=adminAddKullaniciDto.Hedefleri,
                                SifreSifirlamaMetni = adminAddKullaniciDto.SifreSifirlamaMetni
                            };

                            _databaseContext.Kullanicilar.Add(kullanici);
                            _databaseContext.SaveChanges();
                            TempData["AdminAddUserSuccessMessage"] = "Kullanıcı Eklendi";


                            //antrenörler eşleştirelim
                            Kullanici antrenor = await _databaseContext.Kullanicilar.FirstOrDefaultAsync(x => x.DanisanSayisi < 5 && x.KullaniciRolu.Equals("antrenor") && x.UzmanlikAlanlari.Contains(kullanici.Hedefleri));
                            KullanicilarinAntrenorleri kullanicilarinAntrenorleri = new KullanicilarinAntrenorleri();
                            if (antrenor != null)
                            {
                                kullanicilarinAntrenorleri.KullaniciId = kullanici.Id;
                                kullanicilarinAntrenorleri.AntrenorId = antrenor.Id;
                                _databaseContext.KullanicilarinAntrenorleris.Add(kullanicilarinAntrenorleri);
                                _databaseContext.SaveChanges();

                                antrenor.DanisanSayisi = antrenor.DanisanSayisi + 1;
                                _databaseContext.Kullanicilar.Update(antrenor);
                                _databaseContext.SaveChanges();
                            }
                            else
                            {
                                TempData["AdminAddUserSuccessMessage"] = null;
                                TempData["AdminAddUserErrorMessage"] = "Kullanıcı eklendi. Bir antrenörle eşleştirilemedi.";
                                return View("AddAccount", adminAddKullaniciDto);
                            }

                            return RedirectToAction("AddAccount");

                        }
                    }
                    else
                    {
                        TempData["AdminAddUserErrorMessage"] = "Bu eposta sistemde kayıtlı";
                        return View("AddAccount", adminAddKullaniciDto);
                    }
                }
                catch (Exception ex)
                {
                    TempData["AdminAddUserErrorMessage"] = $"Sql Server Connection Error: {ex.Message}";
                    if (ex.InnerException != null)
                    {
                        _logger.LogInformation("İç Özel Durum: " + ex.InnerException.Message);
                    }
                    return View("AddAccount", adminAddKullaniciDto);
                }
            }
            TempData["AdminAddUserErrorMessage"] = "Valid değil";
            return View("AddAccount", adminAddKullaniciDto);

        }



    }
}
