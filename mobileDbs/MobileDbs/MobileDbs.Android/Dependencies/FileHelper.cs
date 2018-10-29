using System;
using System.IO;
using MobileDbs.Droid.Dependencies;
using MobileDbs.Infrastructure;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace MobileDbs.Droid.Dependencies
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}