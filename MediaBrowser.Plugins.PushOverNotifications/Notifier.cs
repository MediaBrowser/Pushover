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
using MediaBrowser.Controller;
using System.Web;

namespace MediaBrowser.Plugins.PushOverNotifications
{
    public class Notifier : IUserNotifier
    {
        private ILogger _logger;
        private IServerApplicationHost _appHost;
        private IHttpClient _httpClient;

        public Notifier(ILogger logger, IServerApplicationHost applicationHost, IHttpClient httpClient)
        {
            _logger = logger;
            _appHost = applicationHost;
            _httpClient = httpClient;
        }

        private Plugin Plugin => _appHost.Plugins.OfType<Plugin>().First();

        public string Name => Plugin.StaticName;

        public string Key => "pushovernotifications";

        public string SetupModuleUrl => Plugin.NotificationSetupModuleUrl;

        public async Task SendNotification(InternalNotificationRequest request, CancellationToken cancellationToken)
        {
            var options = request.Configuration.Options;

            options.TryGetValue("Token", out string token);
            options.TryGetValue("UserKey", out string userKey);
            options.TryGetValue("DeviceName", out string deviceName);
            options.TryGetValue("Priority", out string priority);

            var parameters = new Dictionary<string, string>
                {
                    {"token", token},
                    {"user", userKey},
                };

            var messageTitle = HttpUtility.UrlEncode(request.Title);

            if (!string.IsNullOrEmpty(deviceName))
                parameters.Add("device", deviceName);

            if (string.IsNullOrEmpty(request.Description))
                parameters.Add("message", messageTitle);
            else
            {
                var description = HttpUtility.UrlEncode(request.Description);
                parameters.Add("title", messageTitle);
                parameters.Add("message", description);
            }

            if (!string.IsNullOrEmpty(priority))
                parameters.Add("priority", priority);

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
