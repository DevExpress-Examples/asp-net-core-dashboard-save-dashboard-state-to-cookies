using DevExpress.AspNetCore;
using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Sql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace WebDashboardAspNetCore {
    public class Startup {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment) {
            Configuration = configuration;
            FileProvider = hostingEnvironment.ContentRootFileProvider;
        }

        public IConfiguration Configuration { get; }
        public IFileProvider FileProvider { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            // Configures services to use the Web Dashboard Control.
            services.AddHttpContextAccessor();

            services
                .AddDevExpressControls()
                .AddControllersWithViews();
            services.AddScoped<DashboardConfigurator>((System.IServiceProvider serviceProvider) => {
                DashboardConfigurator configurator = new DashboardConfigurator();
                configurator.SetDashboardStorage(new DashboardFileStorage(FileProvider.GetFileInfo("Data/Dashboards").PhysicalPath));
                configurator.SetConnectionStringsProvider(new DashboardConnectionStringsProvider(Configuration));
                configurator.SetDataSourceStorage(CreateDataSourceStorage());
                configurator.ConfigureDataConnection += Configurator_ConfigureDataConnection;
                configurator.SetDashboardStateService(new CustomDashboardStateService(serviceProvider.GetService<IHttpContextAccessor>()));
                return configurator;
            });
        }

        private void Configurator_ConfigureDataConnection(object sender, ConfigureDataConnectionWebEventArgs e) {
            if (e.ConnectionName == "sqliteConnection") {
                SQLiteConnectionParameters sqliteParams = new SQLiteConnectionParameters();
                sqliteParams.FileName = "file:Data/nwind.db";
                e.ConnectionParameters = sqliteParams;
            }
        }

        public DataSourceInMemoryStorage CreateDataSourceStorage() {
            DataSourceInMemoryStorage dataSourceStorage = new DataSourceInMemoryStorage();

            DashboardSqlDataSource sqlDataSource = new DashboardSqlDataSource("SQL Data Source", "sqliteConnection");
            sqlDataSource.DataProcessingMode = DataProcessingMode.Client;
            DevExpress.DataAccess.Sql.SelectQuery query = SelectQueryFluentBuilder
                .AddTable("SalesPerson")
                .SelectAllColumns()
                .Build("Sales Person");
            sqlDataSource.Queries.Add(query);
            dataSourceStorage.RegisterDataSource("sqlDataSource", sqlDataSource.SaveToXml());

            return dataSourceStorage;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            // Registers the DevExpress middleware.
            app.UseDevExpressControls();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                // Maps the dashboard route.
                EndpointRouteBuilderExtension.MapDashboardRoute(endpoints, "api/dashboard", "DefaultDashboard");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
