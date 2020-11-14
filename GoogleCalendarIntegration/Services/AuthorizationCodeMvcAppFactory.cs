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
    public class AuthorizationCodeMvcAppFactory
    {
        public FlowMetadata FlowMetadata { get; }
        public Controller CurrentController { get; }

        public AuthorizationCodeMvcAppFactory(FlowMetadata flowMetadata, Controller currentController)
        {
            FlowMetadata = flowMetadata;
            CurrentController = currentController;
        }

        public AuthorizationCodeMvcApp Create()
        {
            var result = new AuthorizationCodeMvcApp(CurrentController, FlowMetadata);
            return result;
        }

    }
}