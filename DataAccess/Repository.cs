using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using Domain;
using System.Data;


namespace DataAccess
{
    public class Repository
    {
        private string connectionString ;
        private SqlConnection sqlCon;
        public Repository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["OrderManagementSystem"].ConnectionString;
            sqlCon = new SqlConnection(connectionString);
        }
        public List<OrderItem> GetOrders(int ID)
        {
            List<OrderItem> orderItems = new List<OrderItem>();
            sqlCon.Open();
            SqlCommand sqlcom = new SqlCommand("select * from orderitems where orderheaderid=@id", sqlCon);
            sqlcom.Parameters.AddWithValue("@id", ID);
            if (sqlCon.State != ConnectionState.Open)
            {
                sqlCon.Close();
                sqlCon.Open();
            }

            sqlcom.ExecuteNonQuery();
            SqlDataReader sqlReader = sqlcom.ExecuteReader();
            while (sqlReader.Read())
            {
                if (sqlReader.HasRows)
                {
                    OrderItem orderItem = new OrderItem();
                    orderItem.OrderHeaderId = ID;
                    orderItem.StockItemId = sqlReader.GetInt32(1);
                    orderItem.Description = sqlReader.GetString(2);
                    orderItem.Price = sqlReader.GetDecimal(3);
                    orderItem.Quantity = sqlReader.GetInt32(4);
                    orderItems.Add(orderItem);
                }
            }
            sqlCon.Close();
            return orderItems;
        }
        public decimal CalculateFinalTotal(int ID)
        {
            decimal finaltotal=0;
            List<OrderItem> orderItems = new List<OrderItem>();
            orderItems = GetOrders(ID);
            foreach (OrderItem orIt in orderItems)
                finaltotal += orIt.Total;

            return finaltotal;
        }
        public List<StockItem> GetStockItems()
        {
            List<StockItem> stockItems = new List<StockItem>();
            string strCommand = "select * from StockItems";
            SqlCommand sqlcom = new SqlCommand(strCommand, sqlCon);
            if (sqlCon.State != ConnectionState.Open)
            {
                sqlCon.Close();
                sqlCon.Open();
            }
            SqlDataReader sqlReader = sqlcom.ExecuteReader();
            if (sqlReader.HasRows)
            {
                while (sqlReader.Read())
                {
                    StockItem stockItem = new StockItem
                    {
                        Id = sqlReader.GetInt32(0),
                        Name = sqlReader.GetString(1),
                        Price = sqlReader.GetDecimal(2),
                        InStock = sqlReader.GetInt32(3),
                    };
                    stockItems.Add(stockItem);
                }
            }
            sqlCon.Close();
            return stockItems;
        }

        public void AddItemToOrder(OrderItem orderItem, OrderStates states, DateTime time)
        {
            OrderItem oItem = new OrderItem();
            List<OrderItem> orderItems = GetOrders(orderItem.OrderHeaderId);
            
            sqlCon.Open();
            SqlCommand sqlcom2 = new SqlCommand("dbo.sp_UpsertOrderItem", sqlCon);
            sqlcom2.CommandType = System.Data.CommandType.StoredProcedure;
            OrderHeader orderHeader = new OrderHeader();
            oItem = orderHeader.UpdateOrInsertOrderItem1(orderItems, orderItem.OrderHeaderId, orderItem.StockItemId, orderItem.Quantity, orderItem.Price, orderItem.Description);
            //oItem = orderHeader.UpdateOrInsertOrderItem1(orderItem.OrderHeaderId, orderItem.StockItemId, orderItem.Quantity, orderItem.Price, orderItem.Description);
            oItem.OrderHeader = orderItem.OrderHeader;
            sqlcom2.Parameters.AddWithValue("@orderHeaderId", oItem.OrderHeaderId);
            sqlcom2.Parameters.AddWithValue("@stockItemId", oItem.StockItemId);
            sqlcom2.Parameters.AddWithValue("@description", oItem.Description);
            sqlcom2.Parameters.AddWithValue("@price", oItem.Price);
            sqlcom2.Parameters.AddWithValue("@quantity", oItem.Quantity);
            sqlcom2.ExecuteNonQuery();

            sqlCon.Close();
        }
      
