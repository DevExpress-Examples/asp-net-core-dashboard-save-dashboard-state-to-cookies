function onBeforeRender(sender) {
    const dashboardControl = sender;
    if (dashboardControl)
        dashboardControl.on('dashboardStateChanged', onDashboardStateChanged);
}

function onDashboardStateChanged(e) {
    // Set the number of days until the cookie should expire (exdays):
    const exdays = 1;
    const date = new Date();
    date.setTime(date.getTime() + (exdays * 24 * 60 * 60 * 1000));
    let expires = "expires=" + date.toUTCString();
    // Get the dashboard state:
    let dState = "dashboardState=" + encodeURIComponent(e.component.getDashboardState()) + ";";
    // Assign the cookie name (dashboardState), the cookie value, and the expires string to document.cookie:
    document.cookie = dState + expires + ";path=" + encodeURIComponent("/");
}