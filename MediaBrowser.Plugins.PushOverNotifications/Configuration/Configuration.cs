using Emby.Notifications;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Model.Plugins;
using System.Collections.Generic;
using System;

namespace MediaBrowser.Plugins.PushOverNotifications
{
    /// <summary>
    /// Class PluginConfiguration
    /// </summary>
    public class NotificationsConfigurationFactory : IConfigurationFactory
    {
        public IEnumerable<ConfigurationStore> GetConfigurations()
        {
            return new[]
            {
                new ConfigurationStore
                {
                     ConfigurationType = typeof(PushoverNotificationsOptions),
                     Key = "pushovernotifications"
                }
            };
        }
    }
    public static class NotificationsConfigExtension
    {
        public static PushoverNotificationsOptions GetNotificationsOptions(this IConfigurationManager config)
        {
            return config.GetConfiguration<PushoverNotificationsOptions>("pushovernotifications");
        }

        public static NotificationInfo[] GetConfiguredNotifications(this IConfigurationManager config)
        {
            return config.GetNotificationsOptions().Notifications;
        }

        public static void SaveNotificationsConfiguration(this IConfigurationManager config, PushoverNotificationsOptions options)
        {
            config.SaveConfiguration("pushovernotifications", options);
        }
    }

    public class PushoverNotificationsOptions
    {
        public PushoverNotificationInfo[] Notifications { get; set; } = Array.Empty<PushoverNotificationInfo>();
    }

    public class PushoverNotificationInfo : NotificationInfo
    {
        public string Token { get; set; }
        public string UserKey { get; set; }
        public string DeviceName { get; set; }
    }
}
