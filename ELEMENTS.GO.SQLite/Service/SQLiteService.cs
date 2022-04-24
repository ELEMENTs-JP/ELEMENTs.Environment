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
        public Guid MasterGUID { get; set; } = Guid.Empty;
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
    }
}
