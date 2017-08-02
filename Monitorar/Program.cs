
using Monitorar.Class;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;

namespace Monitorar
{
    class Program
    {
        static void Main()
        {
            //Run.Log();

            HostFactory.Run(configurator =>
            {
                configurator.Service<Run>(s =>
                {
                    s.ConstructUsing(name => new Run());
                    s.WhenStarted((service, control) => service.Start(control));
                    s.WhenStopped((service, control) => service.Stop(control));
                });
                configurator.RunAsLocalSystem();

                configurator.SetDescription("Serviço responsável por converter arquivos para o Layout do SGO");
                configurator.SetDisplayName("Conversor_Abbott");
                configurator.SetServiceName("Conversor_Abbott");
            });
        }
    
    }
}
