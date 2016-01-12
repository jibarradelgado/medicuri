using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedDAL.Tipos
{
    public class DALTipos
    {
        DAL.medicuriEntities oMedicuriEntities;

        public DALTipos()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

       /// <summary>
       /// DAL - Agregar nuevo registro
       /// </summary>
       /// <param name="oTipo">Tipo a registrar</param>
       /// <returns></returns>
        public bool NuevoRegistro(DAL.tipos oTipo)
        {
            try
            {
                //Agregar el registro
                oMedicuriEntities.AddTotipos(oTipo);
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
                //Filtro 1
                case 1:
                    sConsulta = "it.Nombre LIKE '%'+@Dato+'%'";
                    break;
                //Filtro 2
                case 2:
                    //sConsulta = "it.Nombre Descripcion '%'+@Dato+'%'";
                    sConsulta = "it." + sCadena+"=true";
                    break;
                //Filtro 3
                case 3:
                    sConsulta = "it.Nombre LIKE '%'+@Dato+'%'";
                    break;
            }

            try
            {
                var oQuery = from q in oMedicuriEntities.tipos.Where(sConsulta, new ObjectParameter("Dato", sCadena))
                             orderby q.Nombre
                             select q;
                return oQuery;
            }
            catch
            {
                return null;
            }
            
        }

        public string Buscar(int idTipo) {
            try {
                var oQuery = from q in oMedicuriEntities.tipos.
                             Where("it.idTipo=@idTipo",
                             new ObjectParameter("idTipo", idTipo))
                             select q;

                DAL.tipos tipo = oQuery.First<DAL.tipos>();
                return tipo.Nombre;
            }
            catch {
                return "";
            }
        }


        /// <summary>
        /// DAL - Muestra todos los registros de la tabla tipos
        /// </summary>
        /// <returns>Objeto</returns>
        public object MostrarLista()
        {
            var oQuery = from q in oMedicuriEntities.tipos orderby q.Nombre select q;
            return oQuery;

        }

        /// <summary>
        /// Recupera todos los tipos existentes
        /// </summary>
        /// <returns></returns>
        public List<MedDAL.DAL.tipos> RecuperarTipos()
        {
            List<MedDAL.DAL.tipos> lstTipos = new List<DAL.tipos>();

            var oQuery = from q in oMedicuriEntities.tipos
                         select q;

            lstTipos.AddRange(oQuery);

            return lstTipos;
        }

        /// <summary>
        /// DAL - Función que regresa todos los tipos de Almacen Activos
        /// </summary>
        /// <returns>List</returns>
        public List<MedDAL.DAL.tipos> RecuperarTiposAlmacen()
        {
            List<MedDAL.DAL.tipos> lstTipos = new List<DAL.tipos>();

            var oQuery = from q in oMedicuriEntities.tipos
                         where q.Almacenes == true && q.Activo==true
                         select q;

            lstTipos.AddRange(oQuery);

            return lstTipos;
        }

        /// <summary>
        /// DAL - Función que regresa todos los tipos de Clientes Activos
        /// </summary>
        /// <returns>List</returns>
        public List<MedDAL.DAL.tipos> RecuperarTiposClientes()
        {
            List<MedDAL.DAL.tipos> lstTipos = new List<DAL.tipos>();

            var oQuery = from q in oMedicuriEntities.tipos
                         where q.Clientes == true && q.Activo == true
                         select q;

            lstTipos.AddRange(oQuery);

            return lstTipos;
        }

        /// <summary>
        /// DAL - Función que regresa todos los tipos de Productos Activos
        /// </summary>
        /// <returns>List</returns>
        public List<MedDAL.DAL.tipos> RecuperarTiposProductos()
        {
            List<MedDAL.DAL.tipos> lstTipos = new List<DAL.tipos>();

            var oQuery = from q in oMedicuriEntities.tipos
                         where q.Productos == true && q.Activo == true
                         select q;

            lstTipos.AddRange(oQuery);

            return lstTipos;
        }


        /// <summary>
        /// DAL - Función que regresa todos los tipos de Proveedores Activos
        /// </summary>
        /// <returns>List</returns>
        public List<MedDAL.DAL.tipos> RecuperarTiposProveedores()
        {
            List<MedDAL.DAL.tipos> lstTipos = new List<DAL.tipos>();

            var oQuery = from q in oMedicuriEntities.tipos
                         where q.Proveedores == true && q.Activo == true
                         select q;

            lstTipos.AddRange(oQuery);

            return lstTipos;
        }

        /// <summary>
        /// DAL - Función que regresa todos los tipos de Vendedores Activos
        /// </summary>
        /// <returns>List</returns>
        public List<MedDAL.DAL.tipos> RecuperarTiposVendedores()
        {
            List<MedDAL.DAL.tipos> lstTipos = new List<DAL.tipos>();

            var oQuery = from q in oMedicuriEntities.tipos
                         where q.Vendedores == true && q.Activo == true
                         select q;

            lstTipos.AddRange(oQuery);

            return lstTipos;
        }

        /// <summary>
        /// DAL - Función que regresa todos los tipos de Recetas Activos
        /// </summary>
        /// <returns>List</returns>
        public List<MedDAL.DAL.tipos> RecuperarTiposRecetas()
        {
            List<MedDAL.DAL.tipos> lstTipos = new List<DAL.tipos>();

            var oQuery = from q in oMedicuriEntities.tipos
                         where q.Recetas == true && q.Activo == true
                         select q;

            lstTipos.AddRange(oQuery);

            return lstTipos;
        }

       /// <summary>
       /// DAL - Editar Registro
       /// </summary>
       /// <param name="oTipo">Tipo a Editar</param>
       /// <returns></returns>
        public bool EditarRegistro(DAL.tipos oTipo)
        {
            try
            {

                //Recuperar el objeto a editar
                var oQuery = from q in oMedicuriEntities.tipos.
                             Where("it.idTipo=@idTipo",
                             new ObjectParameter("idTipo", oTipo.idTipo))
                             select q;

                DAL.tipos oTipoOriginal = oQuery.First<DAL.tipos>();

                //Modificar los valores
                oTipoOriginal.Nombre = oTipo.Nombre;
                oTipoOriginal.Almacenes = oTipo.Almacenes;
                oTipoOriginal.Clientes = oTipo.Clientes;
                oTipoOriginal.Productos = oTipo.Productos;
                oTipoOriginal.Proveedores = oTipo.Proveedores;
                oTipoOriginal.Vendedores = oTipo.Vendedores;
                oTipoOriginal.Recetas = oTipo.Recetas;
                oTipoOriginal.Activo = oTipo.Activo;
                
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Eliminar un tipo
        /// </summary>
        /// <param name="iIdTipo">Id Tipo a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(int iIdTipo)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.tipos.
                              Where("it.idTipo=@idTipo",
                              new ObjectParameter("idTipo", iIdTipo))
                             select q;

                DAL.tipos oTipoOriginal = oQuery.First<DAL.tipos>();
                oMedicuriEntities.DeleteObject(oTipoOriginal);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// DAL Metodo que regresa los tipos de Almacenes
        /// </summary>
        /// <param name="sCadena">Cadena que contenga el tipo</param>
        /// <returns></returns>
        public string[] BuscarTipoAlmacenesAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.tipos
                             where q.Nombre.Contains(sCadena) && q.Activo == true
                             select q.Nombre;

                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }

        /// <summary>
        /// DAL Metodo que regresa los tipos de Clientes
        /// </summary>
        /// <param name="sCadena">Cadena que contenga el tipo</param>
        /// <returns></returns>
        public string[] BuscarTipoClientesAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.tipos
                             where q.Nombre.Contains(sCadena) && q.Activo == true
                             select q.Nombre;

                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }

        /// <summary>
        /// DAL Metodo que regresa los tipos de Productos
        /// </summary>
        /// <param name="sCadena">Cadena que contenga el tipo</param>
        /// <returns></returns>
        public string[] BuscarTipoProductosAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.tipos
                             where q.Nombre.Contains(sCadena) && q.Activo == true
                             select q.Nombre;

                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }

        /// <summary>
        /// DAL Metodo que regresa los tipos de Proveedores
        /// </summary>
        /// <param name="sCadena">Cadena que contenga el tipo</param>
        /// <returns></returns>
        public string[] BuscarTipoProveedoresAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.tipos
                             where q.Nombre.Contains(sCadena) && q.Activo == true
                             select q.Nombre;

                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }

        /// <summary>
        /// DAL Metodo que regresa los tipos de Vendedores
        /// </summary>
        /// <param name="sCadena">Cadena que contenga el tipo</param>
        /// <returns></returns>
        public string[] BuscarTipoVendedoresAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.tipos
                             where q.Nombre.Contains(sCadena) && q.Activo == true
                             select q.Nombre;

                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }

        /// <summary>
        /// DAL Metodo que regresa los tipos de Recetas
        /// </summary>
        /// <param name="sCadena">Cadena que contenga el tipo</param>
        /// <returns></returns>
        public string[] BuscarTipoRecetasAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.tipos
                             where q.Nombre.Contains(sCadena) && q.Activo == true
                             select q.Nombre;

                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }
    }
}
