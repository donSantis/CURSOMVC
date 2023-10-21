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
	public class CD_Cliente
	{
		public List<Cliente> Listar()
		{
			List<Cliente> lista = new List<Cliente>();
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					string query = "SELECT ID_CLIENTE, NOMBRES,APELLIDOS,CORREO,CONTRASEÑA,REESTABLECER,ACTIVO FROM CLIENTE";
					SqlCommand cmd = new SqlCommand(query, conexion);
					cmd.CommandType = CommandType.Text;
					conexion.Open();
					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							lista.Add(
								new Cliente()
								{
									ID_CLIENTE = Convert.ToInt32(dr["ID_CLIENTE"]),
									NOMBRES = dr["NOMBRES"].ToString(),
									APELLIDOS = dr["APELLIDOS"].ToString(),
									CORREO = dr["CORREO"].ToString(),
									CONTRASEÑA = dr["CONTRASEÑA"].ToString(),
									REESTABLECER = Convert.ToBoolean(dr["REESTABLECER"]),
								});
						}

					}
				}
			}
			catch
			{
				lista = new List<Cliente>();
			}
			return lista;
		}

		public int Registrar(Cliente obj, out string Mensaje)
		{
			int idAutogenerado = 0;
			Mensaje = string.Empty;
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					SqlCommand cmd = new SqlCommand("SP_REGISTRAR_CLIENTE", conexion);
					cmd.Parameters.AddWithValue("NOMBRES", obj.NOMBRES);
					cmd.Parameters.AddWithValue("APELLIDOS", obj.APELLIDOS);
					cmd.Parameters.AddWithValue("CORREO", obj.CORREO);
					cmd.Parameters.AddWithValue("CONTRASEÑA", obj.CONTRASEÑA);
					cmd.Parameters.AddWithValue("REESTABLECER", obj.REESTABLECER);
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
		public bool CambiarClave(int idcliente, string nuevaclave, out string Mensaje)
		{
			bool resultado = false;
			Mensaje = string.Empty;
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					SqlCommand cmd = new SqlCommand("UPDATE CLIENTE SET CONTRASEÑA = @nuevaclave, REESTABLECER = 0 WHERE ID_CLIENTE = @id", conexion);
					cmd.Parameters.AddWithValue("@id", idcliente);
					cmd.Parameters.AddWithValue("@nuevaclave", nuevaclave);
					cmd.CommandType = CommandType.Text;
					conexion.Open();
					resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
				}
			}
			catch (Exception ex)
			{
				resultado = false;
				Mensaje = ex.Message;
			}
			return resultado;
		}

		public bool ReestablecerClave(int idcliente, string clave, out string Mensaje)
		{
			bool resultado = false;
			Mensaje = string.Empty;
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					SqlCommand cmd = new SqlCommand("UPDATE CLIENTE SET CONTRASEÑA = @clave, REESTABLECER = 1 WHERE ID_CLIENTE = @id", conexion);
					cmd.Parameters.AddWithValue("@id", idcliente);
					cmd.Parameters.AddWithValue("@clave", clave);
					cmd.CommandType = CommandType.Text;
					conexion.Open();
					resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
				}
			}
			catch (Exception ex)
			{
				resultado = false;
				Mensaje = ex.Message;
			}
			return resultado;
		}

		public bool Editar(Cliente obj, out string Mensaje)
		{
			bool resultado = false;
			Mensaje = string.Empty;
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					SqlCommand cmd = new SqlCommand("SP_EDITAR_USUARIO", conexion);
					cmd.Parameters.AddWithValue("IDUSUARIO", obj.ID_CLIENTE);
					cmd.Parameters.AddWithValue("NOMBRES", obj.NOMBRES);
					cmd.Parameters.AddWithValue("APELLIDOS", obj.APELLIDOS);
					cmd.Parameters.AddWithValue("CORREO", obj.CORREO);
					//cmd.Parameters.AddWithValue("CONTRASEÑA", obj.CONTRASEÑA);
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
					SqlCommand cmd = new SqlCommand("DELETE TOP (1) FROM CLIENTE WHERE ID_CLIENTE = @id", conexion);
					cmd.Parameters.AddWithValue("@id", id);
					cmd.CommandType = CommandType.Text;
					conexion.Open();
					resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
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
