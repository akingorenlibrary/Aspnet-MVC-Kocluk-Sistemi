﻿using System.ComponentModel.DataAnnotations;

namespace Kocluk.Dto
{
    public class UserPasswordUpdateDto
    {
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz")]
        [MaxLength(100, ErrorMessage = "Şifre alanı maksimum 100 karakter olmalı")]
        [MinLength(10, ErrorMessage = "Şifre minimum 10 karakter olmalı")]
        public string Sifre { get; set; }
    }
}
