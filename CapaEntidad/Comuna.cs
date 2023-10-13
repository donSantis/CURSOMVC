using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
	public class Comuna
	{
		public int ID_COMUNA { get; set; }
		public string DESCRIPCION { get; set; }
		public Region ID_REGION { get; set; }
		public bool ACTIVO { get; set; }
	}
}
