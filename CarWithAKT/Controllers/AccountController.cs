using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CarWithAKT.Models;
using static CarWithAKT.Models.Auth;

namespace CarWithAKT.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginAuth model)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                Client client = null;
                using (CarWithAKTContext db = new CarWithAKTContext())
                {
                    client = db.Client.FirstOrDefault(u => u.Phone == model.Phone && u.Password == model.Password);

                }
                if (client != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Phone, true);
                    return RedirectToAction("Index", "Cars");

                }
                else
                {
                    ModelState.AddModelError("", "Неверный логин или пароль");
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegAuth model)
        {
            if (ModelState.IsValid)
            {
                Client client = null;
                using (CarWithAKTContext db = new CarWithAKTContext())
                {
                    client = db.Client.FirstOrDefault(u => u.Phone == model.Phone);
                }
                if (client == null)
                {
                    // создаем нового пользователя
                    using (CarWithAKTContext db = new CarWithAKTContext())
                    {
                        db.Client.Add(new Client { Phone = model.Phone, Password = model.Password, Email = model.Email, RoleId = 1, Name = model.Name, Surname = model.Surname });
                        db.SaveChanges();

                        client = db.Client.Where(u => u.Phone == model.Phone && u.Password == model.Password).FirstOrDefault();
                    }
                    // если пользователь удачно добавлен в бд
                    if (client != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Phone, true);
                        return RedirectToAction("Index", "Cars");
                        
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }

            return View(model);
        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Cars");
        }

    }
}