using System;
using System.Collections.Generic;

namespace PracticeFacturacion.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            FacturacionHeaders = new HashSet<FacturacionHeader>();
        }

        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Documento { get; set; } = null!;
        public string Direccion { get; set; } = null!;

        public virtual ICollection<FacturacionHeader> FacturacionHeaders { get; set; }
    }
}
