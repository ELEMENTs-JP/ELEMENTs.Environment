using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ELEMENTS.Infrastructure;

namespace ELEMENTS
{
    public interface INavigationRepository
    {
        string Title { get; set; }
        string Text { get; set; }
        List<NavigationEntry> Store { get; set; }
        List<NavigationEntry> Items { get; set; }
        List<NavigationFilter> Groups { get; set; }
        void Filter(string FilterID);
        void Save();
    }
    public class NavigationRepository : INavigationRepository
    {
        public string Title { get; set; }
        public string Text { get; set; }

        public List<NavigationEntry> Store { get; set; } = new List<NavigationEntry>();
        public List<NavigationEntry> Items { get; set; } = new List<NavigationEntry>();
        public List<NavigationFilter> Groups { get; set; } = new List<NavigationFilter>();
        public void Filter(string GroupID)
        {
            try
            {
                Items = Store.Where(se => se.Group == GroupID).ToList();
            }
            catch (Exception ex)
            {

            }
        }

        // Load // Save 
        private static string relativeFilePath = "./Configuration";
        private static string fileName = "navigation.config";
        public void Save()
        {
            try
            {
                NavigationRepository config = this as NavigationRepository;
                if (config == null)
                {
                    Console.WriteLine("Datei konnte nicht konvertiert werden");
                    return;
                }

                string json = ToJsonString(config);

                if (!Directory.Exists(relativeFilePath))
                {
                    Directory.CreateDirectory(relativeFilePath);
                }

                File.WriteAllText(relativeFilePath + "/" + fileName, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAIL: " + ex.Message);
            }
        }

        public static NavigationRepository Load()
        {
            try
            {
                if (!Directory.Exists(relativeFilePath))
                {
                    Directory.CreateDirectory(relativeFilePath);
                }

                if (!File.Exists(relativeFilePath + "/" + fileName))
                {
                    NavigationRepository config = new NavigationRepository();
                    config.Save();
                    return config;
                }

                string json = File.ReadAllText(relativeFilePath + "/" + fileName);
                NavigationRepository configuration = ToObjectFromJson(json);
                if (configuration == null)
                {
                    Console.WriteLine("Datei konnte nicht konvertiert werden");
                    return null;
                }
                return configuration;
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAIL: " + ex.Message);
            }

            return new NavigationRepository();
        }

        // JSON 
        string ToJsonString(NavigationRepository theObject)
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
                Console.WriteLine("FAIL: " + ex.Message);
            }
            return json;
        }
        static NavigationRepository ToObjectFromJson(string json)
        {
            NavigationRepository obj = new NavigationRepository();

            if (string.IsNullOrEmpty(json))
            {
                return obj;
            }

            try
            {
                obj = JsonSerializer.Deserialize<NavigationRepository>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAIL: " + ex.Message);
            }
            return obj;
        }
    }

    public class NavigationFilter
    {
        public string ID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }
}
