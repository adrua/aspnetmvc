using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;

namespace Multiclick.Arkeos.ICXBOG.Models
{
    public class ArkeosDBCoreContext 
                    : DbContext
    {
        public DbSet<ICXINV_Productos> ICXINV_Productos { get; set; }
        public DbSet<ICXINV_Compras> ICXINV_Compras { get; set; }
        public DbSet<ICXINV_Ventas> ICXINV_Ventas { get; set; }

        public void Migration(bool DataLossAllowed = false)
        {
            var sql = MigrationScriptAsync();

            var configuration = new Configuration(DataLossAllowed);
            configuration.ContextType = typeof(ArkeosDBCoreContext);
            var migrator = new DbMigrator(configuration);
            migrator.Update();         
        }
         
        public string MigrationScriptAsync() 
        {
            var sql = "";
            
            try {
                var configuration = new Configuration();
                configuration.ContextType = typeof(ArkeosDBContext);
                var migrator = new DbMigrator(configuration);
                
                var scriptor = new MigratorScriptingDecorator(migrator);
                
                sql = scriptor.ScriptUpdate(sourceMigration: null, targetMigration: null);
                var nInx = sql.IndexOf("INSERT [dbo].[__MigrationHistory]");
                
                if(nInx > 0) {
                    sql = sql.Substring(nInx);
                    //this.Database.ExecuteSqlCommand(sql);
                }
            
            } catch (Exception  ){
                sql = "";  
            }
            return sql;
        }
    }

    internal sealed class Configuration : DbMigrationsConfiguration<ArkeosDBCoreContext>
    {
        public Configuration(bool DataLossAllowed = false)
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed  = DataLossAllowed;
            MigrationsDirectory = @"~/app_data";
            ContextKey  = "ICXINV_Productos_2110_ICXBOG_v1";
        }
    }

    public class ArkeosDBContext 
                    : ArkeosDBCoreContext
    {
		//Foreings Tables

    }


}

