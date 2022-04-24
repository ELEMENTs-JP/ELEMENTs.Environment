using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ELEMENTS.Infrastructure;
using Microsoft.AspNetCore.Hosting;

namespace ELEMENTS.Data.SQLite
{
    public class SQLiteContext : DbContext
    {
        //internal static string path = Path.GetFullPath(Path.Combine(
        //    AppDomain.CurrentDomain.GetData("DB") as string
        //    ?? AppDomain.CurrentDomain.BaseDirectory, "database.db")).ToString();

        internal static string FullDirectory = "";
        internal static string DbFileName = "fallback.db";
        internal static string FullFilePath = "";

        bool useIntegratedSecurity = false;
        public SQLiteContext()
        {
            useIntegratedSecurity = false;
        }
        public SQLiteContext(bool uis = false)
        {
            useIntegratedSecurity = uis;

            // https://www.entityframeworktutorial.net/code-first/database-initialization-strategy-in-code-first.aspx 
            // Database.SetInitializer<SchoolDBContext>(new CreateDatabaseIfNotExists<SchoolDBContext>());
        }

        public SQLiteContext(DbContextOptions options)
            : base(options)
        {
            useIntegratedSecurity = false;
        }

        // Configuration 
        #region Configuration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                optionsBuilder.UseSqlite("Data Source=" + SQLiteContext.FullFilePath);

