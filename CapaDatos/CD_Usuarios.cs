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
	public class CD_Usuarios
	{
		public List<Usuario> Listar()
		{
			List<Usuario> lista = new List<Usuario>();
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					string query = "SELECT ID_USUARIO, NOMBRES,APELLIDOS,CORREO,CONTRASEÑA,REESTABLECER,ACTIVO FROM USUARIO";
					SqlCommand cmd = new SqlCommand(query, conexion);
					cmd.CommandType = CommandType.Text;
					conexion.Open();
					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							lista.Add(
								new Usuario()
								{
									ID_USUARIO = Convert.ToInt32(dr["ID_USUARIO"]),
									NOMBRES = dr["NOMBRES"].ToString(),
									APELLIDOS = dr["APELLIDOS"].ToString(),
									CORREO = dr["CORREO"].ToString(),
									CONTRASEÑA = dr["CONTRASEÑA"].ToString(),
									REESTABLECER = Convert.ToBoolean(dr["REESTABLECER"]),
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
				lista = new List<Usuario>();
			}
			return lista;
		}

		public int Registrar(Usuario obj, out string Mensaje)
		{
			int idAutogenerado = 0;
			Mensaje = string.Empty;
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					SqlCommand cmd = new SqlCommand("SP_REGISTRAR_USUARIO", conexion);
					cmd.Parameters.AddWithValue("NOMBRES", obj.NOMBRES);
					cmd.Parameters.AddWithValue("APELLIDOS", obj.APELLIDOS);
					cmd.Parameters.AddWithValue("CORREO", obj.CORREO);
					cmd.Parameters.AddWithValue("CONTRASEÑA", obj.CONTRASEÑA);
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

		public bool Editar(Usuario obj, out string Mensaje)
		{
			bool resultado = false;
			Mensaje = string.Empty;
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					SqlCommand cmd = new SqlCommand("SP_EDITAR_USUARIO", conexion);
					cmd.Parameters.AddWithValue("IDUSUARIO", obj.ID_USUARIO);
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
					SqlCommand cmd = new SqlCommand("DELETE TOP (1) FROM USUARIO WHERE ID_USUARIO = @id", conexion);
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

		public bool CambiarClave(int idusuario,string nuevaclave, out string Mensaje)
		{
			bool resultado = false;
			Mensaje = string.Empty;
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					SqlCommand cmd = new SqlCommand("UPDATE USUARIO SET CONTRASEÑA = @nuevaclave, REESTABLECER = 0 WHERE ID_USUARIO = @id", conexion);
					cmd.Parameters.AddWithValue("@id", idusuario);
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

		public bool ReestablecerClave(int idusuario, string clave, out string Mensaje)
		{
			bool resultado = false;
			Mensaje = string.Empty;
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					SqlCommand cmd = new SqlCommand("UPDATE USUARIO SET CONTRASEÑA = @clave, REESTABLECER = 1 WHERE ID_USUARIO = @id", conexion);
					cmd.Parameters.AddWithValue("@id", idusuario);
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



	}
}
