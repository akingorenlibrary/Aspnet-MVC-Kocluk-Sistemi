using Humanizer;
using Kocluk.Data;
using Kocluk.Dto;
using Kocluk.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kocluk.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {

        private readonly ILogger<LoginController> _logger;
        private readonly DatabaseContext _databaseContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserController(ILogger<LoginController> logger, DatabaseContext databaseContext, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _databaseContext = databaseContext;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("ProfileImageUpdate")]
        public async Task<IActionResult> ProfileImageUpdate(UserProfileImageUpdate userProfileImageUpdate)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int id = int.Parse(User.FindFirst("Id").Value);
                    Kullanici user = await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Id == id);
                    string folder = "images/";
                    string fileName = Guid.NewGuid().ToString() + "_" + userProfileImageUpdate.ProfilFotografDosyasi.FileName;
                    folder += fileName;
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                    using (var stream = new FileStream(serverFolder, FileMode.Create))
                    {
                        await userProfileImageUpdate.ProfilFotografDosyasi.CopyToAsync(stream);
                    }

                    user.ProfilFototagrafUrl = fileName;

                    _databaseContext.Kullanicilar.Update(user);
                    await _databaseContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    TempData["UpdateFormErrorMessage"] = $"{ex.Message}";
                    if (ex.InnerException != null)
                    {
                        _logger.LogInformation("Durum: " + ex.InnerException.Message);
                    }
                    return View("InfoChangeImageForm");
                }
            }
            else
            {
                TempData["UpdateFormErrorMessage"] = "Boş bırakmayın";
                return View("InfoChangeImageForm");
            }

            TempData["UpdateFormSuccessMessage"] = "Güncelleme başarılı";
            return View("InfoChangeImageForm");
        }


        [HttpPost("ProfileNameUpdate")]
        public async Task<IActionResult> ProfileNameUpdate(UserNameUpdateDto userNameUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst("Id").Value;
                Kullanici user = await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Id.ToString() == userId);
                user.Adi = userNameUpdateDto.Name;
                _databaseContext.Kullanicilar.Update(user);
                await _databaseContext.SaveChangesAsync();

                TempData["UpdateFormSuccessMessage"] = "Başarılı";
            }
            else
            {
                TempData["UpdateFormErrorMessage"] = "Boş bırakmayın";
            }
            return View("InfoChangeFormName");
        }

        [HttpPost("ProfilePasswordUpdate")]
        public async Task<IActionResult> ProfilePasswordUpdate(UserPasswordUpdateDto userPasswordUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst("Id").Value;
                Kullanici user = await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Id.ToString() == userId);
                user.Sifre = userPasswordUpdateDto.Sifre;
                _databaseContext.Kullanicilar.Update(user);
                await _databaseContext.SaveChangesAsync();

                TempData["UpdateFormSuccessMessage"] = "Başarılı";
            }
            else
            {
                TempData["UpdateFormErrorMessage"] = "Boş bırakmayın";
            }
            return View("InfoChangeFormPassword");
        }


        [HttpPost("ProfileProfessionUpdate")]
        public async Task<IActionResult> ProfileProfessionUpdate(UserProfessionUpdateDto userProfessionUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst("Id").Value;
                Kullanici user = await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Id.ToString() == userId);
                user.UzmanlikAlanlari = userProfessionUpdateDto.UzmanlikAlanlari;
                _databaseContext.Kullanicilar.Update(user);
                await _databaseContext.SaveChangesAsync();

                TempData["UpdateFormSuccessMessage"] = "Başarılı";
            }
            else
            {
                TempData["UpdateFormErrorMessage"] = "Boş bırakmayın";
            }
            return View("InfoChangeFormProfession");
        }



        [HttpPost("ProfileExperienceUpdate")]
        public async Task<IActionResult> ProfileExperienceUpdate(UserExperienceUpdateDto userExperienceUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst("Id").Value;
                Kullanici user = await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Id.ToString() == userId);
                user.Deneyimleri = userExperienceUpdateDto.Deneyimleri;
                _databaseContext.Kullanicilar.Update(user);
                await _databaseContext.SaveChangesAsync();

                TempData["UpdateFormSuccessMessage"] = "Başarılı";
            }
            else
            {
                TempData["UpdateFormErrorMessage"] = "Boş bırakmayın";
            }
            return View("InfoChangeFormExperience");
        }



        [HttpPost("ProfileSurnameUpdate")]
        public async Task<IActionResult> ProfileSurnameUpdate(UserSurnameUpdateDto userSurnameUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst("Id").Value;
                Kullanici user = await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Id.ToString() == userId);
                user.Soyadi = userSurnameUpdateDto.Surname;
                _databaseContext.Kullanicilar.Update(user);
                await _databaseContext.SaveChangesAsync();

                TempData["UpdateFormSuccessMessage"] = "Başarılı";
            }
            else
            {
                TempData["UpdateFormErrorMessage"] = "Boş bırakmayın";
            }
            return View("InfoChangeFormSurname");
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirst("Id").Value;
            Kullanici user = await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Id.ToString() == userId);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return View("Profile", user);
            }
        }

        [HttpGet("Account/InfoChange")]
        public async Task<IActionResult> InfoChange()
        {

            return View("InfoChange");

        }

        [HttpGet("Account/InfoChange/{Field}")]
        public async Task<IActionResult> InfoChangeForm([FromRoute] String field)
        {

            if (field.Equals("ProfileImage"))
            {
                return View("InfoChangeImageForm");
            }
            else if (field.Equals("Name"))
            {
                return View("InfoChangeFormName");
            }
            else if (field.Equals("Surname"))
            {
                return View("InfoChangeFormSurname");
            }
            else if (field.Equals("Password"))
            {
                return View("InfoChangeFormPassword");
            }
            else if (field.Equals("Experience"))
            {
                return View("InfoChangeFormExperience");
            }
            else if (field.Equals("Profession"))
            {
                return View("InfoChangeFormProfession");
            }
            return View("InfoChangeFormName");

        }


        [HttpGet("Antrenor")]
        public async Task<IActionResult> AntrenorAndProgram()
        {
            var userId = User.FindFirst("Id").Value;

            KullanicilarinAntrenorleri kullanicilarinAntrenorleri = await _databaseContext.KullanicilarinAntrenorleris
                .SingleOrDefaultAsync(x => x.KullaniciId == int.Parse(userId));

            if (kullanicilarinAntrenorleri == null)
            {
                return NotFound();
            }

            if (kullanicilarinAntrenorleri.AntrenorId != null)
            {
                Kullanici antrenor = await _databaseContext.Kullanicilar
                    .SingleOrDefaultAsync(x => x.Id == kullanicilarinAntrenorleri.AntrenorId);

                if (antrenor != null)
                {
                    return View("AntrenorAndProgram", antrenor);
                }
            }
            return NotFound();
        }



        [HttpGet("Danisanlar")]
        public async Task<IActionResult> Danisanlar()
        {
            var userId = User.FindFirst("Id").Value;

            List<KullanicilarinAntrenorleri> danisanlar = await _databaseContext.KullanicilarinAntrenorleris
                .Where(x => x.AntrenorId == int.Parse(userId))
                .ToListAsync();

            await _databaseContext.Kullanicilar
                .Where(k => danisanlar.Select(d => d.KullaniciId).Contains(k.Id))
                .LoadAsync();

            List<Kullanici> danisanListesi = danisanlar
                .Select(danisan => danisan.Kullanici)
                .ToList();

            return View("Danisanlar", danisanListesi);
        }


        [HttpGet("Antrenor/DanisanEgzersizProgrami/{id}")]
        public async Task<IActionResult> DanisanEgzersizProgrami([FromRoute] int id)
        {

            var userId = User.FindFirst("Id").Value;

            DanisanEgzersizProgrami danisanEgzersizProgrami = new DanisanEgzersizProgrami();
            danisanEgzersizProgrami = await _databaseContext.DanisanEgzersizProgramlari
            .Where(x => x.AntrenorId == int.Parse(userId) && x.DanisanId == id)
            .FirstOrDefaultAsync();

            if (danisanEgzersizProgrami != null)
            {
                Kullanici danisan = await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Id == id);
                Kullanici antrenor = await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Id.ToString() == userId);
                danisanEgzersizProgrami.Kullanici = danisan;
                danisanEgzersizProgrami.Antrenor = antrenor;
            }

            var viewModel = new DanisanEgzersizProgramiViewModel2
            {
                DanisanEgzersizProgrami = danisanEgzersizProgrami,
                DanisanId = id
            };

            return View("DanisanEgzersizProgrami", viewModel);
        }


        [HttpGet("Antrenor/DanisanProgramOlustur/{id}")]
        public async Task<IActionResult> DanisanProgramOlustur([FromRoute] int id)
        {
            DanisanEgzersizProgramiDto danisanEgzersizProgramiDto = new DanisanEgzersizProgramiDto();
            var viewModel = new DanisanEgzersizProgramiViewModel
            {
                DanisanEgzersizProgramiDto = danisanEgzersizProgramiDto,
                DanisanId = id
            };
            return View("DanisanProgramOlustur", viewModel);
        }


        [HttpPost("DanisanEgzersizProgramiOLustur")]
        public async Task<IActionResult> DanisanEgzersizProgramiOLustur(DanisanEgzersizProgramiViewModel danisanEgzersizProgramiViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirst("Id").Value;
                    DanisanEgzersizProgrami danisanEgzersizProgrami = new DanisanEgzersizProgrami()
                    {
                        EgzersizAdi = danisanEgzersizProgramiViewModel.DanisanEgzersizProgramiDto.EgzersizAdi,
                        Hedefleri = danisanEgzersizProgramiViewModel.DanisanEgzersizProgramiDto.Hedefleri,
                        SetSayisi = danisanEgzersizProgramiViewModel.DanisanEgzersizProgramiDto.SetSayisi,
                        DanisanId = danisanEgzersizProgramiViewModel.DanisanId,
                        TekrarSayisi = danisanEgzersizProgramiViewModel.DanisanEgzersizProgramiDto.TekrarSayisi,
                        VideoRehberi = danisanEgzersizProgramiViewModel.DanisanEgzersizProgramiDto.VideoRehberi,
                        PrograminSuresi = danisanEgzersizProgramiViewModel.DanisanEgzersizProgramiDto.PrograminSuresi,
                        AntrenorId = int.Parse(userId)
                    };
                    _databaseContext.DanisanEgzersizProgramlari.Add(danisanEgzersizProgrami);
                    _databaseContext.SaveChanges();
                    TempData["DanisanProgramOlusturBasarili"] = "Eklendi";
                    return View("DanisanProgramOlustur");
                }
                catch (Exception ex)
                {
                    TempData["DanisanProgramOlusturHatali"] = $"{ex.Message}";
                    if (ex.InnerException != null)
                    {
                        _logger.LogInformation("Durum: " + ex.InnerException.Message);
                    }
                    return View("DanisanProgramOlustur");
                }
            }
            else
            {

                TempData["DanisanProgramOlusturHatali"] = "Boş bırakmayın";
                return View("DanisanProgramOlustur");
            }
        }


        [HttpGet("Antrenor/DanisanEgzersizProgramSil/{id}")]
        public async Task<IActionResult> DanisanEgzersizProgramSil([FromRoute] int id)
        {
            var antrenorId = User.FindFirst("Id").Value;

            var danisanEgzersizProgram = await _databaseContext.DanisanEgzersizProgramlari
                .FirstOrDefaultAsync(d => d.DanisanId == id && d.AntrenorId == int.Parse(antrenorId));

            if (danisanEgzersizProgram != null)
            {
                _databaseContext.DanisanEgzersizProgramlari.Remove(danisanEgzersizProgram);
                await _databaseContext.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction("Danisanlar", "User");

        }


        [HttpGet("Antrenor/DanisanBeslenmeProgrami/{id}")]
        public async Task<IActionResult> DanisanBeslenmeProgrami([FromRoute] int id)
        {

            var userId = User.FindFirst("Id").Value;

            DanisanBeslenmeProgrami danisanBeslenmeProgrami = new DanisanBeslenmeProgrami();
            danisanBeslenmeProgrami = await _databaseContext.DanisanBeslenmeProgramlari
            .Where(x => x.AntrenorId == int.Parse(userId) && x.DanisanId == id)
            .FirstOrDefaultAsync();

            if (danisanBeslenmeProgrami != null)
            {
                Kullanici danisan = await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Id == id);
                Kullanici antrenor = await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Id.ToString() == userId);
                danisanBeslenmeProgrami.Kullanici = danisan;
                danisanBeslenmeProgrami.Antrenor = antrenor;
            }

            var viewModel = new DanisanBeslenmeProgramiViewModel2
            {
                DanisanBeslenmeProgrami = danisanBeslenmeProgrami,
                DanisanId = id
            };

            return View("DanisanBeslenmeProgrami", viewModel);
        }

        [HttpGet("Antrenor/DanisanBeslenmeProgramOlustur/{id}")]
        public async Task<IActionResult> DanisanBeslenmeProgramOlustur([FromRoute] int id)
        {
            DanisanBeslenmeProgramiDto danisanBeslenmeProgramiDto = new DanisanBeslenmeProgramiDto();
            var viewModel = new DanisanBeslenmeProgramiViewModel
            {
                DanisanBeslenmeProgramiDto = danisanBeslenmeProgramiDto,
                DanisanId = id
            };
            return View("DanisanBeslenmeProgramOlustur", viewModel);
        }


        [HttpPost("DanisanBeslenmeProgramiOLustur")]
        public async Task<IActionResult> DanisanBeslenmeProgramiOLustur(DanisanBeslenmeProgramiViewModel danisanBeslenmeProgramiViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirst("Id").Value;
                    DanisanBeslenmeProgrami danisanBeslenmeProgrami = new DanisanBeslenmeProgrami()
                    {
                        Hedef = danisanBeslenmeProgramiViewModel.DanisanBeslenmeProgramiDto.Hedef,
                        KaloriHedefi = danisanBeslenmeProgramiViewModel.DanisanBeslenmeProgramiDto.KaloriHedefi,
                        GunlukOgunler = danisanBeslenmeProgramiViewModel.DanisanBeslenmeProgramiDto.GunlukOgunler,
                        DanisanId = danisanBeslenmeProgramiViewModel.DanisanId,
                        AntrenorId = int.Parse(userId)
                    };
                    _databaseContext.DanisanBeslenmeProgramlari.Add(danisanBeslenmeProgrami);
                    _databaseContext.SaveChanges();
                    TempData["DanisanProgramOlusturBasarili"] = "Eklendi";
                    return View("DanisanBeslenmeProgramOlustur");
                }
                catch (Exception ex)
                {
                    TempData["DanisanProgramOlusturHatali"] = $"{ex.Message}";
                    if (ex.InnerException != null)
                    {
                        _logger.LogInformation("Durum: " + ex.InnerException.Message);
                    }
                    return View("DanisanBeslenmeProgramOlustur");
                }
            }
            else
            {

                TempData["DanisanProgramOlusturHatali"] = "Boş bırakmayın";
                return View("DanisanBeslenmeProgramOlustur");
            }
        }



        [HttpGet("Antrenor/DanisanBeslenmeProgramSil/{id}")]
        public async Task<IActionResult> DanisanBeslenmeProgramSil([FromRoute] int id)
        {
            var antrenorId = User.FindFirst("Id").Value;

            var danisanBeslenmeProgrami = await _databaseContext.DanisanBeslenmeProgramlari
                .FirstOrDefaultAsync(d => d.DanisanId == id && d.AntrenorId == int.Parse(antrenorId));

            if (danisanBeslenmeProgrami != null)
            {
                _databaseContext.DanisanBeslenmeProgramlari.Remove(danisanBeslenmeProgrami);
                await _databaseContext.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction("Danisanlar", "User");

        }


        [HttpGet("AntrenorProgramBilgisi")]
        public async Task<IActionResult> AntrenorProgramBilgisi()
        {
            var danisanId = int.Parse(User.FindFirst("Id").Value);

            var danisanBeslenmeProgrami = await _databaseContext.DanisanBeslenmeProgramlari
                .FirstOrDefaultAsync(d => d.DanisanId == danisanId);

            var danisanEgzersizProgram = await _databaseContext.DanisanEgzersizProgramlari
                .FirstOrDefaultAsync(d => d.DanisanId == danisanId);

            KullanicilarinAntrenorleri kullanicilarinAntrenorleri = await _databaseContext.KullanicilarinAntrenorleris.SingleOrDefaultAsync(x => x.KullaniciId == danisanId);
            Kullanici antrenor = await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Id == kullanicilarinAntrenorleri.AntrenorId);

            AntrenorProgramBilgisiViewModel antrenorProgramBilgisiViewModel = new AntrenorProgramBilgisiViewModel()
            {
                danisanBeslenmeProgrami = danisanBeslenmeProgrami,
                danisanEgzersizProgrami = danisanEgzersizProgram,
                antrenor = antrenor
            };

            return View("AntrenorProgramBilgisi", antrenorProgramBilgisiViewModel);
        }

        [HttpGet("Mesajlar")]
        public IActionResult Mesajlar()
        {
            return View("Mesajlar");
        }


        [HttpGet("KullaniciAntrenoreMesajGonderiyor")]
        public IActionResult KullaniciAntrenoreMesajGonderiyor()
        {
            return View("KullaniciAntrenoreMesajGonderiyor");
        }

        [HttpPost]
        public async Task<IActionResult> KullaniciAntrenoreMesajGonderdi(MesajDto mesajDto)
        {
            try
            {
                var danisanId = int.Parse(User.FindFirst("Id").Value);
                KullanicilarinAntrenorleri kullanicilarinAntrenorleri = await _databaseContext.KullanicilarinAntrenorleris.SingleOrDefaultAsync(x => x.KullaniciId == danisanId);
                Kullanici antrenor = await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Id == kullanicilarinAntrenorleri.AntrenorId);
                Kullanici danisan = await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Id == danisanId);

                Mesajlar mesajlar = new Mesajlar()
                {
                    Alici = antrenor,
                    Gonderen = danisan,
                    Mesaj = mesajDto.Mesaj
                };
                _databaseContext.Mesajlar.Add(mesajlar);
                _databaseContext.SaveChanges();
                TempData["MesajGonderili"] = "Mesaj Gönderildi";
                return View("KullaniciAntrenoreMesajGonderiyor");
            }
            catch (Exception ex)
            {
                TempData["MesajGonderilemedi"] = $"Mesaj Gönderilemedi. Hata: {ex.Message}";
                return View("KullaniciAntrenoreMesajGonderiyor");
            }
        }

        [HttpGet("AntrenorMesajDanisanSec")]
        public async Task<IActionResult> AntrenorMesajDanisanSec()
        {
            var userId = User.FindFirst("Id").Value;

            List<KullanicilarinAntrenorleri> danisanlar = await _databaseContext.KullanicilarinAntrenorleris
                .Where(x => x.AntrenorId == int.Parse(userId))
                .ToListAsync();

            await _databaseContext.Kullanicilar
                .Where(k => danisanlar.Select(d => d.KullaniciId).Contains(k.Id))
                .LoadAsync();

            List<Kullanici> danisanListesi = danisanlar
                .Select(danisan => danisan.Kullanici)
                .ToList();
            return View("AntrenorMesajDanisanSec", danisanListesi);
        }


        [HttpGet("Antrenor/MesajGonder/{id}")]
        public IActionResult AntrenorMesajGonder([FromRoute] int id)
        {
            MesajDto mesajDto = new MesajDto()
            {
                Id = id
            };
            return View("AntrenorMesajGonder", mesajDto);
        }


        [HttpPost("AntrenorDanisanaMesajGonder")]
        public async Task<IActionResult> AntrenorDanisanaMesajGonder(MesajDto mesajDto)
        {
            try
            {
                var antrenorId = int.Parse(User.FindFirst("Id").Value);
                Kullanici antrenor = await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Id == antrenorId);
                Kullanici danisan = await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Id == mesajDto.Id);

                Mesajlar mesajlar = new Mesajlar()
                {
                    Alici = danisan,
                    Gonderen = antrenor,
                    Mesaj = mesajDto.Mesaj
                };
                _databaseContext.Mesajlar.Add(mesajlar);
                _databaseContext.SaveChanges();
                TempData["MesajGonderili"] = "Mesaj Gönderildi";
                return View("KullaniciAntrenoreMesajGonderiyor");
            }
            catch (Exception ex)
            {
                TempData["MesajGonderilemedi"] = $"Mesaj Gönderilemedi. Hata: {ex.Message}";
                return View("KullaniciAntrenoreMesajGonderiyor");
            }
        }

        [HttpGet("KullaniciAntrenordenGelenMesajlariOkuyor")]
        public async Task<IActionResult> KullaniciAntrenordenGelenMesajlariOkuyor()
        {
            var danisanId = int.Parse(User.FindFirst("Id").Value);

            List<Mesajlar> mesajlar = await _databaseContext.Mesajlar
                .Where(m => m.Gonderen.Id == danisanId || m.Alici.Id == danisanId)
                .Include(m => m.Gonderen)
                .ToListAsync();

            return View("KullaniciAntrenordenGelenMesajlariOkuyor", mesajlar);
        }


        [HttpGet("AntrenorDanisanSecMesajOku")]
        public async Task<IActionResult> AntrenorDanisanSecMesajOku()
        {
            var userId = User.FindFirst("Id").Value;

            List<KullanicilarinAntrenorleri> danisanlar = await _databaseContext.KullanicilarinAntrenorleris
                .Where(x => x.AntrenorId == int.Parse(userId))
                .ToListAsync();

            await _databaseContext.Kullanicilar
                .Where(k => danisanlar.Select(d => d.KullaniciId).Contains(k.Id))
                .LoadAsync();

            List<Kullanici> danisanListesi = danisanlar
                .Select(danisan => danisan.Kullanici)
                .ToList();
            return View("AntrenorDanisanSecMesajOku", danisanListesi);
        }

        [HttpGet("Antrenor/MesajOku/{id}")]
        public async Task<IActionResult> AntrenorMesajOku([FromRoute] int id)
        {
            var antrenorId = int.Parse(User.FindFirst("Id").Value);

            List<Mesajlar> mesajlar = await _databaseContext.Mesajlar
                .Where(m => (m.Gonderen.Id == antrenorId && m.Alici.Id == id) || (m.Gonderen.Id == id && m.Alici.Id == antrenorId))
                .Include(m => m.Gonderen)
                .Include(m=>m.Alici)
                .ToListAsync();

            return View("KullaniciAntrenordenGelenMesajlariOkuyor", mesajlar);
        }

        [HttpGet("DanisanIlerlemeDurumu")]
        public async Task<IActionResult> DanisanIlerlemeDurumu()
        {
            var userId = int.Parse(User.FindFirst("Id").Value);
            IlerlemeDurumu ilerlemeDurumu = await _databaseContext.IlerlemeDurumlari.SingleOrDefaultAsync(ilerleme => ilerleme.KullaniciId == userId);

            return View("DanisanIlerlemeDurumu", ilerlemeDurumu);
        }

        [HttpGet("Danisan/IlerlemeDurumuEkle")]
        public async Task<IActionResult> IlerlemeDurumuEkle()
        {
            return View("IlerlemeDurumuEkle");
        }


        [HttpPost("IlerlemeDurumuKaydet")]
        public async Task<IActionResult> IlerlemeDurumuKaydet(IlermeDurumuDto ilermeDurumuDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("Id").Value);
                Kullanici user = await _databaseContext.Kullanicilar.SingleOrDefaultAsync(x => x.Id == userId);
                IlerlemeDurumu ilerlemeDurumu = new IlerlemeDurumu()
                {
                    Kullanici = user,
                    Kilo = ilermeDurumuDto.Kilo,
                    Boy = ilermeDurumuDto.Boy,
                    VucutKitleIndeksi = ilermeDurumuDto.VucutKitleIndeksi,
                    VucutYagOrani = ilermeDurumuDto.VucutYagOrani
                };
                _databaseContext.IlerlemeDurumlari.Add(ilerlemeDurumu);
                _databaseContext.SaveChanges();

                TempData["IlerlemeDurumuBasarili"] = "Eklendi";
                return View("IlerlemeDurumuEkle");
            }
            catch (Exception ex)
            {
                TempData["IlerlemeDurumuHatali"] = "Eklenemedi";
                return View("IlerlemeDurumuEkle");
            }

        }


        [HttpGet("Update/Kilo")]
        public IActionResult UpdateIlerlemeDurumu()
        {
            return View("KiloGuncelle");

        }

        [HttpPost("KiloGuncelle")]
        public async Task<IActionResult> KiloGuncelle(KiloDto kiloDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("Id").Value);
                var ilerlemeDurumu = await _databaseContext.IlerlemeDurumlari.SingleOrDefaultAsync(x => x.KullaniciId == userId);
                ilerlemeDurumu.Kilo = kiloDto.Kilo;
                _databaseContext.IlerlemeDurumlari.Update(ilerlemeDurumu);
                _databaseContext.SaveChanges();
                TempData["IlerlemeDurumuBasarili"] = "Güncellendi";
                return RedirectToAction("DanisanIlerlemeDurumu");
            }
            catch(Exception ex)
            {
                TempData["IlerlemeDurumuHatali"] = "Güncellenemedi";
                return RedirectToAction("Update/Kilo");
            }
        }

        [HttpGet("Update/Boy")]
        public IActionResult UpdateIlerlemeDurumuBoy()
        {
            return View("BoyGuncelle");

        }

        [HttpPost("BoyGuncelle")]
        public async Task<IActionResult> BoyGuncelle(BoyDto boyDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("Id").Value);
                var ilerlemeDurumu = await _databaseContext.IlerlemeDurumlari.SingleOrDefaultAsync(x => x.KullaniciId == userId);
                ilerlemeDurumu.Boy = boyDto.Boy;
                _databaseContext.IlerlemeDurumlari.Update(ilerlemeDurumu);
                _databaseContext.SaveChanges();
                TempData["IlerlemeDurumuBasarili"] = "Güncellendi";
                return RedirectToAction("DanisanIlerlemeDurumu");
            }
            catch (Exception ex)
            {
                TempData["IlerlemeDurumuHatali"] = "Güncellenemedi";
                return RedirectToAction("Update/Boy");
            }
        }


        [HttpGet("Update/VucutYagOrani")]
        public IActionResult UpdateIlerlemeDurumuVucutYagOrani()
        {
            return View("VucutYagOraniGuncelle");

        }

        [HttpPost("VucutYagOraniGuncelle")]
        public async Task<IActionResult> VucutYagOraniGuncelle(VucutYagOraniDto vucutYagOraniDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("Id").Value);
                var ilerlemeDurumu = await _databaseContext.IlerlemeDurumlari.SingleOrDefaultAsync(x => x.KullaniciId == userId);
                ilerlemeDurumu.VucutYagOrani = vucutYagOraniDto.VucutYagOrani;
                _databaseContext.IlerlemeDurumlari.Update(ilerlemeDurumu);
                _databaseContext.SaveChanges();
                TempData["IlerlemeDurumuBasarili"] = "Güncellendi";
                return RedirectToAction("DanisanIlerlemeDurumu");
            }
            catch (Exception ex)
            {
                TempData["IlerlemeDurumuHatali"] = "Güncellenemedi";
                return RedirectToAction("Update/VucutYagOrani");
            }
        }


        [HttpGet("Update/KasKutlesi")]
        public IActionResult UpdateIlerlemeDurumuKasKutlesi()
        {
            return View("KasKutlesiGuncelle");

        }

        [HttpPost("KasKutlesiGuncelle")]
        public async Task<IActionResult> KasKutlesiGuncelle(KasKutlesiGuncelleDto kasKutlesiGuncelleDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("Id").Value);
                var ilerlemeDurumu = await _databaseContext.IlerlemeDurumlari.SingleOrDefaultAsync(x => x.KullaniciId == userId);
                ilerlemeDurumu.KasKutlesi = kasKutlesiGuncelleDto.KasKutlesi;
                _databaseContext.IlerlemeDurumlari.Update(ilerlemeDurumu);
                _databaseContext.SaveChanges();
                TempData["IlerlemeDurumuBasarili"] = "Güncellendi";
                return RedirectToAction("DanisanIlerlemeDurumu");
            }
            catch (Exception ex)
            {
                TempData["IlerlemeDurumuHatali"] = "Güncellenemedi";
                return RedirectToAction("Update/KasKutlesi");
            }
        }



        [HttpGet("Update/VucutKitleIndeksi")]
        public IActionResult UpdateIlerlemeDurumuVucutKitleIndeksi()
        {
            return View("VucutKitleIndeksiGuncelle");

        }

        [HttpPost("VucutKitleIndeksiGuncelle")]
        public async Task<IActionResult> VucutKitleIndeksiGuncelle(VucutKitleIndeksiGuncelleDto vucutKitleIndeksiGuncelleDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("Id").Value);
                var ilerlemeDurumu = await _databaseContext.IlerlemeDurumlari.SingleOrDefaultAsync(x => x.KullaniciId == userId);
                ilerlemeDurumu.VucutKitleIndeksi = vucutKitleIndeksiGuncelleDto.VucutKitleIndeksi;
                _databaseContext.IlerlemeDurumlari.Update(ilerlemeDurumu);
                _databaseContext.SaveChanges();
                TempData["IlerlemeDurumuBasarili"] = "Güncellendi";
                return RedirectToAction("DanisanIlerlemeDurumu");
            }
            catch (Exception ex)
            {
                TempData["IlerlemeDurumuHatali"] = "Güncellenemedi";
                return RedirectToAction("Update/VucutKitleIndeksi");
            }
        }

    }
}
