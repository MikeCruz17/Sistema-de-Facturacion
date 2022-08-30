using Microsoft.AspNetCore.Mvc.Rendering;

namespace PracticeFacturacion.Models
{
    public class SelectListClientes : FacturacionHeader
    {
        public IEnumerable<SelectListItem> Clientes { get; set; }
    }
}
