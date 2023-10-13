using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
	public class CN_Producto
	{
		private CD_Producto objCapaDato = new CD_Producto();
		public List<Producto> Listar()
		{
			return objCapaDato.Listar();
		}
		public int Registrar(Producto obj, out string Mensaje)
		{
			Mensaje = string.Empty;
			if (string.IsNullOrEmpty(obj.NOMBRE) || string.IsNullOrWhiteSpace(obj.NOMBRE))
			{
				Mensaje = "El nombre no puede estar vacio";
			}
			if (string.IsNullOrEmpty(obj.DESCRIPTION) || string.IsNullOrWhiteSpace(obj.DESCRIPTION))
			{
				Mensaje = "El DESCRIPTION no puede estar vacio";
			}
			if (obj.PRECIO == 0)
			{
				Mensaje = "El precio no puede estar vacio";
			}
			if (obj.STOCK == 0)
			{
				Mensaje = "El stock no puede estar vacio";
			}

			if (string.IsNullOrEmpty(Mensaje))
			{
				return objCapaDato.Registrar(obj, out Mensaje);
			}
			else
			{
				return 0;
			}
		}

		public bool Editar(Producto obj, out string Mensaje)
		{
			Mensaje = string.Empty;
			if (string.IsNullOrEmpty(obj.NOMBRE) || string.IsNullOrWhiteSpace(obj.NOMBRE))
			{
				Mensaje = "El nombre no puede estar vacio";
			}
			if (string.IsNullOrEmpty(obj.DESCRIPTION) || string.IsNullOrWhiteSpace(obj.DESCRIPTION))
			{
				Mensaje = "El DESCRIPTION no puede estar vacio";
			}
			if (obj.PRECIO == 0)
			{
				Mensaje = "El precio no puede estar vacio";
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

		public bool GuardarDatosImagen(Producto obj, out string Mensaje)
		{
			return objCapaDato.GuardarDatosImagen(obj, out Mensaje);

		}
		public bool Eliminar(int id, out string Mensaje)
		{
			return objCapaDato.Eliminar(id, out Mensaje);

		}
	}
}
