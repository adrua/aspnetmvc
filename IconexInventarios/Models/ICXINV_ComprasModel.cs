//ICXINV_ComprasModel.cs
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
	[Table("Compras",Schema="ICXINV")] 
    public class ICXINV_Compras : ICloneable
	{	
        [Display(Name="Compania!"),Key(),Column(Order=0), Required()]
		public int Compania {get; set;}
        [Display(Name="Compra I D!"),Key(),Column(Order=1), Required()]
        public int ICXINVCompraCompraID  {get; set;}
        [Display(Name="Descripcion:"), Required(), StringLength(18)]
        public string ICXINVProductoItemCode  {get; set;}
        [Display(Name="Cantidad:"),DisplayFormat(DataFormatString="{0:c}"),  DataType(DataType.Currency), Required()]
        public decimal ICXINVCompraCantidad  {get; set;}
        [Display(Name="Precio Unitario:"),DisplayFormat(DataFormatString="{0:c}"),  DataType(DataType.Currency), Required()]
        public decimal ICXINVCompraPrecioUnitario  {get; set;}
        [Display(Name="Precio Total="),DisplayFormat(DataFormatString="{0:c}"),  DataType(DataType.Currency)]
        public decimal? ICXINVCompraPrecioTotal  {get; set;}

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
            row.ICXINVCompraCompraID = this.ICXINVCompraCompraID;
		}
	}
	
	public static class ICXINV_Compras_Extension
	{	        
        public static ICXINV_Compras Create_ICXINV_Compras(this ArkeosDBContext db)
        {
        	var row = new ICXINV_Compras();
        	
			//row.ICXINVCompraCompraID = 0;
			//row.ICXINVCompraCantidad = 1l;
			//row.ICXINVCompraPrecioUnitario = 0.0;
			//row.ICXINVCompraPrecioTotal = 0.0;

			//row.ICXINVCompraCompraID = db.ICXINV_Compras.Max((p) => p.ICXINVCompraCompraID) + 1;

        	return row;
        }

	    public static ICXINV_Compras Get_ICXINV_Compras(this ArkeosDBContext db,
                                         int Compania,
			                             int ICXINVCompraCompraID)			                             
        {
            ICXINV_Compras entity = (
            				from r in db.ICXINV_Compras
            					where r.Compania == Compania
                                   && r.ICXINVCompraCompraID == ICXINVCompraCompraID
					           select r).FirstOrDefault();

			return entity;
		}

        public static ICXINV_Compras Get_ICXINV_Compras(this ArkeosDBContext db, 
                                                                         ICXINV_Compras row)
        {
            ICXINV_Compras entity = (
            				from r in db.ICXINV_Compras
            					where r.Compania == row.Compania
                                    && r.ICXINVCompraCompraID == row.ICXINVCompraCompraID
            					select r).FirstOrDefault();

			return entity;
		}

    }

}
