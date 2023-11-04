using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using CapaEntidad.Paypal;

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
            if (producto != null)
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
                BASE64 = CN_Recursos.ConvertirBase64(Path.Combine(p.RUTA_IMAGEN, p.NOMBRE_IMAGEN), out conversion),
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
        [HttpPost]
        public JsonResult ListarProductosCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).ID_CLIENTE;
            List<Carrito> lista = new List<Carrito>();
            bool conversion;
            lista = new CN_Carrito().ListarProductosCarrito(idcliente).Select(oc => new Carrito()
            {
                ID_PRODUCTO = new Producto()
                {
                    ID_PRODUCTO = oc.ID_PRODUCTO.ID_PRODUCTO,
                    NOMBRE = oc.ID_PRODUCTO.NOMBRE,
                    ID_MARCA = oc.ID_PRODUCTO.ID_MARCA,
                    PRECIO = oc.ID_PRODUCTO.PRECIO,
                    RUTA_IMAGEN = oc.ID_PRODUCTO.RUTA_IMAGEN,
                    BASE64 = CN_Recursos.ConvertirBase64(Path.Combine(oc.ID_PRODUCTO.RUTA_IMAGEN, oc.ID_PRODUCTO.NOMBRE_IMAGEN), out conversion),
                    EXTENSION = Path.GetExtension(oc.ID_PRODUCTO.NOMBRE_IMAGEN),
                    NOMBRE_IMAGEN = oc.ID_PRODUCTO.NOMBRE_IMAGEN

                },
                CANTIDAD = oc.CANTIDAD
            }).ToList();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult OperacionCarrito(int idproducto, bool sumar)
        {
            int idcliente = ((Cliente)Session["Cliente"]).ID_CLIENTE;
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, true, out mensaje);
            return Json(new { respuesta = respuesta, mensaje = mensaje, }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarCarrito(int idproducto)
        {
            int idcliente = ((Cliente)Session["Cliente"]).ID_CLIENTE;
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Carrito().EliminarCarrito(idcliente, idproducto);
            return Json(new { respuesta = respuesta, mensaje = mensaje, }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerRegion()
        {
            List<Region> lista = new List<Region>();
            lista = new CN_Ubicacion().ObtenerRegion();
            return Json(new { lista = lista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ObtenerComuna(int idcomuna)
        {
            List<Comuna> lista = new List<Comuna>();
            lista = new CN_Ubicacion().ObtenerComuna(idcomuna);
            return Json(new { lista = lista }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Carrito()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> ProcesarPago(List<Carrito> lista_carrito, Venta venta)
        {

            decimal total = 0;
            DataTable detalle_venta = new DataTable();
            detalle_venta.Locale = new CultureInfo("es-PE");
            detalle_venta.Columns.Add("IDPRODUCTO",typeof(string));
            detalle_venta.Columns.Add("CANTIDAD",typeof(int));
            detalle_venta.Columns.Add("TOTAL",typeof(decimal));
            List<Item> listaItem = new List<Item>();    
            foreach (Carrito carrito in lista_carrito)
            {
                decimal subtotal = Convert.ToDecimal(carrito.CANTIDAD.ToString()) * carrito.ID_PRODUCTO.PRECIO;
                total += subtotal;
                listaItem.Add(new Item() {
                    name = carrito.ID_PRODUCTO.NOMBRE,
                    quantity = carrito.CANTIDAD.ToString(),
                    unit_amount = new UnitAmount() {
                        currency_code = "USD",
                        value = carrito.ID_PRODUCTO.PRECIO.ToString("G", new CultureInfo("es-PE"))
                    }
                });
                detalle_venta.Rows.Add(new object[]
                {
                    carrito.ID_PRODUCTO.ID_PRODUCTO,
                    carrito.CANTIDAD,
                    subtotal
                });
            }
            PurchaseUnit purchaseUnit = new PurchaseUnit() {
                    amount = new Amount() {
                        currency_code = "USD",
                        value = total.ToString("G", new CultureInfo("es-PE")),
                        breakdown = new Breakdown()
                        {
                            item_total = new ItemTotal()
                            {
                                currency_code = "USD",
                                value = total.ToString("G", new CultureInfo("es-PE")),
                            }
                        }
                },
                description = "Compra de articulo de mi tienda",
                items = listaItem
            };

            Checkout_Order checkout_Order = new Checkout_Order()
            {
                intent = "CAPTURE",
                purchase_units = new List<PurchaseUnit>() { purchaseUnit },
                application_context = new ApplicationContext()
                {
                    brand_name = "MiTienda.com",
                    landing_page = "NO_PREFERENCE",
                    user_action = "PAY_NOW",
                    return_url = "https://localhost:44343/Tienda/PagoEfectuado",
                    cancel_url = "https://localhost:44343/Tienda/Carrito"
                }
            };
            venta.MONTO_TOTAL = total;
            venta.ID_CLIENTE = ((Cliente)Session["Cliente"]).ID_CLIENTE;
            TempData["VENTA"] = venta;
            TempData["DETALLEVENTA"] = detalle_venta;
            CN_Paypal paypal = new CN_Paypal();
            Response_Paypal<Response_Checkout> response_paypal = new Response_Paypal<Response_Checkout>();
            response_paypal = await paypal.CrearSolicitud(checkout_Order);
            return Json(new { response_paypal },JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> PagoEfectuado()
        {
            string token = Request.QueryString["token"];
            CN_Paypal paypal = new CN_Paypal();
            Response_Paypal<Response_Capture> response_paypal = new Response_Paypal<Response_Capture>();
            response_paypal = await paypal.AprobarPago(token);
            bool status = Convert.ToBoolean(Request.QueryString["status"]);
            ViewData["Status"] = response_paypal.Status;
            if(response_paypal.Status)
            {
                Venta venta = (Venta)TempData["Venta"];
                DataTable detalle_venta = (DataTable)TempData["DetalleVenta"];
                venta.ID_TRANSACCION = response_paypal.Response.purchase_units[0].payments.captures[0].id;
                string mensaje = string.Empty;
                bool respuesta = new CN_Venta().Registrar(venta, detalle_venta, out mensaje);
                ViewData["IdTransaccion"] = venta.ID_TRANSACCION;

            }
            return View();
        }

    }
}
