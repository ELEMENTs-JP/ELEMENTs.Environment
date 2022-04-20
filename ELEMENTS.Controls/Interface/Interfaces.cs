using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ELEMENTS
{
    public interface IFooter
    {
        IEnumerable<IFooterGroup> Groups { get; set; }
    }
    public interface IFooterGroup
    {
        string GroupTitle { get; set; }
        IEnumerable<IFooterLink> Links { get; set; }
    }
    public interface IFooterLink
    { 
        string LinkTitle { get; set; }
        string LinkUrl { get; set; }
    }
    public class FooterLink : IFooterLink
    {
        public string LinkTitle { get; set; }
        public string LinkUrl { get; set; }
    }
    public class FooterGroup : IFooterGroup
    {
        public string GroupTitle { get; set; }

        public IEnumerable<IFooterLink> Links { get; set; }
    }



    public class Footer : IFooter
    {
        public Footer()
        {

        }

        public IEnumerable<IFooterGroup> Groups { get; set; }


        // Load // Save 
        public void Save()
        {
            try
            {
                Footer config = this as Footer;
                if (config == null)
                {
                    Console.WriteLine("Konfiguration konnte nicht konvertiert werden");
                    return;
                }

                string json = ToJsonString(config);

                if (!Directory.Exists("./Configuration"))
                {
                    Directory.CreateDirectory("./Configuration");
                }

                File.WriteAllText("./Configuration/Footer.config", json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler: " + ex.Message);
            }
        }

        public static Footer Load()
        {
            try
            {
                if (!Directory.Exists("./Configuration"))
                {
                    Directory.CreateDirectory("./Configuration");
                }

                if (!File.Exists("./Configuration/Footer.config"))
                {
                    Footer config = new Footer();
                    config.Save();
                    return config;
                }

                string json = File.ReadAllText("./Configuration/Footer.config");
                Footer configuration = ToObjectFromJson(json);
                if (configuration == null)
                {
                    Console.WriteLine("Konfiguration konnte nicht konvertiert werden");
                    return null;
                }
                return configuration;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler: " + ex.Message);
            }

            return new Footer();
        }

        // JSON
        string ToJsonString(Footer theObject)
        {
            string json = string.Empty;
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };
                json = JsonSerializer.Serialize(theObject, options);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler: " + ex.Message);
            }
            return json;
        }
        static Footer ToObjectFromJson(string json)
        {
            Footer obj = new Footer();

            if (string.IsNullOrEmpty(json))
            {
                return obj;
            }

            try
            {
                obj = JsonSerializer.Deserialize<Footer>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler: " + ex.Message);
            }
            return obj;
        }

    }
}
