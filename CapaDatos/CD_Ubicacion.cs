using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace CapaDatos
{
    public class CD_Ubicacion
    {
        public List<Region> ObtenerRegion()
        {
            List<Region> lista = new List<Region>();
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT * FROM REGION";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Region()
                                {
                                    ID_REGION = Convert.ToInt32(dr["ID_REGION"]),
                                    DESCRIPCION = dr["DESCRIPCION"].ToString(),
                                });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Region>();
            }
            return lista;
        }

        public List<Comuna> ObtenerComuna(int idregion)
        {
            List<Comuna> lista = new List<Comuna>();
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT * FROM COMUNA WHERE ID_REGION = @ID_REGION";

                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("CONTRASEÑA", idregion);
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Comuna()
                                {
                                    ID_COMUNA = Convert.ToInt32(dr["ID_COMUNA"]),
                                    DESCRIPCION = dr["DESCRIPCION"].ToString(),
                                });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Comuna>();
            }
            return lista;
        }
    }
}
