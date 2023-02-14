using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Input.Manipulations;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using ScottPlot;

namespace Weather
{
    /// <summary>
    /// Логика взаимодействия для Weath.xaml
    /// </summary>
    public partial class Weath : Page
    {
        public string Mode;
        public Weath()
        {
            InitializeComponent();
            Mode = "Month";
            fnstart();
        }

        private async void fnstart()
        {
            var user_id = await GetId(User.Username, User.Password);
            var user_cities = await Find(user_id.ToString());
            List<Cities> citi = JsonConvert.DeserializeObject<List<Cities>>(user_cities);
            Selectedcity.ItemsSource = citi;
            Selectedcity.SelectedValuePath = "Id";
            Selectedcity.DisplayMemberPath = "City1";
        }
        static async Task<string> Find(string user_id)
        {
            HttpClient httpClient = new HttpClient();
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/get_cities/" + user_id);
            return await response.Content.ReadAsStringAsync();
        }
        static async Task<string> GetId(string username, string password)
        {
            HttpClient httpClient = new HttpClient();
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/get_user_id/" + username + "/" + password);
            return await response.Content.ReadAsStringAsync();
        }


        static async Task<string> Get_current(string city_id)
        {
            HttpClient httpClient = new HttpClient();
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/get_current/" + city_id);
            return await response.Content.ReadAsStringAsync();
        }


        static async Task<string> Get_min(string city_id)
        {
            HttpClient httpClient = new HttpClient();
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/get_minimum/" + city_id);
            return await response.Content.ReadAsStringAsync();
        }

