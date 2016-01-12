using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedNeg.VendedorEspecialidad
{
    public class BlVendedorEspecialidad
    {
        MedDAL.VendedorEspecialidad.DALVendedorEspecialidad oEspecialidad;

        public BlVendedorEspecialidad() {
            oEspecialidad = new MedDAL.VendedorEspecialidad.DALVendedorEspecialidad();
        }

        /// <summary>
        /// recupera una especialidad registrada
        /// </summary>
        /// <param name="sCadena">nombre de la especialidad</param>
        /// <returns></returns>
        public object Buscar(string sCadena)
        {
            return oEspecialidad.Buscar(sCadena);
        }

        /// <summary>
        /// recupera una especialidad registrada
        /// </summary>
        /// <param name="idEspecialidad">id de la especialidad</param>
        /// <returns></returns>
        public object Buscar(int idEspecialidad){
            return oEspecialidad.Buscar(idEspecialidad);
        }

        /// <summary>
        /// Inserta una nueva especialidad
        /// </summary>
        /// <param name="oVendedorEspecialidad">obj especialdiad a registrar</param>
        /// <returns></returns>
        public object NuevoRegistro(MedDAL.DAL.vendedores_especialidad oVendedorEspecialidad) {
            return oEspecialidad.NuevoRegistro(oVendedorEspecialidad);
        }
    }
}
