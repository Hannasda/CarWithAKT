﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarWithAKT.Models
{
    public class Auth
    {
        public class LoginAuth
        {
            public string Phone { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public class RegAuth
        {
            public string Phone { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Паролі не відповідають")]
            public string ConfirmPassword { get; set; }

            [Required, RegularExpression("([A-Z]+)|([a-z]+)", ErrorMessage ="Використовуються недозволені символи")]
            public string Name { get; set; }
            public string Surname { get; set; }
            [Required]
            [DataType(DataType.EmailAddress, ErrorMessage ="Не коректна адреса електронної пошти")]
            public string Email { get; set; }
        }
    }
}