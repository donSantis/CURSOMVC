﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Carrito
    {
        private CD_Carrito objCapaDato = new CD_Carrito();

        public bool ExisteCarrito(int idcliente, int idproducto)
        {
            return objCapaDato.ExisteCarrito(idcliente, idproducto);
        }
        public bool OperacionCarrito(int idcliente, int idproducto, bool sumar, out string Mensaje)
        {
            return objCapaDato.OperacionCarrito(idcliente, idproducto,sumar,out Mensaje);
        }
        public int CantidadCarrito(int idcliente)
        {
            return objCapaDato.CantidadCarrito(idcliente);
        }
        public bool EliminarCarrito(int idcliente, int idproducto)
        {
            return objCapaDato.EliminarCarrito(idcliente,idproducto);
        }
        public List<Carrito> ListarProductosCarrito(int idcliente)
        {
            return objCapaDato.ListarProductosCarrito(idcliente);
        }
    }
}
