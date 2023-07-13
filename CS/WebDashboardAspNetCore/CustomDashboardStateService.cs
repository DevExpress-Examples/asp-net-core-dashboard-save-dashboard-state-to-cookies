using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using Microsoft.AspNetCore.Http;
using System.Web;
using System.Xml.Linq;

namespace WebDashboardAspNetCore {
    public class CustomDashboardStateService : IDashboardStateService {
        IHttpContextAccessor? contextAccessor;
        public CustomDashboardStateService(IHttpContextAccessor? contextAccessor) {
            this.contextAccessor = contextAccessor;
        }
        public DashboardState? GetState(string dashboardId, XDocument dashboard) {
            var cookie = contextAccessor?.HttpContext?.Request.Cookies["dashboardState"];
            if (cookie != null) {
                DashboardState dashboardState = new DashboardState();
                dashboardState.LoadFromJson(HttpUtility.UrlDecode(cookie));
                return dashboardState;
            } else
                return null;
        }
    }
}
