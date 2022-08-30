using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticeFacturacion.Models;
using PracticeFacturacion.Services;

namespace PracticeFacturacion.Controllers
{
    public class FacturaController : Controller
    {
        // CREACION DE CAMPO TIPO DE LAS CLASES SERVICIOS.
        private readonly UsuarioServices _user;
        private readonly FacturacionServices _fact;

        // CONSTRUCTOR QUE POSEE LOS CAMPOS.
        public FacturaController(UsuarioServices user, FacturacionServices fact)
        {
            _user = user;
            _fact = fact;
        }

        // CREACION DE LISTA QUE ALMACENA LOS DATOS DE LA TABLA DINAMICA.
        public static List<F_Detail_2> Lista = new();

        // ACTION-RESULT QUE ME MUESTRA UN FORMULARIO Y UNA LA TABLA DINAMICA.
        [HttpGet]
        public async Task<ActionResult> Facturacion()
        {
            // SI HAY UN ERROR EN EL MODELO...
            if (!ModelState.IsValid)
            {
                // SI HUBO UN ERROR.
                TempData["Error"] = "Hubo un error en el modelo.";
                return RedirectToAction("Facturacion");
            }


            // CREACION DEL LIST-ITEM TIPO CLIENTE PARA EL DROP-DOWN LIST.
            SelectListClientes Modelo = new ();

            // ALMACENANDO LOS DATOS EXTRAIDO DE LA TABLA CLIENTES.
            Modelo.Clientes = await _user.ObtenerTodo();

            // TEMPDATE QUE ME ALMACENA LA LISTA Y, ES USADA COMO PARAMETRO EN 
            // EL PARTIAL-VIEW.
            TempData["Lista"] = Lista;

            return View(Modelo);
        }

        // INGRESO DE LOS DATOS EN LA LISTA QUE USA LA TABLA DINAMICA.
        [HttpPost]
        public ActionResult Ingresar(F_Detail_2 List)
        {
            // SI HAY UN ERROR EN EL MODELO...
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Facturacion");
            }

            // SI EL NUMERO DE ELEMENTOS ES IGUAL AL 0, INSERTARA LOS DATOS.
            if (Lista.Count() == 0)
            {
                // AÑADIENDO OBJETO A LA LISTA.
                Lista.Add(List);
                return RedirectToAction("Facturacion");
            }
            // SI EL CLIENTE-ID DE LA PRIMERA POSICION ES IGUAL AL OBJETO A INSERTAR
            // INSERTARA LOS DATOS.
            else if(Lista[0].ClienteID == List.ClienteID)
            {
                // AÑADIENDO OBJETO A LA LISTA.
                Lista.Add(List);
                return RedirectToAction("Facturacion");
            }

            // EL ID DEL USUARIO ES DIFERENTE AL ANTES INSERTADO...
            TempData["Error"] = "No puede insertar un cliente diferente en el header";

            return RedirectToAction("Facturacion");
        }

        // GET: FacturaController/Create
        public ActionResult TablaDinamica()
        {
            // SI HAY UN ERROR EN EL MODELO...
            if (!ModelState.IsValid)
            {
                return View(Lista);
            }

            // SI LA CANTIDAD POSICIONES DE LA LISTA ES IGUAL O MAYOR A UNO
            // RETORNARA LOS DATOS EN LA LISTA.
            if (Lista.Count >= 1)
            {
                return View(Lista);
            }

            // MENSAJE DE ERROR
            TempData["Error"] = "Hubo un error al insertar al consultar la lista.";
            return View(Lista);
        }

        
        // ACTION-RESULT QUE ALMACENA EL HEADER EN LA BD.
        public async Task<IActionResult> GuardarOperaciones()
        {
            // CREACION DE VARIABLES QUE ALMACENARAN EL TOTAL
            // DEL ITBI Y PRECIO.
            double TotalFacturado = 0;
            double TotalITBIS = 0;

            // CICLO QUE ALMACENARA EN DOS VARIABLES LOS TOTALES
            // CORRESPONDIENTES.
            foreach (var item in Lista)
            {
                TotalFacturado += item.Total;
                TotalITBIS += item.Itbis;
                
            }
            
            // CREACION TIPO HEADER QUE ALMACENARA LOS TOTALES Y EL ID.
            FacturacionHeader FacturacionHeader = new FacturacionHeader
            {
                ClienteId = Lista[0].ClienteID,
                TotalFacturado = TotalFacturado,
                Itbis = TotalITBIS // ITBIS DEBERIA TENER EL ITBIS-FACTURADO, FUE ERROR.
            };

            // EXTRACCION DEL ID HEADER AL MOMENTO DE INGRESAR EL OBJETO TIPO HEADER.
            var ID = await _fact.AgregarHeader(FacturacionHeader);

            // SI EL ID ES IGUAL A 0, OCURRIO UN ERROR.
            if(ID == 0)
            {
                // MENSAJE DE ERROR
                TempData["Error"] = "Hubo un error al insertar el header";

                return RedirectToAction("Facturacion");
            }

            // SI EL ID ES MAYOR A 1, RETORNARA A LA VISTA CON UN PARAMETRO.
            return RedirectToAction("GuardarFacturacionDetails", new {IdHeader = ID});

        }

        // ACTION-RESULT QUE RECIBE EL ID DEL HEADER POR PARAMETRO.
        public async Task<IActionResult> GuardarFacturacionDetails(int IdHeader)
        {
            // VALOR POR DEFECTO DEL RESULT.
            var Result = false;

            // CICLO QUE ME INSERTA CADA POSICION DE LA LISTA EN LA TABLA DETAILS.
            foreach (var item in Lista)
            {
                Result = await _fact.AgregarDetails(item, IdHeader);

            }

            // SI ES TRUE.
            if (Result)
            {
                // ACCION QUE LIMPIAR LA LISTA.
                Lista.Clear();

                // AVISO AL USUARIO
                TempData["Validando"] = "Los datos fueron insertados correctamente :)";

                // RETORNO DE LA VISTA FACTURACION.
                return RedirectToAction("Facturacion");
            }

            // SI HUBO UN ERROR...

            // MENSAJE DE ERROR
            TempData["Error"] = "No se insertaron los datos de manera correcta";

            return RedirectToAction("Facturacion");

        }  
    }
}
