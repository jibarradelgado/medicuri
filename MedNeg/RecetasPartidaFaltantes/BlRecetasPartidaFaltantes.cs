using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedNeg.RecetasPartidaFaltantes
{
    public class BlRecetasPartidaFaltantes
    {
        MedDAL.RecetasPartidaFaltantes.DALRecetasPartidaFaltantes odalRecetasPartidaFaltantes;

        public BlRecetasPartidaFaltantes()
        {
            odalRecetasPartidaFaltantes = new MedDAL.RecetasPartidaFaltantes.DALRecetasPartidaFaltantes();
        }

        public List<MedDAL.DAL.recetas_partida_faltantes> BuscarPorProductoAlmacen(int idProducto, int idAlmacen)
        {
            return odalRecetasPartidaFaltantes.BuscarPorProductoAlmacen(idProducto, idAlmacen);
        }

        public bool NuevoRegistro(MedDAL.DAL.recetas_partida_faltantes oRecetasPartidaFaltante)
        {
            return odalRecetasPartidaFaltantes.NuevoRegistro(oRecetasPartidaFaltante);
        }

        public bool EliminarRegistro(MedDAL.DAL.recetas_partida_faltantes oRecetasPartidaFaltante)
        {
            return odalRecetasPartidaFaltantes.EliminarRegistro(oRecetasPartidaFaltante);
        }

        public bool EliminarTodo()
        {
            return odalRecetasPartidaFaltantes.EliminarTodo();
        }
    }
}
