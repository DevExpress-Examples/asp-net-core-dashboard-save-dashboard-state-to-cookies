# Dashboard for ASP.NET Core - How to save a dashboard state to cookies

The example shows how to save the current dashboard state (such as master filter or parameter values) to cookies on the client side and restore this state on the server side.

![](web-dashboard-cookies.png)

## Client

The [DashboardControlOptions.onDashboardStateChanged](https://docs.devexpress.com/Dashboard/js-DevExpress.Dashboard.DashboardControlOptions?p=netframework#js_devexpress_dashboard_dashboardcontroloptions_ondashboardstatechanged) event occurs every time the dashboard state changes. In the event handler, the [DashboardControl.getDashboardState](https://docs.devexpress.com/Dashboard/js-DevExpress.Dashboard.DashboardControl?p=netframework#js_devexpress_dashboard_dashboardcontrol_getdashboardstate) method call gets the current dashboard state. The [document.cookie](https://www.w3schools.com/js/js_cookies.asp) property is used to save the dashboard state to cookies every time the state changes.

## Server

The [DashboardConfigurator.SetDashboardStateService](https://docs.devexpress.com/Dashboard/DevExpress.DashboardWeb.DashboardConfigurator.SetDashboardStateService(DevExpress.DashboardWeb.IDashboardStateService)) method
specifies a service that allows you to manage a dashboard state. In this service, the [HttpRequest.Cookies](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httprequest.cookies) property gets a collection of cookies sent by the client.

## Files to Look At

* [CustomDashboardStateService.cs](./CS/WebDashboardAspNetCore/CustomDashboardStateService.cs)
* [Startup.cs](./CS/WebDashboardAspNetCore/Startup.cs#L39)
* [Index.cshtml](../CS/WebDashboardAspNetCore/Views/Home/Index.cshtml)
* [Script.js](./CS/WebDashboardAspNetCore/wwwroot/js/Script.js)

## Documentation

- [Manage Dashboard State](https://docs.devexpress.com/Dashboard/119997/web-dashboard/aspnet-core-dashboard-control/manage-dashboard-state)

## More Examples

- [Dashboard for MVC - How to specify a default dashboard state in code](https://github.com/DevExpress-Examples/asp-net-mvc-dashboard-specify-default-state-in-code)
- [Dashboard for MVC - How to save a dashboard state to cookies](https://github.com/DevExpress-Examples/mvc-dashboard-save-dashboard-state-to-cookies)
- [Dashboard for Web Forms - How to Save a Dashboard State to Cookies](https://github.com/DevExpress-Examples/asp-net-web-forms-dashboard-save-dashboard-state-to-cookies)
