using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class OrderHeader
    {
        public List<OrderItem> _orderItems = new List<OrderItem>();
        public int _orderState;
        //public OrderHeader(int ID, DateTime DT, int stateId)
        //{
        //    ID = Id;
        //    DT = DateTime;
        //    stateId = _orderState;
        //    setState(stateId);
            
        //}
        public OrderState _state;
        public int Id { get; set; }
        public OrderHeader()
        {
           
        }
        public OrderHeader(int _orderState)
        {
            setState(_orderState);
        }
        public OrderStates State
        {
            //get { return _state.State; }
            get ; 
            set ; 
        }
        public DateTime DateTime{get ;set ;}
        
        public decimal Total
        {
            get
            {
                return _orderItems.Count;
            }
        }
        public void setState(int stateId)
        {
            switch (stateId)
            {
                case 1:
                    _state = new OrderNew(this);
                    break;
                case 2:
                    _state = new OrderPending(this);
                    break;
                case 3:
                    _state = new OrderRejected(this);
                    break;
                case 4:
                    _state = new OrderComplete(this);
                    break;
                default:
                    throw new InvalidOrderStateException($"Invalid State Id:{stateId}");

            }
        }
        public decimal FinalTotal
        {
            get
            {
                decimal finalTotal = 0;
                foreach (OrderItem ordItem in _orderItems)
                    finalTotal += ordItem.Total;
                return finalTotal;
            }
        }
        public OrderItem UpdateOrInsertOrderItem1(List<OrderItem> _orderItems, int orderHeaderId, int stockId, int qty, decimal price, string desc)
        {
            OrderItem orderItem = null;
            foreach (OrderItem item in _orderItems)
            {
                if (item.StockItemId == stockId)
                {
                    orderItem = item;
                }
            }
            if (orderItem != null)
            {
                orderItem.Quantity += qty;
            }
            else
            {
                orderItem = new OrderItem();
                orderItem.OrderHeaderId = orderHeaderId;
                orderItem.StockItemId = stockId;
                orderItem.Quantity = qty;
                orderItem.Price = price;
                orderItem.Description = desc;

                _orderItems.Add(orderItem);
            }
            return orderItem;
        }


        public OrderItem UpdateOrInsertOrderItem( int orderHeaderId, int stockId, int qty, decimal price, string desc)
        {
            OrderItem orderItem = null;
            foreach (OrderItem item in _orderItems)
            {
                if (item.StockItemId == stockId)
                {
                    orderItem = item;
                }
            }
            if (orderItem != null)
            {
                orderItem.Quantity += qty;
            }
            else
            {
                orderItem = new OrderItem();
                orderItem.OrderHeaderId = orderHeaderId;
                orderItem.StockItemId = stockId;
                orderItem.Quantity = qty;
                orderItem.Price = price;
                orderItem.Description = desc;

                _orderItems.Add(orderItem);
            }
            return orderItem;
        }

        public void AddOrderItem()
        {

        }
        public void Complete()
        {
            _state.Complete(ref _state);
        }
        public void Reject()
        {
            _state.Reject(ref _state);
        }
       
        public void Submit()
        {
            //if (!OrderItems.Any())
            //{
            //    throw new InvalidOrderStateException("An empty order cannot be submitted");
            //}
            _state.Submit(ref _state);
        }
    }
}
