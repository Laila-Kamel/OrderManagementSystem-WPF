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
using Domain;
using DataAccess;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for OrderDetailView.xaml
    /// </summary>
    public partial class OrderDetailView : Page
    {
        Repository repo=new Repository();
        OrderHeader orderHeader;
        List<OrderItem> items;
        public OrderDetailView(List<OrderItem> order)
        {
            InitializeComponent();
            items = order;
            dgOrders.ItemsSource = items;
            int test = order[0].OrderHeader.Id;
            orderHeader = repo.SelectHeaderOfOrder(test);
            txtBoxOrderId.Text = orderHeader.Id.ToString();
            txtBoxState.Text = orderHeader.State.ToString();
            txtBoxDateTime.Text = orderHeader.DateTime.ToString();
            if(txtBoxState.Text=="Pending")
            {
                btnProceed.Visibility = Visibility.Visible;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OrderItem orderItem = (OrderItem)dgOrders.SelectedItem;
            decimal total=orderItem.Total;
            int id = orderItem.OrderHeader.Id;
            int stockId = orderItem.StockItemId;
            repo.Delete(id, stockId,total);
            NavigationService.Navigate(new OrdersView());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrdersView());
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            StockItem stockItem = new StockItem();

            foreach (OrderItem item in items)
            {
                stockItem=repo.SelectStockItemById(item.StockItemId);
                if (item.Quantity> stockItem.InStock)
                {
                    orderHeader._orderState = 3;
                    //orderHeader.Reject();
                    repo.UpdateOrderState(orderHeader);
                }
                else if(item.Quantity < stockItem.InStock)
                {
                    orderHeader._orderState = 4;
                    //orderHeader.Complete();
                    repo.UpdateOrderState(orderHeader);
                }
            }
            NavigationService.Navigate(new MainWindow());
        }
    }
}
