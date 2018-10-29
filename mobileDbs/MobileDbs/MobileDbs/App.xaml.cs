using Prism;
using Prism.Ioc;
using MobileDbs.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Unity;
using MobileDbs.Infrastructure;
using MobileDbs.Domain.Services;
using MobileDbs.Infrastructure.Enums;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MobileDbs
{
    public partial class App : PrismApplication
    {
        private IContainerRegistry _containerRegistry;

        public DatabaseType DatabaseType { get; set; }

        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */

        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync($"NavigationPage/{nameof(SelectDatabasePage)}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            _containerRegistry = containerRegistry;
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SelectDatabasePage>();
            containerRegistry.RegisterForNavigation<PerformancePage>();
            containerRegistry.RegisterForNavigation<ConcurrencyPage>();
            containerRegistry.RegisterForNavigation<TestPerSecondPage>();
            containerRegistry.RegisterForNavigation<FilteringPage>();


            // By default register SQLite
            RegisterSQLite();
            RegisterDal(containerRegistry);
        }

        public void RegisterSQLite()
        {
            DatabaseType = DatabaseType.SQLite;

            _containerRegistry.Register<ICustomerRepository, Infrastructure.SQLite.CustomerRepository>();
            _containerRegistry.Register<ICompanyRepository, Infrastructure.SQLite.CompanyRepository>();
            _containerRegistry.Register<IEmployeeRepository, Infrastructure.SQLite.EmployeeRepository>();
        }

        public void RegisterRealm()
        {
            DatabaseType = DatabaseType.Realm;

            _containerRegistry.Register<ICustomerRepository, Infrastructure.Realm.CustomerRepository>();
            _containerRegistry.Register<ICompanyRepository, Infrastructure.Realm.CompanyRepository>();
            _containerRegistry.Register<IEmployeeRepository, Infrastructure.Realm.EmployeeRepository>();
        }

        private void RegisterDal(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ICompanyService, CompanyService>();
            containerRegistry.Register<ICustomerService, CustomerService>();
            containerRegistry.Register<IEmployeeService, EmployeeService>();

            containerRegistry.RegisterInstance(DependencyService.Get<IFileHelper>());

            containerRegistry.RegisterSingleton<Infrastructure.SQLite.SQLiteManager>();
            containerRegistry.RegisterSingleton<Infrastructure.Realm.RealmManager>();
        }
    }
}
