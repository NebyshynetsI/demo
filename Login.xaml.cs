using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;

namespace WpfTotalnik
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        private void toggleTheme(object sender, RoutedEventArgs e)
        {
            ITheme theme = paletteHelper.GetTheme();
            if (IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                IsDarkTheme= false;
                theme.SetBaseTheme(Theme.Light);
                paletteHelper.SetTheme(theme);
            }
            else
            {
                IsDarkTheme= true;
                theme.SetBaseTheme(Theme.Dark);
                paletteHelper.SetTheme(theme);
            }
        }

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }
        public bool IsAuthenticated { get; set; }
        public void AuthUser(string userName, string password)
        {
            if(userName == "Vitya" && password == "SoftTeco")
            {
                IsAuthenticated= true;
            }
        }
        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            AuthUser(txtUsername.Text, txtPassword.Password);
            if(IsAuthenticated)
            {
                this.Hide();
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Wrong Creds!!!");
                txtUsername.Text = "";
                txtPassword.Password = "";
            }
        }
    }
}
