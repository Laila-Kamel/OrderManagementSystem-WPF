using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    class OrderPending : OrderState
    {
        public OrderPending(OrderHeader orderHeader) : base(orderHeader)
        {

        }
        public override OrderStates State
        {
            get { return OrderStates.Pending; }
            set {; }
        }
        public override void Complete(ref OrderState _state)
        {
            _state = new OrderComplete(_orderHeader);
        }
        public override void Reject(ref OrderState _state)
        {
            _state = new OrderRejected(_orderHeader);
        }
        public override void Submit(ref OrderState _state)
        {
            throw new InvalidOrderStateException("This order has already been submitted before");
        }
    }
}
