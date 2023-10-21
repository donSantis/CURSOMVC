using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
	[Authorize]
	public class MantenedorController : Controller
    {
		// GET: Mantenedor
		public ActionResult Categoria()
		{
			return View();
		}
		public ActionResult Marca()
		{
			return View();
		}
		public ActionResult Producto()
		{
			return View();
		}
		public ActionResult Region()
		{
			return View();
		}
		public ActionResult Comuna()
		{
			return View();
		}
		//CATEGORIAS
		[HttpGet]
		public JsonResult ListarCategorias()
		{
			List<Categoria> lista = new List<Categoria>();
			lista = new CN_Categoria().Listar();
			return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult GuardarCategoria(Categoria objeto)
		{
			object resultado;
			string mensaje = string.Empty;
			if (objeto.ID_CATEGORIA == 0)
			{
				resultado = new CN_Categoria().Registrar(objeto, out mensaje);
			}
			else
			{
				resultado = new CN_Categoria().Editar(objeto, out mensaje);
			}
			return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public JsonResult EliminarCategoria(int id)
		{
			bool respuesta = false;
			string mensaje = string.Empty;
			respuesta = new CN_Categoria().Eliminar(id, out mensaje);
			return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		public JsonResult ListarMarcas()
		{
			List<Marca> lista = new List<Marca>();
			lista = new CN_Marca().Listar();
			return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult GuardarMarca(Marca objeto)
		{
			object resultado;
			string mensaje = string.Empty;
			if (objeto.ID_MARCA == 0)
			{
				resultado = new CN_Marca().Registrar(objeto, out mensaje);
			}
			else
			{
				resultado = new CN_Marca().Editar(objeto, out mensaje);
			}
			return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public JsonResult EliminarMarca(int id)
		{
			bool respuesta = false;
			string mensaje = string.Empty;
			respuesta = new CN_Marca().Eliminar(id, out mensaje);
			return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
		}
		//REGION

		[HttpGet]
		public JsonResult ListarRegiones()
		{
			List<Region> lista = new List<Region>();
			lista = new CN_Region().Listar();
			return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult GuardarRegion(Region objeto)
		{
			object resultado;
			string mensaje = string.Empty;
			if (objeto.ID_REGION == 0)
			{
				resultado = new CN_Region().Registrar(objeto, out mensaje);
			}
			else
			{
				resultado = new CN_Region().Editar(objeto, out mensaje);
			}
			return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public JsonResult EliminarRegion(int id)
		{
			bool respuesta = false;
			string mensaje = string.Empty;
			respuesta = new CN_Region().Eliminar(id, out mensaje);
			return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
		}

		//COMUNA

		[HttpGet]
		public JsonResult ListarComunas()
		{
			List<Comuna> lista = new List<Comuna>();
			lista = new CN_Comuna().Listar();
			return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult GuardarComuna(Comuna objeto)
		{
			object resultado;
			string mensaje = string.Empty;
			if (objeto.ID_COMUNA == 0)
			{
				resultado = new CN_Comuna().Registrar(objeto, out mensaje);
			}
			else
			{
				resultado = new CN_Comuna().Editar(objeto, out mensaje);
			}
			return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public JsonResult EliminarComuna(int id)
		{
			bool respuesta = false;
			string mensaje = string.Empty;
			respuesta = new CN_Comuna().Eliminar(id, out mensaje);
			return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
		}


		//PRODUCTO

		[HttpGet]
		public JsonResult ListarProductos()
		{
			List<Producto> lista = new List<Producto>();
			lista = new CN_Producto().Listar();
			return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
		{
			string mensaje = string.Empty;
			bool operacion_exitosa = true;
			bool guardar_imagen_exito = true;

			Producto producto = new Producto();
			producto = JsonConvert.DeserializeObject<Producto>(objeto);

			decimal precio;
			if(decimal.TryParse(producto.PRECIO_TEXTO, NumberStyles.AllowDecimalPoint, new CultureInfo("es-PE"), out precio))
			{
				producto.PRECIO = precio;
			}
			else
			{
				return Json(new { operacion_exitosa = false, mensaje = "El formato del precio debe ser ##.##" }, JsonRequestBehavior.AllowGet);	
			}


			if (producto.ID_PRODUCTO == 0)
			{
				int idProductoGenerado = new CN_Producto().Registrar(producto, out mensaje);
				if (idProductoGenerado != 0)
				{
					producto.ID_PRODUCTO = idProductoGenerado;
				}
				else
				{
					operacion_exitosa = false;
				}
			}
			else
			{
				operacion_exitosa = new CN_Producto().Editar(producto, out mensaje);
			}
			if (operacion_exitosa)
			{
				if(archivoImagen != null)
				{
					string ruta_guardar = ConfigurationManager.AppSettings["ServidorFotos"];
					string extension = Path.GetExtension(archivoImagen.FileName);
					string nombre_imagen = string.Concat(producto.ID_PRODUCTO.ToString(),extension);
					try
					{
						archivoImagen.SaveAs(Path.Combine(ruta_guardar,nombre_imagen));
					}
					catch (Exception ex) { 
						string msg = ex.Message;
						guardar_imagen_exito = false;
					}
					if (guardar_imagen_exito)
					{
						producto.RUTA_IMAGEN = ruta_guardar;
						producto.NOMBRE_IMAGEN = nombre_imagen;
						bool rspta = new CN_Producto().GuardarDatosImagen(producto, out mensaje);
					}
					else
					{
						mensaje = "Se guardo el producto pero no la imagen";
					}
				}
			}
			return Json(new { operacion_exitosa = operacion_exitosa, mensaje = mensaje, id_generado = producto.ID_PRODUCTO }, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public JsonResult EliminarProducto(int id)
		{
			bool respuesta = false;
			string mensaje = string.Empty;
			respuesta = new CN_Producto().Eliminar(id, out mensaje);
			return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public JsonResult ImagenProducto(int id)
		{
			bool conversion;
			Producto producto = new CN_Producto().Listar().Where(p => p.ID_PRODUCTO == id).FirstOrDefault();
			string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(producto.RUTA_IMAGEN, producto.NOMBRE_IMAGEN), out conversion);
			return Json(new {	conversion = conversion, textoBase64 = textoBase64, extension = Path.GetExtension(producto.NOMBRE_IMAGEN)}, JsonRequestBehavior.AllowGet);

		}


	}
}