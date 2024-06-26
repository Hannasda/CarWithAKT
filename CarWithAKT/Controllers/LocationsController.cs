using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarWithAKT.Models;

namespace CarWithAKT.Controllers
{
    public class LocationsController : Controller
    {
        private CarWithAKTContext db = new CarWithAKTContext();

        // GET: Locations
        public ActionResult Index()
        {
            return View(db.Locations.ToList());
        }

        [Authorize(Roles = "admin")]
        public ActionResult AdminIndex()
        {
            return View(db.Locations.ToList());
        }

        // GET: Locations/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Locations/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Adress")] Location location)
        {
            if (ModelState.IsValid)
            {
                if(db.Locations.Any(x=>x.Name.ToLower() == location.Name.ToLower()))
                {
                    return RedirectToAction("Index");
                }
                db.Locations.Add(location);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(location);
        }
        // GET: Firms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location loc = db.Locations.Find(id);
            if (loc == null)
            {
                return HttpNotFound();
            }
            return View(loc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Adress")] Location loc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(loc);
        }


        // GET: Locations/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            db.Locations.Remove(location);
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
    }
}
