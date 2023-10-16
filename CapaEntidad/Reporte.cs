using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
	public class Reporte
	{
		public string FECHA_VENTA { get; set; }
		public string CLIENTE { get; set; }
		public string PRODUCTO { get; set; }
		public decimal PRECIO { get; set; }
		public int CANTIDAD { get; set; }
		public decimal TOTAL { get; set; }
		public string ID_TRANSACCION { get; set; }
	}
}
