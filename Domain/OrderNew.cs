using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain
{
    public class OrderNew:OrderState
    {
        public OrderNew(OrderHeader orderHeader):base (orderHeader)
        {

        }
        
        public override OrderStates State 
        { 
            get { return OrderStates.New; }
            set {; }
        }
        public override void Complete(ref OrderState _state)
        {
            throw new InvalidOrderStateException("A new order must first be submitted");
        }
        public override void Reject(ref OrderState _state)
        {
            throw new InvalidOrderStateException("A new order must first be submitted");
        }
        public override void Submit(ref OrderState _state)
        {
            _state = new OrderPending(_orderHeader);
        }
    }
}
