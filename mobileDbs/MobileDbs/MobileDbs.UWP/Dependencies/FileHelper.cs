using System.IO;
using Windows.Storage;
using MobileDbs.UWP.Dependencies;
using Xamarin.Forms;
using MobileDbs.Infrastructure;

[assembly: Dependency(typeof(FileHelper))]
namespace MobileDbs.UWP.Dependencies
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);
        }
    }
}
