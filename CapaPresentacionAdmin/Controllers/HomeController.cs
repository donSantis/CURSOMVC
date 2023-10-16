﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using CapaEntidad;
using CapaNegocio;
using ClosedXML.Excel;

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

		[HttpGet]
		public JsonResult ListaReporte(string fechainicio, string fechafin,string idtransaccion)
		{
			List<Reporte> lista = new List<Reporte>();

			lista = new CN_Reporte().Ventas(fechainicio,fechafin,idtransaccion);
			return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult VistaDashboard()
		{
			Dashboard objeto = new CN_Reporte().VerDashboard();
			return Json(new { resultado = objeto }, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public FileResult ExportarVenta(string fechainicio, string fechafin, string idtransaccion)
		{
			List<Reporte> lista = new List<Reporte>();
			if (idtransaccion == null)
			{
				idtransaccion = " ";
			}
			lista = new CN_Reporte().Ventas(fechainicio, fechafin, idtransaccion);
			DataTable dt = new DataTable();
			dt.Locale = new System.Globalization.CultureInfo("es-PE");
			dt.Columns.Add("Fecha Venta", typeof(string));
			dt.Columns.Add("Cliente", typeof(string));
			dt.Columns.Add("Producto", typeof(string));
			dt.Columns.Add("Precio", typeof(decimal));
			dt.Columns.Add("Cantidad", typeof(int));
			dt.Columns.Add("Total", typeof(decimal));
			dt.Columns.Add("ID Transaccion", typeof(string));
			

			foreach(Reporte rp in lista)
			{
				dt.Rows.Add(new object[]
				{
					rp.FECHA_VENTA,
					rp.CLIENTE,
					rp.PRODUCTO, 
					rp.PRECIO,
					rp.CANTIDAD,
					rp.TOTAL,
					rp.ID_TRANSACCION
				});
			}
			dt.TableName = "Datos";
			using (XLWorkbook wb = new XLWorkbook())
			{
				wb.Worksheets.Add(dt);
				using (MemoryStream stream = new MemoryStream())
				{
					wb.SaveAs(stream);
					return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVenta desde" + fechainicio +"al " + fechafin + ".xlsx");
				}
			}
		}

	}
}