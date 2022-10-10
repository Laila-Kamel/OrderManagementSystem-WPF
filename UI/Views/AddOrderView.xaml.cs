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
    /// Interaction logic for AddOrderView.xaml
    /// </summary>
    public partial class AddOrderView : Page
    {
        private Repository repo = new Repository();
        OrderHeader orhead =new OrderHeader();
        //OrderNew orstate = new OrderNew();
        int id;
        bool avail;
        public AddOrderView(int ID, bool available)
        {
            InitializeComponent();
            avail = available;
            orhead = repo.SelectHeaderOfOrder(ID);
            id = ID;
            txtboxOrderId.Text = orhead.Id.ToString();
            txtState.Text = orhead.State.ToString();
            txtboxDateTime.Text = orhead.DateTime.ToString();
            dgOrderListItems.ItemsSource = repo.GetOrders(ID);
            txtboxTotal.Text = "$" + repo.CalculateFinalTotal(ID).ToString();
            //DataContext = orhead;
        }

        public AddOrderView(int ID)
        {
            InitializeComponent();
            orhead = repo.SelectHeaderOfOrder(ID);
            id = ID;
            txtboxOrderId.Text = orhead.Id.ToString();
            txtState.Text = orhead.State.ToString();
            txtboxDateTime.Text = orhead.DateTime.ToString();
            dgOrderListItems.ItemsSource = repo.GetOrders(ID);
            txtboxTotal.Text = "$" + repo.CalculateFinalTotal(ID).ToString();
            //DataContext = orhead;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddOrderItemView(orhead.Id));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainWindow());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            OrderHeader orderHeader = repo.SelectHeaderOfOrder(id);
            orderHeader._orderState = 2;
            //orderHeader.Submit();
            repo.UpdateOrderState(orderHeader);

            //repo.UpdateOrderState(id, );
            NavigationService.Navigate(new MainWindow());
        }

        private void dgOrderListItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
