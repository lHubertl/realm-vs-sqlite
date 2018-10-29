using MobileDbs.Domain.Models;
using MobileDbs.Helpers;
using MobileDbs.Infrastructure;
using Prism.Navigation;
using System;
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
    public class TestPerSecondPageViewModel : ViewModelBase
    {
        private string _testResult;
        private int _generalCount;
        private StringBuilder _strBld;
        private List<int> _averageCounts;
        private ICustomerService _customerService;
        private List<CustomerModel> _cachedCustomers;

        private List<int> _createAverage;
        private List<int> _readAverage;
        private List<int> _updateAverage;
        private List<int> _deleteAverage;

        public string TestResult
        {
            get => _testResult;
            set => SetProperty(ref _testResult, value);
        }

        public int GeneralCount
        {
            get => _generalCount;
            set => SetProperty(ref _generalCount, value);
        }

        private int _averageCount;
        public int AverageCount
        {
            get => _averageCount;
            set => SetProperty(ref _averageCount, value);
        }

        private string _averageCrud;
        public string AverageCrud
        {
            get => _averageCrud;
            set => SetProperty(ref _averageCrud, value);
        }

        public ICommand TestCommand => new Command(async () => await ExecuteTestCommand());

        public TestPerSecondPageViewModel(INavigationService navigationService, ICustomerService customerService) : base(navigationService)
        {
            _customerService = customerService;
        }

        private async Task ExecuteTestCommand()
        {
            IsBusy = true;

            // Clear data
            TestResult = "";
            _strBld = new StringBuilder();
            _averageCounts = new List<int>();
            _createAverage = new List<int>();
            _readAverage = new List<int>();
            _updateAverage = new List<int>();
            _deleteAverage = new List<int>();
            _cachedCustomers = new List<CustomerModel>();

            await ExecuteTest();

            IsBusy = false;
        }

        private async Task ExecuteTest()
        {
            for (int i = 0; i < 10; i++)
            {
                GeneralCount = 0;
                _strBld.Clear();
                _cachedCustomers.Clear();
                await _customerService.ClearAll();

                await TestingTools.CancelTaskAfterTime(GenerateCustomers, 1000);
                // need to better R.U.D. operations
                _cachedCustomers.AddRange((await _customerService.GenerateRecord(1000)).Data);
                await TestingTools.CancelTaskAfterTime(ReadCustomers, 1000);
                await TestingTools.CancelTaskAfterTime(UpdateCustomers, 1000);
                await TestingTools.CancelTaskAfterTime(DeleteCustomers, 1000);

                _averageCounts.Add(GeneralCount);
                AverageCount = (int)_averageCounts.Average();
                AverageCrud =
                    $"C: {(int)_createAverage.Average()} | " +
                    $"R: {(int)_readAverage.Average()} | " +
                    $"U: {(int)_updateAverage.Average()} | " +
                    $"D: {(int)_deleteAverage.Average()}";
            }
        }

        private async Task GenerateCustomers(CancellationToken token)
        {
            int count = 0;

            var watch = Stopwatch.StartNew();

            while (!token.IsCancellationRequested)
            {
                _cachedCustomers.AddRange((await _customerService.GenerateRecord(1)).Data);
                count++;
            }

            watch.Stop();
            GeneralCount += count;
            _createAverage.Add(count);
            _strBld.AppendLine($"Generated {count} customers for {watch.ElapsedMilliseconds}ms");
            TestResult = _strBld.ToString();
        }

        private async Task ReadCustomers(CancellationToken token)
        {
            int count = 0;

            var watch = Stopwatch.StartNew();
            List<CustomerModel> customers = null;

            int id = 0;
            while (!token.IsCancellationRequested)
            {
                if(id >=  _cachedCustomers.Count - 1)
                {
                    id = 0;
                }
                id++;

                customers = (await _customerService.ReadById(_cachedCustomers[id].Guid)).Data?.ToList();
                count++;
            }

            watch.Stop();

            GeneralCount += count;
            _readAverage.Add(count);
            _strBld.AppendLine($"Readed {count} times for {watch.ElapsedMilliseconds}ms");
            TestResult = _strBld.ToString();
        }

        private async Task UpdateCustomers(CancellationToken token)
        {
            int count = 0;

            var watch = Stopwatch.StartNew();

            for (int i = 0; i < _cachedCustomers.Count; i++)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                if (i == _cachedCustomers.Count - 1)
                {
                    i = 0;
                }

                await _customerService.UpdateRecord(_cachedCustomers[i]);
                count++;
            }

            watch.Stop();

            GeneralCount += count;
            _updateAverage.Add(count);
            _strBld.AppendLine($"Updated {count} customers for {watch.ElapsedMilliseconds}ms");
            TestResult = _strBld.ToString();
        }

        private async Task DeleteCustomers(CancellationToken token)
        {
            int count = 0;

            var watch = Stopwatch.StartNew();

            for (int i = 0; i < _cachedCustomers.Count; i++)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                if (i == _cachedCustomers.Count - 1)
                {
                    i = 0;
                }

                await _customerService.DeleteRecord(_cachedCustomers[i]);
                count++;
            }

            watch.Stop();

            GeneralCount += count;
            _deleteAverage.Add(count);
            _strBld.AppendLine($"Deleted {count} customers for {watch.ElapsedMilliseconds}ms");
            TestResult = _strBld.ToString();
        }
    }
}
