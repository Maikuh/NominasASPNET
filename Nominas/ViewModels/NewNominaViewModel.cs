using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Nominas.ViewModels
{
    public class NewNominaViewModel
    {
        [Required]
        public string Codigo_Empleado { get; set; }

        [Required]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public string Sueldo { get; set; }

    }

    public class LoginDBContext : DbContext
    {
        public DbSet<Nomina> nomina { get; set; }
    }
}