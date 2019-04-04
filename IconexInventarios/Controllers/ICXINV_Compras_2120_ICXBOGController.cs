using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    public class ICXINV_Compras_2120_ICXBOGController : Controller
    {
        private ArkeosDBContext db = new ArkeosDBContext();
        
        //
        // GET: /ICXINV_Compras/

        public ViewResult Index() 
        {

            if((bool?)Session["ArkeosFactory.Testing.ICXINV_Compras_2120_ICXBOG"] ?? false ) {
                try 
                {
                    //ViewBag.SqlUpdate = db.MigrationScript();
                    var dataLost = (bool?)Session["ArkeosFactory.Testing.ICXINV_Compras_2120_ICXBOG.dataLost"] ?? false;
                    db.Migration(dataLost);
                } 
                catch(Exception ex) 
                {
                    ViewBag.SqlUpdate = ex.Message;
                }
            }
            
            var routes =  System.Web.Routing.RouteTable.Routes;
            //routes.Remove(routes["ICXINVCompras2120ICXBOG"]);
            if(routes["ICXINVCompras2120ICXBOG"] == null)
            {
              routes.MapRoute(
                    name: "ICXINVCompras2120ICXBOG",
                    url: "ICXINV_Compras_2120_ICXBOG/{Controller}/{action}/{id}",
                    defaults: new { MainController="ICXINV_Compras_2120_ICXBOG", Controller = "ICXINV_Compras_2120_ICXBOG", action = "Index", id = UrlParameter.Optional }
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
            
            var q = from ICXINV_Compras in db.ICXINV_Compras
                      join ICXINV_Productos in db.ICXINV_Productos.AsNoTracking()
                        on new { ICXINV_Compras.Compania,  ICXINVProductoItemCode = ICXINV_Compras.ICXINVProductoItemCode} 
                        equals new { ICXINV_Productos.Compania,  ICXINV_Productos.ICXINVProductoItemCode} 
                        orderby ICXINV_Compras.ICXINVCompraCompraID
                    select new { ICXINV_Compras, ICXINV_Productos }
                      ;

            //q = Support.jqGridFilter(q, 
					       //          gridSettings,
					       //          new string[] { "Compania", "ICXINVCompraCompraID" });

			// create json data
            int pageIndex = gridSettings.pageIndex;
            int pageSize = gridSettings.pageSize;
            int totalRecords = q.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            int startRow = (pageIndex - 1) * pageSize;
            int endRow = startRow + pageSize;

            dynamic jsonData = null;

            Session["ICXINV_Compras2120Settings"] = gridSettings;

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
                        id = HttpUtility.JavaScriptStringEncode(c.ICXINV_Compras.Compania.ToString()   
                        										 + "_" + c.ICXINV_Compras.ICXINVCompraCompraID.ToString("#")).Replace(" ", "__032__"),

                        cell = new string[]
                        {
    							      
                        	c.ICXINV_Compras.Compania.ToString(),
	                    	c.ICXINV_Compras.ICXINVCompraCompraID.ToString(),
	                    	c.ICXINV_Compras.ICXINVProductoItemCode,
	                    	c.ICXINV_Productos.ICXINVProductoDescripcion.ToString(),
	                    	c.ICXINV_Compras.ICXINVCompraCantidad.ToString("#"),
	                    	c.ICXINV_Compras.ICXINVCompraPrecioUnitario.ToString(),
							(c.ICXINV_Compras.ICXINVCompraPrecioTotal.HasValue)?c.ICXINV_Compras.ICXINVCompraPrecioTotal.Value.ToString():"",
							(c.ICXINV_Compras.Fecha_Impresion.HasValue)?c.ICXINV_Compras.Fecha_Impresion.Value.ToString("yyyy-MM-dd hh:mm:ss"):"",
							(c.ICXINV_Compras.Fecha_Reimpresion.HasValue)?c.ICXINV_Compras.Fecha_Reimpresion.Value.ToString("yyyy-MM-dd hh:mm:ss"):"",
							c.ICXINV_Compras.Fuente,
							c.ICXINV_Compras.FuenteImport,
							(c.ICXINV_Compras.Proceso.HasValue)?c.ICXINV_Compras.Proceso.Value.ToString():"",
							(c.ICXINV_Compras.Fecha_Computador.HasValue)?c.ICXINV_Compras.Fecha_Computador.Value.ToString("yyyy-MM-dd hh:mm:ss"):"",
                			c.ICXINV_Compras.Usuario,
							c.ICXINV_Compras.Timestamp.ToString(),
							c.ICXINV_Compras.Secuencia.ToString()
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
                    result = Add_ICXINV_Compras();
                    break;
                case OPERATION.edit:
                    result = Update_ICXINV_Compras(GetKeys(gridSettings.id));
                    break;
                case OPERATION.del:
                    result = Delete_ICXINV_Compras(GetKeys(gridSettings.id));
                    break;
                default:
                    break;
            }
            return Json(result);
        }
        

        private bool Add_ICXINV_Compras()
        {
        	var row = new ICXINV_Compras();
        	
            row.Compania = 1;
            row.ICXINVCompraCompraID = ( db.ICXINV_Compras.Max((p) => (int?)p.ICXINVCompraCompraID) ?? 0 ) + 1;

            row.ICXINVProductoItemCode = Request.Form["ICXINVProductoItemCode"];
            row.ICXINVCompraCantidad = decimal.Parse(Request.Form["ICXINVCompraCantidad"]);
            row.ICXINVCompraPrecioUnitario = decimal.Parse(Request.Form["ICXINVCompraPrecioUnitario"]);
            if(!String.IsNullOrEmpty(Request.Form["ICXINVCompraPrecioTotal"])){
                row.ICXINVCompraPrecioTotal = decimal.Parse(Request.Form["ICXINVCompraPrecioTotal"]);
              }

            row.Fuente = "CP2120";
            row.Fecha_Computador = DateTime.Now;
            row.Usuario = HttpContext.User.Identity.Name;

            dynamic erow = GetOtherValues();

			db.ICXINV_Compras.Add(row);
			
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

        private bool Update_ICXINV_Compras(ICXINV_Compras row)
        {
            row = db.Get_ICXINV_Compras(row);					
            ICXINV_Compras crow = null;

            if (row != null)
            {
                crow = (ICXINV_Compras)row.Clone();    
				row.ICXINVProductoItemCode = Request.Form["ICXINVProductoItemCode"];
				row.ICXINVCompraCantidad = decimal.Parse(Request.Form["ICXINVCompraCantidad"]);
				row.ICXINVCompraPrecioUnitario = decimal.Parse(Request.Form["ICXINVCompraPrecioUnitario"]);

				if(string.IsNullOrEmpty(Request.Form["ICXINVCompraPrecioTotal"]) )
				{
					row.ICXINVCompraPrecioTotal = null;
				}
				else
				{ 
					row.ICXINVCompraPrecioTotal = decimal.Parse(Request.Form["ICXINVCompraPrecioTotal"]);
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
        
        private bool Delete_ICXINV_Compras(ICXINV_Compras row)
        {

            row = db.Get_ICXINV_Compras(row);

            dynamic erow = GetOtherValues();

            if (row != null)
            {
				db.ICXINV_Compras.Remove(row);
      		    db.SaveChanges();
            }
            return true;
        }

        private ICXINV_Compras GetKeys(string id)
        {

            id = id.Replace("__032__", " ") + "___________________";
            
            var _Keys = id.Split('_');
            
            var Keys = new ICXINV_Compras() {    
                                    Compania = int.Parse(_Keys[0]),
                                    ICXINVCompraCompraID = int.Parse("0" + _Keys[1])
                        };
                        
            return Keys;                    
        }
        
      //Autocomplete
        //  GET: /ICXINV_Compras/ICXINV_Productos
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
					                         ICXINVProductoItemCode = c.ICXINVProductoItemCode
                                            }
                               ).ToArray()
            ;
            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }
        
        private string GetICXINV_Productos(int Compania, string ICXINVProductoItemCode, string ICXINVProductoDescripcion)
        {
        	string sDesc = ( from c in db.ICXINV_Productos
                        where c.Compania == Compania
                           && c.ICXINVProductoItemCode == ICXINVProductoItemCode 
                           && c.ICXINVProductoDescripcion == ICXINVProductoDescripcion 
                       select c.ICXINVProductoDescripcion).FirstOrDefault();
        	
        	return sDesc;
        }


        private dynamic GetOtherValues()
        {
            string _ICXINVProductoDescripcion = Request.Form["ICXINVProductoDescripcion"];
                          
			var erow = new {
                              ICXINVProductoDescripcion = _ICXINVProductoDescripcion,     		
                          };
            return erow;        
        }
        
        private bool IsValid(ICXINV_Compras row, dynamic erow)
        {

            return ModelState.IsValid;
        }


        protected override void Dispose(bool disposing )
        {
            base.Dispose(disposing);
        }

    }
}