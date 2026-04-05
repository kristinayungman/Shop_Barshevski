using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using UserApp;

namespace UserApp.pages
{
    public partial class ProductSellerPage : Page
    {
        public ProductSellerPage()
        {
            InitializeComponent();
            RefreshGrid();
            categoryCombo.ItemsSource = DB.Context.Category.ToList();
            ProductGrid.CellEditEnding += ProductGrid_CellEditEnding;
        }

        private void RefreshGrid()
        {
            ProductGrid.ItemsSource = DB.Context.Product.ToList();
        }

        private void ProductGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction != DataGridEditAction.Commit)
                return;
            try
            {
                DB.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddProductPage());
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            var product = ProductGrid.SelectedItem as Product;
            if (product == null)
            {
                MessageBox.Show("Выберите товар в таблице.", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (MessageBox.Show($"Удалить товар «{product.name}»?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            try
            {
                DB.Context.Product.Remove(product);
                DB.Context.SaveChanges();
                MessageBox.Show("Товар удалён.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NavProducts_Click(object sender, RoutedEventArgs e) => PageNavigation.NavigateProducts(this);

        private void NavOrders_Click(object sender, RoutedEventArgs e) => PageNavigation.NavigateOrders(this);

        private void NavBack_Click(object sender, RoutedEventArgs e) => PageNavigation.GoBack(this);

        private void NavExit_Click(object sender, RoutedEventArgs e) => PageNavigation.Logout(this);
    }
}
