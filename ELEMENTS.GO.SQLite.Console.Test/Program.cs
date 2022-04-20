using System;
using System.Collections.Generic;
using ELEMENTS.Infrastructure;
using System.Linq;
using ELEMENTS.Data.SQLite;

namespace ELEMENTS.GO.SQLite
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("INFO: Master wird erzeugt");
            Guid masterGUID = new Guid("D6E81B59-9228-497E-A286-5633AAFC1F15");
            Console.WriteLine("OK - Master wurde erzeugt");

            // Factory Instanziieren 
            Console.WriteLine("INFO: Factory wird erzeugt");
            IFactory factory = new SQLiteFactory();
            Console.WriteLine("OK - Factory wurde erzeugt");

            // Pfad setzen 
            Console.WriteLine("INFO: Pfad wird gesetzt");
            factory.SetDatabasePath();
            Console.WriteLine("OK - Pfad wurde gesetzt");

            // Datenbank löschen 
            Console.WriteLine("Datenbank wird gelöscht");
            IFactoryStatusInfo status = factory.DeleteDatabase();
            if (status.Status != "OK")
            {
                Console.WriteLine("FAIL - Datenbank wurde nicht gelöscht");
                throw new Exception("Datenbank wurde nicht gelöscht");
            }
            if (status.Status == "OK")
            {
                Console.WriteLine("OK - Datenbank wurde gelöscht");
            }

            // Datenbank 
            Console.WriteLine("Datenbank wird erzeugt");
            status = factory.CreateDatabase();
            if (status.Status != "OK")
            {
                Console.WriteLine("FAIL - Datenbank wurde nicht erstellt");
                throw new Exception("Datenbank wurde nicht erstellt");
            }
            if (status.Status == "OK")
            {
                Console.WriteLine("OK - Datenbank Erzeugung OK");
            }

            // Migrate 
            Console.WriteLine("Datenbank wird migriert");
            status = factory.MigrateDatabase();
            if (status.Status != "OK")
            {
                Console.WriteLine("FAIL - Migration wurde nicht durchgeführt");
                throw new Exception("Migration wurde nicht durchgeführt");
            }
            if (status.Status == "OK")
            {
                Console.WriteLine("OK - Migration wurde durchgeführt");
            }

            // Vater 
            Guid VaterGUID = Guid.NewGuid();
            IInputDTO inputVater = InputDTO.CreateTemplate(
                VaterGUID, "Vater",
                masterGUID, "TEST", "Item");

            IDTO VaterDTO = factory.Create(inputVater);
            if (VaterDTO == null)
            {
                Console.WriteLine("FAIL - Vater wurde nicht erstellt");
                throw new Exception("DTO wurde nicht erstellt");
            }
            if (VaterDTO != null)
            {
                Console.WriteLine("OK - Vater wurde erstellt");
            }

            // Metadaten 
            IItemMetadataDTO mtd = VaterDTO.GetMetadata();
            if (mtd == null)
            {
                Console.WriteLine("FAIL - Metadaten wurden nicht geladen");
                throw new Exception("Metadaten wurde nicht erstellt");
            }
            if (mtd != null)
            {
                Console.WriteLine("OK - Metadaten wurden geladen");
            }

            // Set 
            mtd.ItemGUID = VaterGUID;
            mtd.Language = "DE";
            mtd.Version = "1.0";

            // Set 
            mtd = VaterDTO.SetMetadata(mtd);
            if (mtd.ItemGUID != VaterGUID)
            {
                Console.WriteLine("FAIL - Item GUID wurde nicht gesetzt");
                throw new Exception("DTO wurde nicht erstellt");
            }
            if (mtd.ItemGUID == VaterGUID)
            {
                Console.WriteLine("OK - Item GUID wurde gesetzt");
            }
            if (mtd.Language != "DE")
            {
                Console.WriteLine("FAIL - Language wurde nicht gesetzt");
                throw new Exception("DTO wurde nicht erstellt");
            }
            if (mtd.Language == "DE")
            {
                Console.WriteLine("OK - Language wurde gesetzt");
            }
            if (mtd.Version != "1.0")
            {
                Console.WriteLine("FAIL - Version wurde nicht gesetzt");
                throw new Exception("DTO wurde nicht erstellt");
            }
            if (mtd.Version == "1.0")
            {
                Console.WriteLine("OK - Version wurde gesetzt");
            }

            // Get 
            mtd = VaterDTO.GetMetadata();
            if (mtd.ItemGUID != VaterGUID)
            {
                Console.WriteLine("FAIL - Item GUID wurde nicht gesetzt");
                throw new Exception("DTO wurde nicht erstellt");
            }
            if (mtd.ItemGUID == VaterGUID)
            {
                Console.WriteLine("OK - Item GUID wurde gesetzt");
            }
            if (mtd.Language != "DE")
            {
                Console.WriteLine("FAIL - Language wurde nicht gesetzt");
                throw new Exception("DTO wurde nicht erstellt");
            }
            if (mtd.Language == "DE")
            {
                Console.WriteLine("OK - Language wurde gesetzt");
            }
            if (mtd.Version != "1.0")
            {
                Console.WriteLine("FAIL - Version wurde nicht gesetzt");
                throw new Exception("DTO wurde nicht erstellt");
            }
            if (mtd.Version == "1.0")
            {
                Console.WriteLine("OK - Version wurde gesetzt");
            }

            // Set 
            mtd = VaterDTO.SetMetadataSave(mtd);
            if (mtd.ItemGUID != VaterGUID)
            {
                Console.WriteLine("FAIL - Item GUID wurde nicht gesetzt");
                throw new Exception("DTO wurde nicht erstellt");
            }
            if (mtd.ItemGUID == VaterGUID)
            {
                Console.WriteLine("OK - Item GUID wurde gesetzt");
            }
            if (mtd.Language != "DE")
            {
                Console.WriteLine("FAIL - Language wurde nicht gesetzt");
                throw new Exception("DTO wurde nicht erstellt");
            }
            if (mtd.Language == "DE")
            {
                Console.WriteLine("OK - Language wurde gesetzt");
            }
            if (mtd.Version != "1.0")
            {
                Console.WriteLine("FAIL - Version wurde nicht gesetzt");
                throw new Exception("DTO wurde nicht erstellt");
            }
            if (mtd.Version == "1.0")
            {
                Console.WriteLine("OK - Version wurde gesetzt");
            }

            // Get 
            mtd = VaterDTO.GetMetadata();
            if (mtd.ItemGUID != VaterGUID)
            {
                Console.WriteLine("FAIL - Item GUID wurde nicht gesetzt");
                throw new Exception("DTO wurde nicht erstellt");
            }
            if (mtd.ItemGUID == VaterGUID)
            {
                Console.WriteLine("OK - Item GUID wurde gesetzt");
            }
            if (mtd.Language != "DE")
            {
                Console.WriteLine("FAIL - Language wurde nicht gesetzt");
                throw new Exception("DTO wurde nicht erstellt");
            }
            if (mtd.Language == "DE")
            {
                Console.WriteLine("OK - Language wurde gesetzt");
            }
            if (mtd.Version != "1.0")
            {
                Console.WriteLine("FAIL - Version wurde nicht gesetzt");
                throw new Exception("DTO wurde nicht erstellt");
            }
            if (mtd.Version == "1.0")
            {
                Console.WriteLine("OK - Version wurde gesetzt");
            }

            // Vater erneut laden 
            VaterDTO = factory.GetItemByID(inputVater);
            if (VaterDTO == null)
            {
                Console.WriteLine("FAIL - Vater wurde nicht geladen");
                throw new Exception("DTO wurde nicht geladen");
            }
            if (VaterDTO != null)
            {
                Console.WriteLine("OK - Vater wurde geladen");
            }

            // Get 
            mtd = VaterDTO.GetMetadata();
            if (mtd.ItemGUID != VaterGUID)
            {
                Console.WriteLine("FAIL - Item GUID wurde nicht gesetzt");
                throw new Exception("DTO wurde nicht erstellt");
            }
            if (mtd.ItemGUID == VaterGUID)
            {
                Console.WriteLine("OK - Item GUID wurde gesetzt");
            }
            if (mtd.Language != "DE")
            {
                Console.WriteLine("FAIL - Language wurde nicht gesetzt");
                throw new Exception("DTO wurde nicht erstellt");
            }
            if (mtd.Language == "DE")
            {
                Console.WriteLine("OK - Language wurde gesetzt");
            }
            if (mtd.Version != "1.0")
            {
                Console.WriteLine("FAIL - Version wurde nicht gesetzt");
                throw new Exception("DTO wurde nicht erstellt");
            }
            if (mtd.Version == "1.0")
            {
                Console.WriteLine("OK - Version wurde gesetzt");
            }

            // Properties 
            List<IProperty> props = VaterDTO.Properties;
            if (props.Count == 0)
            {
                Console.WriteLine("OK - Properties sind leer");
            }
            if (props.Count != 0)
            {
                Console.WriteLine("FAIL - Properties sind NICHT leer");
            }
            props = VaterDTO.LoadProperties();
            if (props.Count == 0)
            {
                Console.WriteLine("OK - Properties sind leer");
            }
            if (props.Count != 0)
            {
                Console.WriteLine("FAIL - Properties sind NICHT leer");
            }

            // Set Property 
            VaterDTO.SetProperty("Value", "Eigenschaft");

            props = VaterDTO.LoadProperties();
            if (props.Count == 1)
            {
                Console.WriteLine("OK - Properties wurde gespeichert und geladen");
            }
            if (props.Count != 1)
            {
                Console.WriteLine("FAIL - Properties wurde NICHT gespeichert und geladen");
            }

            // Delete 
            VaterDTO.SetProperty("", "Eigenschaft");
            VaterDTO.RemoveDeleteProperty(props.FirstOrDefault());
            props = VaterDTO.LoadProperties();
            if (props.Count != 1)
            {
                Console.WriteLine("OK - Setting wurde wieder gelöscht");
            }
            if (props.Count == 1)
            {
                Console.WriteLine("FAIL - Setting wurde NICHT wieder gelöscht");
            }

            // Settings 
            List<ISetting> sets = VaterDTO.Settings;
            if (sets.Count == 0)
            {
                Console.WriteLine("OK - Settings sind leer");
            }
            if (sets.Count != 0)
            {
                Console.WriteLine("OK - Settings sind NICHT leer");
            }
            sets = VaterDTO.LoadSettings();
            if (sets.Count == 0)
            {
                Console.WriteLine("OK - Settings sind leer");
            }
            if (sets.Count != 0)
            {
                Console.WriteLine("FAIL - Settings sind NICHT leer");
            }

            // Set Setting 
            VaterDTO.SetSetting("Value", "Setting", "Level");

            sets = VaterDTO.LoadSettings();
            if (sets.Count == 1)
            {
                Console.WriteLine("OK - Setting wurde gespeichert und geladen");
            }
            if (sets.Count != 1)
            {
                Console.WriteLine("FAIL - Setting wurde NICHT gespeichert und geladen");
            }

            // Delete 
            VaterDTO.RemoveDeleteSetting(sets.FirstOrDefault());
            sets = VaterDTO.LoadSettings();
            if (sets.Count == 0)
            {
                Console.WriteLine("OK - Setting wurde wieder gelöscht");
            }
            if (sets.Count != 0)
            {
                Console.WriteLine("FAIL - Setting wurde NICHT gelöscht");
            }

            // Sohn 
            Guid SohnGUID = Guid.NewGuid();
            IInputDTO inputSohn = InputDTO.CreateTemplate(
                SohnGUID, "Sohn",
                masterGUID, "TEST", "Item");

            IDTO SohnDTO = factory.Create(inputSohn);
            if (SohnDTO == null)
            {
                Console.WriteLine("FAIL - Sohn wurde nicht erstellt");
                throw new Exception("DTO wurde nicht erstellt");
            }
            if (SohnDTO != null)
            {
                Console.WriteLine("OK - Sohn wurde erstellt");
            }

            // Assign 
            IRelationDTO VaterSohn = RelationDTO.CreateTemplate(
                VaterGUID, "Item",
                SohnGUID, "Item",
                "Zuordnung",
                masterGUID
                );
            bool VaterSohnErgebnis = factory.AssignRelation(VaterSohn);
            if (VaterSohnErgebnis == false)
            {
                Console.WriteLine("FAIL - Vater Sohn Zuordnung wurde nicht erstellt");
                throw new Exception("Zuordnung wurde nicht vorgenommen");
            }
            if (VaterSohnErgebnis == true)
            {
                Console.WriteLine("OK - Vater Sohn Zuordnung wurde erstellt");
            }


            // Check 
            IRelationDTO VaterSohnCheck = factory.GetRelation(VaterSohn);
            if (VaterSohnCheck == null)
            {
                Console.WriteLine("FAIL - Vater Sohn Zuordnung konnte nicht geladen werden");
                throw new Exception("Zuordnung wurde nicht vorgenommen");
            }
            if (VaterSohnCheck != null)
            {
                Console.WriteLine("OK - Vater Sohn Zuordnung wurde geladen");
            }

            // All Items 
            IQueryParameter iqp = QueryParameter.DefaultItemsQuery(
                masterGUID, "TEST", "Item",
                QueryType.List, 1, 999);
            int count = factory.GetItemsCount(iqp);
            if (count != 2)
            {
                Console.WriteLine("FAIL - Weniger oder mehr Datensätze wurden gefunden");
                throw new Exception("Es wurden keine 2 Datensätze erzeugt");
            }
            if (count == 2)
            {
                Console.WriteLine("OK - Es wurden genau 2 gefunden");
            }

            // Get Items 
            List<IDTO> items = factory.GetItems(iqp);
            if (items.Count != 2)
            {
                Console.WriteLine("FAIL - Es wurden zu viel oder zu wenig Datensätze geladen");
                throw new Exception("Es wurden keine 2 Datensätze gefunden");
            }
            if (items.Count == 2)
            {
                Console.WriteLine("OK - Es wurden genau 2 Datensätze geladen");
            }

            // Get Parents 
            iqp.TypeOfQuery = QueryType.ParentsByChild;
            iqp.ChildGUID = SohnGUID;
            items.Clear();
            items = factory.GetItems(iqp);
            if (items.Count != 1)
            {
                Console.WriteLine("FAIL - Es wurden zu viel oder zu wenig Parents geladen");
                throw new Exception("Es wurden zu viele oder gar keine Datensätze gefunden");
            }
            if (items.FirstOrDefault().GUID != VaterGUID)
            {
                Console.WriteLine("FAIL - Es wurde NICHT der VATER geladen");
                throw new Exception("Vater wurde nicht gefunden");
            }
            if (items.FirstOrDefault().GUID == VaterGUID)
            {
                Console.WriteLine("OK - Es wurde genau der VATER geladen");
            }

            // Get Children 
            iqp.TypeOfQuery = QueryType.ChildrenByParent;
            iqp.ParentGUID = VaterGUID;
            items.Clear();
            items = factory.GetItems(iqp);
            if (items.Count != 1)
            {
                Console.WriteLine("FAIL - Es wurden zu viel oder zu wenig Children geladen");
                throw new Exception("Es wurden zu viele oder gar keine Datensätze gefunden");
            }
            if (items.FirstOrDefault().GUID != SohnGUID)
            {
                Console.WriteLine("FAIL - Es wurde NICHT der SOHN geladen");
                throw new Exception("Sohn wurde nicht gefunden");
            }
            if (items.FirstOrDefault().GUID == VaterGUID)
            {
                Console.WriteLine("OK - Es wurde genau der SOHN geladen");
            }

            // Remove 
            factory.RemoveRelation(VaterSohn);

            // Check 
            VaterSohnCheck = factory.GetRelation(VaterSohn);
            if (VaterSohnCheck != null)
            {
                Console.WriteLine("FAIL - Die Beziehung wurde nicht entfernt");
                throw new Exception("Entfernung der Beziehung wurde nicht vorgenommen");
            }
            if (VaterSohnCheck == null)
            {
                Console.WriteLine("OK - Die Beziehung wurde entfernt");
            }

            // Delete Vater 
            status = factory.Delete(inputVater);
            if (status.Status != "OK")
            {
                Console.WriteLine("FAIL - Vater wurde NICHT gelöscht");
                throw new Exception("DTO wurde nicht gelöscht");
            }
            if (status.Status == "OK")
            {
                Console.WriteLine("OK - Vater wurde gelöscht");
            }

            // Check 
            VaterDTO = factory.GetItemByID(inputVater);
            if (VaterDTO != null)
            {
                Console.WriteLine("FAIL - Vater konnte geladen werden");
                throw new Exception("DTO wurde nicht gelöscht");
            }
            if (VaterDTO == null)
            {
                Console.WriteLine("OK - Vater wurde nicht mehr geladen");
            }

            // Delete Sohn 
            status = factory.Delete(inputSohn);
            if (status.Status != "OK")
            {
                Console.WriteLine("FAIL - Sohn wurde nicht gelöscht");
                throw new Exception("DTO wurde nicht gelöscht");
            }
            if (status.Status == "OK")
            {
                Console.WriteLine("OK - Sohn wurde gelöscht");
            }
            // Check 
            SohnDTO = factory.GetItemByID(inputSohn);
            if (SohnDTO != null)
            {
                Console.WriteLine("FAIL - Sohn wurde mehr geladen");
                throw new Exception("DTO wurde nicht gelöscht");
            }
            if (SohnDTO == null)
            {
                Console.WriteLine("OK - Sohn wurde nicht mehr geladen");
            }

            Console.WriteLine("Hit Enter for Exit");
            Console.ReadLine();
        }
    }
}
