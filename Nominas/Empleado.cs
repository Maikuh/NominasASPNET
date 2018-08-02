//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Nominas
{
    using System;
    using System.Collections.Generic;
    
    public partial class Empleado
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Empleado()
        {
            this.Nomina = new HashSet<Nomina>();
        }
    
        public int Codigo_Empleado { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public System.DateTime Fecha_Nacimiento { get; set; }
        public int ID_Cargo { get; set; }
        public int ID_Direccion { get; set; }
        public int Codigo_Horario { get; set; }
    
        public virtual Cargo Cargo { get; set; }
        public virtual Direccion Direccion { get; set; }
        public virtual Horario Horario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Nomina> Nomina { get; set; }
    }
}
