using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarWithAKT.Models
{
    public class Client
    {
        public int Id { set; get; }
        [Required(ErrorMessage = "Введіть пароль")]
        [DataType(DataType.Password)]
        public string Password { set; get; }
        [Required(ErrorMessage = "Введіть ім`я")]
        public string Name { set; get; }
        [Required(ErrorMessage = "Введіть прізвище")]
        public string Surname { set; get; }
        [Required(ErrorMessage = "Введіть номер телефону")]
        public string Phone { set; get; }
        [Required(ErrorMessage = "Введіть адресу електронної пошти")]
        public string Email { set; get; }
        public int RoleId { set; get; }
        public Role Role { set; get; }
    }

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}