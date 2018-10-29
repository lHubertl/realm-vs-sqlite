using System.Windows.Input;
using MobileDbs.Infrastructure;
using Prism.Navigation;
using Xamarin.Forms;

namespace MobileDbs.ViewModels
{
    public class SelectDatabasePageViewModel : ViewModelBase
	{
	    private string _selectedDbName;
	    public string SelectedDbName
        {
	        get => _selectedDbName;
	        set => SetProperty(ref _selectedDbName, value);
	    }

	    private bool _dbSwitcher;
	    public bool DbSwitcher
        {
	        get => _dbSwitcher;
	        set
	        {
	            SetProperty(ref _dbSwitcher, value);
	            if (value)
	            {
                    (Application.Current as App)?.RegisterSQLite();
                }
	            else
	            {
                    (Application.Current as App)?.RegisterRealm();
                }

                SelectedDbName = (Application.Current as App)?.DatabaseType.ToString();
            }
	    }

	    public ICommand NavigateToTestPage => new Command<object>(async (o) => { await NavigationService.NavigateAsync(o as string); });

        public ICommand CleanDbCommand => new Command(async() =>
        {
            var customerService = (ICustomerService)(Application.Current as App)?.Container.Resolve(typeof(ICustomerService));
            var employeeService = (IEmployeeService)(Application.Current as App)?.Container.Resolve(typeof(IEmployeeService));
            var companyService = (ICompanyService)(Application.Current as App)?.Container.Resolve(typeof(ICompanyService));
            customerService?.ClearAll();
            employeeService?.ClearAll();
            companyService?.ClearAll();
            await (Application.Current as App)?.MainPage.DisplayAlert("Alert", $"{SelectedDbName} cleared all records", "OK");
        });

        public SelectDatabasePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _dbSwitcher = (Application.Current as App)?.DatabaseType == Infrastructure.Enums.DatabaseType.SQLite;
        }
    }
}
