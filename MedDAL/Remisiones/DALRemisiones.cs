using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace MedDAL.Remisiones
{
    public class DALRemisiones
    {
        DAL.medicuriEntities oMedicuriEntities = new DAL.medicuriEntities();

        public DALRemisiones()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// DAL - Insertar nuevo registro
        /// </summary>
        /// <param name="oRemision">Remision a guardar</param>
        /// <returns>true registrado, false no registrado</returns>
        public bool NuevoRegistro(DAL.remisiones oRemision)
        {
            try
            {
                //Agregar el registro
                oMedicuriEntities.AddToremisiones(oRemision);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Recuperar una remision mediante su folio
        /// </summary>
        /// <param name="sFolio">Folio a buscar</param>
        /// <returns></returns>
        public DAL.remisiones BuscarRemisionFolio(string sFolio)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.remisiones
                             where q.Folio == sFolio
                             select q;
                return oQuery.First<MedDAL.DAL.remisiones>();
            }
            catch
            {
                return null;
            }


        }

        /// <summary>
        /// Recuperar una remision mediante su Id
        /// </summary>
        /// <param name="iIdRemision">Folio a buscar</param>
        /// <returns></returns>
        public DAL.remisiones BuscarRemision(int iIdRemision)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.remisiones
                             where q.idRemision == iIdRemision
                             select q;
                return oQuery.First<MedDAL.DAL.remisiones>();
            }
            catch
            {
                return null;
            }


        }

       /// <summary>
       /// DAL - Agregar detalle de partida de una remision
       /// </summary>
       /// <param name="oRemisionPartida">Registro de la partida</param>
       /// <returns></returns>
        public bool NuevoDetalleRemision(DAL.remisiones_partida oRemisionPartida)
        {
            try
            {
                //Agregar el registro
                oMedicuriEntities.AddToremisiones_partida(oRemisionPartida);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// DAL - Saber si existe un folio repetido
        /// </summary>
        /// <param name="sFolio">Folio a comparar</param>
        /// <returns></returns>
        public bool ValidarFolioRepetido(string sFolio)
        {

            try
            {
                var oQuery = from q in oMedicuriEntities.remisiones
                             where q.Folio == sFolio
                             select q;

                if (oQuery.Count() > 0)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// REcupera la lista de los clientes
        /// </summary>
        /// <returns></returns>
        public IQueryable<RemisionesView> MostrarLista()
        {
            var oQuery = (from q in oMedicuriEntities.remisiones
                          select new RemisionesView 
                          { 
                              idRemision = q.idRemision, 
                              idCliente = q.idCliente, 
                              Nombre = q.clientes.Nombre, 
                              Apellidos = q.clientes.Apellidos,
                              idPedido = q.idPedido == null ? 0 : (int)q.idPedido, 
                              Folio = q.Folio, 
                              Fecha = q.Fecha, 
                              Estatus = q.Estatus 
                          });

            

            return oQuery;
        }

        /// <summary>
        /// Buscar mediante filtros parametros
        /// </summary>
        /// <param name="sCadena">Parametro a buscar</param>
        /// <param name="iFiltro">Filtro</param>
        /// <returns>var oQuery</returns>
        public IQueryable<RemisionesView> Buscar(string sCadena, int iFiltro)
        {
            string sConsulta = "";

            switch (iFiltro)
            {

                case 1:
                    sConsulta = "it.Folio LIKE '%'+@Dato+'%' ";
                    break;

                case 2:
                    sConsulta = "it.clientes.nombre LIKE '%'+@Dato+'%'";
                    break;

                //case 3:
                //    //sConsulta = "it.Fecha LIKE '%'+@Dato+'%'";
                //    sConsulta = "it.Fecha='@Dato'";

                //    break;


            }

            if (iFiltro == 3)
            {


                DateTime dFecha = Convert.ToDateTime(sCadena);
                var oQuery = from q in oMedicuriEntities.remisiones
                             where (q.Fecha == dFecha)
                             select new RemisionesView 
                          { 
                              idRemision = q.idRemision, 
                              idCliente = q.idCliente, 
                              Nombre = q.clientes.Nombre, 
                              Apellidos = q.clientes.Apellidos,
                              idPedido = q.idPedido == null ? 0 : (int)q.idPedido,
                              Folio = q.Folio, 
                              Fecha = q.Fecha, 
                              Estatus = q.Estatus 
                          };
                return oQuery;

            }
            else
            {
                var oQuery = from q in oMedicuriEntities.remisiones.Where(sConsulta, new ObjectParameter("Dato", sCadena))                             
                             select new RemisionesView 
                          { 
                              idRemision = q.idRemision, 
                              idCliente = q.idCliente, 
                              Nombre = q.clientes.Nombre, 
                              Apellidos = q.clientes.Apellidos, 
                              idPedido = q.idPedido == null? 0 : (int)q.idPedido, 
                              Folio = q.Folio, 
                              Fecha = q.Fecha, 
                              Estatus = q.Estatus 
                          };
                return oQuery;
            }

        }

        /// <summary>
        /// Recuperar la partida de una remision
        /// </summary>
        /// <param name="iIdRemision"></param>
        /// <returns></returns>
        public IQueryable<DAL.remisiones_partida> RecuperarPartidaRemision(int iIdRemision)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.remisiones_partida
                             where q.idRemision == iIdRemision
                             select q;

                return oQuery;

            }
            catch
            {
                return null;
            }
         }

        /// <summary>
        /// Editar registro
        /// </summary>
        /// <param name="oRemision"></param>
        /// <returns></returns>
        public bool EditarRegistro(DAL.remisiones oRemision)
        {
            try
            {
                var oQuery= from q in oMedicuriEntities.remisiones.Where("it.idRemision=@idRemision",
                            new ObjectParameter("idRemision",oRemision.idRemision))
                            select q;

                DAL.remisiones oRemisionOriginal= oQuery.First<DAL.remisiones>();
                oRemisionOriginal.Estatus=oRemision.Estatus;

                oMedicuriEntities.SaveChanges();
                return true;            

            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Eliminar la partida de una remision
        /// </summary>
        /// <param name="iIdRemision"></param>
        /// <returns></returns>
        public bool EliminarRemisionPartida(int iIdRemision)
        {
            try
            {
               var oQuery = from q in oMedicuriEntities.remisiones_partida.
                             Where("it.idRemision=@idRemision",
                             new ObjectParameter("idRemision",iIdRemision))
                             select q;

               foreach (DAL.remisiones_partida oRegistro in oQuery)
               {
                   oMedicuriEntities.DeleteObject(oRegistro);

               }
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Eliminar una remision
        /// </summary>
        /// <param name="iIdRemision">Id remision a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(int iIdRemision)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.remisiones.
                              Where("it.idRemision=@idRemision",
                              new ObjectParameter("idRemision", iIdRemision))
                             select q;

                DAL.remisiones oRemisionOriginal = oQuery.First<DAL.remisiones>();
                oMedicuriEntities.DeleteObject(oRemisionOriginal);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Buscar folios asincronamente para llenar
        /// </summary>
        /// <param name="sCadena"></param>
        /// <returns></returns>
        public string[] BuscarFolioAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.remisiones
                             where q.Folio.Contains(sCadena) && q.Estatus=="2"
                             select q.Folio;

                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }
    }
   
}
