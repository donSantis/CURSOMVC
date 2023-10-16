using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
	public class CD_Reporte
	{
		public List<Reporte> Ventas(string fechainicio, string fechafin, string idtransaccion)
		{
			List<Reporte> lista = new List<Reporte>();
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					SqlCommand cmd = new SqlCommand("SP_REPORTE_VENTAS", conexion);
					cmd.Parameters.AddWithValue("fechainicio", fechainicio);
					cmd.Parameters.AddWithValue("fechafin", fechafin);
					cmd.Parameters.AddWithValue("idtransaccion", idtransaccion);
					cmd.CommandType = CommandType.StoredProcedure;
					conexion.Open();
					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
								lista.Add(
								new Reporte()
								{
									FECHA_VENTA = dr["FVENTA"].ToString(),
									CLIENTE = "CLIENT",
									PRODUCTO = dr["PRODUCTO"].ToString(),
									PRECIO = Convert.ToDecimal(dr["PRECIO"].ToString()),
									CANTIDAD = Convert.ToInt32(dr["CANTIDAD"].ToString()),
									TOTAL = Convert.ToDecimal(dr["TOTAL"].ToString()),
									ID_TRANSACCION = dr["ID_TRANSACCION"].ToString(),

								});
						}

					}
				}
				{

				}
			}
			catch
			{
				lista = new List<Reporte>();
			}
			return lista;
		}
		public Dashboard VerDashboard()
		{
			Dashboard objeto = new Dashboard();
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					SqlCommand cmd = new SqlCommand("SP_REPORTE_DASHBOARD", conexion);
					cmd.CommandType = CommandType.StoredProcedure;
					conexion.Open();
					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							objeto = new Dashboard()
							{
								TotalCliente = Convert.ToInt32(dr["TotalCliente"]),
								TotalVenta = Convert.ToInt32(dr["TotalVenta"]),
								TotalProducto = Convert.ToInt32(dr["totalProducto"]),
							};
						}
					}
				}
			}
			catch
			{
				objeto = new Dashboard();
			}
			return objeto;
		}
	}

}
