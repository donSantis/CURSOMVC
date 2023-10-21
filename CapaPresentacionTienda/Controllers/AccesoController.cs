using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
		public ActionResult Registrar()
		{
			return View();
		}
		public ActionResult Reestablecer()
		{
			return View();
		}
		public ActionResult CambiarClave()
		{
			return View();
		}
		[HttpPost]
		public ActionResult Registrar(Cliente objeto)
		{
			int resultado;
			string mensaje = string.Empty;

			ViewData["NOMBRES"] = string.IsNullOrEmpty(objeto.NOMBRES) ? "" : objeto.NOMBRES;
			ViewData["APELLIDOS"] = string.IsNullOrEmpty(objeto.APELLIDOS) ? "" : objeto.APELLIDOS;
			ViewData["CORREO"] = string.IsNullOrEmpty(objeto.CORREO) ? "" : objeto.CORREO;

			if (objeto.CONTRASEÑA != objeto.CONFIRMAR_CLAVE)
			{
				ViewBag.Error = "Las contraseñas no coinciden";
				return View();
			}
			objeto.REESTABLECER = true;
			objeto.ACTIVO = true;
			resultado = new CN_Cliente().Registrar(objeto, out mensaje);

			if (resultado > 0)
			{
				ViewBag.Error = null;
				return RedirectToAction("Index","Acceso");
			}
			else
			{
				ViewBag.Error = mensaje;
				return View();

			}

		}
		[HttpPost]
		public ActionResult Index(string correo, string clave)
		{
			Cliente cliente = null;
			cliente = new CN_Cliente().Listar().Where(item => item.CORREO == correo && item.CONTRASEÑA == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();
			if (cliente == null) {
				ViewBag.Error = "Correo o contraseña incorrectos";
				return View();

			}
			else
			{
				if (cliente.REESTABLECER )
				{
					TempData["ID_CLIENTE"] = cliente.ID_CLIENTE;
					return RedirectToAction("CambiarClave", "Acceso");
				}
				else
				{
					FormsAuthentication.SetAuthCookie(cliente.CORREO, false);
					Session["Cliente"] = cliente;
					ViewBag.Error = null;
					return RedirectToAction("Index", "Tienda");


				}
			}
		}
		[HttpPost]
		public ActionResult Reestablecer(string correo)
		{
			Cliente cliente = new Cliente();
			cliente = new CN_Cliente().Listar().Where(u => u.CORREO == correo).FirstOrDefault();

			if (cliente == null)
			{
				ViewBag.Error = "No se encontro cliente relacionado a esté correo";
				return View();
			}
			string mensaje = string.Empty;
			bool respuesta = new CN_Cliente().ReestablecerClave(cliente.ID_CLIENTE, correo, out mensaje);
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

		[HttpPost]
		public ActionResult CambiarClave(string idcliente, string claveactual, string nuevaclave, string confirmarclave)
		{
			Cliente cliente = new Cliente();
			cliente = new CN_Cliente().Listar().Where(u => u.ID_CLIENTE == int.Parse(idcliente)).FirstOrDefault();
			if (cliente.CONTRASEÑA != CN_Recursos.ConvertirSha256(claveactual))
			{
				TempData["ID_CLIENTE"] = idcliente;
				ViewData["vclve"] = "";
				ViewBag.Error = "contraseña incorrecta";
				return View();
			}
			else if (nuevaclave != confirmarclave)
			{
				TempData["ID_CLIENTE"] = idcliente;
				ViewData["vclve"] = "";
				ViewBag.Error = "contraseña no coiciden";
				return View();
			}
			ViewData["vclave"] = "";
			nuevaclave = CN_Recursos.ConvertirSha256(nuevaclave);
			string mensaje = string.Empty;
			bool respuesta = new CN_Cliente().CambiarClave(int.Parse(idcliente), nuevaclave, out mensaje);
			if (respuesta)
			{
				return RedirectToAction("Index");
			}
			else
			{
				TempData["ID_CLIENTE"] = idcliente;
				ViewBag.Error = mensaje;
				return View();
			}
		}

		public ActionResult CerrarSesion()
		{
			FormsAuthentication.SignOut();
			return RedirectToAction("Index", "Acceso");
		}

	}
}