using System;
using System.Collections.Generic;

namespace PracticeFacturacion.Models
{
    public partial class FacturacionDetail
    {
        public int Id { get; set; }
        public int IdHeader { get; set; }
        public int Cantidad { get; set; }
        public string DescripcionProducto { get; set; } = null!;
        public double Itbis { get; set; }
        public double CostoUnitario { get; set; }
        public double Total { get; set; }

        public virtual FacturacionHeader IdHeaderNavigation { get; set; } = null!;
    }
}
