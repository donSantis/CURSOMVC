using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace CapaDatos
{
	public class CD_Producto
	{
		public List<Producto> Listar()
		{
			List<Producto> lista = new List<Producto>();
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					StringBuilder sb = new StringBuilder();
					sb.AppendLine("SELECT C.ID_COMUNA , C.DESCRIPCION[DesComuna],R.DESCRIPCION[DesRegion],C.ACTIVO ");
					sb.AppendLine("FROM COMUNA C");
					sb.AppendLine("INNER JOIN REGION R ON C.ID_REGION = R.ID_REGION");
					string query = "SELECT P.ID_PRODUCTO,P.NOMBRE,P.DESCRIPTION[DPRO],M.DESCRIPCION[DMAR],C.DESCRIPCION[DCAT],P.ID_MARCA,P.ID_CATEGORIA,P.PRECIO,P.STOCK,P.RUTA_IMAGEN,P.NOMBRE_IMAGEN,P.ACTIVO FROM PRODUCTO P INNER JOIN MARCA M ON P.ID_MARCA = M.ID_MARCA INNER JOIN CATEGORIA C ON P.ID_CATEGORIA = C.ID_CATEGORIA";
					
					SqlCommand cmd = new SqlCommand(query, conexion);
					cmd.CommandType = CommandType.Text;
					conexion.Open();
					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{

							lista.Add(
								new Producto()
								{
									ID_PRODUCTO = Convert.ToInt32(dr["ID_PRODUCTO"]),

									NOMBRE = dr["NOMBRE"].ToString(),
									DESCRIPTION = dr["DPRO"].ToString(),
									ID_MARCA = new Marca
									{
										ID_MARCA = Convert.ToInt32(dr["ID_MARCA"]),
										DESCRIPCION = dr["DMAR"].ToString()
									},
									ID_CATEGORIA = new Categoria
									{
										ID_CATEGORIA = Convert.ToInt32(dr["ID_CATEGORIA"]),
										DESCRIPCION = dr["DCAT"].ToString()
									},
									PRECIO = Convert.ToDecimal(dr["PRECIO"], new CultureInfo("es-CL")),
									STOCK = Convert.ToInt32(dr["STOCK"]),
									RUTA_IMAGEN = dr["RUTA_IMAGEN"].ToString(),
									NOMBRE_IMAGEN = dr["NOMBRE_IMAGEN"].ToString(),
									ACTIVO = Convert.ToBoolean(dr["ACTIVO"]),
								});
						}

					}
				}
			}
			catch
			{
				lista = new List<Producto>();
			}
			return lista;
		}



		public int Registrar(Producto obj, out string Mensaje)
		{
			int idAutogenerado = 0;
			Mensaje = string.Empty;
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					SqlCommand cmd = new SqlCommand("SP_REGISTRAR_PRODUCTO", conexion);
					cmd.Parameters.AddWithValue("NOMBRE", obj.NOMBRE);
					cmd.Parameters.AddWithValue("DESCRIPTION", obj.DESCRIPTION);
					cmd.Parameters.AddWithValue("ID_MARCA", obj.ID_MARCA.ID_MARCA);
					cmd.Parameters.AddWithValue("ID_CATEGORIA", obj.ID_CATEGORIA.ID_CATEGORIA);
					cmd.Parameters.AddWithValue("PRECIO", obj.PRECIO);
					cmd.Parameters.AddWithValue("STOCK", obj.STOCK);
					cmd.Parameters.AddWithValue("ACTIVO", obj.ACTIVO);
					cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
					cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
					cmd.CommandType = CommandType.StoredProcedure;

					conexion.Open();

					cmd.ExecuteNonQuery();
					idAutogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
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

		public bool Editar(Producto obj, out string Mensaje)
		{
			bool resultado = false;
			Mensaje = string.Empty;
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					SqlCommand cmd = new SqlCommand("SP_EDITAR_PRODUCTO", conexion);
					cmd.Parameters.AddWithValue("IDPRODUCTO", obj.ID_PRODUCTO);
					cmd.Parameters.AddWithValue("NOMBRE", obj.NOMBRE);
					cmd.Parameters.AddWithValue("DESCRIPTION", obj.DESCRIPTION);
					cmd.Parameters.AddWithValue("ID_" +
						"MARCA", obj.ID_MARCA.ID_MARCA);
					cmd.Parameters.AddWithValue("ID_" +
						"CATEGORIA", obj.ID_CATEGORIA.ID_CATEGORIA);
					cmd.Parameters.AddWithValue("PRECIO", obj.PRECIO);
					cmd.Parameters.AddWithValue("STOCK", obj.STOCK);
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

		public bool GuardarDatosImagen(Producto obj, out string Mensaje)
		{
			bool resultado = false;
			Mensaje = string.Empty;
			try
			{
				using (SqlConnection conexion = new SqlConnection(Conexion.cn))
				{
					string query = "UPDATE PRODUCTO SET RUTA_IMAGEN = @RUTA_IMAGEN, NOMBRE_IMAGEN = @NOMBRE_IMAGEN WHERE ID_PRODUCTO = @IDPRODUCTO";
					SqlCommand cmd = new SqlCommand(query, conexion);
					cmd.Parameters.AddWithValue("@RUTA_IMAGEN", obj.RUTA_IMAGEN);
					cmd.Parameters.AddWithValue("@NOMBRE_IMAGEN", obj.NOMBRE_IMAGEN);
					cmd.Parameters.AddWithValue("@IDPRODUCTO", obj.ID_PRODUCTO);
					cmd.CommandType = CommandType.Text;
					conexion.Open();
					if (cmd.ExecuteNonQuery() > 0)
					{
						resultado = true;
					}
					else
					{
						Mensaje = "No se pudo actualizar la imagen";
					}
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
					SqlCommand cmd = new SqlCommand("SP_ELIMINAR_PRODUCTO", conexion);
					cmd.Parameters.AddWithValue("ID_PRODUCTO", id);
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
