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
    
    public partial class Retencion
    {
        public Retencion()
        {
            
        }
        public Retencion(int id, int nominaId, string nombre, decimal cantidad)
        {
            Codigo_Retencion = id;
            Codigo_Nomina = nominaId;
            Nombre = nombre;
            Cantidad = cantidad;
        }

        public int Codigo_Retencion { get; set; }
        public int Codigo_Nomina { get; set; }
        public string Nombre { get; set; }
        public decimal Cantidad { get; set; }
    
        public virtual Nomina Nomina { get; set; }
    }
}