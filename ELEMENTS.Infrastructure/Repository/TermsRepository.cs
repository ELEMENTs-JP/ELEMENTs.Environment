using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ELEMENTS
{
    public interface ITermsRepository
    {
        string Title { get; set; }
        string LastChange { get; set; }
        string Text { get; set; }
        void Save();
    }
    public class TermsRepository : ITermsRepository
    {
        public string Title { get; set; }
        public string LastChange { get; set; }
        public string Text { get; set; }

        // Load // Save 
        private static string relativeFilePath = "./Configuration";
        private static string fileName = "termsof.config";
        public void Save()
        {
            try
            {
                TermsRepository config = this as TermsRepository;
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

        public static TermsRepository Load()
        {
            try
            {
                if (!Directory.Exists(relativeFilePath))
                {
                    Directory.CreateDirectory(relativeFilePath);
                }

                if (!File.Exists(relativeFilePath + "/" + fileName))
                {
                    TermsRepository config = new TermsRepository();
                    config.Save();
                    return config;
                }

                string json = File.ReadAllText(relativeFilePath + "/" + fileName);
                TermsRepository configuration = ToObjectFromJson(json);
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

            return new TermsRepository();
        }

        // JSON
        string ToJsonString(TermsRepository theObject)
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
        static TermsRepository ToObjectFromJson(string json)
        {
            TermsRepository obj = new TermsRepository();

            if (string.IsNullOrEmpty(json))
            {
                return obj;
            }

            try
            {
                obj = JsonSerializer.Deserialize<TermsRepository>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAIL: " + ex.Message);
            }
            return obj;
        }
    }
}
