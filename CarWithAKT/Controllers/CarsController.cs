using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarWithAKT.Models;
using CarWithAKT.ViewModels;

namespace CarWithAKT.Controllers
{
    public class CarsController : Controller
    {
        private CarWithAKTContext db = new CarWithAKTContext();
        private static string a = null;

        // GET: Cars
        public ActionResult Index(string Bas)
        {
            ViewBag.MinPrice = db.Car.Min(x => x.Price);
            ViewBag.MaxPrice = db.Car.Max(x => x.Price);
            ViewBag.Firms = db.Firm.ToList(); // Добавлено

            var Bass = db.Basket.Include(x => x.Client).Where(x => x.Client.Phone == User.Identity.Name).Include(x => x.Car).ToList();
            if (Bas != null && Bas != "")
            {
                Basket FT = null;
                foreach (var F in Bass)
                {
                    if (F.Car.Id == Convert.ToInt32(Bas))
                    {
                        FT = F;
                        break;
                    }
                }
                if (User.Identity.IsAuthenticated)
                {
                    if (FT == null)
                    {
                        var a = db.Client.Where(y => y.Phone == User.Identity.Name).First().Id;
                        db.Basket.Add(new Basket { ClientId = a, CarId = Convert.ToInt32(Bas) });
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Basket.Remove(FT);
                        db.SaveChanges();
                    }
                }
                else { return RedirectToAction("Login", "Account"); }
            }
            var BassEdited = db.Basket.Include(x => x.Client).Where(x => x.Client.Phone == User.Identity.Name).Include(x => x.Car).ToList();
            ViewBag.Bas = BassEdited;
            var car = db.Car.Include(c => c.Country).Include(c => c.Firm).ToList();
            return View(car.ToList());
        }

        [HttpPost]
        public ActionResult FilterCars(int? MinPrice, int? MaxPrice, List<int> SelectedFirms) // Добавлено
        {
            var cars = db.Car.Include(c => c.Firm).AsQueryable();

            if (MinPrice.HasValue && MaxPrice.HasValue)
            {
                cars = cars.Where(x => x.Price >= MinPrice && x.Price <= MaxPrice);
            }

            if (SelectedFirms != null && SelectedFirms.Any())
            {
                cars = cars.Where(x => SelectedFirms.Contains(x.FirmId));
            }

            ViewBag.MinPrice = db.Car.Min(x => x.Price);
            ViewBag.MaxPrice = db.Car.Max(x => x.Price);
            ViewBag.Firms = db.Firm.ToList();

            return View("Index", cars.ToList());
        }
        // GET: Cars/Details/5
        public ActionResult Details(int? id, string Bas)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Car.Include(y => y.Firm).Include(y => y.Country).Where(model => model.Id == id).First();
            if (car == null)
            {
                return HttpNotFound();
            }
            a = car.Img;
            a = a.Remove(0, 2);
            ViewBag.imag = a;
            var Bass = db.Basket.Include(x => x.Client).Where(x => x.Client.Phone == User.Identity.Name).Include(x => x.Car).ToList();
            if (Bas != null && Bas != "")
            {
                Basket FT = null;
                foreach (var F in Bass)
                {
                    if (F.Car.Id == Convert.ToInt32(Bas))
                    {
                        FT = F;
                        break;
                    }
                }
                if (User.Identity.IsAuthenticated)
                {
                    if (FT == null)
                    {
                        var a = db.Client.Where(y => y.Phone == User.Identity.Name).First().Id;
                        db.Basket.Add(new Basket { ClientId = a, CarId = Convert.ToInt32(Bas) });
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Basket.Remove(FT);
                        db.SaveChanges();
                    }
                }
                else { return RedirectToAction("Login", "Account"); }
            }
            var BassEdited = db.Basket.Include(x => x.Client).Where(x => x.Client.Phone == User.Identity.Name).Include(x => x.Car).ToList();
            ViewBag.Bas = BassEdited;

            return View(car);
        }

        // GET: Cars/Create
        public ActionResult Create()
        {
            SelectList firm = new SelectList(db.Firm, "Id", "Name");
            ViewBag.firms = firm;
            SelectList countries = new SelectList(db.Country, "Id", "Name");
            ViewBag.countries = countries;
            return View(new Car());
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Color,Body_type,Fuel_type,Fuel_use,Engine_cap,Mass,Engine_type,Seats_num,Door_num,Power,Driver_type,Transmission_type,Price,Modelname,FirmId,CountryId,Img")] Car car)
        {
            car.Img = "../Img/" + car.Img;
            if (ModelState.IsValid)
            {
                db.Car.Add(car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            SelectList firm = new SelectList(db.Firm, "Id", "Name");
            ViewBag.firms = firm;
            SelectList countries = new SelectList(db.Country, "Id", "Name");
            ViewBag.countries = countries;
            return View(car);
        }

        // GET: Cars/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Car.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            SelectList firm = new SelectList(db.Firm, "Id", "Name");
            ViewBag.firms = firm;
            SelectList countries = new SelectList(db.Country, "Id", "Name");
            ViewBag.countries = countries;
            a = car.Img;
            return View(car);
        }
        // POST: Cars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Color,Body_type,Fuel_type,Fuel_use,Engine_cap,Mass,Engine_type,Seats_num,Door_num,Power,Driver_type,Transmission_type,Price,Modelname,FirmId,CountryId,Img")] Car car)
        {
            if (car.Img == null) car.Img = a;
            else car.Img = "../Img/" + car.Img;
            if (ModelState.IsValid)
            {
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(db.Country, "Id", "Name", car.CountryId);
            ViewBag.FirmId = new SelectList(db.Firm, "Id", "Name", car.FirmId);
            return View(car);
        }

        // GET: Cars/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Car.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            db.Car.Remove(car);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Basket(string Bas, string CNTM, string CNTP)
        {
            if (Bas != null && Bas != "")
            {
                var del = db.Basket.Find(Convert.ToInt32(Bas));
                db.Basket.Remove(del);
                db.SaveChanges();
            }
            if (CNTM != null && CNTM != "")
            {
                var mns = db.Basket.Find(Convert.ToInt32(CNTM));
                if (mns.Count > 1)
                {
                    mns.Count = mns.Count - 1;
                    db.Entry(mns).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            if (CNTP != null && CNTP != "")
            {
                var mns = db.Basket.Find(Convert.ToInt32(CNTP));
                if (mns.Count < 3)
                {
                    mns.Count = mns.Count + 1;
                    db.Entry(mns).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            var a = db.Client.Where(y => y.Phone == User.Identity.Name).First().Id;
            List<ViewModelForBasket> results = new List<ViewModelForBasket>();
            var b = from basket in db.Basket
                    join car in db.Car on basket.CarId equals car.Id
                    join firm in db.Firm on car.FirmId equals firm.Id
                    where basket.ClientId == a
                    select new
                    {
                        car.Img,
                        basket.Id,
                        firm.Name,
                        car.Price,
                        car.Modelname,
                        basket.Count
                    };
            foreach (var item in b)
            {
                results.Add(new ViewModelForBasket { Img = item.Img, Id = item.Id, Name = item.Name, Price = item.Price, Modelname = item.Modelname, Count = item.Count });
            }
            return View(results);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Basket(List<ViewModelForBasket> model)
        {
            List<Basket> results = new List<Basket>();
            var a = db.Client.Where(y => y.Phone == User.Identity.Name).First().Id;

            return RedirectToAction("Create", "Order");
        }
    }
}