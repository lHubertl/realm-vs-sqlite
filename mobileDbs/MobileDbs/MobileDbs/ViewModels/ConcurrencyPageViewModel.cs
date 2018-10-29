using System;
using MobileDbs.Domain.Models;
using MobileDbs.Helpers;
using MobileDbs.Infrastructure;
using Prism.Navigation;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileDbs.ViewModels
{
    public class ConcurrencyPageViewModel : ViewModelBase
    {
        private int _generatingCustomersCount = 1000;
        private string _testResult;
        private string _generalTime;
        private StringBuilder _strBld = new StringBuilder();
        private ICustomerService _customerService;

        private List<CustomerModel> _customerCollectionFromViewModel;
        private List<CustomerModel> _customersfromDb;

        public string TestResult
        {
            get => _testResult;
            set => SetProperty(ref _testResult, value);
        }

        public string GeneralTime
        {
            get => _generalTime;
            set => SetProperty(ref _generalTime, value);
        }

        public ICommand TestCommand => new Command(async () => await ExecuteTestCommand());

        public ConcurrencyPageViewModel(INavigationService navigationService, ICustomerService customerService) : base(navigationService)
        {
            _customerService = customerService;
        }

        private async Task ExecuteTestCommand()
        {
            IsBusy = true;

            // Clear data
            TestResult = "";
            _strBld.Clear();
            _customerCollectionFromViewModel = new List<CustomerModel>();
            _customersfromDb = new List<CustomerModel>();

            await CleanAllCustomers();

            GeneralTime = await TestingTools.Diagnostic(ExecuteTest(), "{0}");

            IsBusy = false;
        }

        private async Task ExecuteTest()
        {
            int iterations = 10;

            // Generate 10 000 customers in 10 threads
            for (int i = 0; i < iterations; i++)
            {
                var threadStart = new ThreadStart(CreateCustomers);
                Thread thread = new Thread(threadStart);
                thread.Start();
            }

            // Wait while customers will be created
            while (_customerCollectionFromViewModel.Count <= _generatingCustomersCount * (iterations - 1))
            {
                await Task.Delay(300);
                Debug.WriteLine($"Wait for creating {_generatingCustomersCount * iterations} customers");
            }

            //Get all customers from db
            var customersFromDb = await ReadAllCustomersAsync();

            //Compare all customers
            CheckIfAllCustomersHasRightData();
        }

        private async Task CleanAllCustomers()
        {
            var message = await TestingTools.Diagnostic(_customerService.ClearAll(), "Delete customers by {0}ms");
            _strBld.AppendLine($"{message} - Count {_generatingCustomersCount} ||| Thread id = {Thread.CurrentThread.ManagedThreadId}");
            UpdateLabel();
        }

        private void CreateCustomers()
        {
            var action = new Action(() =>
                _customerCollectionFromViewModel.AddRange(_customerService.GenerateRecord(_generatingCustomersCount)
                    .Result.Data));
            var message = TestingTools.Diagnostic(action, "Create customers by {0}ms");
            _strBld.AppendLine($"{message} - Count {_generatingCustomersCount} ||| Thread id = {Thread.CurrentThread.ManagedThreadId}");
            UpdateLabel();
        }

        private async Task<List<CustomerModel>> ReadAllCustomersAsync()
        {
            var readCustomerTask = Task.Run(async () => {
                _customersfromDb.AddRange((await _customerService.ReadAllRecords()).Data.ToList());
            });
            var message = await TestingTools.Diagnostic(readCustomerTask, "Read customers by {0}ms");
            _strBld.AppendLine($"{message} - Count {_customersfromDb?.Count}");

            UpdateLabel();

            return _customersfromDb;
        }

        private void CheckIfAllCustomersHasRightData()
        {
            int errorCount = 0;
            var watch = Stopwatch.StartNew();

            _strBld.AppendLine();
            try
            {
                _customersfromDb.OrderBy(x => x.Guid);
                _customerCollectionFromViewModel.OrderBy(x => x.Guid);

                if(_customerCollectionFromViewModel.Count == _customersfromDb.Count)
                {
                    for (int i = 0; i < _customerCollectionFromViewModel.Count; i++)
                    {
                        var customerVm = _customerCollectionFromViewModel[i];
                        var customerDb = _customersfromDb[i];

                        if (customerVm.Guid == customerDb.Guid)
                        {
                            if (customerDb.Age != customerVm.Age
                                || customerDb.Name != customerVm.Name
                                || customerDb.IsActive != customerVm.IsActive
                                || customerDb.LastVisit != customerVm.LastVisit
                                || customerDb.Salary != customerVm.Salary)
                            {
                                errorCount++;
                            }
                            continue;
                        }
                        else
                        {
                            errorCount++;
                        }
                    }
                }
                else
                {
                    _strBld.AppendLine($"Error: Different count of data");
                }
            }
            catch (System.Exception e)
            {
                _strBld.AppendLine(e.Message);
                errorCount++;
            }

            watch.Stop();
            _strBld.AppendLine($"Was checked {_customerCollectionFromViewModel.Count} items from ViewModel and {_customersfromDb.Count} from DataBase");
            _strBld.AppendLine($"End checking the data for {watch.ElapsedMilliseconds}ms. Result: {errorCount} errors");
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            TestResult = _strBld.ToString();
        }
    }
}
