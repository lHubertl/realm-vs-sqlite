using MobileDbs.Domain.Models;
using MobileDbs.Helpers;
using MobileDbs.Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileDbs.ViewModels
{
    public class FilteringPageViewModel : ViewModelBase
    {
        private string _foundItems;
        private ICustomerService _customerService;
        
        public string FoundItems {
            get => _foundItems;
            set => SetProperty(ref _foundItems, value);
        }
        
        public FilteringPageViewModel(INavigationService navigationService,
                                      ICustomerService customerService) : base(navigationService)
        {
            _customerService = customerService;
        }

        public ICommand FilteringCommand {
            get {
                return new DelegateCommand(async () =>
                {
                    FoundItems = string.Empty;
                    IsBusy = true;

                    var watch = Stopwatch.StartNew();
                    var filteringCustomers = (await _customerService.ReadByPredicate(item => item.Age < 35)).Data;
                    watch.Stop();
                    
                    FoundItems += String.Format("Customers: {0} \n Time: {1}ms\n", filteringCustomers.Count(), watch.ElapsedMilliseconds);

                    IsBusy = false;

                });
            }
        }
        public ICommand AddMoreCommand {
            get {
                return new DelegateCommand(async () => {
                    var message = await TestingTools.Diagnostic(_customerService.GenerateRecord(1000), "Create customers by {0}ms");
                    FoundItems += String.Format("{0} - Count 1000\n", message);
                });
            }
        }
       
    }
}
