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
using UserApp;

namespace UserApp.pages
{
    /// <summary>
    /// Логика взаимодействия для AddProductPage.xaml
    /// </summary>
    public partial class AddProductPage : Page
    {
        public Product _product = new Product();
        public AddProductPage()
        {
            InitializeComponent();
            categoriesCombo.ItemsSource = DB.Context.Category.ToList();
            DataContext = _product;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_product.name))
                errors.AppendLine("Введите название");
            if (_product.Category == null)
                errors.AppendLine("Выберите категорию");
            if (_product.price <= 0)
                errors.AppendLine("Цена должна быть больше 0");
            if (_product.count < 0)
                errors.AppendLine("Количество не может быть отрицательным");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Проверка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _product.category_id = _product.Category.id;
                DB.Context.Product.Add(_product);
                DB.Context.SaveChanges();
                MessageBox.Show("Товар добавлен.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService?.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void NavProducts_Click(object sender, RoutedEventArgs e) => PageNavigation.NavigateProducts(this);

        private void NavOrders_Click(object sender, RoutedEventArgs e) => PageNavigation.NavigateOrders(this);

        private void NavBack_Click(object sender, RoutedEventArgs e) => PageNavigation.GoBack(this);

        private void NavExit_Click(object sender, RoutedEventArgs e) => PageNavigation.Logout(this);
    }
}
