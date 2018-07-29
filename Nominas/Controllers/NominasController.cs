using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Nominas;

namespace Nominas.Controllers
{
    public class NominasController : Controller
    {
        private NominaDbContext db = new NominaDbContext();

        // GET: Nominas
        public ActionResult Index()
        {
            var nomina = db.Nomina.Include(n => n.Empleado);
            return View(nomina.ToList());
        }

        // GET: Nominas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nomina nomina = db.Nomina.Find(id);
            if (nomina == null)
            {
                return HttpNotFound();
            }
            return View(nomina);
        }

        // GET: Nominas/Create
        public ActionResult Create()
        {
            ViewBag.Codigo_Empleado = new SelectList(db.Empleado, "Codigo_Empleado", "Nombre");
            return View();
        }

        // POST: Nominas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Codigo_Nomina,Codigo_Suplemento,Codigo_Empleado,Sueldo")] Nomina nomina)
        {
            try
            { 
            if (ModelState.IsValid)
            {
                db.Nomina.Add(nomina);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            ViewBag.Codigo_Empleado = new SelectList(db.Empleado, "Codigo_Empleado", "Nombre", nomina.Codigo_Empleado);
            return View(nomina);
        }

        // GET: Nominas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nomina nomina = db.Nomina.Find(id);
            if (nomina == null)
            {
                return HttpNotFound();
            }
            ViewBag.Codigo_Empleado = new SelectList(db.Empleado, "Codigo_Empleado", "Nombre", nomina.Codigo_Empleado);
            return View(nomina);
        }

        // POST: Nominas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var nominaToUpdate = db.Nomina.Find(id);
            if (TryUpdateModel(nominaToUpdate, "",
               new string[] { "Codigo_Empleado", "Nombre", "Sueldo" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(nominaToUpdate);
        }

        //public ActionResult Edit([Bind(Include = "Codigo_Nomina,Codigo_Suplemento,Codigo_Empleado,Sueldo")] Nomina nomina)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(nomina).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.Codigo_Empleado = new SelectList(db.Empleado, "Codigo_Empleado", "Nombre", nomina.Codigo_Empleado);
        //    return View(nomina);
        //}

        // GET: Nominas/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Nomina nomina = db.Nomina.Find(id);
            if (nomina == null)
            {
                return HttpNotFound();
            }
            return View(nomina);
        }

        // POST: Nominas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Nomina nomina = db.Nomina.Find(id);
                db.Nomina.Remove(nomina);
                db.SaveChanges();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
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
