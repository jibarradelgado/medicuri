using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedNeg.BitacoraFaltantes
{
    public class BlBitacoraFaltantes
    {
        MedDAL.BitacoraFaltantes.DALBitacoraFaltantes odalBitacoraFaltantes;

        public BlBitacoraFaltantes()
        {
            odalBitacoraFaltantes = new MedDAL.BitacoraFaltantes.DALBitacoraFaltantes();
        }

        public bool NuevoRegistro(MedDAL.DAL.bitacora_faltantes oBitacoraFaltante)
        {
            return odalBitacoraFaltantes.NuevoRegistro(oBitacoraFaltante);
        }

        public bool EliminarRegistro(MedDAL.DAL.bitacora_faltantes oBitacoraFaltante)
        {
            return odalBitacoraFaltantes.EliminarRegistro(oBitacoraFaltante);
        }

        public bool EliminarTodo()
        {
            return odalBitacoraFaltantes.EliminarTodo();
        }
    }
}
