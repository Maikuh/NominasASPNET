﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class NominaDbContext : DbContext
    {
        public NominaDbContext()
            : base("name=NominaDbContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Cargo> Cargo { get; set; }
        public virtual DbSet<Departamento> Departamento { get; set; }
        public virtual DbSet<Direccion> Direccion { get; set; }
        public virtual DbSet<Empleado> Empleado { get; set; }
        public virtual DbSet<Horario> Horario { get; set; }
        public virtual DbSet<Nomina> Nomina { get; set; }
        public virtual DbSet<Retencion> Retencion { get; set; }
    }
}
