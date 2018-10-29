using MobileDbs.Infrastructure.SQLite.Dto;
using SQLite;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MobileDbs.Infrastructure.SQLite
{
    public class SQLiteManager
    {
        public SQLiteAsyncConnection DataBase { get; private set; }

        public SQLiteManager(IFileHelper fileHelper)
        {
            Task.Run(async () =>
            {
                await InitializeSQLite(fileHelper);
                await InitializeTables();
            }).Wait();
        }

        private async Task InitializeTables()
        {
            await DataBase.CreateTablesAsync<CompanyModelDto, CustomerModelDto, EmployeeModelDto>();
        }

        private async Task InitializeSQLite(IFileHelper fileHelper)
        {
            var dbName = "MobileDbs";
            var password = "xzy192134455";

            var dbPath = fileHelper.GetLocalFilePath($"{dbName}.db3");

            var isDbExist = System.IO.File.Exists(dbPath);

            var watch = Stopwatch.StartNew();

            var isConnected = await EncryptConnectionAsync(dbPath, password);

            watch.Stop();

            Debug.WriteLine($"**DataBase*** SQLite created instance from constructor by {watch.ElapsedMilliseconds}ms");
        }

        private async Task<bool> EncryptConnectionAsync(string dbPath, string password)
        {
            DataBase = new SQLiteAsyncConnection(dbPath);

            await DataBase.QueryAsync<int>($"PRAGMA key={password}");

            try
            {
                // Test query
                await DataBase.QueryAsync<int>("SELECT count(*) FROM sqlite_master");
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine(e);
                return false;
            }

            return true;
        }
    }
}
