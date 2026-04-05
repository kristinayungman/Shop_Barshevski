using System;
using System.Collections.Generic;
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

namespace UserApp.pages
{
    /// <summary>
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        private User _currentUser = new User();
        public RegisterPage()
        {
            InitializeComponent();
            DataContext = _currentUser;
        }

        private void Auto_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AutorisationPage());
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentUser.fio))
                errors.AppendLine("Укажите фио");
            if (string.IsNullOrWhiteSpace(_currentUser.password))
                errors.AppendLine("Укажите пароль");
            if (_currentUser.password.Length < 6)
                errors.AppendLine("Пароль должен содержать минимум 6 символов");

            bool chars, digits, uppercaseLetter, lowercaseLetter;
            chars = digits = uppercaseLetter = lowercaseLetter = false;
            char[] charsArr = { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '+', '№', ';', ':', '?', '.', ',' };

            for (int i = 0; i < charsArr.Length; i++)
            {
                if (_currentUser.password.Contains(charsArr[i]))
                {
                    chars = true;
                    break;
                }
            }

            for (int i = 0; i < _currentUser.password.Length; i++)
            {
                if (char.IsDigit(_currentUser.password[i]))
                {
                    digits = true;
                    break;
                }
            }

            for (int i = 0; i < _currentUser.password.Length; i++)
            {
                if (char.IsLetter(_currentUser.password[i]))
                {
                    if (char.IsUpper(_currentUser.password[i]))
                        uppercaseLetter = true;
                    else
                        lowercaseLetter = true;

                    if (uppercaseLetter && lowercaseLetter)
                        break;
                }
            }

            //if (!chars)
            //    errors.AppendLine("Пароль должен содержать хотя бы 1 специальный символ");
            //if (!digits)
            //    errors.AppendLine("Пароль должен содержать хотя бы 1 цифру");
            //if (!lowercaseLetter)
            //    errors.AppendLine("Пароль должен содержать хотя бы 1 маленькую букву");
            //if (!uppercaseLetter)
            //    errors.AppendLine("Пароль должен содержать хотя бы 1 большую букву");


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            _currentUser.role_id = 1;
            if (_currentUser.id == 0)
                DB.Context.User.Add(_currentUser);
            try
            {
                DB.Context.SaveChanges();
                MessageBox.Show("Информация сохранена", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                SessionManager.CurrentUser = _currentUser;
                NavigationService.Navigate(new LKPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
