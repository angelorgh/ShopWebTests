using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopAdminWeb.Utilities;
using ShopAdminWeb.Models;
using System.Linq.Dynamic.Core;

namespace ShopAdminWeb.Controllers
{
    public class ConsultarController : Controller
    {

        private readonly IDBConnection _dBConnection;

        public ConsultarController(IDBConnection dBConnection)
        {
            _dBConnection = dBConnection;
        }

        public IActionResult ConsultarProductos()
        {
            return View();
        }

        public IActionResult EditarProducto(int? productoid)
        {
            if (productoid == null)
            {
                return RedirectToAction("ConsultarProductos");
            }
            int id = Convert.ToInt32(productoid);
            Producto productoToEdit = _dBConnection.GetProductByID(id);
            List<Marca> marcas = _dBConnection.GetAllMarcas();
            List<Categoria> categorias = _dBConnection.GetAllCategorias();
            List<EstadoProducto> estadosProductos = _dBConnection.GetAllEstadoProducto();

            ViewBag.marcas = marcas;
            ViewBag.categorias = categorias;
            ViewBag.estadosProductos = estadosProductos;

            return View(productoToEdit);
        }

        public IActionResult EditarMarca(int? marcaid)
        {
            if (marcaid == null)
            {
                return RedirectToAction("ConsultarMarcas");
            }
            int id = Convert.ToInt32(marcaid);
            Marca marcaToEdit = DBConnection.GetMarcaByID(id);

            return View(marcaToEdit);
        }

        public IActionResult EditarCategoria(int? categoriaid)
        {
            if (categoriaid == null)
            {
                return RedirectToAction("ConsultarCategorias");
            }
            int id = Convert.ToInt32(categoriaid);
            Categoria categoriaToEdit = DBConnection.GetCategoriaByID(id);

            return View(categoriaToEdit);
        }

        public IActionResult InsertarProducto() 
        {
            List<Marca> marcas = _dBConnection.GetAllMarcas();
            List<Categoria> categorias = _dBConnection.GetAllCategorias();
            List<EstadoProducto> estadosProductos = _dBConnection.GetAllEstadoProducto();

            ViewBag.marcas = marcas;
            ViewBag.categorias = categorias;
            ViewBag.estadosProductos = estadosProductos;

            return View();
        }

        public IActionResult InsertarMarca()
        {
            return View();
        }

        public IActionResult InsertarCategoria()
        {
            return View();
        }

        public IActionResult ConsultarMarcas() 
        {
            return View();
        }

        public IActionResult ConsultarCategorias()
        {
            return View();
        }

        public ActionResult GetProductos()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var productData = _dBConnection.GetProductos().ToList();
                // if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                // {
                //     //productData = productData.OrderBy(sortColumn + " " + sortColumnDirection);
                // }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    productData = productData.Where(m => m.Nombre.Contains(searchValue)
                                                || m.Marca.Nombre.Contains(searchValue)
                                                || m.Categoria.Nombre.Contains(searchValue)
                                                || m.Descripcion.Contains(searchValue)).ToList();
                }
                recordsTotal = productData.Count();
                var data = productData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetMarcas()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var marcasData = _dBConnection.GetAllMarcas().ToList();
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //productData = productData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    marcasData = marcasData.Where(m => m.Nombre.Contains(searchValue)).ToList();
                }
                recordsTotal = marcasData.Count();
                var data = marcasData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetCategorias()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var categoriasData = _dBConnection.GetAllCategorias().ToList();
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //productData = productData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    categoriasData = categoriasData.Where(m => m.Nombre.Contains(searchValue)).ToList();
                }
                recordsTotal = categoriasData.Count();
                var data = categoriasData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult DeleteProducto (int? ID)
        {
            try
            {
                if(ID == null)
                {
                    return Json(false);
                }

                bool result = DBConnection.DeleteProducto(Convert.ToInt32(ID));

                if (result)
                {
                    return Json(true);
                }
                return Json(false);
            }
            catch (Exception e)
            {
                Exception crash = e;
                return Json(false);
            }            
        }

        [HttpPost]
        public JsonResult EditarProducto(int productoid, int marcaid, int categoriaid, int estadoid, string nombre, string descripcion, double precio)
        {
            try
            {
                bool result = _dBConnection.UpdateProducto(productoid, marcaid, categoriaid, estadoid, nombre, descripcion, precio);
                if (result)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public JsonResult InsertarProducto(int marcaid, int categoriaid, int estadoid, string nombre, string descripcion, double precio)
        {
            try
            {
                bool result = DBConnection.InsertProducto(marcaid,categoriaid,estadoid,nombre,descripcion,precio);
                if (result)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public JsonResult InsertarMarca(string nombre)
        {
            try
            {
                bool result = DBConnection.InsertMarca(nombre);
                if (result)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public JsonResult InsertarCategoria(string nombre)
        {
            try
            {
                bool result = DBConnection.InsertCategoria(nombre);
                if (result)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public JsonResult EditarMarca(int marcaid, string nombre)
        {
            try
            {
                bool result = DBConnection.UpdateMarca(marcaid, nombre);
                if (result)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public JsonResult EditarCategoria(int categoriaid, string nombre)
        {
            try
            {
                bool result = DBConnection.UpdateCategoria(categoriaid, nombre);
                if (result)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
    }
}
