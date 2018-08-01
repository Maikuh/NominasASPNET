using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nominas.ViewModels
{
    public class NewEmpleadoViewModel
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required]
        public string Cedula { get; set; }

        [Required]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Fecha_Nacimiento { get; set; }

        [Required, Display(Name = "Cargo")]
        public int ID_Cargo { get; set; }

        [Required, Display(Name = "Horario")]
        public int Codigo_Horario { get; set; }
        public Direccion Direccion { get; set; }

    }
}