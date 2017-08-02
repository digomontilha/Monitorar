﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Monitorar
{

    class Run
    {


        public static void Log()
        {

            string[] args = Environment.GetCommandLineArgs();

            // If a directory is not specified, exit program.
            if (args.Length != 2)
            {
                // Display the proper way to call the program.
                Console.WriteLine("CMD: Monitorar.exe (directory)");
                return;
            }


            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = args[1];
            /* Watch for changes in LastAccess and LastWrite times, and
             the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            watcher.Filter = "*.*";

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);


            // Begin watching.
            watcher.EnableRaisingEvents = true;

            // Wait for the user to quit the program.
            Console.WriteLine("Digite \'s\' e enter para sair .");
            while (Console.Read() != 's') ;


            //Upload de arquivo as 23:00
            string Horario_determinado = "23:00";
            string Horario_atual = DateTime.Now.ToString("h:mm");

            if (Horario_determinado == Horario_atual)
            {
                Upload("serve", "TheUserName", "ThePassword", @"C:\file.txt");
                return;
            }


        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.

            StreamWriter writer = new StreamWriter("arquivos.log", true);
            DateTime data = DateTime.Now;


            Console.WriteLine(@"Arquivo: {0}, Evento: {1}, Data: {2}", e.FullPath, e.ChangeType, data);
            writer.WriteLine(@"Arquivo: {0}, Evento: {1}, Data: {2}", e.FullPath, e.ChangeType, data);
            writer.Close();

        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            StreamWriter writer = new StreamWriter("arquivos.log", true);
            DateTime data = DateTime.Now;




            Console.WriteLine(@"Arquivo: {0} alterou para {1}, Data:{2}", e.OldFullPath, e.FullPath, data);
            writer.WriteLine(@"Arquivo: {0} alterou para {1}, Data:{2}", e.OldFullPath, e.FullPath, data);
            writer.Close();
        }

        private static void Upload(string ftpServer, string userName, string password, string filename)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                client.Credentials = new System.Net.NetworkCredential(userName, password);
                client.UploadFile(ftpServer + "/" + new FileInfo(filename).Name, "STOR", filename);


                StreamWriter writer = new StreamWriter("arquivos.log", true);
                DateTime data = DateTime.Now;

                Console.WriteLine(@"Upload de Arquivo:{0} as {1}", filename, data);
                writer.WriteLine(@"Upload de Arquivo:{0} as {1}", filename, data);
            }
        }

    }

}
