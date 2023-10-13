using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
	public class CN_Region
	{
		private CD_Region objCapaDato = new CD_Region();
		public List<Region> Listar()
		{
			return objCapaDato.Listar();
		}
		public int Registrar(Region obj, out string Mensaje)
		{
			Mensaje = string.Empty;
			if (string.IsNullOrEmpty(obj.DESCRIPCION) || string.IsNullOrWhiteSpace(obj.DESCRIPCION))
			{
				Mensaje = "El nombre no puede estar vacio";
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

		public bool Editar(Region obj, out string Mensaje)
		{
			Mensaje = string.Empty;
			if (string.IsNullOrEmpty(obj.DESCRIPCION) || string.IsNullOrWhiteSpace(obj.DESCRIPCION))
			{
				Mensaje = "El nombre no puede estar vacio";
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
	}
}
