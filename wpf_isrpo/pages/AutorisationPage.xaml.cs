using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
using UserApp;

namespace UserApp.pages
{
    /// <summary>
    /// Логика взаимодействия для AutorisationPage.xaml
    /// </summary>
    public partial class AutorisationPage : Page
    {
        private User _currentUser = new User();
        public AutorisationPage()
        {
            InitializeComponent();
            DataContext = _currentUser;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage());
        }

        private void Auto_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            var existingUser = DB.Context.User.FirstOrDefault(u => u.email == _currentUser.email && u.password == _currentUser.password);
            if (existingUser == null)
                errors.AppendLine("Неверный логин или пароль");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            MessageBox.Show($"Добро пожаловать {existingUser.fio}", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            SessionManager.CurrentUser = existingUser;
            if (existingUser.role_id == SessionManager.RoleAdmin)
                NavigationService.Navigate(new AdminPage());
            else if (existingUser.role_id == SessionManager.RoleSeller)
                NavigationService.Navigate(new ProductSellerPage());
            else
                NavigationService.Navigate(new LKPage());
        }
    }
}
