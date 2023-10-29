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
	public class CD_Comuna
	{
		public List<Comuna> Listar()
		{
			List<Comuna> lista = new List<Comuna>();
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					StringBuilder sb = new StringBuilder();
					sb.AppendLine("SELECT C.ID_COMUNA , C.DESCRIPCION[DesComuna],R.DESCRIPCION[DesRegion],C.ACTIVO ");
					sb.AppendLine("FROM COMUNA C");
					sb.AppendLine("INNER JOIN REGION R ON C.ID_REGION = R.ID_REGION");
					string query = "SELECT  C.ID_COMUNA, C.ID_REGION, C.DESCRIPCION, R.DESCRIPCION[uwu],C.ACTIVO FROM COMUNA C INNER JOIN REGION R ON C.ID_REGION = R.ID_REGION";

					SqlCommand cmd = new SqlCommand(query, conexion);
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
									ID_REGION = new Region { 
										DESCRIPCION = dr["uwu"].ToString() 
									},
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
				lista = new List<Comuna>();
			}
			return lista;
		}



		public int Registrar(Comuna obj, out string Mensaje)
		{
			int idAutogenerado = 0;
			Mensaje = string.Empty;
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					SqlCommand cmd = new SqlCommand("SP_REGISTRAR_COMUNA", conexion);
					cmd.Parameters.AddWithValue("DESCRIPCION", obj.DESCRIPCION);
					cmd.Parameters.AddWithValue("ID_REGION", obj.ID_REGION.ID_REGION);
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

		public bool Editar(Comuna obj, out string Mensaje)
		{
			bool resultado = false;
			Mensaje = string.Empty;
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					SqlCommand cmd = new SqlCommand("SP_EDITAR_COMUNA", conexion);
					cmd.Parameters.AddWithValue("IDCOMUNA", obj.ID_COMUNA);
					cmd.Parameters.AddWithValue("DESCRIPCION", obj.DESCRIPCION);
					cmd.Parameters.AddWithValue("ID_" +
						"REGION", obj.ID_REGION.ID_REGION);
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
					SqlCommand cmd = new SqlCommand("SP_ELIMINAR_COMUNA", conexion);
					cmd.Parameters.AddWithValue("IDCOMUNA", id);
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
