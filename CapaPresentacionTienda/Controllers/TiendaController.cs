﻿using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DetalleProducto(int idproducto = 0)
        {
			Producto producto = new Producto();
			bool conversion;
			producto = new CN_Producto().Listar().Where(p => p.ID_PRODUCTO == idproducto).FirstOrDefault();
			if(producto != null)
			{
				producto.BASE64 = CN_Recursos.ConvertirBase64(Path.Combine(producto.RUTA_IMAGEN, producto.NOMBRE_IMAGEN), out conversion);
				producto.EXTENSION = Path.GetExtension(producto.NOMBRE_IMAGEN);

			}

			return View(producto);
        }
        [HttpGet]
		public JsonResult ListarCategorias()
		{
			List<Categoria> lista = new List<Categoria>();
			lista = new CN_Categoria().Listar();
			return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public JsonResult ListarMarcaPorCategorias(int idCategoria)
		{
			List<Marca> lista = new List<Marca>();
			lista = new CN_Marca().ListarPorCategoria(idCategoria);
			return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public JsonResult ListarProducto(int idcategoria, int idmarca)
		{
			List<Producto> lista = new List<Producto>();
			bool conversion;
			lista = new CN_Producto().Listar().Select(p => new Producto()
			{
				ID_PRODUCTO = p.ID_PRODUCTO,
				NOMBRE = p.NOMBRE,
				DESCRIPTION = p.DESCRIPTION,
				ID_MARCA = p.ID_MARCA,
				ID_CATEGORIA = p.ID_CATEGORIA,
				PRECIO = p.PRECIO,
				STOCK = p.STOCK,
				RUTA_IMAGEN = p.RUTA_IMAGEN,
				NOMBRE_IMAGEN = p.NOMBRE_IMAGEN,
				EXTENSION = Path.GetExtension(p.NOMBRE_IMAGEN),
				BASE64 = CN_Recursos.ConvertirBase64(Path.Combine(p.RUTA_IMAGEN,p.NOMBRE_IMAGEN), out conversion),
				ACTIVO = p.ACTIVO,
			}).Where(p =>
				p.ID_CATEGORIA.ID_CATEGORIA == (idcategoria == 0 ? p.ID_CATEGORIA.ID_CATEGORIA : idcategoria) &&
				p.ID_MARCA.ID_MARCA == (idmarca == 0 ? p.ID_MARCA.ID_MARCA : idmarca) &&
				p.STOCK > 0 && p.ACTIVO == true)
			.ToList();

			var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
			jsonresult.MaxJsonLength = int.MaxValue;
			return jsonresult;
		}

        [HttpPost]
        public JsonResult AgregarCarrito(int idproducto)
        {
			int idcliente = ((Cliente)Session["Cliente"]).ID_CLIENTE;
			bool existe = new CN_Carrito().ExisteCarrito(idcliente, idproducto);
			bool respuesta = false;
			string mensaje = string.Empty;

			if (existe)
			{
				mensaje = "El producto ya existe en el carrito";
			}
			else
			{
				respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, true, out mensaje);
			}
			return Json(new { respuesta = respuesta, mensaje = mensaje, }, JsonRequestBehavior.AllowGet);
        }
		[HttpGet]
        public JsonResult CantidadCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).ID_CLIENTE;
            int cantidad = new CN_Carrito().CantidadCarrito(idcliente);
            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }
}
}
