using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Nominas;
using Nominas.ViewModels;

namespace Nominas.Controllers
{
    public class EmpleadosController : Controller
    {
        private NominaDbContext db = new NominaDbContext();

        // GET: Empleados
        public ActionResult Index()
        {
            var empleado = db.Empleado.Include(e => e.Cargo).Include(e => e.Direccion).Include(e => e.Horario);
            return View(empleado.ToList());
        }

        // GET: Empleados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = db.Empleado.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }

        // GET: Empleados/Create
        public ActionResult Create()
        {
            var viewModel = new NewEmpleadoViewModel();

            var horarios = db.Horario.AsEnumerable().Select(s => new
            {
                s.Codigo_Horario,
                Tiempo = $"{s.Hora_Inicio.ToShortTimeString()} ~ {s.Hora_Fin.ToShortTimeString()}"
            }).ToList();

            ViewBag.ID_Cargo = new SelectList(db.Cargo, "ID_Cargo", "Nombre");
            //ViewBag.ID_Direccion = new SelectList(db.Direccion, "ID_Direccion", "Provincia");
            ViewBag.Codigo_Horario = new SelectList(horarios, "Codigo_Horario", "Tiempo");
            return View(viewModel);
        }

        // POST: Empleados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Empleado empleado)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new NewEmpleadoViewModel();

                return View("Create", viewModel);
            }

            db.Empleado.Add(empleado);
            db.SaveChanges();
            return RedirectToAction("Index");

            //ViewBag.ID_Cargo = new SelectList(db.Cargo, "ID_Cargo", "Nombre", empleado.ID_Cargo);
            //ViewBag.ID_Direccion = new SelectList(db.Direccion, "ID_Direccion", "Provincia", empleado.ID_Direccion);
            //ViewBag.Codigo_Horario = new SelectList(db.Horario, "Codigo_Horario", "Codigo_Horario", empleado.Codigo_Horario);
            //return View(empleado);
        }

        // GET: Empleados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = db.Empleado.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_Cargo = new SelectList(db.Cargo, "ID_Cargo", "Nombre", empleado.ID_Cargo);
            ViewBag.ID_Direccion = new SelectList(db.Direccion, "ID_Direccion", "Provincia", empleado.ID_Direccion);
            ViewBag.Codigo_Horario = new SelectList(db.Horario, "Codigo_Horario", "Codigo_Horario", empleado.Codigo_Horario);
            return View(empleado);
        }

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Codigo_Empleado,Nombre,Apellido,Cedula,Fecha_Nacimiento,ID_Cargo,ID_Direccion,Codigo_Horario")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empleado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Cargo = new SelectList(db.Cargo, "ID_Cargo", "Nombre", empleado.ID_Cargo);
            ViewBag.ID_Direccion = new SelectList(db.Direccion, "ID_Direccion", "Provincia", empleado.ID_Direccion);
            ViewBag.Codigo_Horario = new SelectList(db.Horario, "Codigo_Horario", "Codigo_Horario", empleado.Codigo_Horario);
            return View(empleado);
        }

        // GET: Empleados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = db.Empleado.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Empleado empleado = db.Empleado.Find(id);
            db.Empleado.Remove(empleado);
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
