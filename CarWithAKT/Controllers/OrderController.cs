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
using CarWithAKT.Providers;

namespace CarWithAKT.Controllers
{
    public class OrderController : Controller
    {
        private CarWithAKTContext db = new CarWithAKTContext();

        // GET: TestDrive
        [Authorize(Roles = "admin,worker")]
        public ActionResult Index()
        {
            var testDrive = db.Order.Include(t => t.Client).Include(t => t.Location).Include(t => t.Status).Include(t => t.Worker);
            return View(testDrive.ToList());
        }
        [Authorize]
        public ActionResult MyIndex()
        {
            var id = db.Client.Where(y=>y.Phone == User.Identity.Name).First().Id;
            var testDrive = db.Order.Include(t => t.Client).Include(t => t.Location).Include(t => t.Status).Include(t => t.Worker).Where(y=>y.ClientId == id);
            return View(testDrive.ToList());
        }

        // GET: TestDrive/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order testDrive = db.Order.Find(id);

            var idc = db.Client.Where(y => y.Phone == User.Identity.Name).First().Id;
            if (testDrive == null)
            {
                return HttpNotFound();
            }
            if(idc != testDrive.ClientId && !(User.IsInRole("admin") || User.IsInRole("worker"))) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
             
            List<ViewModelForBasket> results = new List<ViewModelForBasket>();
            var b = from tdcar in db.CarsOnOrder
                    join car in db.Car on tdcar.CarId equals car.Id
                    join firm in db.Firm on car.FirmId equals firm.Id
                    where tdcar.OrderId == testDrive.Id
                    select new
                    {
                        car.Img,
                        tdcar.Id,
                        firm.Name,
                        car.Price,
                        car.Modelname,
                        tdcar.Count
                    };
            foreach (var item in b)
            {
                var ig = item.Img.Substring(2, item.Img.Length - 2);
                results.Add(new ViewModelForBasket {Img = ig, Id = item.Id, Name = item.Name, Price = item.Price, Modelname = item.Modelname, Count = item.Count });
            }

            ViewBag.tdcars = results;

            var testDrives = db.Order.Where(y => y.Id == id).Include(y => y.Location).Include(y => y.Client).Include(y => y.Status).Include(y=>y.Worker).First();
            return View(testDrives);
        }

        // GET: TestDrive/Create
        [Authorize]
        public ActionResult Create()
        {
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
            int s = 0;
            foreach (var item in b)
            {
                results.Add(new ViewModelForBasket {Img = item.Img, Id = item.Id, Name = item.Name, Price = item.Price, Modelname = item.Modelname, Count = item.Count });
                s += (item.Price * item.Count);
            }
            ViewBag.flprs = s;
            ViewBag.tdcars = results;
            SelectList loc = new SelectList(db.Locations, "Id", "Name");
            ViewBag.loc = loc;
            //ViewBag.dt = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.AddDays(1).Day;
            ViewBag.dt = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            return View();
        }

        // POST: TestDrive/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ClientId,LocationId,Date")] Order testDrive)//modelstate не валідний
        {
            DateTime now = DateTime.Now;
            //if (testDrive.Date < now.AddDays(1))
            if(testDrive.Date.Date < now.Date.AddDays(1))
            {
                ModelState.AddModelError("", "Забрать авто можно только с завтрашнего дня");
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
                            car.Modelname
                        };
                foreach (var item in b)
                {
                    results.Add(new ViewModelForBasket {Img = item.Img, Id = item.Id, Name = item.Name, Price = item.Price, Modelname = item.Modelname });
                }
                ViewBag.tdcars = results;
                SelectList loc = new SelectList(db.Locations, "Id", "Name");
                ViewBag.loc = loc;
                return View(testDrive);
            }
            //if (ModelState.IsValid)
            {
                testDrive.ClientId = db.Client.Where(y => y.Phone == User.Identity.Name).First().Id;
                testDrive.StatusId = 1;
                testDrive.CreateDate = DateTime.Now;
                testDrive.WorkerId = null;
                testDrive.FullPrice = 0;
                List<CarsOnOrder> tdcars = new List<CarsOnOrder>();
                var a = db.Client.Where(y => y.Phone == User.Identity.Name).First().Id;
                var b = db.Basket.Where(y => y.ClientId == a).Include(cr => cr.Car).ToList();
                var maxx = db.CarsOnOrder.ToList();
                int max=0;
                if (maxx.Count > 0)
                {
                    max = maxx.Max(y => y.Id);
                }
                int i = 1;

                foreach (var item in b)
                {
                    tdcars.Add(new CarsOnOrder { CarId = item.CarId, OrderId = testDrive.Id , Id=max+i, Count = item.Count});
                    testDrive.FullPrice = item.Count * item.Car.Price + testDrive.FullPrice;
                    db.Basket.Remove(item);
                    i++;
                }
                foreach(var item in tdcars)
                {
                    db.CarsOnOrder.Add(item);
                }
                db.Order.Add(testDrive);
                db.SaveChanges();
                return RedirectToAction("MyIndex","Order");
            }
            //return View(testDrive);
        }

        // GET: TestDrive/Edit/5


        // GET: TestDrive/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order testDrive = db.Order.Find(id);
            if (testDrive == null)
            {
                return HttpNotFound();
            }
            var tdcars = db.CarsOnOrder.Where(y => y.OrderId == testDrive.Id).ToList();
            foreach(var item in tdcars)
            {
                db.CarsOnOrder.Remove(item);
            }
            db.Order.Remove(testDrive);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "worker")]
        public ActionResult Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order b = db.Order.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            else
            {
                var mec = db.Client.Where(y => y.Phone == User.Identity.Name).First().Id;
                var me = db.Worker.Where(y => y.ClientId == mec).First().Id;
                b.StatusId = 2;
                b.WorkerId = me;
                db.Entry(b).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        [Authorize(Roles = "worker,client")]
        public ActionResult Canceled(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order b = db.Order.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            else
            {
                var mec = db.Client.Where(y => y.Phone == User.Identity.Name).First().Id;
                var me = db.Worker.Where(y => y.ClientId == mec).First().Id;
                b.StatusId = 3;
                b.WorkerId = me;
                db.Entry(b).State = EntityState.Modified;
                db.SaveChanges();
                if (User.IsInRole("client"))
                {
                    return RedirectToAction("MyIndex");
                }
                else return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
