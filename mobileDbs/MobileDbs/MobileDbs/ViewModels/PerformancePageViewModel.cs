using MobileDbs.Domain.Models;
using MobileDbs.Helpers;
using MobileDbs.Infrastructure;
using Prism.Navigation;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileDbs.ViewModels
{
    public class PerformancePageViewModel : ViewModelBase
    {
        private const int GenerateRecordsCount = 1000;
        private string _testResult;
        private string _generalTime;
        private string _averageTime;
        private List<long> _allTime;

        private StringBuilder _strBld = new StringBuilder();
        private ICompanyService _companyService;
        private ICustomerService _customerService;
        private IEmployeeService _employeeService;

        private IList<CustomerModel> _customers;
        private IList<EmployeeModel> _employees;
        private IList<CompanyModel> _companies;

        public string TestResult
        {
            get => _testResult;
            set => SetProperty(ref _testResult, value);
        }

        public string GeneralTime {
            get => _generalTime;
            set => SetProperty(ref _generalTime, value);
        }

        public string AverageTime {
            get => _averageTime;
            set => SetProperty(ref _averageTime, value);
        }

        public List<long> AllTime {
            get => _allTime;
            set => SetProperty(ref _allTime, value);
        }

        public ICommand TextCommand => new Command<string>( async (str) =>
        {
            AllTime = new List<long>();
            switch (str)
            {
                case "0":

                    for (int i = 0; i < 30; i++)
                    {
                        await TestInOrder();
                    }
                    break;
                case "1":

                    for (int i = 0; i < 30; i++)
                    {
                        await TestNotInOrder();
                    }
                    break;
            }
            AverageTime = AllTime.Average().ToString("N0");
        });

        public PerformancePageViewModel(INavigationService navigationService,
                                        ICompanyService companyService,
                                        ICustomerService customerService,
                                        IEmployeeService employeeService) : base(navigationService)
        {
            _companyService = companyService;
            _customerService = customerService;
            _employeeService = employeeService;

            GeneralTime = "0";
        }

        private async Task TestInOrder()
        {
            IsBusy = true;
            _strBld.Clear();
            var watch = Stopwatch.StartNew();

            await CreateCustomers();
            await CreateEmployees();
            await CreateCompanies();

            await ReadAllCompaniesAsync();
            await ReadAllCustomersAsync();
            await ReadAllEmployeesAsync();

            await UpdateAllCompanies();
            await UpdateAllCustomers();
            await UpdateAllEmployees();

            await DeleteAllCustomers();
            await DeleteAllEmployees();
            await DeleteAllCompanies();

            watch.Stop();
            GeneralTime = watch.ElapsedMilliseconds.ToString();
            AllTime.Add(watch.ElapsedMilliseconds);
            IsBusy = false;
        }

        private async Task TestNotInOrder()
        {
            IsBusy = true;
            
            _strBld.Clear();

            var watch = Stopwatch.StartNew();

            await Task.WhenAll(CreateCompanies(), CreateCustomers(), CreateEmployees());
            await Task.WhenAll(ReadAllCompaniesAsync(), ReadAllCustomersAsync(), ReadAllEmployeesAsync());
            await Task.WhenAll(UpdateAllCompanies(), UpdateAllCustomers(), UpdateAllEmployees());
            await Task.WhenAll(DeleteAllCompanies(), DeleteAllCustomers(), DeleteAllEmployees());

            watch.Stop();
            GeneralTime = watch.ElapsedMilliseconds.ToString();
            AllTime.Add(watch.ElapsedMilliseconds);
            IsBusy = false;
        }

        #region Create

        private async Task CreateCustomers()
        {
            var message = await  TestingTools.Diagnostic( _customerService.GenerateRecord(GenerateRecordsCount), "Create customers by {0}ms");
            _strBld.AppendLine($"{message} - Count {GenerateRecordsCount}");
            UpdateResult();
        }

        private async Task CreateEmployees()
        {
            var message = await TestingTools.Diagnostic(_employeeService.GenerateRecord(GenerateRecordsCount), "Create employees by {0}ms");
            _strBld.AppendLine($"{message} - Count {GenerateRecordsCount}");
            UpdateResult();
        }

        private async Task CreateCompanies()
        {
            var message = await TestingTools.Diagnostic(_companyService.GenerateRecord(GenerateRecordsCount), "Create companies by {0}ms");
            _strBld.AppendLine($"{message} - Count {GenerateRecordsCount}");
            UpdateResult();
        }
       
        #endregion

        #region Read
        
        private async Task ReadAllCustomersAsync()
        {
            var readCustomerTask = Task.Run(async () => {
                _customers = (await _customerService.ReadAllRecords()).Data.ToList();
            });
            var message = await TestingTools.Diagnostic(readCustomerTask, "Read customers by {0}ms");
            _strBld.AppendLine($"{message} - Count {_customers?.Count}");
            UpdateResult();
        }

        private async Task ReadAllEmployeesAsync()
        {
            var readEmployeesTask = Task.Run(async () => {
                _employees = (await _employeeService.ReadAllRecords()).Data.ToList();
            });
            var message = await TestingTools.Diagnostic(readEmployeesTask, "Read employees by {0}ms");
            _strBld.AppendLine($"{message} - Count {_employees?.Count}");
            UpdateResult();
        }

        private async Task ReadAllCompaniesAsync()
        {
            var readCompaniesTask = Task.Run(async () => {
                _companies = (await _companyService.ReadAllRecords()).Data.ToList();
            });
            var message = await TestingTools.Diagnostic(readCompaniesTask, "Read companies by {0}ms");
            _strBld.AppendLine($"{message} - Count {_companies?.Count}");
            UpdateResult();
        }

        #endregion

        #region Update

        private async Task UpdateAllCustomers()
        {
            var message = await TestingTools.Diagnostic(_customerService.UpdateAllRecords(_customers), "Update customers by {0}ms");
            _strBld.AppendLine($"{message} - Count {_customers?.Count}");
            UpdateResult();

            var all = await _customerService.ReadAllRecords();
        }

        private async Task UpdateAllEmployees()
        {
            var message = await TestingTools.Diagnostic(_employeeService.UpdateAllRecords(_employees), "Update employees by {0}ms");
            _strBld.AppendLine($"{message} - Count {_employees?.Count}");
            UpdateResult();
        }

        private async Task UpdateAllCompanies()
        {
            var message = await TestingTools.Diagnostic(_companyService.UpdateAllRecords(_companies), "Update companies by {0}ms");
            _strBld.AppendLine($"{message} - Count {_companies?.Count}");
            UpdateResult();
        }
       
        #endregion

        #region Delete

        private async Task DeleteAllCustomers()
        {
            var message = await TestingTools.Diagnostic(_customerService.DeleteAllRecords(_customers), "Delete customers by {0}ms");
            _strBld.AppendLine($"{message} - Count {_customers?.Count}");
            UpdateResult();
        }

        private async Task DeleteAllEmployees()
        {
            var message = await TestingTools.Diagnostic(_employeeService.DeleteAllRecords(_employees), "Delete employees by {0}ms");
            _strBld.AppendLine($"{message} - Count {_employees?.Count}");
            UpdateResult();
        }

        private async Task DeleteAllCompanies()
        {
            var message = await TestingTools.Diagnostic(_companyService.DeleteAllRecords(_companies), "Delete companies by {0}ms");
            _strBld.AppendLine($"{message} - Count {_companies?.Count}");
            UpdateResult();
        }

        #endregion

        private void UpdateResult()
        {
            TestResult = _strBld.ToString();
        }
    }
}
