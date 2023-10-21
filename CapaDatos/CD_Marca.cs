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
	public class CD_Marca
	{
		public List<Marca> Listar()
		{
			List<Marca> lista = new List<Marca>();
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					string query = "SELECT ID_MARCA, DESCRIPCION,ACTIVO FROM MARCA";
					SqlCommand cmd = new SqlCommand(query, conexion);
					cmd.CommandType = CommandType.Text;
					conexion.Open();
					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							lista.Add(
								new Marca()
								{
									ID_MARCA = Convert.ToInt32(dr["ID_MARCA"]),
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
				lista = new List<Marca>();
			}
			return lista;
		}



		public int Registrar(Marca obj, out string Mensaje)
		{
			int idAutogenerado = 0;
			Mensaje = string.Empty;
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					SqlCommand cmd = new SqlCommand("SP_REGISTRAR_MARCA", conexion);
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

		public bool Editar(Marca obj, out string Mensaje)
		{
			bool resultado = false;
			Mensaje = string.Empty;
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					SqlCommand cmd = new SqlCommand("SP_EDITAR_MARCA", conexion);
					cmd.Parameters.AddWithValue("IDMARCA", obj.ID_MARCA);
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
					SqlCommand cmd = new SqlCommand("SP_ELIMINAR_MARCA", conexion);
					cmd.Parameters.AddWithValue("IDMARCA", id);
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

		public List<Marca> ListarPorCategoria(int idCategoria )
		{
			List<Marca> lista = new List<Marca>();
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					StringBuilder sb = new StringBuilder();
					sb.AppendLine("SELECT DISTINCT M.ID_MARCA, M.DESCRIPCION FROM PRODUCTO P INNER JOIN CATEGORIA C ON C.ID_CATEGORIA = P.ID_CATEGORIA INNER JOIN MARCA M ON M.ID_MARCA = P.ID_MARCA AND M.ACTIVO = 1 WHERE C.ID_CATEGORIA = IIF(@IDCATEGORIA = 0, C.ID_CATEGORIA, @IDCATEGORIA)");
					SqlCommand cmd = new SqlCommand(sb.ToString(), conexion);
					cmd.Parameters.AddWithValue("@IDCATEGORIA" , idCategoria);
					cmd.CommandType = CommandType.Text;
					conexion.Open();
					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							lista.Add(
								new Marca()
								{
									ID_MARCA = Convert.ToInt32(dr["ID_MARCA"]),
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
				lista = new List<Marca>();
			}
			return lista;
		}
	}
}
