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
            var currentStateInDefinition = this.FlowDefinition.States.FirstOrDefault(s=> s.Name == this.CurrentState);
            
            var sentEventInDefinition = currentStateInDefinition.Events.FirstOrDefault(e=> e.Name == eventName);
            
            var destinationState = sentEventInDefinition.DestinationState;

            foreach (var action in sentEventInDefinition.Actions)
            {
                action.Execute();
            }
          

            CurrentState = destinationState;
            return new SendEventResult() { Succeeded = true };
        }
    }
}
