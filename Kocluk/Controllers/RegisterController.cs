using Kocluk.Data;
using Kocluk.Dto;
using Kocluk.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;

namespace Kocluk.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ILogger<RegisterController> _logger;
        private readonly DatabaseContext _databaseContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RegisterController(ILogger<RegisterController> logger, DatabaseContext databaseContext, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _databaseContext = databaseContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Register(KullaniciRegisterDto dto)
        {
            if (ModelState.IsValid)
            {
                var epostaVarMi=await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Eposta.Trim() == dto.Eposta.Trim());
                if (epostaVarMi == null)
                {
                    if (dto.ProfilFotografDosyasi != null)
                    {
                        string folder = "images/";
                        string fileName = Guid.NewGuid().ToString() + "_" + dto.ProfilFotografDosyasi.FileName;
                        folder += fileName;
                        string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                        await dto.ProfilFotografDosyasi.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

                        Kullanici kullanici = new Kullanici() {
                            Adi = dto.Adi,
                            Soyadi = dto.Soyadi,
                            DogumTarihi = dto.DogumTarihi,
                            Cinsiyet=dto.Cinsiyet,
                            TelefonNumarasi=dto.TelefonNumarasi,
                            ProfilFototagrafUrl= fileName,
                            Eposta=dto.Eposta,
                            Sifre=dto.Sifre,
                            Deneyimleri="null",
                            UzmanlikAlanlari="null",
                            KullaniciRolu="kullanici",
                            Hedefleri=dto.Hedefleri,
                            SifreSifirlamaMetni=dto.SifreSifirlamaMetni
                        };

                        _databaseContext.Kullanicilar.Add(kullanici);
                        _databaseContext.SaveChanges();

                        TempData["KullaniciRegisterMessage"] = "Kayıt başarılı. Giriş yapın";

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
                            TempData["KullaniciRegisterMessage"] = null;
                            TempData["KullaniciRegisterMessage"] = "Kullanıcı eklendi. Bir antrenörle eşleştirilemedi.";
                            return RedirectToAction("Index", "Login");
                        }
                       

                    }
                }
                else
                {
                    TempData["KullaniciRegisterMessage"] = "Bu eposta sistemde kayıtlı";
                    return View("Index", dto);
                }
            }
            return View("Index", dto);
        }

    }
}
