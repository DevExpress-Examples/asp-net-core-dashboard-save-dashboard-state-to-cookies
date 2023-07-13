using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Excel;
using DevExpress.DataAccess.Sql;
using Microsoft.Extensions.FileProviders;

namespace WebDashboardAspNetCore {
    public static class DashboardUtils {
        public static DashboardConfigurator CreateDashboardConfigurator(IConfiguration configuration, IFileProvider fileProvider, IHttpContextAccessor? contextAccessor) {

            DashboardConfigurator configurator = new DashboardConfigurator();
            configurator.SetConnectionStringsProvider(new DashboardConnectionStringsProvider(configuration));

            DashboardFileStorage dashboardFileStorage = new DashboardFileStorage(fileProvider.GetFileInfo("Data/Dashboards").PhysicalPath);
            configurator.SetDashboardStorage(dashboardFileStorage);

            DataSourceInMemoryStorage dataSourceStorage = new DataSourceInMemoryStorage();

            configurator.SetDashboardStateService(new CustomDashboardStateService(contextAccessor));

            // Registers an SQL data source.
            DashboardSqlDataSource sqlDataSource = new DashboardSqlDataSource("SQL Data Source", "sqliteConnection");
            sqlDataSource.DataProcessingMode = DataProcessingMode.Client;
            DevExpress.DataAccess.Sql.SelectQuery query = SelectQueryFluentBuilder
                .AddTable("SalesPerson")
                .SelectAllColumns()
                .Build("Sales Person");
            sqlDataSource.Queries.Add(query);
            dataSourceStorage.RegisterDataSource("sqlDataSource", sqlDataSource.SaveToXml());

            configurator.SetDataSourceStorage(dataSourceStorage);


            configurator.ConfigureDataConnection += (s, e) => {
                if (e.ConnectionName == "sqliteConnection") {
                    SQLiteConnectionParameters sqliteParams = new SQLiteConnectionParameters();
                    sqliteParams.FileName = "Data/nwind.db";
                    e.ConnectionParameters = sqliteParams;
                }
            };
            return configurator;
        }
    }
}
