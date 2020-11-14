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
using GoogleCalendarIntegration.Services;

namespace GoogleCalendarIntegration.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /*
        [HttpPost]
        public ActionResult Test (FormCollection form)
        {
            var eName = form["eventNameSet"];
            var eDesc = form["eventDetailSet"];
            DateTime date = new DateTime();
            date =System.Convert.ToDateTime(form["dateValue"]);

            var dataStore = new DataStore();
            var clientID = WebConfigurationManager.AppSettings["GoogleClientID"];
            var clientSecret = WebConfigurationManager.AppSettings["GoogleClientSecret"];
            var appFlowMetaData = new GoogleAppFlowMetaData(dataStore, clientID, clientSecret);
            var factory = new AuthorizationCodeMvcAppFactory(appFlowMetaData, this);
            var cancellationToken = new CancellationToken();
            var authCodeMvcApp = factory.Create();
            var authResultTask = authCodeMvcApp.AuthorizeAsync(cancellationToken);
            authResultTask.Wait();
            var result = authResultTask.Result;

            var result1 = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = result.Credential,
                ApplicationName = "Google Calendar Integration Test"
            });
          

            var calendarListRequest = new CalendarListResource.ListRequest(result1);
            var calendars = calendarListRequest.Execute();
            var result3 = calendars.Items.First().Id;

            var result4 = new Event();
            result4.Summary = eName;
            result4.Description = eDesc;
            result4.Sequence = 1;
            var eventDate = new EventDateTime();
            eventDate.DateTime = date;
            result4.Start = eventDate;
            result4.End = eventDate;
         

            var eventRequest = new EventsResource.InsertRequest(result1, result4, result3);
            eventRequest.Execute();

            return View("Index");
        }
        */
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult SyncToGoogleCalendar()
        {
            if (string.IsNullOrWhiteSpace(GoogleOauthTokenService.OauthToken))
            {
                var redirectUri = GoogleCalendarSyncer.GetOauthTokenUri(this);
                return Redirect(redirectUri);
            }
            else
            {
                var success = GoogleCalendarSyncer.SyncToGoogleCalendar(this);
                if (!success)
                {
                    return Json("Token was revoked. Try again.");
                }
            }
            return Redirect("~/");
        }

    }
}