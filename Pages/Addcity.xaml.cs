using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace Weather
{
    /// <summary>
    /// Логика взаимодействия для Addcity.xaml
    /// </summary>
    public partial class Addcity : Page
    {
        Cities current_city = new Cities();
        public Addcity()
        {
            InitializeComponent();
        }

        private async void BtnFind_Click(object sender, RoutedEventArgs e)
        {
            string cit = TxCity.Text;
            var response = await Find(cit);
            Dictionary<string, string[]> citi = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(response);
            List<Cities> city_list = new List<Cities>();
            foreach (var citation in citi)
            {
                Cities curr = new Cities();
                curr.City = citation.Value[0];
                curr.longtitude = double.Parse(citation.Value[1].Replace(".",","));
                curr.latitude = double.Parse(citation.Value[2].Replace(".", ","));
                city_list.Add(curr);
            }

            DGridCity.ItemsSource = city_list;
        }
        static async Task<string> Find(string city)
        {
            HttpClient httpClient = new HttpClient();
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/get_geo/" + city);
            return await response.Content.ReadAsStringAsync();
        }
        public class Cities
        {
            public string City { get; set; }

            public double longtitude { get; set; }

            public double latitude { get; set; }

        }

        private async void Adds_Click(object sender, RoutedEventArgs e)
        {
            var current_city = DGridCity.SelectedItems.Cast<Cities>().ToList();
            slct.Text = current_city[0].City;
            var user_id = await GetId(User.Username, User.Password);
            var response = await AddTown(current_city[0].City, current_city[0].longtitude.ToString(), current_city[0].latitude.ToString(), user_id);
            if (response == "Success")
            {
                MessageBox.Show("Город успешно добавлен");
                manager.MainFrame.Navigate(new Weath());
            }
            else if (response == "City already exist")
            {
                MessageBox.Show("Город успешно добавлен");
                manager.MainFrame.Navigate(new Weath());
            }
            else
            {
                MessageBox.Show("Произошла ошибка!");
            }

        }
        static async Task<string> AddTown(string city, string longtitude, string latitude, string user_id)
        {
            HttpClient httpClient = new HttpClient();
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/add_city/"+city+"/"+longtitude+"/"+latitude+"/"+user_id);
            return await response.Content.ReadAsStringAsync();
        }

        static async Task<string> GetId(string username, string password)
        {
            HttpClient httpClient = new HttpClient();
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/get_user_id/" + username + "/" + password);
            return await response.Content.ReadAsStringAsync();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            manager.MainFrame.Navigate(new Weath());
        }

        private void BtnSelected_Click(object sender, RoutedEventArgs e)
        {
            current_city = (sender as Button).DataContext as Cities;
            slct.Text = current_city.City;
        }
    }
}
