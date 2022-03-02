using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MonkeyCache.FileStore;
using Xamarin.Forms;

namespace DataCachedXF.ViewModels
{
    public class MainVM: BaseVM
    {
        private string data = "";
        public string Data
        { get => data; set => SetProperty(ref data, value); }

        private int counter = 0;

        public Command UpdateData { get; }

        public MainVM()
        {
            UpdateData = new Command(async () => Data = await LoadDataAsync());
            Data = "Test data!";
        }

        private async Task<string> LoadDataAsync()
        {
            Data = "...";
            await Task.Delay(1500);
            counter++;
            //return $"new data: {counter}!";
            return await this.GetDataAsync();
        }

        async Task<string> GetDataAsync()
        {
            var url = "https://montemagno.com/monkeys.json";

            //Dev handle online/offline scenario
            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                return Barrel.Current.Get<string>(key: url);
            }

            //Dev handles checking if cache is expired
            if (!Barrel.Current.IsExpired(key: url))
            {
                return Barrel.Current.Get<string>(key: url);
            }


            var client = new HttpClient();
            var json = await client.GetStringAsync(url);
            var data = json.ToString(); // JsonConvert.DeserializeObject<IEnumerable<Monkey>>(json);

            //Saves the cache and pass it a timespan for expiration
            Barrel.Current.Add(key: url, data: data, expireIn: TimeSpan.FromDays(1));

            return data;

        }
    }
}
