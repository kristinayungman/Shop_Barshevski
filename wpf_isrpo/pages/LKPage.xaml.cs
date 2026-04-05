using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
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
    /// Логика взаимодействия для LKPage.xaml
    /// </summary>
    public partial class LKPage : Page
    {
        private User _currentUser;
        public string searchQuery { get; set; } = null;
        public string filterCategories { get; set; } = null;
        public ObservableCollection<Product> products { get; set; } = new ObservableCollection<Product>();
        public ICollectionView productsView { get; set; }
        public LKPage()
        {
            InitializeComponent();
            WelcomeBlock.Text = $"Добро пожаловать, {SessionManager.CurrentUser?.fio}";
            LoadList();

            _currentUser = SessionManager.CurrentUser;

            productsView = CollectionViewSource.GetDefaultView(products);
            productsView.Filter = FilterProducts;
            ProductList.ItemsSource = productsView;

            foreach (var category in DB.Context.Category.ToList())
                CategoriesCombo.Items.Add(new ComboBoxItem() { Tag = category.name, Content = category.name });
        }
        public void LoadList()
        {
            products.Clear();
            foreach (var product in DB.Context.Product.Include("Category").ToList().Where(p => p.count > 0))
                products.Add(product);
        }
        public bool FilterProducts(object obj)
        {

            if (!(obj is Product))
                return false;

            var product = (Product)obj;
            if (searchQuery != null && !product.name.ToLower().Contains(searchQuery.ToLower()))
                return false;

            if (filterCategories != null && (product.Category == null || product.Category.name != filterCategories))
                return false;

            return true;
        }
        private void NavCheckout_Click(object sender, RoutedEventArgs e) => PageNavigation.NavigateCheckout(this);

        private void ResetBtn(object sender, RoutedEventArgs e)
        {
            filterCategories = searchQuery = null;
            CategoriesCombo.SelectedIndex = -1;
            SearchTextBox.Text = "";

            productsView.Refresh();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var t = SearchTextBox.Text?.Trim();
            searchQuery = string.IsNullOrEmpty(t) ? null : t;
            productsView?.Refresh();
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = (System.Windows.Controls.ComboBox)sender;

            if (cb.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedValue = selectedItem.Tag?.ToString();
                filterCategories = selectedValue;
            }
            else
                filterCategories = null;

            productsView.Refresh();
        }
        private void NavProducts_Click(object sender, RoutedEventArgs e) => PageNavigation.NavigateProducts(this);

        private void NavOrders_Click(object sender, RoutedEventArgs e) => PageNavigation.NavigateOrders(this);

        private void NavBack_Click(object sender, RoutedEventArgs e) => PageNavigation.GoBack(this);

        private void NavExit_Click(object sender, RoutedEventArgs e) => PageNavigation.Logout(this);
    }
}
