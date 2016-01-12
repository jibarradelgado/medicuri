using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace MedDAL.VendedorEspecialidad
{
    public class DALVendedorEspecialidad
    {
        DAL.medicuriEntities oMedicuriEntities;

        public DALVendedorEspecialidad()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        // GT: 21/Mar/2011 Función que recupera la vinculacion de un vendedor mediante su nombre
        /// <summary>
        /// DAL - Buscar vinculacion mediante el nombre
        /// </summary>
        /// <param name="sCadena">Parametro de busqueda</param>
        /// <returns></returns>
        public string[] BuscarEspecialidadAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.vendedores_especialidad
                             where q.Especialidad.Contains(sCadena)
                             select q.Especialidad;

                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }

        /// <summary>
        /// Busca una especialidad registrada
        /// </summary>
        /// <param name="sEspecialidad">Nombre de especialidad</param>
        /// <returns></returns>
        public object Buscar(string sEspecialidad)
        {
            var oQuery = from q in oMedicuriEntities.vendedores_especialidad
                         where q.Especialidad == sEspecialidad
                         select q;

            if (oQuery.Count<MedDAL.DAL.vendedores_especialidad>() > 0)
            {
                return (object)oQuery.First<MedDAL.DAL.vendedores_especialidad>();
            }
            else return null;
        }

        /// <summary>
        /// Busca una especialdiad registrada
        /// </summary>
        /// <param name="idEspecialidad">id de despecialidad</param>
        /// <returns></returns>
        public object Buscar(int idEspecialidad)
        {
            var oQuery = from q in oMedicuriEntities.vendedores_especialidad
                         where q.idEspecialidad == idEspecialidad
                         select q;

            if (oQuery.Count<MedDAL.DAL.vendedores_especialidad>() > 0)
            {
                return (object)oQuery.First<MedDAL.DAL.vendedores_especialidad>();
            }
            else return null;
        }


       

        /// <summary>
        /// DAL - Insertar nuevo registro
        /// </summary>
        /// <param name="oEspecialidad">Registro a guardar</param>
        /// <returns>true registrado, false no registrado</returns>
        public bool NuevoRegistro(DAL.vendedores_especialidad oEspecialidad)
        {
            try
            {
                //Agregar el registro
                oMedicuriEntities.AddTovendedores_especialidad(oEspecialidad);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
