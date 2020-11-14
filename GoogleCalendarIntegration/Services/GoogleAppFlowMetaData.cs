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
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Auth.OAuth2.Flows;
using System.Web.Mvc;

namespace GoogleCalendarIntegration.Services
{
    public class GoogleAppFlowMetaData : FlowMetadata
    {
        private IAuthorizationCodeFlow flow { get; set; }

        public override IAuthorizationCodeFlow Flow
        {
            get { return flow; }
        }

        public GoogleAppFlowMetaData(IDataStore dataStore, string clientID, string clientSecret)
        {
            var flowInitializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = clientID,
                    ClientSecret = clientSecret
                },
                Scopes = new[] { CalendarService.Scope.Calendar },
                DataStore = dataStore
            };
            flow = new GoogleAuthorizationCodeFlow(flowInitializer);
        }

        public override string GetUserId(Controller controller)
        {
            return "1";
        }

        public override string AuthCallback
        {
            get
            {
                return @"/AuthCallback/IndexAsync";
            }
        }
    }
}