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
using static Google.Apis.Auth.OAuth2.Web.AuthorizationCodeWebApp;
using System.Web.Configuration;

namespace GoogleCalendarIntegration.Services
{
    public class GoogleCalendarSyncer
    {

        public static string GetOauthTokenUri(Controller controller)
        {
            var authResult = GetAuthResult(controller);
            return authResult.RedirectUri;
        }

        public static bool SyncToGoogleCalendar(Controller controller)
        {
            try
            {
                var authResult = GetAuthResult(controller);

                var service = InitializeService(authResult);

                var calendarId = GetMainCalendarId(service);

                var calendarEvent = GetCalendarEvent();

                SyncCalendarEventToCalendar(service, calendarEvent, calendarId);
                return true;
            }
            catch (Exception ex)
            {
                GoogleOauthTokenService.OauthToken = null;
                return false;
            }
        }

        private static AuthResult GetAuthResult(Controller controller)
        {
            var dataStore = new DataStore();
            var clientID = WebConfigurationManager.AppSettings["GoogleClientID"];
            var clientSecret = WebConfigurationManager.AppSettings["GoogleClientSecret"];
            var appFlowMetaData = new GoogleAppFlowMetaData(dataStore, clientID, clientSecret);
            var factory = new AuthorizationCodeMvcAppFactory(appFlowMetaData, controller);
            var cancellationToken = new CancellationToken();
            var authCodeMvcApp = factory.Create();
            var authResultTask = authCodeMvcApp.AuthorizeAsync(cancellationToken);
            authResultTask.Wait();
            var result = authResultTask.Result;
            return result;
        }

        private static CalendarService InitializeService(AuthResult authResult)
        {
            var result = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = authResult.Credential,
                ApplicationName = "Google Calendar Integration Test"
            });
            return result;
        }

        private static string GetMainCalendarId(CalendarService service)
        {
            var calendarListRequest = new CalendarListResource.ListRequest(service);
            var calendars = calendarListRequest.Execute();
            var result = calendars.Items.First().Id;
            return result;
        }

        private static Event GetCalendarEvent()
        {
            var result = new Event();
            result.Summary = "Test Calendar Event Summary";
            result.Description = "Yay successfully done this part";
            result.Sequence = 1;
            var eventDate = new EventDateTime();
            eventDate.DateTime = DateTime.UtcNow;
            result.Start = eventDate;
            result.End = eventDate;
            return result;
        }

        private static void SyncCalendarEventToCalendar(CalendarService service, Event calendarEvent, string calendarId)
        {
            var eventRequest = new EventsResource.InsertRequest(service, calendarEvent, calendarId);
            eventRequest.Execute();
        }




    }
}