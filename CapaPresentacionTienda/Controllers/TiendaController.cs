using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
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
		public JsonResult ListarProducto(int idCategoria, int idMarca)
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
				BASE64 = CN_Recursos.ConvertirBase64(Path.Combine(p.RUTA_IMAGEN, p.NOMBRE_IMAGEN), out conversion),
				EXTENSION = Path.GetExtension(p.NOMBRE_IMAGEN),
				ACTIVO = p.ACTIVO,
			}).Where(p =>
				p.ID_CATEGORIA.ID_CATEGORIA == (idCategoria == 0 ? p.ID_CATEGORIA.p.ID_CATEGORIA : idCategoria) &&
				p.ID_MARCA.ID_MARCA == (idMarca == 0 ? p.ID_MARCA.ID_MARCA : idMarca) &&
				p.STOCK > 0 && p.ACTIVO == true).ToList();

				;
			return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
		}
	}
}