using System;
using System.Collections;
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
    [Authorize]
    public class NominasController : Controller
    {
        private NominaDbContext db = new NominaDbContext();

        // GET: Nominas
        public ViewResult Index(DateTime? fecha, string sortOrder, string searchString)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.ApellidoSortParm = sortOrder == "Lastname" ? "lastname_desc" : "Lastname";

            var nomina = db.Nomina.Include(n => n.Empleado).Include(r => r.Retencion);
            nomina = from e in db.Nomina
                          select e;
            if (!String.IsNullOrEmpty(searchString))
            {
                nomina = nomina.Where(e => e.Empleado.Nombre.Contains(searchString)
                                       || e.Empleado.Apellido.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    nomina = nomina.OrderByDescending(e => e.Empleado.Nombre);
                    break;
                case "Lastname":
                    nomina = nomina.OrderBy(e => e.Empleado.Apellido);
                    break;
                case "lastname_desc":
                    nomina = nomina.OrderByDescending(e => e.Empleado.Apellido);
                    break;
                default:
                    nomina = nomina.OrderBy(e => e.Empleado.Nombre);
                    break;
            }

            // Make a list of fechas to populate a Dropdown List in the View
            var fechas = db.Nomina.AsEnumerable().Select(n => new { fecha = n.Fecha.ToShortDateString() }).Distinct().ToList();

            if (fecha.HasValue)
            {
                nomina = nomina.Where(n => n.Fecha == fecha);

                // Set the list of fechas in a ViewBag (subject to change to a model prop)
                ViewBag.Fechas = new SelectList(fechas, "Fecha", "Fecha", fecha);
            }
            else
            {
                var defaultFecha = Convert.ToDateTime(fechas.Last().fecha);
                nomina = nomina.Where(n => n.Fecha == defaultFecha);

                // Set the list of fechas in a ViewBag (subject to change to a model prop)
                ViewBag.Fechas = new SelectList(fechas, "Fecha", "Fecha", fechas.Last().fecha);
            }

            if (User.IsInRole("Contable"))
                return View(nomina.ToList());

            return View("IndexDefault", nomina.ToList());
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
        [Authorize(Roles = "Contable")]
        public ActionResult Create()
        {
            // Initialize a ViewModel
            var viewModel = new NewNominaViewModel
            {
                Nomina = new Nomina(),
                Afp = new Retencion(),
                Sfs = new Retencion(),
                Isr = new Retencion(),
                SeguroMedico = new Retencion()
            };

            // Make a list of empleados to populate a Dropdown List in the View
            var empleados = db.Empleado.AsEnumerable().Select(e => new
            {
                e.Codigo_Empleado,
                Empleado = $"{e.Codigo_Empleado}: {e.Nombre} {e.Apellido}"
            }).ToList();

            // Set the list of empleados in a ViewBag (subject to change to a model prop)
            ViewBag.Codigo_Empleado = new SelectList(empleados, "Codigo_Empleado", "Empleado");

            return View(viewModel);
        }

        // POST: Nominas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, Authorize(Roles = "Contable"), ValidateAntiForgeryToken]
        public ActionResult Create(NewNominaViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            try
            {
                // Initialize Nomina with posted values
                // Set each Retencion's Nombre accordingly
                var nomina = new Nomina(viewModel.Nomina.Sueldo, viewModel.Nomina.Bonificaciones, viewModel.Nomina.Horas_Extras, viewModel.Nomina.Codigo_Empleado)
                {
                    Retencion = new List<Retencion>
                    {
                        new Retencion
                        {
                            Cantidad = viewModel.Afp.Cantidad,
                            Nombre = "AFP"
                        },
                        new Retencion
                        {
                            Cantidad = viewModel.Sfs.Cantidad,
                            Nombre = "SFS"
                        },
                        new Retencion
                        {
                            Cantidad = viewModel.Isr.Cantidad,
                            Nombre = "ISR"
                        },
                        new Retencion
                        {
                            Cantidad = viewModel.SeguroMedico.Cantidad,
                            Nombre = "Seguro Medico"
                        },
                    }
                };

                db.Nomina.Add(nomina);
                db.SaveChanges();
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return RedirectToAction("Index");

            //ViewBag.Codigo_Empleado = new SelectList(db.Empleado, "Codigo_Empleado", "Nombre", nomina.Codigo_Empleado);
            //return View(viewModel);
        }

        // GET: Nominas/Edit/5
        [Authorize(Roles = "Contable")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Nomina nomina = db.Nomina.Find(id);

            if (nomina == null)
                return HttpNotFound();

            // Initialize a ViewModel with the Nomina from DB
            var viewModel = new NewNominaViewModel
            {
                Nomina = nomina
            };

            // Set ViewModels Retenciones based on the values from DB (nombre)
            foreach (var retencion in nomina.Retencion)
            {
                if (retencion.Nombre == "AFP")
                    viewModel.Afp = retencion;
                else if (retencion.Nombre == "SFS")
                    viewModel.Sfs = retencion;
                else if (retencion.Nombre == "ISR")
                    viewModel.Isr = retencion;
                else if (retencion.Nombre == "Seguro Medico")
                    viewModel.SeguroMedico = retencion;
            }

            return View(viewModel);
        }

        // POST: Nominas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, Authorize(Roles = "Contable"), ValidateAntiForgeryToken]
        public ActionResult Edit(NewNominaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var nomina = viewModel.Nomina;

                // Get each record in the DB
                var nominaInDb = db.Nomina.Single(n => n.Codigo_Nomina == nomina.Codigo_Nomina);
                var afpInDb = db.Retencion.Single(r => r.Codigo_Retencion == viewModel.Afp.Codigo_Retencion);
                var sfsInDb = db.Retencion.Single(r => r.Codigo_Retencion == viewModel.Sfs.Codigo_Retencion);
                var isrInDb = db.Retencion.Single(r => r.Codigo_Retencion == viewModel.Isr.Codigo_Retencion);
                var seguroMedInDb = db.Retencion.Single(r => r.Codigo_Retencion == viewModel.SeguroMedico.Codigo_Retencion);

                // Modify records in DB accordingly
                nominaInDb.Sueldo = nomina.Sueldo;
                afpInDb.Cantidad = viewModel.Afp.Cantidad;
                sfsInDb.Cantidad = viewModel.Sfs.Cantidad;
                isrInDb.Cantidad = viewModel.Isr.Cantidad;
                seguroMedInDb.Cantidad = viewModel.SeguroMedico.Cantidad;

                // Save changes
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        // GET: Nominas/Delete/5
        [Authorize(Roles = "Contable")]
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (saveChangesError.GetValueOrDefault())
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";

            Nomina nomina = db.Nomina.Find(id);

            if (nomina == null)
                return HttpNotFound();

            return View(nomina);
        }

        // POST: Nominas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken, Authorize(Roles = "Contable")]
        public ActionResult Delete(int id)
        {
            try
            {
                // Get Nomina in the DB
                var nominaInDb = db.Nomina.Single(n => n.Codigo_Nomina == id);

                // Delete each Retencion associated with the Nomina first
                foreach (var ret in nominaInDb.Retencion.ToList())
                {
                    db.Retencion.Remove(ret);
                }

                // Then delete the Nomina
                db.Nomina.Remove(nominaInDb);

                db.SaveChanges();
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
