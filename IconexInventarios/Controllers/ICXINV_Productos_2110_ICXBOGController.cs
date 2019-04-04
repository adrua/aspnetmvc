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
    public class ICXINV_Productos_2110_ICXBOGController : Controller
    {
        private ArkeosDBContext db = new ArkeosDBContext();
        
        //
        // GET: /ICXINV_Productos/

        public ViewResult Index() 
        {

            if((bool?)Session["ArkeosFactory.Testing.ICXINV_Productos_2110_ICXBOG"] ?? false ) {
                try 
                {
                    //ViewBag.SqlUpdate = db.MigrationScript();
                    var dataLost = (bool?)Session["ArkeosFactory.Testing.ICXINV_Productos_2110_ICXBOG.dataLost"] ?? false;
                    db.Migration(dataLost);
                } 
                catch(Exception ex) 
                {
                    ViewBag.SqlUpdate = ex.Message;
                }
            }
            
            var routes =  System.Web.Routing.RouteTable.Routes;
            //routes.Remove(routes["ICXINVProductos2110ICXBOG"]);
            if(routes["ICXINVProductos2110ICXBOG"] == null)
            {
              routes.MapRoute(
                    name: "ICXINVProductos2110ICXBOG",
                    url: "ICXINV_Productos_2110_ICXBOG/{Controller}/{action}/{id}",
                    defaults: new { MainController="ICXINV_Productos_2110_ICXBOG", Controller = "ICXINV_Productos_2110_ICXBOG", action = "Index", id = UrlParameter.Optional }
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
            
            var q = from ICXINV_Productos in db.ICXINV_Productos
                    orderby ICXINV_Productos.ICXINVProductoItemCode
                    select ICXINV_Productos
                      ;

            //q = Support.jqGridFilter(q, 
					       //          gridSettings,
					       //          new string[] { "Compania", "ICXINVProductoItemCode" });

			// create json data
            int pageIndex = gridSettings.pageIndex;
            int pageSize = gridSettings.pageSize;
            int totalRecords = q.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            int startRow = (pageIndex - 1) * pageSize;
            int endRow = startRow + pageSize;

            dynamic jsonData = null;

            Session["ICXINV_Productos2110Settings"] = gridSettings;

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
                        id = HttpUtility.JavaScriptStringEncode(c.Compania.ToString()   
                        										 + "_" + c.ICXINVProductoItemCode).Replace(" ", "__032__"),

                        cell = new string[]
                        {
    							      
                        	c.Compania.ToString(),
	                    	c.ICXINVProductoItemCode,
	                    	c.ICXINVProductoDescripcion,
							(c.ICXINVProductoExistencias.HasValue)?c.ICXINVProductoExistencias.Value.ToString():"",
							(c.ICXINVProductoCosto.HasValue)?c.ICXINVProductoCosto.Value.ToString():"",
							(c.Fecha_Impresion.HasValue)?c.Fecha_Impresion.Value.ToString("yyyy-MM-dd hh:mm:ss"):"",
							(c.Fecha_Reimpresion.HasValue)?c.Fecha_Reimpresion.Value.ToString("yyyy-MM-dd hh:mm:ss"):"",
							c.Fuente,
							c.FuenteImport,
							(c.Proceso.HasValue)?c.Proceso.Value.ToString():"",
							(c.Fecha_Computador.HasValue)?c.Fecha_Computador.Value.ToString("yyyy-MM-dd hh:mm:ss"):"",
                			c.Usuario,
							c.Timestamp.ToString(),
							c.Secuencia.ToString()
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
                    result = Add_ICXINV_Productos();
                    break;
                case OPERATION.edit:
                    result = Update_ICXINV_Productos(GetKeys(gridSettings.id));
                    break;
                case OPERATION.del:
                    result = Delete_ICXINV_Productos(GetKeys(gridSettings.id));
                    break;
                default:
                    break;
            }
            return Json(result);
        }
        

        private bool Add_ICXINV_Productos()
        {
        	var row = new ICXINV_Productos();
        	
            row.Compania = 1;
            row.ICXINVProductoItemCode = Request.Form["ICXINVProductoItemCode"];
            row.ICXINVProductoDescripcion = Request.Form["ICXINVProductoDescripcion"];
            if(!String.IsNullOrEmpty(Request.Form["ICXINVProductoExistencias"])){
                row.ICXINVProductoExistencias = int.Parse(Request.Form["ICXINVProductoExistencias"]);
              }
            if(!String.IsNullOrEmpty(Request.Form["ICXINVProductoCosto"])){
                row.ICXINVProductoCosto = decimal.Parse(Request.Form["ICXINVProductoCosto"]);
              }

            row.Fuente = "CP2110";
            row.Fecha_Computador = DateTime.Now;
            row.Usuario = HttpContext.User.Identity.Name;

            dynamic erow = GetOtherValues();

			db.ICXINV_Productos.Add(row);
			
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

        private bool Update_ICXINV_Productos(ICXINV_Productos row)
        {
            row = db.Get_ICXINV_Productos(row);					
            ICXINV_Productos crow = null;

            if (row != null)
            {
                crow = (ICXINV_Productos)row.Clone();    
				row.ICXINVProductoDescripcion = Request.Form["ICXINVProductoDescripcion"];

				if(string.IsNullOrEmpty(Request.Form["ICXINVProductoExistencias"]) )
				{
					row.ICXINVProductoExistencias = null;
				}
				else
				{ 
					row.ICXINVProductoExistencias = int.Parse(Request.Form["ICXINVProductoExistencias"]);
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
        
        private bool Delete_ICXINV_Productos(ICXINV_Productos row)
        {

            row = db.Get_ICXINV_Productos(row);

            dynamic erow = GetOtherValues();

            if (row != null)
            {
				db.ICXINV_Productos.Remove(row);
      		    db.SaveChanges();
            }
            return true;
        }

        private ICXINV_Productos GetKeys(string id)
        {

            id = id.Replace("__032__", " ") + "___________________";
            
            var _Keys = id.Split('_');
            
            var Keys = new ICXINV_Productos() {    
                                    Compania = int.Parse(_Keys[0]),
                                    ICXINVProductoItemCode = _Keys[1]
                        };
                        
            return Keys;                    
        }
        

        private dynamic GetOtherValues()
        {
                          
			var erow = new {
                          };
            return erow;        
        }
        
        private bool IsValid(ICXINV_Productos row, dynamic erow)
        {

            return ModelState.IsValid;
        }


        protected override void Dispose(bool disposing )
        {
            base.Dispose(disposing);
        }

    }
}