        static async Task<string> Get_max(string city_id)
        {
            HttpClient httpClient = new HttpClient();
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/get_maximum/" + city_id);
            return await response.Content.ReadAsStringAsync();
        }
        static async Task<string> Get_graph(string city_id)
        {
            HttpClient httpClient = new HttpClient();
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/get_graph/" + city_id);
            return await response.Content.ReadAsStringAsync();
        }
        /*
                private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
                {
                    if (e.ChangedButton == MouseButton.Left)
                        this.DragMove();
                }*/



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private async void set_data()
        {
            if (Selectedcity.SelectedValue != null)
            {
                {
                    if (Mode == "Month")
                    {
                        current.Text = await Get_current(Selectedcity.SelectedValue.ToString()) + "°c";
                        var min = await Get_min(Selectedcity.SelectedValue.ToString());
                        string[] mins = min.Replace(",", ";").Replace(".", ",").Split("[")[1].Split("]")[0].Split(";");
                        double[] mini = new double[mins.Count()];
                        for (int i = 0; i < mins.Count(); i++)
                        {
                            mini[i] = double.Parse(mins[i]);
                        }
                        Min.MaxNum = mini.Min().ToString() + "°c";
                        var max = await Get_max(Selectedcity.SelectedValue.ToString());
                        string[] maxs = max.Replace(",", ";").Replace(".", ",").Split("[")[1].Split("]")[0].Split(";");
                        double[] maxi = new double[mins.Count()];
                        for (int i = 0; i < maxs.Count(); i++)
                        {
                            maxi[i] = double.Parse(maxs[i]);
                        }
                        Max.MaxNum = maxi.Max().ToString() + "°c";
                        var avg = await Get_graph(Selectedcity.SelectedValue.ToString());
                        string[] avgs = avg.Replace(",", ";").Replace(".", ",").Split("[")[1].Split("]")[0].Split(";");
                        double[] av = new double[avgs.Count()];
                        for (int i = 0; i < avgs.Count(); i++)
                        {
                            av[i] = double.Parse(avgs[i]);
                        }
                        double a = av.Average();
                        Avg.MaxNum = Math.Round(a, 2).ToString() + "°c";
                        double[] dataY = av;
                        double[] dataX = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13,14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
                        /*                        for (int i = 0; i < av.Count(); i++)
                                                {
                                                    dataX[i] = i;
                                                }*/
                        WpfPlot1.Plot.Clear();
                        WpfPlot1.Plot.AddSignal(dataY, sampleRate: 30);
                        WpfPlot1.Plot.SetAxisLimits(0, 30, -50, 50);
                        WpfPlot1.Plot.Title("Weather");
/*                       WpfPlot1.Plot.Clear();
                        WpfPlot1.Plot.AddScatter(dataX, dataY);*/
                        WpfPlot1.Refresh();

                    }
                    else
                    {
                        current.Text = await Get_current(Selectedcity.SelectedValue.ToString()) + "°c";
                        var min = await Get_min(Selectedcity.SelectedValue.ToString());
                        string[] mins = min.Replace(",", ";").Replace(".", ",").Split("[")[1].Split("]")[0].Split(";");
                        double[] mini = new double[7];
                        int n = 0;
                        for (int i = mins.Count()-7; i < mins.Count(); i++)
                        {
                            mini[n] = double.Parse(mins[i]);
                            n++;
                        }
                        Min.MaxNum = mini.Min().ToString() + "°c";
                        var max = await Get_max(Selectedcity.SelectedValue.ToString());
                        string[] maxs = max.Replace(",", ";").Replace(".", ",").Split("[")[1].Split("]")[0].Split(";");
                        double[] maxi = new double[7];
                        n = 0;
                        for (int i = maxs.Count()-7; i < maxs.Count(); i++)
                        {
                            maxi[n] = double.Parse(maxs[i]);
                            n++;
                        }
                        Max.MaxNum = maxi.Max().ToString() + "°c";
                        var avg = await Get_graph(Selectedcity.SelectedValue.ToString());
                        string[] avgs = avg.Replace(",", ";").Replace(".", ",").Split("[")[1].Split("]")[0].Split(";");
                        double[] av = new double[160];
                        n = 0;
                        for (int i = avgs.Count()-160; i < avgs.Count(); i++)
                        {
                            av[n] = double.Parse(avgs[i]);
                            n++;
                        }
                        double a = av.Average();
                        Avg.MaxNum = Math.Round(a, 2).ToString() + "°c";
                        double[] dataY = av;
                        double[] dataX = new double[av.Count()];
                        for (int i = 0; i < av.Count(); i++)
                        {
                            dataX[i] = i;
                        }
                        WpfPlot1.Plot.Clear();
                        WpfPlot1.Plot.AddSignal(dataY, sampleRate: 24);
                        WpfPlot1.Plot.SetAxisLimits(0, 7, -50, 50);
                        WpfPlot1.Plot.Title("Weather");
                        WpfPlot1.Refresh();
                    }
                }
            }
            else { };
        }
        private async void Selectedcity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            set_data();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            manager.MainFrame.Navigate(new Addcity());
        }
        public class Cities
        {
            public long Id { get; set; }

            public string City1 { get; set; } = null!;

            public double? Longitude { get; set; }

            public double? Latitude { get; set; }

            public virtual ICollection<int> CityInfos { get; set; }

            public virtual ICollection<int> UserInfos { get; set; }

        }

        private void Week_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Week.Style = (System.Windows.Style)Application.Current.Resources["activeTextButton"];
            Month.Style = (System.Windows.Style)Application.Current.Resources["textButton"];
            Mode = "Week";
            set_data();
        }

        private void Month_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Month.Style = (System.Windows.Style)Application.Current.Resources["activeTextButton"];
            Week.Style = (System.Windows.Style)Application.Current.Resources["textButton"];
            Mode = "Month";
            set_data();
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            {// удаление нескольких пользователей
                var usersForRemoving = Selectedcity.SelectedValue.ToString();
                if (MessageBox.Show($"Удалить город?",
                    "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)

                    try {
                        var user_id = await GetId(User.Username, User.Password);
                        var result = await Del(user_id, Selectedcity.SelectedValue.ToString());
                        if (result == "Success") MessageBox.Show("Данные удалены");
                        fnstart();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }


            }
        }
        static async Task<string> Del(string user_id, string city_id)
        {
            HttpClient httpClient = new HttpClient();
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/delete/" + user_id+"/"+city_id);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
