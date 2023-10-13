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
	public class CD_Region
	{
		public List<Region> Listar()
		{
			List<Region> lista = new List<Region>();
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					string query = "SELECT ID_REGION, DESCRIPCION,ACTIVO FROM REGION";
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
									ACTIVO = Convert.ToBoolean(dr["ACTIVO"]),
								});
						}

					}
				}
				{

				}
			}
			catch
			{
				lista = new List<Region>();
			}
			return lista;
		}



		public int Registrar(Region obj, out string Mensaje)
		{
			int idAutogenerado = 0;
			Mensaje = string.Empty;
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					SqlCommand cmd = new SqlCommand("SP_REGISTRAR_REGION", conexion);
					cmd.Parameters.AddWithValue("DESCRIPCION", obj.DESCRIPCION);
					cmd.Parameters.AddWithValue("ACTIVO", obj.ACTIVO);
					cmd.Parameters.Add("RESULTADO", SqlDbType.Int).Direction = ParameterDirection.Output;
					cmd.Parameters.Add("MENSAJE", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
					cmd.CommandType = CommandType.StoredProcedure;

					conexion.Open();

					cmd.ExecuteNonQuery();
					idAutogenerado = Convert.ToInt32(cmd.Parameters["RESULTADO"].Value);
					Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
				}
			}
			catch (Exception ex)
			{
				idAutogenerado = 0;
				Mensaje = ex.Message;
			}
			return idAutogenerado;

		}

		public bool Editar(Region obj, out string Mensaje)
		{
			bool resultado = false;
			Mensaje = string.Empty;
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					SqlCommand cmd = new SqlCommand("SP_EDITAR_REGION", conexion);
					cmd.Parameters.AddWithValue("IDREGION", obj.ID_REGION);
					cmd.Parameters.AddWithValue("DESCRIPCION", obj.DESCRIPCION);
					cmd.Parameters.AddWithValue("ACTIVO", obj.ACTIVO);
					cmd.Parameters.Add("RESULTADO", SqlDbType.Bit).Direction = ParameterDirection.Output;
					cmd.Parameters.Add("MENSAJE", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
					cmd.CommandType = CommandType.StoredProcedure;

					conexion.Open();

					cmd.ExecuteNonQuery();

					resultado = Convert.ToBoolean(cmd.Parameters["RESULTADO"].Value);
					Mensaje = cmd.Parameters["MENSAJE"].Value.ToString();
				}
			}
			catch (Exception ex)
			{
				resultado = false;
				Mensaje = ex.Message;
			}
			return resultado;
		}
		public bool Eliminar(int id, out string Mensaje)
		{
			bool resultado = false;
			Mensaje = string.Empty;
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					SqlCommand cmd = new SqlCommand("SP_ELIMINAR_REGION", conexion);
					cmd.Parameters.AddWithValue("IDREGION", id);
					cmd.Parameters.Add("RESULTADO", SqlDbType.Bit).Direction = ParameterDirection.Output;
					cmd.Parameters.Add("MENSAJE", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
					cmd.CommandType = CommandType.StoredProcedure;

					conexion.Open();

					cmd.ExecuteNonQuery();

					resultado = Convert.ToBoolean(cmd.Parameters["RESULTADO"].Value);
					Mensaje = cmd.Parameters["MENSAJE"].Value.ToString();
				}
			}
			catch (Exception ex)
			{
				resultado = false;
				Mensaje = ex.Message;
			}
			return resultado;
		}
	}
}
