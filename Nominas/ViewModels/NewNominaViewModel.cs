using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
=======
>>>>>>> Added Nomina create functionality,  modified Index view to implement changes, Edit functionality is WIP, Delete is untested.

namespace Nominas.ViewModels
{
    public class NewNominaViewModel
    {
<<<<<<< HEAD
        [Required]
        public string Codigo_Empleado { get; set; }

        [Required]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public string Sueldo { get; set; }

    }

    public class LoginDBContext : DbContext
    {
        public DbSet<Nomina> nomina { get; set; }
=======
        public Nomina Nomina { get; set; }
        public Retencion Afp { get; set; }
        public Retencion Sfs { get; set; }
        public Retencion Isr { get; set; }
        public Retencion SeguroMedico { get; set; }
>>>>>>> Added Nomina create functionality,  modified Index view to implement changes, Edit functionality is WIP, Delete is untested.
    }
}