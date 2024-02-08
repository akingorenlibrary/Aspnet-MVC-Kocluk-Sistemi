﻿using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Linq;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kocluk.Models
{
    public class Kullanici
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Ad alanı boş bırakılamaz")]
        [MaxLength(100,ErrorMessage ="Ad alanı maksimum 100 karakter olmalı")]
        public string Adi { get; set; }

        [Required(ErrorMessage = "Soyad alanı boş bırakılamaz")]
        [MaxLength(100, ErrorMessage = "Soyadı alanı maksimum 100 karakter olmalı")]
        public string Soyadi { get; set; }

        [Required(ErrorMessage = "Doğum Tarihi alanı boş bırakılamaz")]
        [DataType(DataType.Date)]
        public DateTime DogumTarihi { get; set; }

        [Required(ErrorMessage = "Cinsiyet alanı boş bırakılamaz")]
        [MaxLength(1, ErrorMessage = "Cinsiyet alanı maksimum 1 karakter olmalı")]
        public string Cinsiyet { get; set; }

        [Required(ErrorMessage = "Eposta alanı boş bırakılamaz")]
        [EmailAddress(ErrorMessage ="Email formatı hatalı")]
        [MaxLength(100, ErrorMessage = "Eposta alanı maksimum 100 karakter olmalı")]
        public string Eposta { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz")]
        [MaxLength(100, ErrorMessage = "Şifre alanı maksimum 100 karakter olmalı")]
        [MinLength(10,ErrorMessage ="Şifre minimum 10 karakter olmalı")]
        public string Sifre { get; set; }

        [Required(ErrorMessage = "Telefon numarası alanı boş bırakılamaz")]
        [MaxLength(100, ErrorMessage = "Telefon numarası alanı maksimum 100 karakter olmalı")]
        public string TelefonNumarasi { get; set; }

        [Required(ErrorMessage = "Profil fotoğrafı alanı boş bırakılamaz")]
        public string ProfilFototagrafUrl { get; set; }

        [Required(ErrorMessage = "KullaniciRolu alanı boş bırakılamaz")]
        public string KullaniciRolu { get; set; }

        public string UzmanlikAlanlari { get; set; }
        public string Hedefleri { get; set; }

        public string Deneyimleri { get; set; }
        public string SifreSifirlamaMetni { get; set; }
        public int DanisanSayisi { get; set; } = 0;

        public bool isActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<KullanicilarinAntrenorleri> KullanicilarinAntrenorleris { get; set; }
        public ICollection<DanisanEgzersizProgrami> DanisanEgzersizProgramlari { get; set; }
        public ICollection<DanisanBeslenmeProgrami> DanisanBeslenmeProgramlari { get; set; }
        public ICollection<Mesajlar> Mesajlar { get; set; }
        public ICollection<IlerlemeDurumu> IlerlemeDurumlari { get; set; }
    }

}
