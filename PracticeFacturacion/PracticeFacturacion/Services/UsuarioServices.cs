using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PracticeFacturacion.Models;

namespace PracticeFacturacion.Services
{
    public class UsuarioServices
    {
        // CAMPO QUE POSEE EL MODELO DE LA BD.
        public readonly FACTURACIONContext _Context;

        // CONSTRUCTOR QUE POSEE EL MODELO DE LA BD.
        public UsuarioServices(FACTURACIONContext context)
		{
			_Context = context;
		}

		// ENUMERABLE TIPO SELECT-LIST-ITEM QUE EXTRAE EL NOMBRE Y EL ID DEL USUARIO
		// DE LA TABLA CLIENTES.
		public async Task<IEnumerable<SelectListItem>> ObtenerTodo()
        {
			try
			{
				var Lista = await _Context.Clientes.ToListAsync();
				var List = Lista.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
				return List;

			}
			catch 
			{

				return Enumerable.Empty<SelectListItem>();
			}
        }
    }
}
