using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
	public class Producto
	{
		public int ID_PRODUCTO{ get; set; }
		public string NOMBRE { get; set; }
		public string DESCRIPTION { get; set; }
		public Marca ID_MARCA { get; set; }
		public Categoria ID_CATEGORIA{ get; set; }
		public decimal PRECIO { get; set; }
		public string PRECIO_TEXTO { get; set; }
		public int STOCK { get; set; }
		public string RUTA_IMAGEN { get; set; }
		public string NOMBRE_IMAGEN { get; set; }
		public bool ACTIVO { get; set; }
		public decimal BASE64 { get; set; }
		public string EXTENSION { get;}
	}
}
