using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
	public class Cliente
	{
		public int ID_CLIENTE { get; set; }
		public string  NOMBRES { get; set; }
		public string  APELLIDOS { get; set; }
		public string  CORREO { get; set; }
		public string  CONTRASEÑA { get; set; }
		public bool REESTABLECER { get; set; }
		public string CONFIRMAR_CLAVE { get; set; }
		public bool ACTIVO { get; set; }

	}
}
