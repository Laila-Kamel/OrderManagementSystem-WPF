using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public enum OrderStates { New = 1, Pending = 2, Rejected = 3, Complete = 4 }
    public abstract class OrderState
    {
        public OrderHeader _orderHeader;
        public OrderState(OrderHeader ordHd)
        {
            _orderHeader = ordHd;
        }
        
        public abstract OrderStates State
        {
            get;set;
        }
       
        public abstract void Complete(ref OrderState _state);
        public abstract void Reject(ref OrderState _state);
        public abstract void Submit(ref OrderState _state);
        
       
    }
    
}
