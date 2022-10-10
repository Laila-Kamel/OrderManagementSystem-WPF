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
using DataAccess;
using Domain;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for OrdersView.xaml
    /// </summary>
    public partial class OrdersView : Page
    {
        private Repository repo = new Repository();
        private OrderHeader selectedOrderHeader;
        List<OrderHeader> orderHeaders = new List<OrderHeader>();
        List<OrderItem> orderItems;
        public OrdersView()
        {
            InitializeComponent();
            orderHeaders = repo.GetOrderHeaders();
            //foreach (OrderHeader orderHeader in orderHeaders)
            //{
            //    if (orderHeader.Total ==0)
            //        repo.DeleteOrder(orderHeader.Id);
            //}
            dgOrders.ItemsSource = orderHeaders;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            selectedOrderHeader = (OrderHeader)dgOrders.SelectedItem;
            orderItems = repo.GetOrderItems(selectedOrderHeader.Id);
            NavigationService.Navigate(new OrderDetailView(orderItems));
        }
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddOrderView(repo.GetOrderHeader()));
        }

    }
}
