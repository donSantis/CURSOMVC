using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
	public class Venta
	{
		public int ID_VENTA { get; set; }
		public int ID_CLIENTE { get; set; }
		public int TOTAL_PRODUCTO{ get; set; }
		public decimal MONTO_TOTAL { get; set; }
		public string CONTACTO { get; set; }
		public int ID_COMUNA { get; set; }
		public string DIRECCION { get; set; }
		public string TELEFONO { get; set; }
		public string ID_TRANSACCION { get; set; }
		public string FECHA_VENTA { get; set; }
		public List<DetalleVenta> DETALLE_VENTA{ get; set; }
	}
}
