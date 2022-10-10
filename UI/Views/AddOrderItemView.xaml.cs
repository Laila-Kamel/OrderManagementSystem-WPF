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
    /// Interaction logic for AddOrderItemView.xaml
    /// </summary>
    public partial class AddOrderItemView : Page
    {
        
        public Repository repo=new Repository();
        StockItem selectedItem;
        OrderItem ordrItem;
        //private bool available;
        int selectedID;
        public bool Availability;
        //{
        //    get { return Availability; }
        //    set { Availability = available; }
        //}
        int selectedQuantity;
        //OrderState ordState = new OrderNew();
        OrderHeader orderHeader = new OrderHeader();
        int orderId;
        public AddOrderItemView()
        { }
        public AddOrderItemView(int ID)
        {
            InitializeComponent();
            orderId = ID;
            dgOrderItems.ItemsSource = repo.GetStockItems();
        }
       
        //public bool CheckAvailability(bool available)
        //{
        //    availability = available;
        //    return availability;
        //}
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            selectedItem = (StockItem)dgOrderItems.SelectedItem;
           
            if (selectedItem == null)
            {
                MessageBox.Show("Please select an item");
            }
            else 
            {
                if (txtBoxQuantity.Text==string.Empty )
                {
                    MessageBox.Show("Please insert an appropriate quantity value");
                }
                else if(int.Parse(txtBoxQuantity.Text) < 0)
                {
                    MessageBox.Show("Please insert an appropriate quantity value");
                }
                else
                {
                    selectedQuantity = int.Parse(txtBoxQuantity.Text);
                    selectedID = selectedItem.Id;
                    orderHeader.Id = orderId;
                    if (selectedItem.InStock < selectedQuantity)
                    {
                        MessageBox.Show("You selected a quantity more than what is available in the stock, you selected " + selectedQuantity + " and the stock available is " + selectedItem.InStock);
                        ////////////////////////
                        //Availability = false;
                    }

                    //else if (selectedQuantity <= 0)
                    //{
                    //    MessageBox.Show("Please insert an appropriate quantity value");
                    //}
                    //else if (selectedItem.InStock > selectedQuantity)
                    //{
                        //CheckAvailability(false);
                        //Availability = true;
                        repo.UpdateStockItemAmount(selectedID, selectedQuantity);
                    //}
                    //orderHeader._orderState = 2;
                    //repo.UpdateOrderState(orderHeader);


                    ordrItem = new OrderItem
                    {
                        OrderHeader = orderHeader,
                        OrderHeaderId = orderId,
                        StockItemId = selectedID,
                        Description = selectedItem.Name,
                        Price = selectedItem.Price,
                        Quantity = selectedQuantity
                    };

                    repo.AddItemToOrder(ordrItem, OrderStates.New, orderHeader.DateTime);
                    //orderHeader.UpdateOrInsertOrderItem(ordrItem.OrderHeaderId,ordrItem.StockItemId,selectedQuantity,ordrItem.Price,ordrItem.Description)
                    //selectedID = 0;
                    //selectedQuantity = 0;
                    NavigationService.Navigate(new AddOrderView(orderId, Availability));
                }
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
    }
}
