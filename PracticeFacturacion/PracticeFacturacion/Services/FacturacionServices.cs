using PracticeFacturacion.Models;

namespace PracticeFacturacion.Services
{
    public class FacturacionServices
    {
        // CAMPO QUE POSEE EL MODELO DE LA BD.
        private readonly FACTURACIONContext _Context;

        // CONSTRUCTOR QUE POSEE EL MODELO DE LA BD.
        public FacturacionServices(FACTURACIONContext context)
        {
            _Context = context;
        }

        // METODO QUE AGREGAR EL HEADER.
        public async Task<int> AgregarHeader(FacturacionHeader FH)
        {
            try
            {
                await _Context.FacturacionHeaders.AddAsync(FH);
                _Context.SaveChanges();

                // RETORNANDO EL ID DEL REGISTRO INGRESADO.
                return FH.Id;
            }
            catch
            {
                return 0;
            }
        }

        // METODO QUE INSETAR LOS CAMPOS DE LA TABLA DINAMICA EN LA TABLA DETAILS.
        public async Task<bool> AgregarDetails(F_Detail_2 Detail, int ID)
        {
            if(Detail == null || ID < 0)
            {
                return false;
            }

            try
            {
                // CREACION DE OBJETO TIPO DETAIL QUE ALMACENA LOS DATOS
                // RECIBIDOS POR PARAMETRO.
                FacturacionDetail FD = new FacturacionDetail
                {
                    IdHeader = ID,
                    Cantidad = Detail.Cantidad,
                   DescripcionProducto = Detail.DescripcionProducto,
                   Itbis = Detail.Itbis,
                   CostoUnitario = Detail.CostoUnitario,
                   Total = Detail.Total

                };

                await _Context.FacturacionDetails.AddAsync(FD);
                _Context.SaveChanges();

                return true;
            }
            catch 
            {
                return false;
            }
        }
    }
}
