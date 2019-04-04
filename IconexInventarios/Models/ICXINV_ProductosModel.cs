//ICXINV_ProductosModel.cs
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema ;

namespace Multiclick.Arkeos.ICXBOG.Models
{
	[Table("Productos",Schema="ICXINV")] 
    public class ICXINV_Productos : ICloneable
	{	
        [Display(Name="Compania!"),Key(),Column(Order=0), Required()]
		public int Compania {get; set;}
        [Display(Name="Item Code!"),Key(),Column(Order=1), Required(), StringLength(18)]
        public string ICXINVProductoItemCode  {get; set;}
        [Display(Name="Descripcion:"), Required(), StringLength(90)]
        public string ICXINVProductoDescripcion  {get; set;}
        [Display(Name="Existencias=")]
        public int? ICXINVProductoExistencias  {get; set;}
        [Display(Name="Costo="),DisplayFormat(DataFormatString="{0:c}"),  DataType(DataType.Currency)]
        public decimal? ICXINVProductoCosto  {get; set;}

        public DateTime? Fecha_Impresion {get; set;} 
        public DateTime? Fecha_Reimpresion {get; set;} 
        [StringLength(32), Editable(false, AllowInitialValue=true)]
        public String Fuente {get; set;} 
        [StringLength(32), Editable(false, AllowInitialValue=true)]
        public String FuenteImport {get; set;} 
        [Display(Name = "Proceso?")]
        public int? Proceso { get; set; }
        [Editable(false, AllowInitialValue=true)]
        public DateTime? Fecha_Computador {get; set;} 
        [StringLength(100), Editable(false, AllowInitialValue=true)]
        public String Usuario {get; set;}
		[Timestamp()]
        public byte[] Timestamp {get; set;}
        [Editable(false),  DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Secuencia { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void SetKeyTo(dynamic row)
        {
            row.Compania = this.Compania;    
            row.ICXINVProductoItemCode = this.ICXINVProductoItemCode;
		}
	}
	
	public static class ICXINV_Productos_Extension
	{	        
        public static ICXINV_Productos Create_ICXINV_Productos(this ArkeosDBContext db)
        {
        	var row = new ICXINV_Productos();
        	
			//row.ICXINVProductoCosto = 0.0;


        	return row;
        }

	    public static ICXINV_Productos Get_ICXINV_Productos(this ArkeosDBContext db,
                                         int Compania,
			                             string ICXINVProductoItemCode)			                             
        {
            ICXINV_Productos entity = (
            				from r in db.ICXINV_Productos
            					where r.Compania == Compania
                                   && r.ICXINVProductoItemCode == ICXINVProductoItemCode
					           select r).FirstOrDefault();

			return entity;
		}

        public static ICXINV_Productos Get_ICXINV_Productos(this ArkeosDBContext db, 
                                                                         ICXINV_Productos row)
        {
            ICXINV_Productos entity = (
            				from r in db.ICXINV_Productos
            					where r.Compania == row.Compania
                                    && r.ICXINVProductoItemCode == row.ICXINVProductoItemCode
            					select r).FirstOrDefault();

			return entity;
		}

    }

}
