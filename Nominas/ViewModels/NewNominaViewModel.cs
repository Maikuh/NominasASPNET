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
        public Nomina Nomina { get; set; }
        public Retencion Afp { get; set; }
        public Retencion Sfs { get; set; }
        public Retencion Isr { get; set; }
        public Retencion SeguroMedico { get; set; }
    }
}