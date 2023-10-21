using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentacionAdmin.Controllers
{
	public class AccesoController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Error = null;
			return View();
		}
		public ActionResult CambiarClave()
		{
			return View();
		}
		public ActionResult Reestablecer()
		{
			return View();
		}
		[HttpPost]
		public ActionResult Index(string correo, string clave)
		{
			Usuario usuario = new Usuario();
			usuario = new CN_Usuarios().Listar().Where(u => u.CORREO == correo && u.CONTRASEÑA == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();
			if (usuario == null)
			{
				ViewBag.Error = "Correo o contraseña incorrecta";
				return View();
			}
			else
			{
				if (usuario.REESTABLECER)
				{
					TempData["IDUSUARIO"] = usuario.ID_USUARIO;
					return RedirectToAction("CambiarClave");
				}
				FormsAuthentication.SetAuthCookie(usuario.CORREO, false);
				ViewBag.Error = null;
				return RedirectToAction("Index", "Home");
			}
		}

		[HttpPost]
		public ActionResult CambiarClave(string idusuario, string claveactual, string nuevaclave, string confirmarclave)
		{
			Usuario usuario = new Usuario();
			usuario = new CN_Usuarios().Listar().Where(u => u.ID_USUARIO == int.Parse(idusuario)).FirstOrDefault();
			if (usuario.CONTRASEÑA != CN_Recursos.ConvertirSha256(claveactual))
			{
				TempData["ID_USUARIO"] = idusuario;
				ViewData["vclve"] = "";
				ViewBag.Error = "contraseña incorrecta";
				return View();
			}
			else if (nuevaclave != confirmarclave)
			{
				TempData["ID_USUARIO"] = idusuario;
				ViewData["vclve"] = "";
				ViewBag.Error = "contraseña no coiciden";
				return View();
			}
			ViewData["vclave"] = "";
			nuevaclave = CN_Recursos.ConvertirSha256(nuevaclave);
			string mensaje = string.Empty;
			bool respuesta = new CN_Usuarios().CambiarClave(int.Parse(idusuario), nuevaclave, out mensaje);
			if (respuesta)
			{
				return RedirectToAction("Index");
			}
			else
			{
				TempData["ID_USUARIO"] = idusuario;
				ViewBag.Error = mensaje;
				return View();
			}
		}
		[HttpPost]
		public ActionResult Reestablecer(string correo)
		{
			Usuario usuario = new Usuario();
			usuario = new CN_Usuarios().Listar().Where(u => u.CORREO == correo).FirstOrDefault();

			if (usuario == null)
			{
				ViewBag.Error = "No se encontro usuario relacionado a esté correo";
				return View();
			}
			string mensaje = string.Empty;
			bool respuesta = new CN_Usuarios().ReestablecerClave(usuario.ID_USUARIO, correo, out mensaje);
			if (respuesta)
			{
				return RedirectToAction("Index", "Acceso");
			}
			else
			{
				ViewBag.Error = mensaje;
				return View();
			}
		}

		public ActionResult CerrarSesion( )
		{
			FormsAuthentication.SignOut();
			return RedirectToAction("Index", "Acceso");
		}
	}
}