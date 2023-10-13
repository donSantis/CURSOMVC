using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionAdmin.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult Usuarios()
		{
			return View();
		}
		[HttpGet]
		public JsonResult ListarUsuarios()
		{
			List<Usuario> lista = new List<Usuario>();
			lista = new CN_Usuarios().Listar();
			return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public JsonResult GuardarUsuario(Usuario objeto)
		{
			object resultado;
			string mensaje = string.Empty;
			if (objeto.ID_USUARIO == 0)
			{
				resultado = new CN_Usuarios().Registrar(objeto, out mensaje);
			}
			else
			{
				resultado = new CN_Usuarios().Editar(objeto, out mensaje);
			}
			return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public JsonResult EliminarUsuario(int id)
		{
			bool respuesta = false;
			string mensaje = string.Empty;
			respuesta = new CN_Usuarios().Eliminar(id, out mensaje);
			return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
		}

	}
}