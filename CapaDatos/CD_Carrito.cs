using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Carrito
    {
        public bool ExisteCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_EXISTE_CARRITO", conexion);
                    cmd.Parameters.AddWithValue("ID_CLIENTE", idcliente);
                    cmd.Parameters.AddWithValue("ID_PRODUCTO", idproducto);
                    cmd.Parameters.Add("RESULTADO", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conexion.Open();

                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["RESULTADO"].Value);
               
                }
            }
            catch (Exception ex)
            {
                resultado = false;
            }
            return resultado;

        }

        public bool OperacionCarrito(int idcliente, int idproducto,bool sumar, out string Mensaje)
        {
            bool resultado = true;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_OPERACION_CARRITO", conexion);
                    cmd.Parameters.AddWithValue("ID_CLIENTE", idcliente);
                    cmd.Parameters.AddWithValue("ID_PRODUCTO", idproducto);
                    cmd.Parameters.AddWithValue("SUMAR", sumar);
                    cmd.Parameters.Add("RESULTADO", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("MENSAJE", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["RESULTADO"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;

        }

        public int CantidadCarrito(int idcliente)
        {
            int resultado = 0;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM CARRITO WHERE ID_CLIENTE = @idcliente", conexion);
                    cmd.Parameters.AddWithValue("@idcliente", idcliente);
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();
                    resultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
            }
            return resultado;
        }

        public List<Carrito> ListarProductosCarrito(int idcliente)
        {
            List<Carrito> lista = new List<Carrito>();
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    StringBuilder sb = new StringBuilder();
                    string query = "SELECT * FROM FN_OBTENER_CARRITO_CLIENTE(@ID_CLIENTE)";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@ID_CLIENTE", idcliente);
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Carrito()
                                {
                                    ID_PRODUCTO = new Producto()
                                    {
                                        ID_PRODUCTO = Convert.ToInt32(dr["ID_PRODUCTO"]),
                                        NOMBRE = dr["NOMBRE"].ToString(),
                                        ID_MARCA = new Marca
                                        {
                                            DESCRIPCION = dr["DMAR"].ToString()
                                        },
                                        PRECIO = Convert.ToDecimal(dr["PRECIO"], new CultureInfo("es-CL")),
                                        RUTA_IMAGEN = dr["RUTA_IMAGEN"].ToString(),
                                        NOMBRE_IMAGEN = dr["NOMBRE_IMAGEN"].ToString(),
                                    },
                                    CANTIDAD =  Convert.ToInt32(dr["CANTIDAD"])
                                });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Carrito>();
            }
            return lista;
        }

        public bool EliminarCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_ELIMINAR_CARRITO", conexion);
                    cmd.Parameters.AddWithValue("ID_CLIENTE", idcliente);
                    cmd.Parameters.AddWithValue("ID_PRODUCTO", idproducto);
                    cmd.Parameters.Add("RESULTADO", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conexion.Open();

                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["RESULTADO"].Value);

                }
            }
            catch (Exception ex)
            {
                resultado = false;
            }
            return resultado;

        }

    }
}
