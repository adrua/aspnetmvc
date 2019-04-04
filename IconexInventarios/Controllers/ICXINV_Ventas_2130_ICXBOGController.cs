using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Multiclick.Arkeos.ICXBOG.Models;
using Mvc.HtmlHelpers ;

namespace Multiclick.Arkeos.ICXBOG.Controllers
{
    public class ICXINV_Ventas_2130_ICXBOGController : Controller
    {
        private ArkeosDBContext db = new ArkeosDBContext();
        
        //
        // GET: /ICXINV_Ventas/

        public ViewResult Index() 
        {

            if((bool?)Session["ArkeosFactory.Testing.ICXINV_Ventas_2130_ICXBOG"] ?? false ) {
                try 
                {
                    //ViewBag.SqlUpdate = db.MigrationScript();
                    var dataLost = true; //(bool?)Session["ArkeosFactory.Testing.ICXINV_Ventas_2130_ICXBOG.dataLost"] ?? false;
                    db.Migration(dataLost);
                } 
                catch(Exception ex) 
                {
                    ViewBag.SqlUpdate = ex.Message;
                }
            }
            
            var routes =  System.Web.Routing.RouteTable.Routes;
            //routes.Remove(routes["ICXINVVentas2130ICXBOG"]);
            if(routes["ICXINVVentas2130ICXBOG"] == null)
            {
              routes.MapRoute(
                    name: "ICXINVVentas2130ICXBOG",
                    url: "ICXINV_Ventas_2130_ICXBOG/{Controller}/{action}/{id}",
                    defaults: new { MainController="ICXINV_Ventas_2130_ICXBOG", Controller = "ICXINV_Ventas_2130_ICXBOG", action = "Index", id = UrlParameter.Optional }
                );
            }

            return View();
        }

