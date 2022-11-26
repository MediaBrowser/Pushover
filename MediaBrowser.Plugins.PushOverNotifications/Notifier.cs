using System.Collections.Generic;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Notifications;
using MediaBrowser.Model.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Emby.Notifications;
using MediaBrowser.Controller.Configuration;

namespace MediaBrowser.Plugins.PushOverNotifications
{
    public class Notifier : INotifier
    {
        private IServerConfigurationManager _config;
        private ILogger _logger;
        private IHttpClient _httpClient;

        public static string TestNotificationId = "system.pushovernotificationtest";
        public Notifier(IServerConfigurationManager config, ILogger logger, IHttpClient httpClient)
        {
            _config = config;
            _logger = logger;
            _httpClient = httpClient;
        }

        public string Name
        {
            get { return Plugin.StaticName; }
        }

        public NotificationInfo[] GetConfiguredNotifications()
        {
            return _config.GetConfiguredNotifications();
        }

        public async Task SendNotification(InternalNotificationRequest request, CancellationToken cancellationToken)
        {
            var options = request.Configuration as PushoverNotificationInfo;

            var parameters = new Dictionary<string, string>
                {
                    {"token", options.Token},
                    {"user", options.UserKey},
                };

            // TODO: Improve this with escaping based on what PushOver api requires
            var messageTitle = request.Title.Replace("&", string.Empty);

            if (!string.IsNullOrEmpty(options.DeviceName))
                parameters.Add("device", options.DeviceName);

            if (string.IsNullOrEmpty(request.Description))
                parameters.Add("message", messageTitle);
            else
            {
                parameters.Add("title", messageTitle);
                parameters.Add("message", request.Description);
            }

            _logger.Debug("PushOver to Token : {0} - {1} - {2}", options.Token, options.UserKey, request.Description);

            var httpRequestOptions = new HttpRequestOptions
            {
                Url = "https://api.pushover.net/1/messages.json",
                CancellationToken = cancellationToken
            };

            httpRequestOptions.SetPostData(parameters);

            using (await _httpClient.Post(httpRequestOptions).ConfigureAwait(false))
            {

            }
        }
    }
}
