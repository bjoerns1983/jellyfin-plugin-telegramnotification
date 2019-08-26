using System;
using System.Collections.Generic;
using Jellyfin.Plugin.TelegramNotification.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using System.IO;

namespace Jellyfin.Plugin.TelegramNotification
{
    public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
    {
        public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer) : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
        }
        public override string Name
        {
            get { return "Telegram Notifications"; }
        }

        public override string Description
        {
            get
            {
                return "Sends notifications via Telegram Service.";
            }
        }

        public override Guid Id => Guid.Parse("37eab764-2033-4c03-8ba0-651d19c08c72");


        public static Plugin Instance { get; private set; }

        public IEnumerable<PluginPageInfo> GetPages()
        {
            return new[]
            {
                new PluginPageInfo
                {
                    Name = Name,
                    EmbeddedResourcePath = GetType().Namespace + ".Configuration.config.html"
                }
            };
        }
    }
}
