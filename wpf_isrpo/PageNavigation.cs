using System.Windows.Controls;
using UserApp.pages;

namespace UserApp
{
    public static class PageNavigation
    {
        public static void NavigateProducts(Page from)
        {
            if (!SessionManager.IsAuthenticated)
            {
                from.NavigationService.Navigate(new AutorisationPage());
                return;
            }
            if (SessionManager.CurrentUser.role_id == SessionManager.RoleBuyer)
                from.NavigationService.Navigate(new LKPage());
            else
                from.NavigationService.Navigate(new ProductSellerPage());
        }

        public static void NavigateCheckout(Page from)
        {
            if (!SessionManager.IsAuthenticated || SessionManager.CurrentUser.role_id != SessionManager.RoleBuyer)
            {
                from.NavigationService.Navigate(new AutorisationPage());
                return;
            }
            from.NavigationService.Navigate(new CheckoutPage());
        }

        public static void NavigateOrders(Page from)
        {
            if (!SessionManager.IsAuthenticated)
            {
                from.NavigationService.Navigate(new AutorisationPage());
                return;
            }
            if (SessionManager.CurrentUser.role_id == SessionManager.RoleBuyer)
                from.NavigationService.Navigate(new ZakazPage());
            else
                from.NavigationService.Navigate(new Prodavec());
        }

        public static void GoBack(Page from)
        {
            try
            {
                if (from.NavigationService.CanGoBack)
                    from.NavigationService.GoBack();
            }
            catch { }
        }

        public static void Logout(Page from)
        {
            SessionManager.Logout();
            from.NavigationService.Navigate(new AutorisationPage());
        }
    }
}
