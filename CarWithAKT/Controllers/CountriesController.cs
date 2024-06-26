using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarWithAKT.Models;
using System.IO;

namespace CarWithAKT.Controllers
{
    public class CountriesController : Controller
    {
        private CarWithAKTContext db = new CarWithAKTContext();

        public ActionResult AdminIndex()
        {
            return View(db.Country.ToList());
        }

        // GET: Firms
        public ActionResult Index()
        {
            return View(db.Country.ToList());
        }

        // GET: Firms/Create
        public ActionResult Create()
        {
            var dir = Server.MapPath("~");
            var filenames = Directory.GetFiles($"{dir}Img\\Flags");
            int j = 0;
            foreach(var item in filenames)
            { 
                for (int i = item.Length - 1; i != 0; i--)
                {
                    if (item[i] == '\\')
                    {
                        filenames[j] = item.Substring(i+1, item.Length-i-5);
                        break;
                    } 
                }
                j++;
            }

            var flnm = new SelectList(filenames,"Shortname");

            ViewBag.Shortname = flnm;
            return View();
        }

        // POST: Firms/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Shortname")] Country country)
        {

        if (ModelState.IsValid)
            {
                if(db.Country.Any(c => c.Name.ToLower() == country.Name.ToLower()))
                {
                    return RedirectToAction("AdminIndex");
                }
                db.Country.Add(country);
                db.SaveChanges();
                return RedirectToAction("AdminIndex");
            }

            return View(country);
        }

        // GET: Firms/Edit/5

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country cntr = db.Country.Find(id);
            if (cntr == null)
            {
                return HttpNotFound();
            }
            var dir = Server.MapPath("~");
            var filenames = Directory.GetFiles($"{dir}Img\\Flags");
            int j = 0;
            foreach (var item in filenames)
            {
                for (int i = item.Length - 1; i != 0; i--)
                {
                    if (item[i] == '\\')
                    {
                        filenames[j] = item.Substring(i + 1, item.Length - i - 5);
                        break;
                    }
                }
                j++;
            }

            var flnm = new SelectList(filenames, "Shortname");

            ViewBag.Shortname = flnm;
            return View(cntr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Shortname")] Country cntr)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cntr).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cntr);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country country = db.Country.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            db.Country.Remove(country);
            db.SaveChanges();
            return RedirectToAction("AdminIndex");
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
