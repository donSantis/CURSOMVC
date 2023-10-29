using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Ubicacion
    {
        private CD_Ubicacion objCapaDato = new CD_Ubicacion();
        public List<Region> ObtenerRegion()
        {
            return objCapaDato.ObtenerRegion();
        }
        public List<Comuna> ObtenerComuna(int idregion)
        {
            return objCapaDato.ObtenerComuna(idregion);
        }
    }
}
