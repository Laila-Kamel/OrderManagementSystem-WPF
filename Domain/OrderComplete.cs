using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    class OrderComplete : OrderState
    {
        public OrderComplete(OrderHeader orderHeader):base(orderHeader)
        {

        }
        public override OrderStates State
        {
            get { return OrderStates.Complete; }
            set {; }
        }
        public override void Complete(ref OrderState _state)
        {
            throw new NotImplementedException();
        }
        public override void Reject(ref OrderState _state)
        {
            throw new NotImplementedException();
        }
        public override void Submit(ref OrderState _state)
        {
            throw new NotImplementedException();
        }
    }
}
