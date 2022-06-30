using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELEMENTS.Data.SQLite;
using ELEMENTS.Infrastructure;

namespace ELEMENTS.Data.SQLite
{
    public class SQLiteService : ISQLiteService
    {
        public SQLiteService()
        {
            if (Factory == null)
            { 
                Factory = new SQLiteFactory();
            }

            // Master GUID 
            Factory.MasterGUID = new Guid("76D9F592-E0C3-4819-B564-E2A7A722CAA0");

            // DB File 
            SQLiteContext.DbFileName = "ELEMENTs.db";
            Factory.SetDatabasePath();

            // 
        }

        public SQLiteService(Guid MasterGUID, string DBFileName = "ELEMENTs.db")
        {
            if (Factory == null)
            {
                Factory = new SQLiteFactory();
            }

            // Master GUID 
            Factory.MasterGUID = MasterGUID;

            // DB File 
            SQLiteContext.DbFileName = DBFileName;
            Factory.SetDatabasePath();

            // 
        }
        
        // 
        public void SetFileName(string DBFileName = "ELEMENTs.db")
        {
            if (Factory == null)
            {
                Factory = new SQLiteFactory();
            }

            // DB File 
            SQLiteContext.DbFileName = DBFileName;
            Factory.SetDatabasePath();
        }
        public void SetMasterGUID(Guid MasterGUID)
        {
            if (Factory == null)
            {
                Factory = new SQLiteFactory();
            }

            // Master GUID 
            Factory.MasterGUID = MasterGUID;
        }
        
        // Methods 
        public IFactoryStatusInfo CreateDatabase(string name)
        {
            try
            {
                // create factory  
                IFactory factory = new SQLiteFactory();

                // set database name 
                SQLiteContext.DbFileName = name + ".db";
                factory.SetDatabasePath();

                // create database 
                IFactoryStatusInfo status = factory.CreateDatabase();
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAIL: " + ex.Message);
                return null;
            }
        }
        public IFactoryStatusInfo MigrateDatabase(string name)
        {
            try
            {
                // create factory  
                IFactory factory = new SQLiteFactory();

                // set database name 
                SQLiteContext.DbFileName = name + ".db";
                factory.SetDatabasePath();

                // create database 
                IFactoryStatusInfo status = factory.MigrateDatabase();
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAIL: " + ex.Message);
                return null;
            }
        }
        public string GetMigrationVersion()
        {
            try
            {
                // create factory  
                IFactory factory = new SQLiteFactory();

                // set database name 
                return factory.GetDatabaseVersion();
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAIL: " + ex.Message);
                return string.Empty;
            }
        }
        public IFactory Factory { get; set; } = new SQLiteFactory();
    }
}
