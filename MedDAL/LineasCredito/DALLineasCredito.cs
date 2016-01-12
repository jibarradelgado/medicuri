using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedDAL.LineasCredito
{
    public class DALLineasCredito
    {
        DAL.medicuriEntities oMedicuriEntities;

        //Constructor
        public DALLineasCredito()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// Insertar Linea de crédito
        /// </summary>
        /// <param name="oLineaCredito">Linea de crédito a insertar</param>
        /// <returns></returns>
        public bool NuevoRegistro(DAL.lineas_creditos oLineaCredito)
        {
            try
            {
               
                oMedicuriEntities.AddTolineas_creditos(oLineaCredito);
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Buscar mediante filtros parametros
        /// </summary>
        /// <param name="sCadena">Parametro a buscar</param>
        /// <param name="iFiltro">Filtro</param>
        /// <returns>var oQuery</returns>
        public object Buscar(string sCadena, int iFiltro)
        {
            string sConsulta = "";
            switch (iFiltro)
            {
                //Institución Emisora
                case 1:
                    sConsulta = "it.InstitucionEmisora LIKE '%'+@Dato+'%' and it.Activo = true";
                    break;
                //FuenteCuenta
                case 2:
                    sConsulta = "it.FuenteCuenta LIKE '%'+@Dato+'%' and it.Activo = true";
                    break;
                //Numero Cuenta
                case 3:
                    sConsulta = "it.NumeroCuenta LIKE '%'+@Dato+'%' and it.Activo = true";
                    break;
               
                    
            }

            /*var oQuery = from q in oMedicuriEntities.estados.
                                      Where(sConsulta,
                                      new ObjectParameter("Dato", sCadena))
                         select q;*/
            var oQuery = from q in oMedicuriEntities.lineas_creditos.Where(sConsulta,new ObjectParameter("Dato", sCadena))
                         select q;

            return oQuery;
        }


        /// <summary>
        /// DAL - Muestra todos los registros de la tabla
        /// </summary>
        /// <returns>Objeto</returns>
        public object MostrarLista()
        {

            var oQuery = from q in oMedicuriEntities.lineas_creditos select q;
            return oQuery;

        }

        public IQueryable<DAL.lineas_creditos> MostrarListaNoVencidas()
        {

            var oQuery = from q in oMedicuriEntities.lineas_creditos
                         where q.FechaVencimiento.CompareTo(DateTime.Now)>=0 && q.Activo
                         select q;
            return oQuery;

        }

        /// <summary>
        /// DAL - muestra todos los registros activos de la tabla
        /// </summary>
        /// <returns>IQueryable de DAL.lineas_creditos  conteniendo los objetos</returns>
        public IQueryable<DAL.lineas_creditos> MostrarListaActivos()
        {

            var oQuery = from q in oMedicuriEntities.lineas_creditos 
                         where q.Activo==true
                         select q;
            return oQuery;

        }

        /// <summary>
        /// Recuperar una linea de crédito mediante su llave primaria
        /// </summary>
        /// <param name="iId">Llave primaria</param>
        /// <returns>(object)oQuery.First<MedDAL.DAL.lineas_creditos></returns>
        public object Buscar(int iId)
        {
            var oQuery = from q in oMedicuriEntities.lineas_creditos
                         where q.idLineaCredito == iId
                         select q;

            return (object)oQuery.First<MedDAL.DAL.lineas_creditos>();
        }

        /// <summary>
        /// Recuperar una linea de crédito mediante su clave
        /// </summary>
        /// <param name="sClave">Clave de la line de credito</param>
        /// <returns>(object)oQuery.First<MedDAL.DAL.lineas_creditos></returns>
        public object Buscar(string sClave)
        {
            var oQuery = from q in oMedicuriEntities.lineas_creditos
                         where q.Clave==sClave
                         select q;

            return (object)oQuery.First<MedDAL.DAL.lineas_creditos>();
        }


        /// <summary>
        /// Validar linea de credito
        /// </summary>
        /// <param name="sClave">Nombre de usuario</param>
        /// <returns>Object</returns>
        public int ValidarClaveRepetida(string sClave)
        {
            try
            {

                //Recuperar el objeto a editar
                var oQuery = from q in oMedicuriEntities.lineas_creditos.
                             Where("it.Clave=@Clave",
                             new ObjectParameter("Clave", sClave))
                             select q;

                return oQuery.Count();

            }
            catch
            {
                return 0;
            }
        }
    
        /// <summary>
        /// Editar Línea de crédito
        /// </summary>
        /// <param name="oLineaCredito">Línea de crédito a editar</param>
        /// <returns></returns>
        public bool EditarRegistro(DAL.lineas_creditos oLineaCredito)
        {
            try
            {
                          
                //Recuperar el objeto a editar
                var oQuery = from q in oMedicuriEntities.lineas_creditos.
                             Where("it.idLineaCredito=@idLineaCredito",
                             new ObjectParameter("idLineaCredito", oLineaCredito.idLineaCredito))
                             select q;
                
                DAL.lineas_creditos oLineaOriginal = oQuery.First<DAL.lineas_creditos>();

                //Modificar los valores

                oLineaOriginal.InstitucionEmisora = oLineaCredito.InstitucionEmisora;
                oLineaOriginal.FuenteCuenta = oLineaCredito.FuenteCuenta;
                oLineaOriginal.NumeroCuenta = oLineaCredito.NumeroCuenta;
                oLineaOriginal.Monto = oLineaCredito.Monto;
                oLineaOriginal.FechaMinistracion = oLineaCredito.FechaMinistracion;
                oLineaOriginal.FechaVencimiento = oLineaCredito.FechaVencimiento;
                oLineaOriginal.Activo = oLineaCredito.Activo;

                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Eliminar Línea de Crédito
        /// </summary>
        /// <param name="oLineaCredito">Línea de crédito a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(DAL.lineas_creditos oLineaCredito)
        {
            try
            {
               var oQuery = from q in oMedicuriEntities.lineas_creditos.
                             Where("it.idLineaCredito=@idLineaCredito",
                             new ObjectParameter("idLineaCredito", oLineaCredito.idLineaCredito))
                             select q;

                DAL.lineas_creditos oLineaOriginal = oQuery.First<DAL.lineas_creditos>();
                oMedicuriEntities.DeleteObject(oLineaOriginal);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void DescontarCreditoALinea(int idLineaCredito, decimal cantidad)
        {
            var oQuery = from q in oMedicuriEntities.lineas_creditos
                         where q.idLineaCredito==idLineaCredito
                         select q;

            MedDAL.DAL.lineas_creditos lCred = oQuery.First();
            
            lCred.Monto -= cantidad;

            oMedicuriEntities.SaveChanges();
        }

        public void AumentarCreditoALinea(int idLineaCredito, decimal cantidad)
        {
            var oQuery = from q in oMedicuriEntities.lineas_creditos
                         where q.idLineaCredito == idLineaCredito
                         select q;

            MedDAL.DAL.lineas_creditos lCred = oQuery.First();

            lCred.Monto += cantidad;

            oMedicuriEntities.SaveChanges();
        }

        public decimal BuscarCantidadCreditoLineaDeCredito(int idLineaCredito)
        {
            var oQuery = from q in oMedicuriEntities.lineas_creditos
                         where q.idLineaCredito == idLineaCredito 
                         select q;

         return oQuery.First().Monto;
        }


        /// <summary>
        /// DAL Metodo que regresa las claves que contengan el parametro
        /// </summary>
        /// <param name="sCadena">Cadena que contenga la clave</param>
        /// <returns></returns>
        public string[] BuscarClaveLineaCreditoAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.lineas_creditos
                             where q.Clave.Contains(sCadena)
                             select q.Clave;


                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }

    }
}
