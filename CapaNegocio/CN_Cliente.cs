using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
	public class CN_Cliente
	{
		private CD_Cliente objCapaDato = new CD_Cliente();
		public List<Cliente> Listar()
		{
			return objCapaDato.Listar();
		}
		public int Registrar(Cliente obj, out string Mensaje)
		{
			Mensaje = string.Empty;
			if (string.IsNullOrEmpty(obj.NOMBRES) || string.IsNullOrWhiteSpace(obj.NOMBRES))
			{
				Mensaje = "El nombre no puede estar vacio";
			}
			else if (string.IsNullOrEmpty(obj.APELLIDOS) || string.IsNullOrWhiteSpace(obj.APELLIDOS))
			{
				Mensaje = "El apellidos no puede estar vacio";
			}
			else if (string.IsNullOrEmpty(obj.CORREO) || string.IsNullOrWhiteSpace(obj.CORREO))
			{
				Mensaje = "El correo no puede estar vacio";
			}
			if (string.IsNullOrEmpty(Mensaje))
			{
				string contraseña = CN_Recursos.GenerarClave();
				string asunto = "Creacion de nueva cuenta de Cliente";
				string mensaje_correo = "<h3> Su cuenta fue creada exitosamente </h3></br><p>Su contraseña es: !Clave!</p>";
				mensaje_correo = mensaje_correo.Replace("!Clave!", contraseña);

				bool respuesta = CN_Recursos.EviarCorreo(obj.CORREO, asunto, mensaje_correo);
				if (respuesta)
				{
					obj.CONTRASEÑA = CN_Recursos.ConvertirSha256(contraseña);
					return objCapaDato.Registrar(obj, out Mensaje);
				}
				else
				{
					Mensaje = "No se puede enviar el correo";
					return 0;
				}


			}
			else
			{
				return 0;
			}
		}

		public bool Editar(Cliente obj, out string Mensaje)
		{
			Mensaje = string.Empty;
			if (string.IsNullOrEmpty(obj.NOMBRES) || string.IsNullOrWhiteSpace(obj.NOMBRES))
			{
				Mensaje = "El nombre no puede estar vacio";
			}
			else if (string.IsNullOrEmpty(obj.APELLIDOS) || string.IsNullOrWhiteSpace(obj.APELLIDOS))
			{
				Mensaje = "El apellidos no puede estar vacio";
			}
			else if (string.IsNullOrEmpty(obj.CORREO) || string.IsNullOrWhiteSpace(obj.CORREO))
			{
				Mensaje = "El correo no puede estar vacio";
			}

			if (string.IsNullOrEmpty(Mensaje))
			{
				return objCapaDato.Editar(obj, out Mensaje);
			}
			else
			{
				return false;
			}
		}
		public bool Eliminar(int id, out string Mensaje)
		{
			return objCapaDato.Eliminar(id, out Mensaje);

		}

		public bool CambiarClave(int idcliente, string nuevaclave, out string Mensaje)
		{
			return objCapaDato.CambiarClave(idcliente, nuevaclave, out Mensaje);
		}
		public bool ReestablecerClave(int idcliente, string correo, out string Mensaje)
		{
			Mensaje = string.Empty;
			string nuevaclave = CN_Recursos.GenerarClave();
			bool resultado = objCapaDato.ReestablecerClave(idcliente, CN_Recursos.ConvertirSha256(nuevaclave), out Mensaje);
			if (resultado)
			{
				string asunto = "Contraseña reestablecida";
				string mensaje_correo = "<h3> Su cuenta fue reestablecida correctamente</h3></br><p>Su contraseña para acceder ahora es: !Clave!</p>";
				mensaje_correo = mensaje_correo.Replace("!Clave!", nuevaclave);
				bool respuesta = CN_Recursos.EviarCorreo(correo, asunto, mensaje_correo);

				if (respuesta)
				{
					return true;
				}
				else
				{
					Mensaje = "No se pudo enviar el correo";
					return false;
				}

			}
			else
			{
				Mensaje = "No se pudo reestablecer la cuenta";
				return false;
			}

		}

	}
}
