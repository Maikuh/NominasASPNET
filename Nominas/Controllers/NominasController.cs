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
    public class NominasController : Controller
    {
        private NominaDbContext db = new NominaDbContext();

        // GET: Nominas
        public ActionResult Index()
        {
            var nomina = db.Nomina.Include(n => n.Empleado).Include(r => r.Retencion);
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
            var viewModel = new NewNominaViewModel
            {
                Nomina = new Nomina(),
                Afp = new Retencion(),
                Sfs = new Retencion(),
                Isr = new Retencion(),
                SeguroMedico = new Retencion()
            };

            var empleados = db.Empleado.AsEnumerable().Select(e => new
            {
                e.Codigo_Empleado,
                Empleado = $"{e.Codigo_Empleado}: {e.Nombre} {e.Apellido}"
            }).ToList();

            ViewBag.Codigo_Empleado = new SelectList(empleados, "Codigo_Empleado", "Empleado");

            return View(viewModel);
        }

        // POST: Nominas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
<<<<<<< HEAD
        //public ActionResult Create(NewNominaViewModel viewModel)
        //{
        //       var nomina = new Nomina()
        //       {
        //              Codigo_Empleado = Int32.Parse(viewModel.Codigo_Empleado),
        //              Sueldo = Convert.ToDecimal(viewModel.Sueldo)

        //       };
        //    db.Nomina.Add(nomina);
        //    db.SaveChanges();
        //    return View();
        //}


        public ActionResult Create([Bind(Include = "Codigo_Nomina,Codigo_Suplemento,Codigo_Empleado,Sueldo")] Nomina nomina)
=======
        public ActionResult Create(NewNominaViewModel viewModel)
>>>>>>> Added Nomina create functionality,  modified Index view to implement changes, Edit functionality is WIP, Delete is untested.
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
<<<<<<< HEAD
            {
                if (ModelState.IsValid)
                {
                    db.Nomina.Add(nomina);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
=======
            { 
                var nomina = new Nomina(viewModel.Nomina.Sueldo, viewModel.Nomina.Codigo_Empleado)
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
>>>>>>> Added Nomina create functionality,  modified Index view to implement changes, Edit functionality is WIP, Delete is untested.
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
        public ActionResult Edit(Nomina nomina)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nomina).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Codigo_Empleado = new SelectList(db.Empleado, "Codigo_Empleado", "Nombre", nomina.Codigo_Empleado);
            return View(nomina);
        }

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
