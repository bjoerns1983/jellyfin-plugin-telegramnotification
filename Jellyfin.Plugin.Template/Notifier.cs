using System.Collections.Generic;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Notifications;
using Microsoft.Extensions.Logging;
using Jellyfin.Plugin.TelegramNotification.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Jellyfin.Plugin.TelegramNotification
{
    public class Notifier : INotificationService
    {
        private readonly ILogger _logger;
        private readonly IHttpClient _httpClient;

        public Notifier(ILogger logger, IHttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public bool IsEnabledForUser(User user)
        {
            var options = GetOptions(user);

            return options != null && IsValid(options) && options.Enabled;
        }

        private TeleGramOptions GetOptions(User user)
        {
            return Plugin.Instance.Configuration.Options
                .FirstOrDefault(i => string.Equals(i.MediaBrowserUserId, user.Id.ToString("N"), StringComparison.OrdinalIgnoreCase));
        }

        public string Name
        {
            get { return Plugin.Instance.Name; }
        }

        public async Task SendNotification(UserNotification request, CancellationToken cancellationToken)
        {

            var options = GetOptions(request.User);
            string message = Uri.EscapeDataString(request.Name);

            if (string.IsNullOrEmpty(request.Description) == false && options.SendDescription == true)
            {
                message = Uri.EscapeDataString(request.Name + "\n\n" + request.Description); 
            }

            _logger.LogDebug("TeleGram to Token : {0} - {1} - {2}", options.BotToken, options.ChatID, request.Name);

            var _httpRequest = new HttpRequestOptions
            {
                Url = "https://api.telegram.org/bot" + options.BotToken + "/sendmessage?chat_id=" + options.ChatID + "&text=" + message,
                CancellationToken = cancellationToken
            };

            using (await _httpClient.Post(_httpRequest).ConfigureAwait(false))
            {

            }
        }

        private bool IsValid(TeleGramOptions options)
        {
            return !string.IsNullOrEmpty(options.ChatID) &&
                !string.IsNullOrEmpty(options.BotToken);
        }
    }
}
