using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UserApp;

namespace UserApp.pages
{
    public partial class Prodavec : Page
    {
        public List<KeyValuePair<int, string>> StatusChoices => OrderStatuses.ChoiceList;

        public Prodavec()
        {
            InitializeComponent();
            DataContext = this;
            TitleBlock.Text = SessionManager.CurrentUser.role_id == SessionManager.RoleAdmin
                ? "Все заказы"
                : "Заказы покупателей";

            OrderGrid.ItemsSource = DB.Context.Order.ToList();
        }

        private void StatusCombo_DropDownClosed(object sender, EventArgs e)
        {
            var cb = sender as ComboBox;
            var order = cb?.DataContext as Order;
            if (order == null)
                return;

            order.status = OrderStatuses.NormalizeCode(order.status);
            try
            {
                DB.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(FormatException(ex), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static string FormatException(Exception ex)
        {
            var msg = ex.Message;
            for (var inner = ex.InnerException; inner != null; inner = inner.InnerException)
                msg += Environment.NewLine + "→ " + inner.Message;
            return msg;
        }

        private void NavProducts_Click(object sender, RoutedEventArgs e) => PageNavigation.NavigateProducts(this);

        private void NavOrders_Click(object sender, RoutedEventArgs e) => PageNavigation.NavigateOrders(this);

        private void NavBack_Click(object sender, RoutedEventArgs e) => PageNavigation.GoBack(this);

        private void NavExit_Click(object sender, RoutedEventArgs e) => PageNavigation.Logout(this);
    }
}
