using System.Windows.Navigation;
using UserApp.pages;

namespace UserApp
{
    public partial class NavigationHost : NavigationWindow
    {
        public NavigationHost()
        {
            InitializeComponent();
            Navigate(new AutorisationPage());
        }
    }
}
