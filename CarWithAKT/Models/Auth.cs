using System;
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
            [Compare("Password", ErrorMessage = "Пароли не совпадают")]
            public string ConfirmPassword { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            [Required]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
        }
    }
}