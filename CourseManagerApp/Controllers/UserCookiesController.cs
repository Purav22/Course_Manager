using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CourseManagerApp.Controllers
{
    public abstract class UserCookiesController : Controller
    {
        private static readonly string FirstVisitedDateCookieName = "first_visited_date";

        public string UserVisitMessage()
        {
            DateTime? firstVisitedDate = ManageFirstVisitedDate();
            return firstVisitedDate == null ? "Hey, Welcome to the Course Manager App!"
                : String.Format(@"Welcome back! You first used this app on: {0}", firstVisitedDate.Value.ToString("MM/dd/yyy hh:mm:ss tt", CultureInfo.InvariantCulture));
        }

        private DateTime? ManageFirstVisitedDate()
        {
            //getting the value from cookie.
            string? cookieValue = Request.Cookies[FirstVisitedDateCookieName];

            //extend the life of the cookie to 90 days.
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(60 * 24 * 90)
            };

            DateTime? firstVisitedDate = null;
            if (!string.IsNullOrEmpty(cookieValue))
            {
                firstVisitedDate = DateTime.Parse(cookieValue);
            }
            //updating value to cookie storage.
            Response.Cookies.Append(FirstVisitedDateCookieName,
                firstVisitedDate != null ? firstVisitedDate.ToString() : DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                cookieOptions);
            return firstVisitedDate;
        }
    }
}
