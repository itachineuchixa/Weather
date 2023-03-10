using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.Threading.Tasks;

namespace Weather
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }
        private async void BtnAuth_Click(object sender, RoutedEventArgs e)
        {
            string username = TxbLogin.Text;
            string password = PsbPassword.Password.ToString();
            var response = await Auth(username, password);
            if (response.ToString() == "Success")
            {
                User.Password = password;
                User.Username = username;
                manager.MainFrame.Navigate(new Weath());
            }
            else { MessageBox.Show("Неверные данные!"); };
        }
        static async Task<string> Auth(string username, string password)
        {
            HttpClient httpClient = new HttpClient();
            var dt = DateTime.Now;
            DateTime month = dt.AddMonths(-1);
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/login/"+username+"/"+password);
            return await response.Content.ReadAsStringAsync();
        }


        private void Reg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            manager.MainFrame.Navigate(new Register());
        }
    }
}
