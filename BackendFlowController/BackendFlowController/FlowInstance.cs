using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendFlowController
{
    public class FlowInstance
    {
        public string CurrentState { get; set; }
        public FlowDefinition FlowDefinition { get; set; }

        public SendEventResult SendEvent(string eventName)
        {
            CurrentState = "State2";
            return new SendEventResult() { Succeeded = true };
        }
    }
}
