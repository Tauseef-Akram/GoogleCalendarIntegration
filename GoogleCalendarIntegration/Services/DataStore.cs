using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Json;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleCalendarIntegration.Services
{
    public class DataStore : IDataStore
    {
        public Task ClearAsync()
        {
            GoogleOauthTokenService.OauthToken = null;
            return Task.Delay(0);
        }

        public Task DeleteAsync<T>(string key)
        {
            GoogleOauthTokenService.OauthToken = null;
            return Task.Delay(0);
        }

        public Task<T> GetAsync<T>(string key)
        {
            var result = GoogleOauthTokenService.OauthToken;
            var value = result == null ? default(T) : NewtonsoftJsonSerializer.Instance.Deserialize<T>(result);
            return Task.FromResult<T>(value);
        }

        public Task StoreAsync<T>(string key, T value)
        {
            var jsonData = JsonConvert.SerializeObject(value);
            GoogleOauthTokenService.OauthToken = jsonData;
            return Task.Delay(0);
        }
    }
}