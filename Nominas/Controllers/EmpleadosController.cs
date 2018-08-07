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
using PagedList;

namespace Nominas.Controllers
{
    [Authorize]
    public class EmpleadosController : Controller
    {
        private NominaDbContext db = new NominaDbContext();


        // GET: Empleados
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.ApellidoSortParm = sortOrder == "Lastname" ? "lastname_desc" : "Lastname";
            ViewBag.CargoSortParm = sortOrder == "Cargo" ? "Cargo_desc" : "Cargo";
            ViewBag.DateSortParm = sortOrder == "BirthDate" ? "date_desc" : "BirthDate";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var empleados = from e in db.Empleado
                           select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                empleados = empleados.Where(e => e.Nombre.Contains(searchString)
                                       || e.Apellido.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    empleados = empleados.OrderByDescending(e => e.Nombre);
                    break;
                case "Lastname":
                    empleados = empleados.OrderBy(e => e.Apellido);
                    break;
                case "lastname_desc":
                    empleados = empleados.OrderByDescending(e => e.Apellido);
                    break;
                case "BirthDate":
                    empleados = empleados.OrderBy(e => e.Fecha_Nacimiento);
                    break;
                case "date_desc":
                    empleados = empleados.OrderByDescending(e => e.Fecha_Nacimiento);
                    break;
                default:
                    empleados = empleados.OrderBy(e => e.Nombre);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            if (User.IsInRole("Admin"))

                return View(empleados.ToPagedList(pageNumber, pageSize));
            return View("IndexDefault", empleados.ToPagedList(pageNumber, pageSize));
        }

        //public ActionResult Index()
        //{
        //    var empleado = db.Empleado.Include(e => e.Cargo).Include(e => e.Direccion).Include(e => e.Horario);
        //    return View(empleado.ToList());
        //}

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
        [Authorize(Roles = "Admin")]
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
        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
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
        [Authorize(Roles = "Admin")]
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

            var horarios = db.Horario.AsEnumerable().Select(s => new
            {
                s.Codigo_Horario,
                Tiempo = $"{s.Hora_Inicio:t} a {s.Hora_Fin:t}"
            }).ToList();

            ViewBag.ID_Cargo = new SelectList(db.Cargo, "ID_Cargo", "Nombre", empleado.ID_Cargo);
            ViewBag.Codigo_Horario = new SelectList(horarios, "Codigo_Horario", "Tiempo", empleado.Codigo_Horario);

            return View(empleado);
        }

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public ActionResult Edit(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empleado.Direccion).State = EntityState.Modified;

                db.Entry(empleado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var horarios = db.Horario.AsEnumerable().Select(s => new
            {
                s.Codigo_Horario,
                Tiempo = $"{s.Hora_Inicio:t} a {s.Hora_Fin:t}"
            }).ToList();

            ViewBag.ID_Cargo = new SelectList(db.Cargo, "ID_Cargo", "Nombre");
            ViewBag.Codigo_Horario = new SelectList(horarios, "Codigo_Horario", "Tiempo");

            return View(empleado);
        }

        // GET: Empleados/Delete/5
        [Authorize(Roles = "Admin")]
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
        [ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Empleado empleado = db.Empleado.Single(e => e.Codigo_Empleado == id);

            foreach (var nomina in empleado.Nomina.ToList())
            {
                foreach (var retencion in nomina.Retencion.ToList())
                {
                    db.Retencion.Remove(retencion);
                }

                db.Nomina.Remove(nomina);
            }

            db.Direccion.Remove(empleado.Direccion);
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
