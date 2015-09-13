using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using Autofac;
using Autofac.Extras.Attributed;
using Autofac.Features.Metadata;
using Autofac.Integration.Mef;
using log4net;
using log4net.Config;
using Parmalen.Contracts;
using Parmalen.Contracts.Intent;

namespace Parmalen.Engine
{
    internal class Program
    {
        private static DirectoryCatalog _directoryCatalog;
        private static IContainer _container;
        private static ILog _log;

        public static void Main()
        {
            SetupMef();
            SetupAutofac();
            SetupLog4Net();

            var wit = _container.Resolve<WitService>();
            try
            {
                while (true)
                {
                    var task = wit.CaptureTextIntentAsync("Como va a estar el tiempo mañana en Buenos Aires?").Result;
                    //var task = wit.CaptureSpeechIntentAsync().Result;
                    if (task != null)
                    {
                        foreach (var outcome in task.Outcomes)
                        {
                            var intent = _container.Resolve<IEnumerable<Meta<IIntent>>>()
                                .SingleOrDefault(x => x.Metadata["Name"].Equals(outcome.Intent));
                            if (intent == null)
                            {
                                _log.ErrorFormat("There is no plugin implemented for the intent: {0}", outcome.Intent);
                                continue;
                            }
                            intent.Value.Run(outcome.Entities);
                        }
                    }
                }
            }
            catch (ApplicationException e)
            {
                _log.FatalFormat("Error connecting to Wit Service, app will now close", e);
                return;
            }
        }

        private static void SetupLog4Net()
        {
            XmlConfigurator.Configure();
            _log = LogManager.GetLogger(typeof(Program));
        }

        private static void SetupMef()
        {
            var extensionsFolder = AppDomain.CurrentDomain.BaseDirectory;
            var fileSystemWatcher = new FileSystemWatcher(extensionsFolder, "*.dll");

            _directoryCatalog = new DirectoryCatalog(extensionsFolder);

            fileSystemWatcher.Changed += ExtensionsFolderUpdated;
            fileSystemWatcher.Created += ExtensionsFolderUpdated;
            fileSystemWatcher.Deleted += ExtensionsFolderUpdated;
            fileSystemWatcher.Renamed += ExtensionsFolderUpdated;
        }

        private static void SetupAutofac()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<AttributedMetadataModule>();
            builder.RegisterModule<LoggingModule>();
            builder.RegisterComposablePartCatalog(_directoryCatalog);
            builder.RegisterType<WitService>();

            _container = builder.Build();
        }

        private static void ExtensionsFolderUpdated(object sender, FileSystemEventArgs e)
        {
            _directoryCatalog.Refresh();
        }
    }
}
