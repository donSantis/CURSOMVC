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
	public class CD_Categoria
	{
		public List<Categoria> Listar()
		{
			List<Categoria> lista = new List<Categoria>();
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					string query = "SELECT ID_CATEGORIA, DESCRIPCION,ACTIVO FROM CATEGORIA";
					SqlCommand cmd = new SqlCommand(query, conexion);
					cmd.CommandType = CommandType.Text;
					conexion.Open();
					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							lista.Add(
								new Categoria()
								{
									ID_CATEGORIA = Convert.ToInt32(dr["ID_CATEGORIA"]),
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
				lista = new List<Categoria>();
			}
			return lista;
		}



		public int Registrar(Categoria obj, out string Mensaje)
		{
			int idAutogenerado = 0;
			Mensaje = string.Empty;
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					SqlCommand cmd = new SqlCommand("SP_REGISTRAR_CATEGORIA", conexion);
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

		public bool Editar(Categoria obj, out string Mensaje)
		{
			bool resultado = false;
			Mensaje = string.Empty;
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					SqlCommand cmd = new SqlCommand("SP_EDITAR_CATEGORIA", conexion);
					cmd.Parameters.AddWithValue("IDCATEGORIA", obj.ID_CATEGORIA);
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
					SqlCommand cmd = new SqlCommand("SP_ELIMINAR_CATEGORIA", conexion);
					cmd.Parameters.AddWithValue("IDCATEGORIA", id);
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
