using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    class OrderRejected: OrderState
    {
        public OrderRejected(OrderHeader orderHeader) : base(orderHeader)
        {

        }
        public override OrderStates State
        {
            get { return OrderStates.Rejected; }
            set {; }
        }
        public override void Complete(ref OrderState _state)
        {
            throw new InvalidOrderStateException("This order chas already been rejected" );
        }
        public override void Reject(ref OrderState _state)
        {
            throw new InvalidOrderStateException("This order has already been rejected and cannot be submitted again");
        }
        public override void Submit(ref OrderState _state)
        {
            throw new InvalidOrderStateException("A rejected order cannot be resubmitted");
        }
    }
}
