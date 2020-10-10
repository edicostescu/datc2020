using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace L03
{
    class Program
    {
        static string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = "datc-2020/L03";
        static string filePath = "F:/UPT/Anul 4, SEM 1/OPTIONALE_AN4.txt";
        static DriveService service;
        static void init()
        {
            UserCredential credential;

            using (var stream = new FileStream("client_id.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token";

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    Environment.UserName,
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }
        static void printAllFiles()
        {
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 100;
            listRequest.Fields = "nextPageToken, files(id, name)";
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                    Console.WriteLine(file.Name + " having ID: " + file.Id);
            }
            else
            {
                Console.WriteLine("List is empty!");
            }
        }
        static void uploadFile()
        {
            if (System.IO.File.Exists(filePath))
            {
                Google.Apis.Drive.v3.Data.File body = new Google.Apis.Drive.v3.Data.File();
                body.Name = System.IO.Path.GetFileName(filePath);
                byte[] byteArray = System.IO.File.ReadAllBytes(filePath);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
                try
                {
                    FilesResource.CreateMediaUpload request = service.Files.Create(body, stream, filePath);
                    request.SupportsTeamDrives = true;
                    request.Upload();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("The file does not exist.");
            }
        }
        static void Main(string[] args)
        {
            init();
            uploadFile();
            printAllFiles();
        }
    }
}
