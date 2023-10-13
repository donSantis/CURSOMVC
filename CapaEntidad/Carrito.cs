using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
	public class Carrito
	{
		public int ID_CARRITO { get; set; }
		public Cliente ID_CLIENTE { get; set; }
		public Producto ID_PRODUCTO { get; set; }
		public int CANTIDAD { get; set; }
	}
}