    	public JsonResult Search(GridSettings gridSettings)
    	{

            //Esta solo aplica para MS Sql Server 2005 o Superior
            if(db.Database.Connection.ToString() == "System.Data.SqlClient.SqlConnection"){
                db.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            
            var q = from ICXINV_Ventas in db.ICXINV_Ventas
                      join ICXINV_Productos in db.ICXINV_Productos.AsNoTracking()
                        on new { ICXINV_Ventas.Compania,  ICXINVProductoItemCode = ICXINV_Ventas.ICXINVProductoItemCode} 
                        equals new { ICXINV_Productos.Compania,  ICXINV_Productos.ICXINVProductoItemCode}
                        orderby ICXINV_Ventas.ICXINVVentaVentaID
                    select new { ICXINV_Ventas, ICXINV_Productos }
                      ;


			// create json data
            int pageIndex = gridSettings.pageIndex;
            int pageSize = gridSettings.pageSize;
            int totalRecords = q.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            int startRow = (pageIndex - 1) * pageSize;
            int endRow = startRow + pageSize;

            dynamic jsonData = null;

            Session["ICXINV_Ventas2130Settings"] = gridSettings;

        	var _qq = q.Skip(startRow).Take( gridSettings.pageSize).ToList();
            jsonData = new
	        {
                total = totalPages,
                page = pageIndex,
                records = totalRecords,
	            rows = (
	                from c in _qq
	                select new
	                {
                        id = HttpUtility.JavaScriptStringEncode(c.ICXINV_Ventas.Compania.ToString()   
                        										 + "_" + c.ICXINV_Ventas.ICXINVVentaVentaID.ToString("#")).Replace(" ", "__032__"),

                        cell = new string[]
                        {
    							      
                        	c.ICXINV_Ventas.Compania.ToString(),
	                    	c.ICXINV_Ventas.ICXINVVentaVentaID.ToString(),
	                    	c.ICXINV_Ventas.ICXINVProductoItemCode,
	                    	c.ICXINV_Productos.ICXINVProductoDescripcion.ToString(),
	                    	c.ICXINV_Ventas.ICXINVVentaCantidad.ToString(),
	                    	c.ICXINV_Ventas.ICXINVVentaPrecioUnitario.ToString(),
							(c.ICXINV_Ventas.ICXINVVentaPrecioTotal.HasValue)?c.ICXINV_Ventas.ICXINVVentaPrecioTotal.Value.ToString():"",
							(c.ICXINV_Productos.ICXINVProductoCosto.HasValue)?c.ICXINV_Productos.ICXINVProductoCosto.Value.ToString():"",
							(c.ICXINV_Ventas.Fecha_Impresion.HasValue)?c.ICXINV_Ventas.Fecha_Impresion.Value.ToString("yyyy-MM-dd hh:mm:ss"):"",
							(c.ICXINV_Ventas.Fecha_Reimpresion.HasValue)?c.ICXINV_Ventas.Fecha_Reimpresion.Value.ToString("yyyy-MM-dd hh:mm:ss"):"",
							c.ICXINV_Ventas.Fuente,
							c.ICXINV_Ventas.FuenteImport,
							(c.ICXINV_Ventas.Proceso.HasValue)?c.ICXINV_Ventas.Proceso.Value.ToString():"",
							(c.ICXINV_Ventas.Fecha_Computador.HasValue)?c.ICXINV_Ventas.Fecha_Computador.Value.ToString("yyyy-MM-dd hh:mm:ss"):"",
                			c.ICXINV_Ventas.Usuario,
							c.ICXINV_Ventas.Timestamp.ToString(),
							c.ICXINV_Ventas.Secuencia.ToString()
						}
	            }).ToArray()
	    	};
	        return Json(jsonData, JsonRequestBehavior.AllowGet);
  		}

		public ActionResult CRUD(GridSettings gridSettings)
        {

            bool result = false;

            switch (gridSettings.operation)
            {
                case OPERATION.add:
                    result = Add_ICXINV_Ventas();
                    break;
                case OPERATION.edit:
                    result = Update_ICXINV_Ventas(GetKeys(gridSettings.id));
                    break;
                case OPERATION.del:
                    result = Delete_ICXINV_Ventas(GetKeys(gridSettings.id));
                    break;
                default:
                    break;
            }
            return Json(result);
        }
        

        private bool Add_ICXINV_Ventas()
        {
        	var row = new ICXINV_Ventas();
        	
            row.Compania = 1;
            row.ICXINVVentaVentaID = ( db.ICXINV_Ventas.Max((p) => (int?)p.ICXINVVentaVentaID) ?? 0 ) + 1;

            row.ICXINVProductoItemCode = Request.Form["ICXINVProductoItemCode"];
            row.ICXINVVentaCantidad = int.Parse(Request.Form["ICXINVVentaCantidad"]);
            row.ICXINVVentaPrecioUnitario = decimal.Parse(Request.Form["ICXINVVentaPrecioUnitario"]);
            if(!String.IsNullOrEmpty(Request.Form["ICXINVVentaPrecioTotal"])){
                row.ICXINVVentaPrecioTotal = decimal.Parse(Request.Form["ICXINVVentaPrecioTotal"]);
              }
            if(!String.IsNullOrEmpty(Request.Form["ICXINVProductoCosto"])){
                row.ICXINVProductoCosto = decimal.Parse(Request.Form["ICXINVProductoCosto"]);
              }

            row.Fuente = "CP2130";
            row.Fecha_Computador = DateTime.Now;
            row.Usuario = HttpContext.User.Identity.Name;

            dynamic erow = GetOtherValues();

			db.ICXINV_Ventas.Add(row);
			
			if(IsValid(row, erow)){
    		    db.SaveChanges();
    		} else {
    		    var errMsg = "<br>" + string.Join("<br>", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
    		    throw new Exception(errMsg);
    		}
            return true;
        }

        private bool Update_ICXINV_Ventas(ICXINV_Ventas row)
        {
            row = db.Get_ICXINV_Ventas(row);					
            ICXINV_Ventas crow = null;

            if (row != null)
            {
                crow = (ICXINV_Ventas)row.Clone();    
				row.ICXINVProductoItemCode = Request.Form["ICXINVProductoItemCode"];
				row.ICXINVVentaCantidad = int.Parse(Request.Form["ICXINVVentaCantidad"]);
				row.ICXINVVentaPrecioUnitario = decimal.Parse(Request.Form["ICXINVVentaPrecioUnitario"]);

				if(string.IsNullOrEmpty(Request.Form["ICXINVVentaPrecioTotal"]) )
				{
					row.ICXINVVentaPrecioTotal = null;
				}
				else
				{ 
					row.ICXINVVentaPrecioTotal = decimal.Parse(Request.Form["ICXINVVentaPrecioTotal"]);
				}

				if(string.IsNullOrEmpty(Request.Form["ICXINVProductoCosto"]) )
				{
					row.ICXINVProductoCosto = null;
				}
				else
				{ 
					row.ICXINVProductoCosto = decimal.Parse(Request.Form["ICXINVProductoCosto"]);
				}

                row.Usuario = HttpContext.User.Identity.Name;
                row.Fecha_Computador = DateTime.Now;

                dynamic erow = GetOtherValues();
    			
    			if(IsValid(row, erow)){
        		    db.SaveChanges();
        		} else {
        		    var errMsg = "<br>" + string.Join("<br>", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
        		    throw new Exception(errMsg);
        		}
            }
            return true;
        }
        
        private bool Delete_ICXINV_Ventas(ICXINV_Ventas row)
        {

            row = db.Get_ICXINV_Ventas(row);

            dynamic erow = GetOtherValues();

            if (row != null)
            {
				db.ICXINV_Ventas.Remove(row);
      		    db.SaveChanges();
            }
            return true;
        }

        private ICXINV_Ventas GetKeys(string id)
        {

            id = id.Replace("__032__", " ") + "___________________";
            
            var _Keys = id.Split('_');
            
            var Keys = new ICXINV_Ventas() {    
                                    Compania = int.Parse(_Keys[0]),
                                    ICXINVVentaVentaID = int.Parse("0" + _Keys[1])
                        };
                        
            return Keys;                    
        }
        
      //Autocomplete
        //  GET: /ICXINV_Ventas/ICXINV_Productos
       [AllowAnonymous]
        public JsonResult ICXINV_Productos(string term, string column, int pageSize)
        {
            IQueryable< ICXINV_Productos> q = null;
            switch (column.ToUpper())
            {
                case "ICXINVPRODUCTODESCRIPCION":
                    //Esta solo aplica para MS Sql Server 2005 o Superior
                    if(db.Database.Connection.ToString() == "System.Data.SqlClient.SqlConnection"){
                        db.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                    }
                    
                    q = from c in db.ICXINV_Productos.AsNoTracking()
                        where c.ICXINVProductoDescripcion.Contains(term)
                        orderby c.ICXINVProductoDescripcion
                        select c;
                    break;
            }

            // create json data

            List< ICXINV_Productos> _qq = q.Take(pageSize).ToList();

            dynamic jsonData = (from c in _qq
                                select new { label = c.ICXINVProductoDescripcion,
                                             value = c.ICXINVProductoItemCode.ToString(),
					                         ICXINVProductoItemCode = c.ICXINVProductoItemCode,
					                         ICXINVProductoCosto = c.ICXINVProductoCosto
                                            }
                               ).ToArray()
            ;
            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }
        
        private string GetICXINV_Productos(int Compania, string ICXINVProductoItemCode, string ICXINVProductoDescripcion, decimal ICXINVProductoCosto)
        {
        	string sDesc = ( from c in db.ICXINV_Productos
                        where c.Compania == Compania
                           && c.ICXINVProductoItemCode == ICXINVProductoItemCode 
                           && c.ICXINVProductoDescripcion == ICXINVProductoDescripcion 
                           && c.ICXINVProductoCosto == ICXINVProductoCosto 
                       select c.ICXINVProductoDescripcion).FirstOrDefault();
        	
        	return sDesc;
        }


        private dynamic GetOtherValues()
        {
            string _ICXINVProductoDescripcion = Request.Form["ICXINVProductoDescripcion"];
            decimal _ICXINVProductoCosto;     		
              decimal.TryParse(Request.Form["ICXINVProductoCosto"], out _ICXINVProductoCosto);     		
                          
			var erow = new {
                              ICXINVProductoDescripcion = _ICXINVProductoDescripcion,     		
                              ICXINVProductoCosto = _ICXINVProductoCosto,     		
                          };
            return erow;        
        }
        
        private bool IsValid(ICXINV_Ventas row, dynamic erow)
        {

            return ModelState.IsValid;
        }


        protected override void Dispose(bool disposing )
        {
            base.Dispose(disposing);
        }

    }
}