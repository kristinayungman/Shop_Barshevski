using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using UserApp;

namespace UserApp.pages
{
    public partial class CheckoutPage : Page
    {
        public CheckoutPage()
        {
            InitializeComponent();
            ReloadLines();
        }

        private void ReloadLines()
        {
            var lines = DB.Context.Product.Where(p => p.count > 0).ToList()
                .Select(p => new PickLine(p))
                .ToList();
            PickList.ItemsSource = lines;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            var lines = PickList.ItemsSource as IEnumerable<PickLine>;
            if (lines == null)
                return;
            var chosen = lines.Where(x => x.IsSelected).ToList();
            if (chosen.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы один товар.", "Заказ", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var user = SessionManager.CurrentUser;
            foreach (var line in chosen)
            {
                var p = DB.Context.Product.Find(line.ProductId);
                if (p == null || p.count < 1)
                {
                    MessageBox.Show($"Товар недоступен: {line.Caption}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    ReloadLines();
                    return;
                }
            }

            try
            {
                foreach (var line in chosen)
                {
                    var p = DB.Context.Product.Find(line.ProductId);
                    p.count--;
                    DB.Context.Order.Add(new Order
                    {
                        user_id = user.id,
                        product_id = p.id,
                        timestamp = System.DateTime.Now,
                        status = OrderStatuses.Placed
                    });
                }
                DB.Context.SaveChanges();
                MessageBox.Show("Заказ оформлен.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new ZakazPage());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(FormatException(ex) + "\n\nЕсли в таблице [Order] колонка status ещё типа bit или nvarchar, выполните скрипт Scripts\\MigrateOrderStatusToInt.sql в SSMS.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NavProducts_Click(object sender, RoutedEventArgs e) => PageNavigation.NavigateProducts(this);

        private void NavCheckout_Click(object sender, RoutedEventArgs e) => PageNavigation.NavigateCheckout(this);

        private void NavOrders_Click(object sender, RoutedEventArgs e) => PageNavigation.NavigateOrders(this);

        private void NavBack_Click(object sender, RoutedEventArgs e) => PageNavigation.GoBack(this);

        private void NavExit_Click(object sender, RoutedEventArgs e) => PageNavigation.Logout(this);

        private static string FormatException(Exception ex)
        {
            var msg = ex.Message;
            for (var inner = ex.InnerException; inner != null; inner = inner.InnerException)
                msg += Environment.NewLine + "→ " + inner.Message;
            return msg;
        }

        public sealed class PickLine : INotifyPropertyChanged
        {
            private bool _isSelected;

            public PickLine(Product p)
            {
                ProductId = p.id;
                Caption = $"{p.name} — {p.price} руб. (остаток: {p.count})";
            }

            public int ProductId { get; }

            public string Caption { get; }

            public bool IsSelected
            {
                get => _isSelected;
                set { if (_isSelected == value) return; _isSelected = value; OnPropertyChanged(); }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged([CallerMemberName] string name = null) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
