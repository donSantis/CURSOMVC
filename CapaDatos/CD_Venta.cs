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
    public class CD_Venta
    {
        public bool Registrar(Venta obj,DataTable DetalleVenta, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("USP_REGISTRAR_VENTA", conexion);
                    cmd.Parameters.AddWithValue("IDCLIENTE", obj.ID_CLIENTE);
                    cmd.Parameters.AddWithValue("TOTALPRODUCTO", obj.TOTAL_PRODUCTO);
                    cmd.Parameters.AddWithValue("MONTOTOTAL", obj.MONTO_TOTAL);
                    cmd.Parameters.AddWithValue("CONTACTO", obj.CONTACTO);
                    cmd.Parameters.AddWithValue("IDCOMUNA", obj.ID_COMUNA);
                    cmd.Parameters.AddWithValue("TELEFONO", obj.TELEFONO);
                    cmd.Parameters.AddWithValue("DIRECCION", obj.DIRECCION);
                    cmd.Parameters.AddWithValue("IDTRANSACCION", obj.ID_TRANSACCION);
                    cmd.Parameters.AddWithValue("DETALLEVENTA", DetalleVenta);
                    cmd.Parameters.Add("RESULTADO", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("MENSAJE", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["RESULTADO"].Value);
                    Mensaje = cmd.Parameters["MENSAJE"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }
            return respuesta;
        }
    }
}
