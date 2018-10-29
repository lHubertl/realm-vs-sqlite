using Prism.Mvvm;
using Prism.Navigation;
using System.Threading;
using Xamarin.Forms;

namespace MobileDbs.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        private int busyRequest = 0;

        protected INavigationService NavigationService { get; private set; }

        public string Title { get; }

        public bool IsBusy
        {
            get => busyRequest > 0;
            set
            {
                var result = value ? Interlocked.Increment(ref busyRequest) : Interlocked.Decrement(ref busyRequest);
                if (result == busyRequest)
                {
                    RaisePropertyChanged(nameof(IsBusy));
                }
                else
                {
                    // TODO: TestIt
                    throw new System.InvalidOperationException();
                }
            }
        }

        public ViewModelBase(INavigationService navigationService)
        {
            Title = (Application.Current as App)?.DatabaseType.ToString();
            NavigationService = navigationService;
        }

        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {
            
        }

        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
            
        }

        public virtual void Destroy()
        {
            
        }
    }
}
