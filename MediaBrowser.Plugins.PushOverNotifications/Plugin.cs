using System;
using System.Collections.Generic;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using System.IO;
using MediaBrowser.Model.Drawing;
using System.Linq;

namespace MediaBrowser.Plugins.PushOverNotifications
{
    /// <summary>
    /// Class Plugin
    /// </summary>
    public class Plugin : BasePlugin, IHasWebPages, IHasThumbImage, IHasTranslations
    {
        public IEnumerable<PluginPageInfo> GetPages()
        {
            return new[]
            {
                new PluginPageInfo
                {
                    Name = "pushovernotifications",
                    EmbeddedResourcePath = GetType().Namespace + ".Configuration.pushover.html",
                    EnableInMainMenu = true,
                    MenuIcon = "notifications"
                },
                new PluginPageInfo
                {
                    Name = "pushovernotificationsjs",
                    EmbeddedResourcePath = GetType().Namespace + ".Configuration.pushover.js"
                },
                new PluginPageInfo
                {
                    Name = "pushovernotificationeditorjs",
                    EmbeddedResourcePath = GetType().Namespace + ".Configuration.pushovereditor.js"
                },
                new PluginPageInfo
                {
                    Name = "pushovereditortemplate",
                    EmbeddedResourcePath = GetType().Namespace + ".Configuration.pushovereditor.template.html"
                }
            };
        }

        public TranslationInfo[] GetTranslations()
        {
            var basePath = GetType().Namespace + ".strings.";

            return GetType()
                .Assembly
                .GetManifestResourceNames()
                .Where(i => i.StartsWith(basePath, StringComparison.OrdinalIgnoreCase))
                .Select(i => new TranslationInfo
                {
                    Locale = Path.GetFileNameWithoutExtension(i.Substring(basePath.Length)),
                    EmbeddedResourcePath = i

                }).ToArray();
        }

        private Guid _id = new Guid("6C3B6965-C257-47C2-AA02-64457AE21D91");
        public override Guid Id
        {
            get { return _id; }
        }

        public static string StaticName = "Pushover Notifications";

        /// <summary>
        /// Gets the name of the plugin
        /// </summary>
        /// <value>The name.</value>
        public override string Name
        {
            get { return StaticName; }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public override string Description
        {
            get
            {
                return "Sends notifications via Pushover Service.";
            }
        }

        public Stream GetThumbImage()
        {
            var type = GetType();
            return type.Assembly.GetManifestResourceStream(type.Namespace + ".thumb.png");
        }

        public ImageFormat ThumbImageFormat
        {
            get
            {
                return ImageFormat.Png;
            }
        }
    }
}
