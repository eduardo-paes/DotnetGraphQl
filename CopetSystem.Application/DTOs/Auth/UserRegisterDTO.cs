﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CopetSystem.Application.DTOs.Auth
{
	public class UserRegisterDTO
    {
		[Required]
        public string? Name { get; set; }
		[Required]
        public string? Role { get; set; }
		[Required]
        public string? Email { get; set; }
		[Required]
        public string? CPF { get; set; }
		[Required]
        public string? Password { get; set; }
    }
}