        public List<OrderHeader> GetOrderHeaders()
        {
            List<OrderHeader> orderHeaders = new List<OrderHeader>();
            OrderHeader orderHeader = null;
            SqlCommand cmd = new SqlCommand("sp_SelectOrderHeaders", sqlCon);
            if(sqlCon.State!=ConnectionState.Open)
            {
                sqlCon.Close();
                sqlCon.Open();
            }
            
            SqlDataReader dataReader = cmd.ExecuteReader();
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    int orderHeaderId = dataReader.GetInt32(0);
                    if (orderHeader == null || orderHeader.Id != orderHeaderId)
                    {
                        int stateId = dataReader.GetInt32(1);
                        DateTime dateTime = dataReader.GetDateTime(2);
                        orderHeader = new OrderHeader(stateId);
                        orderHeader.Id = orderHeaderId;
                        orderHeader.State =(OrderStates)stateId;
                        orderHeader.DateTime = dateTime;
                        orderHeaders.Add(orderHeader);
                    }
                    int stockId = dataReader.GetInt32(3);
                    string desc = dataReader.GetString(4);
                    decimal price = (decimal)dataReader.GetDecimal(5);
                    int qty = dataReader.GetInt32(6);
                    orderHeader.UpdateOrInsertOrderItem(orderHeaderId, stockId, qty, price, desc);
                }
            }
            return orderHeaders;
        }
        
        public void InsertOrUpdateOrderItems(OrderItem orderItem, OrderStates states, DateTime time)
        {
            sqlCon.Open();
            SqlCommand sqlcom2 = new SqlCommand("dbo.sp_InsertOrderItem", sqlCon);
            sqlcom2.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcom2.Parameters.AddWithValue("@orderHeaderId", orderItem.OrderHeaderId);
            sqlcom2.Parameters.AddWithValue("@stockItemId", orderItem.StockItemId);
            sqlcom2.Parameters.AddWithValue("@description", orderItem.Description);
            sqlcom2.Parameters.AddWithValue("@price", orderItem.Price);
            sqlcom2.Parameters.AddWithValue("@quantity", orderItem.Quantity);
            sqlcom2.ExecuteNonQuery();

            sqlCon.Close();
        }

        public int GetOrderHeader()
        {
            //generate a new order id
            OrderHeader ordHeader = new OrderHeader();
            int Iden = 0;
            SqlCommand com1 = new SqlCommand("dbo.sp_InsertOrderHeader", sqlCon);
            if (sqlCon.State != ConnectionState.Open)
            {
                sqlCon.Close();
                sqlCon.Open();
            }
            com1.CommandType = System.Data.CommandType.StoredProcedure;
            Iden =Convert.ToInt32(com1.ExecuteScalar());
           
            return Iden;
        }

        public StockItem SelectStockItemById(int id)
        {
            StockItem stockItem = new StockItem();
            string strCommand = "sp_SelectStockItemById";
            SqlCommand sqlcom = new SqlCommand(strCommand, sqlCon);
            sqlcom.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcom.Parameters.AddWithValue("@Id", id);
            sqlCon.Open();
            sqlcom.ExecuteNonQuery();
            SqlDataReader sqlReader = sqlcom.ExecuteReader();
            while (sqlReader.Read())
            {
                if (sqlReader.HasRows)
                {
                    stockItem.Id = sqlReader.GetInt32(0);
                    stockItem.Name = sqlReader.GetString(1);
                    stockItem.Price = sqlReader.GetDecimal(2);
                    stockItem.InStock = sqlReader.GetInt32(3);
                }
            }
            sqlCon.Close();
            return stockItem;
        }
        public List<OrderItem> GetOrderItems(int ID)
        {
            List<OrderItem> orderItems = new List<OrderItem>();
            string strCommand="sp_SelectOrderHeaderById";
            SqlCommand sqlcom = new SqlCommand(strCommand, sqlCon);
            sqlcom.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcom.Parameters.AddWithValue("@Id", ID);
            if (sqlCon.State != ConnectionState.Open)
            {
                sqlCon.Close();
                sqlCon.Open();
            }
            sqlcom.ExecuteNonQuery();
            SqlDataReader sqlReader = sqlcom.ExecuteReader();
            while (sqlReader.Read()) 
            {
                if (sqlReader.HasRows)
                {
                    OrderItem orderItem = new OrderItem();
                    OrderHeader orderHeader = new OrderHeader();
                    orderHeader.Id= sqlReader.GetInt32(0);
                    orderHeader.State= (OrderStates)sqlReader.GetInt32(1);
                    orderHeader.DateTime= sqlReader.GetDateTime(2);
                    orderItem.OrderHeader = orderHeader;
                    orderItem.StockItemId = sqlReader.GetInt32(3);
                    orderItem.Description = sqlReader.GetString(4);
                    orderItem.Price = sqlReader.GetDecimal(5);
                    orderItem.Quantity = sqlReader.GetInt32(6);
                    orderItems.Add(orderItem);
                }
            }
            sqlCon.Close();
            return orderItems;
        }
        public void Delete(int id, int sId,decimal tot)
        {
                List<OrderItem> orderItems = new List<OrderItem>();
                string strCommand = "sp_DeleteOrderHeaderAndOrderItems";
                SqlCommand sqlcom = new SqlCommand(strCommand, sqlCon);
                sqlcom.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcom.Parameters.AddWithValue("@orderHeaderId", id);
                sqlCon.Open();
                sqlcom.ExecuteNonQuery();
                orderItems=GetOrderItems(id);
                if(orderItems.Count==0)
                {
                strCommand = "sp_DeleteOrderItem";
                sqlcom = new SqlCommand(strCommand, sqlCon);
                sqlcom.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcom.Parameters.AddWithValue("@orderHeaderId", id);
                sqlcom.Parameters.AddWithValue("@stockItemId", sId);
                sqlCon.Open();
                sqlcom.ExecuteNonQuery();
                }
            sqlCon.Close();
        }
        //public void DeleteOrder(int id)
        //{
        //    List<OrderHeader> orderHeaders = new List<OrderHeader>();
        //    orderHeaders = GetOrderHeaders();
        //            string strCommand = "sp_DeleteOrderHeaderAndOrderItems";
        //            SqlCommand sqlcom = new SqlCommand(strCommand, sqlCon);
        //            sqlcom.CommandType = System.Data.CommandType.StoredProcedure;
        //            sqlcom.Parameters.AddWithValue("@orderHeaderId", id);
        //            sqlCon.Open();
        //            sqlcom.ExecuteNonQuery();
        //    sqlCon.Close();
        //}
        public void UpdateOrderState(OrderHeader orderHeader)
        {
            SqlCommand sqlcom = new SqlCommand("sp_UpdateOrderState", sqlCon);
            sqlcom.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCon.Open();
            sqlcom.Parameters.AddWithValue("@orderHeaderId", orderHeader.Id);
            sqlcom.Parameters.AddWithValue("@stateId", orderHeader._orderState);
            sqlcom.ExecuteNonQuery();
            sqlCon.Close();
        }
        public void DeleteItem(int id, int sId)
        {
            string strCommand = "sp_DeleteOrderItem";
            SqlCommand sqlcom = new SqlCommand(strCommand, sqlCon);
            sqlcom.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcom.Parameters.AddWithValue("@orderHeaderId", id);
            sqlcom.Parameters.AddWithValue("@stockItemId", sId);
            sqlCon.Open();
            sqlcom.ExecuteNonQuery();
            sqlCon.Close();
        }
        public OrderHeader SelectHeaderOfOrder(int id)
        {
          
            OrderHeader orderHeader=new OrderHeader();
            string strCommand = "sp_selectOrderHeaderById";
            SqlCommand sqlcom = new SqlCommand(strCommand, sqlCon);
            sqlcom.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcom.Parameters.AddWithValue("@Id", id);
            sqlCon.Open();
            sqlcom.ExecuteNonQuery();
            SqlDataReader sqlReader = sqlcom.ExecuteReader();
            while (sqlReader.Read())
            {
                if (sqlReader.HasRows)
                {
                    orderHeader.Id = sqlReader.GetInt32(0);
                    orderHeader.State = (OrderStates)sqlReader.GetInt32(1);
                    orderHeader.DateTime = sqlReader.GetDateTime(2);
                }
            }
            sqlCon.Close();
            return orderHeader;
        }
        public void UpdateStockItemAmount(int stkId, int quantity)
        {
            
            string strCommand = "sp_UpdateStockItemAmount";
            SqlCommand sqlcom = new SqlCommand(strCommand, sqlCon);
            sqlcom.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcom.Parameters.AddWithValue("@id", stkId);
            sqlcom.Parameters.AddWithValue("@amount", -quantity);
            sqlCon.Open();
            SqlDataReader sqlReader = sqlcom.ExecuteReader();
            while (sqlReader.Read())
            {
                if (sqlReader.HasRows)
                {
                    StockItem stockItem = new StockItem();
                    stockItem.InStock = sqlReader.GetInt32(0);
                }
            }
            sqlCon.Close();
        }
    }
 }