                base.OnConfiguring(optionsBuilder);
            }
            catch (Exception ex)
            {
                // BaseHelper.LogEx(ex);
            }
        }
        #endregion

        // Events 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                // SQLite 
                #region SQLite
                // verhindert, dass GUIDs in der DB als TEXT klein geschrieben werden 
                // ermöglicht nach groß und klein geschriebenen GUIDs in der Datenbank als TEXT zu suchen 
                modelBuilder.UseCollation("NOCASE");

                foreach (var entity in modelBuilder.Model.GetEntityTypes())
                {
                    foreach (var property in entity.GetProperties().Where(p => p.ClrType == typeof(Guid)))
                    {
                        if (property.Name.ToLower().Contains("GUID".ToLower()))
                        {
                            property.SetColumnType("TEXT NOT NULL COLLATE NOCASE");
                        }
                    }
                }
                #endregion

                modelBuilder.Entity<tbl_TEC_Relation>().HasKey(c => new { c.MasterGUID, c.RelationType, c.AppType, c.ParentGUID, c.ChildGUID  });
            }
            catch (Exception ex)
            {

            }
        }

        // Tables 
        public DbSet<tbl_CON_Content> tbl_CON_Content { get; set; } // Content Items 
        public DbSet<tbl_TEC_Relation> tbl_TEC_Relation { get; set; } // Relations 
        public DbSet<tbl_MTD_Property> tbl_MTD_Property { get; set; } // Relations 
        public DbSet<tbl_SET_Setting> tbl_SET_Setting { get; set; } // Relations 
    }


    public static class SQLiteHelper
    {
        // Connection + Builder 
        public static SQLiteContext GetEntities(bool integrated = false)
        {
            try
            {
                return new SQLiteContext(integrated);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
        internal static IFactoryStatusInfo EnsureDatabaseCreated()
        {
            IFactoryStatusInfo info = new FactoryStatusInfo();
            info.Status = "";
            info.Message = "";

            try
            {
                if (string.IsNullOrEmpty(SQLiteContext.FullFilePath))
                {
                    // Error 
                    info.Status = "FAIL";
                    info.Message = "Datenbankpfad wurde nicht gesetzt";
                    return info;
                }

                if (File.Exists(SQLiteContext.FullFilePath))
                {
                    // EXISTS already 
                    info.Status = "OK";
                    info.Message = "Datenbank existiert bereits und musste nicht erstellt werden. Löschen Sie die Datenbank, wenn Sie eine neue erstellen wollen oder geben Sie einen anderen Namen an.";
                    return info;
                }

                // Context 
                using (SQLiteContext ctx = GetEntities())
                {
                    // Create 
                    ctx.Database.EnsureCreated();
                }

                if (!File.Exists(SQLiteContext.FullFilePath))
                {
                    // NOT Exists 
                    info.Status = "FAIL";
                    info.Message = "Datenbank konnte nach Erstellungsversuch nicht erstellt werden.";
                    return info;
                }

                // Status 
                info.Status = "OK";
                info.Message = "Datenbank wurde erstellt";
                return info;
            }
            catch (Exception ex)
            {
                // Error 
                string msg = ex.Message;
                info.Status = "Fehler";
                info.Message = "" + ex.Message;
                return info;
            }
        }
        internal static IFactoryStatusInfo MigrateDatabase()
        {
            IFactoryStatusInfo info = new FactoryStatusInfo();
            info.Status = "";
            info.Message = "";

            try
            {
                if (string.IsNullOrEmpty(SQLiteContext.FullFilePath))
                {
                    // Error 
                    info.Status = "FAIL";
                    info.Message = "Datenbankpfad wurde nicht gesetzt";
                    return info;
                }

                if (!File.Exists(SQLiteContext.FullFilePath))
                {
                    // Error 
                    info.Status = "FAIL";
                    info.Message = "Datenbank existiert nicht";
                    return info;
                }

                // Context 
                using (SQLiteContext ctx = GetEntities())
                {
                    // Migration 
                    ctx.Database.Migrate();
                }

                info.Status = "OK";
                info.Message = "Migration wurde durchgeführt";
                return info;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;

                // Error 
                info.Status = "Fehler";
                info.Message = "" + ex.Message;
                return info;
            }
        }
        internal static IFactoryStatusInfo DeleteDatabase()
        {
            IFactoryStatusInfo info = new FactoryStatusInfo();
            info.Status = "";
            info.Message = "";

            try
            {
                if (string.IsNullOrEmpty(SQLiteContext.FullFilePath))
                {
                    // Error 
                    info.Status = "FAIL";
                    info.Message = "Datenbankpfad wurde nicht gesetzt";
                    return info;
                }

                if (!File.Exists(SQLiteContext.FullFilePath))
                {
                    // EXISTS already 
                    info.Status = "OK";
                    info.Message = "Datenbank existiert nicht und musste nicht gelöscht werden. Löschen war nicht notwendig.";
                    return info;
                }

                // Context 
                using (SQLiteContext ctx = GetEntities())
                {
                    // Create 
                    ctx.Database.EnsureDeleted();
                }

                if (File.Exists(SQLiteContext.FullFilePath))
                {
                    // Exists 
                    try
                    {
                        File.Delete(SQLiteContext.FullFilePath);
                    }
                    catch (Exception ex)
                    {
                        // Exists 
                        info.Status = "FAIL";
                        info.Message = "Datenbank konnte nach direkten Datei-Löschversuch nicht gelöscht werden.";
                        return info;
                    }
                }

                if (File.Exists(SQLiteContext.FullFilePath))
                {
                    // Exists 
                    info.Status = "FAIL";
                    info.Message = "Datenbank konnte nach Löschversuch nicht gelöscht werden.";
                    return info;
                }

                // Status 
                info.Status = "OK";
                info.Message = "Datenbank wurde erfolgreich gelöscht";
                return info;
            }
            catch (Exception ex)
            {
                // Error 
                string msg = ex.Message;
                info.Status = "Fehler";
                info.Message = "" + ex.Message;
                return info;
            }
        }

        public static string GetMigrationVersion()
        {
            string version = string.Empty;
            
            try
            {
                var query = "SELECT MigrationId FROM __EFMigrationsHistory LIMIT 1";

                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();

                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = query;
                            object result = command.ExecuteReader();
                            version = result.ToSecureString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            
            return version;
        }
    }


    public class SQLiteContextService : IContextService
    {
        // https://docs.microsoft.com/de-de/aspnet/core/blazor/file-uploads?view=aspnetcore-6.0&pivots=server 

        private readonly IWebHostEnvironment _environment;
        public SQLiteContextService(IWebHostEnvironment environment)
        {
            _environment = environment;

            string contentRootPath = _environment.ContentRootPath;

            //ClientHelper.BaseDBsPath = contentRootPath + "\\" + defaultDBsPath + "\\";
            //ClientHelper.BaseFilesPath = contentRootPath + "\\" + defaultFilePath + "\\";
        }

        public string GetContentRootPath()
        {
            return _environment.ContentRootPath;
        }

    }

    public interface IContextService
    {
        string GetContentRootPath();
    }
}
