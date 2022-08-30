using System;
using System.Collections.Generic;

namespace PracticeFacturacion.Models
{
    public partial class FacturacionHeader
    {
        public FacturacionHeader()
        {
            FacturacionDetails = new HashSet<FacturacionDetail>();
        }

        public int Id { get; set; }
        public DateTime FechaFactura { get; set; }
        public int ClienteId { get; set; }
        public double TotalFacturado { get; set; }
        public double Itbis { get; set; }

        public virtual Cliente Cliente { get; set; } = null!;
        public virtual ICollection<FacturacionDetail> FacturacionDetails { get; set; }
    }
}
