using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace ELEMENTS
{
    public static class Helper
    {
        public static string ToSecureString(this object strg)
        {
            try
            {
                if (strg == null)
                    return string.Empty;

                return strg.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fail: " + ex.Message);
                return string.Empty;
            }
        }

        public static string SplitGetLast(string text, string separator = ".")
        {
            try
            {
                string[] arr = text.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
                int length = arr.Length;
                return arr[length - 1];
            }
            catch
            {
                return string.Empty;
            }
        }
    }

    public class FileNotification
    {
        public string FullFilePath { get; set; }
        public Guid FileGUID { get; set; }
        public string OriginalFileName { get; set; }
    }
    public class FileNotificationService
    {
        public event Func<FileNotification, Task> Notify;

        public void NotifySync(FileNotification notification)
        {
            if (Notify != null)
            {
                Notify.Invoke(notification);
            }
        }
    }

    public interface IFileUploadService
    {
        string GetContentRootPath();
        Task UploadAsync(Microsoft.AspNetCore.Components.Forms.IBrowserFile file);
        FileNotificationService NotificationService { get; }
    }

    public class FileUploadService : IFileUploadService
    {
        // https://docs.microsoft.com/de-de/aspnet/core/blazor/file-uploads?view=aspnetcore-6.0&pivots=server 

        public static string defaultFilePath = "FILES";
        private long defaultFileSizeInBytes = 1024 * 1024 * 50;
        private readonly IWebHostEnvironment _environment;
        public FileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;

            string contentRootPath = _environment.ContentRootPath;

            //ClientHelper.BaseDBsPath = contentRootPath + "\\" + defaultDBsPath + "\\";
            //ClientHelper.BaseFilesPath = contentRootPath + "\\" + defaultFilePath + "\\";
        }

        public FileNotificationService NotificationService { get; } = new FileNotificationService();
        public string GetContentRootPath()
        {
            return _environment.ContentRootPath;
        }
        public async Task UploadAsync(Microsoft.AspNetCore.Components.Forms.IBrowserFile file)
        {
            try
            {
                // Dateigröße 
                if (file.Size >= defaultFileSizeInBytes)
                    return;

                string contentRootPath = _environment.ContentRootPath;

                Guid itemGUID = Guid.NewGuid();
                string relativeSaveFilePath =
                    contentRootPath.ToString() + "\\" +
                    defaultFilePath.ToString();

                // Verzeichnis 
                if (!Directory.Exists(relativeSaveFilePath))
                {
                    Directory.CreateDirectory(relativeSaveFilePath);
                }
                if (!Directory.Exists(relativeSaveFilePath))
                    return;

                // Voller Dateipfad 
                var extension = Helper.SplitGetLast(file.Name);
                var fullFilePath = Path.Combine(relativeSaveFilePath, itemGUID.ToString() + "." + extension);

                // Write DOWN 
                await using FileStream fs = new(fullFilePath, FileMode.Create);
                Task f = file.OpenReadStream(defaultFileSizeInBytes).CopyToAsync(fs);
                await f;

                // Notification 
                FileNotification token = new FileNotification();
                token.FileGUID = itemGUID;
                token.FullFilePath = fullFilePath;
                token.OriginalFileName = file.Name;
                NotificationService.NotifySync(token);

                await Task.FromResult<string>("OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAIL: " + ex.Message);
            }
        }


    }

    public interface IFileDragDropService
    {
        string GetContentRootPath();
        Task UploadAsync(Microsoft.AspNetCore.Components.Forms.IBrowserFile file);
        FileNotificationService NotificationService { get; }
    }

    public class FileDragDropUploadService : IFileDragDropService
    {
        // https://docs.microsoft.com/de-de/aspnet/core/blazor/file-uploads?view=aspnetcore-6.0&pivots=server 
        public static string defaultFilePath = "FILES";
        private long defaultFileSizeInBytes = 1024 * 1024 * 50;
        private readonly IWebHostEnvironment _environment;
        public FileDragDropUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;

            string contentRootPath = _environment.ContentRootPath;

            //ClientHelper.BaseDBsPath = contentRootPath + "\\" + defaultDBsPath + "\\";
            //ClientHelper.BaseFilesPath = contentRootPath + "\\" + defaultFilePath + "\\";
        }
        // Properties 
        public FileNotificationService NotificationService { get; } = new FileNotificationService();
        public string GetContentRootPath()
        {
            return _environment.ContentRootPath;
        }
        public async Task UploadAsync(Microsoft.AspNetCore.Components.Forms.IBrowserFile file)
        {
            try
            {
                // Dateigröße 
                if (file.Size >= defaultFileSizeInBytes)
                    return;

                string contentRootPath = _environment.ContentRootPath;

                Guid itemGUID = Guid.NewGuid();
                string relativeSaveFilePath =
                    contentRootPath.ToString() + "\\" +
                    defaultFilePath.ToString();

                // Verzeichnis 
                if (!Directory.Exists(relativeSaveFilePath))
                {
                    Directory.CreateDirectory(relativeSaveFilePath);
                }
                if (!Directory.Exists(relativeSaveFilePath))
                    return;

                // Voller Dateipfad 
                var extension = Helper.SplitGetLast(file.Name);
                var fullFilePath = Path.Combine(relativeSaveFilePath, itemGUID.ToString() + "." + extension);

                // Write DOWN 
                await using FileStream fs = new(fullFilePath, FileMode.Create);
                Task f = file.OpenReadStream(defaultFileSizeInBytes).CopyToAsync(fs);
                await f;

                // Notification 
                FileNotification token = new FileNotification();
                token.FileGUID = itemGUID;
                token.FullFilePath = fullFilePath;
                token.OriginalFileName = file.Name;
                NotificationService.NotifySync(token);

                // Feedback 
                await Task.FromResult<string>("OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAIL: " + ex.Message);
            }
        }


    }



}
