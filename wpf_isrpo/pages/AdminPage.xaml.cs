using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using UserApp;

namespace UserApp.pages
{
    public partial class AdminPage : Page
    {
        public List<Role> AssignableRoles { get; private set; }

        public AdminPage()
        {
            InitializeComponent();
            DataContext = this;
            AssignableRoles = DB.Context.Role.Where(r => r.id != SessionManager.RoleAdmin).ToList();
            ReloadUsers();
        }

        private void ReloadUsers()
        {
            var list = DB.Context.User
                .Include(u => u.Role)
                .Where(u => u.role_id != SessionManager.RoleAdmin)
                .ToList();
            UserGrid.ItemsSource = list;
        }

        private void UserRoleCombo_DropDownClosed(object sender, EventArgs e)
        {
            var cb = (ComboBox)sender;
            var user = cb.DataContext as User;
            if (user == null)
                return;
            var role = cb.SelectedItem as Role;
            if (role == null)
                return;
            if (user.role_id == role.id)
                return;

            user.role_id = role.id;
            user.Role = role;
            try
            {
                DB.Context.SaveChanges();
                MessageBox.Show("Роль сохранена.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                ReloadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                ReloadUsers();
            }
        }

        private void NavProducts_Click(object sender, RoutedEventArgs e) => PageNavigation.NavigateProducts(this);

        private void NavOrders_Click(object sender, RoutedEventArgs e) => PageNavigation.NavigateOrders(this);

        private void NavBack_Click(object sender, RoutedEventArgs e) => PageNavigation.GoBack(this);

        private void NavExit_Click(object sender, RoutedEventArgs e) => PageNavigation.Logout(this);
    }
}
