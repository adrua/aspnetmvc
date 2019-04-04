//ICXINV_VentasModel.cs
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
	[Table("Ventas",Schema="ICXINV")] 
    public class ICXINV_Ventas : ICloneable
	{	
        [Display(Name="Compania!"),Key(),Column(Order=0), Required()]
		public int Compania {get; set;}
        [Display(Name="Venta I D!"),Key(),Column(Order=1), Required()]
        public int ICXINVVentaVentaID  {get; set;}
        [Display(Name="Descripcion:"), Required(), StringLength(18)]
        public string ICXINVProductoItemCode  {get; set;}
        [Display(Name="Cantidad:"), Required()]
        public int ICXINVVentaCantidad  {get; set;}
        [Display(Name="Precio Unitario:"),DisplayFormat(DataFormatString="{0:c}"),  DataType(DataType.Currency), Required()]
        public decimal ICXINVVentaPrecioUnitario  {get; set;}
        [Display(Name="Precio Total="),DisplayFormat(DataFormatString="{0:c}"),  DataType(DataType.Currency)]
        public decimal? ICXINVVentaPrecioTotal  {get; set;}
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
            row.ICXINVVentaVentaID = this.ICXINVVentaVentaID;
		}
	}
	
	public static class ICXINV_Ventas_Extension
	{	        
        public static ICXINV_Ventas Create_ICXINV_Ventas(this ArkeosDBContext db)
        {
        	var row = new ICXINV_Ventas();
        	
			//row.ICXINVVentaVentaID = 0;
			//row.ICXINVVentaCantidad = 1l;
			//row.ICXINVVentaPrecioUnitario = 0.0;
			//row.ICXINVVentaPrecioTotal = 0.0;
			//row.ICXINVProductoCosto = 0.0;

			//row.ICXINVVentaVentaID = db.ICXINV_Ventas.Max((p) => p.ICXINVVentaVentaID) + 1;

        	return row;
        }

	    public static ICXINV_Ventas Get_ICXINV_Ventas(this ArkeosDBContext db,
                                         int Compania,
			                             int ICXINVVentaVentaID)			                             
        {
            ICXINV_Ventas entity = (
            				from r in db.ICXINV_Ventas
            					where r.Compania == Compania
                                   && r.ICXINVVentaVentaID == ICXINVVentaVentaID
					           select r).FirstOrDefault();

			return entity;
		}

        public static ICXINV_Ventas Get_ICXINV_Ventas(this ArkeosDBContext db, 
                                                                         ICXINV_Ventas row)
        {
            ICXINV_Ventas entity = (
            				from r in db.ICXINV_Ventas
            					where r.Compania == row.Compania
                                    && r.ICXINVVentaVentaID == row.ICXINVVentaVentaID
            					select r).FirstOrDefault();

			return entity;
		}

    }

}
