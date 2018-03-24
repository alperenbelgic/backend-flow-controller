using System.Collections.Generic;

namespace BackendFlowController
{
    public class Event
    {
        public Event(string name, List<IAction> actions = null, string destinationState = "")
        {
            this.Name = name;
            this.Actions = actions ?? new List<IAction>();
            this.DestinationState = destinationState;
        }

        public List<IAction> Actions { get; private set; }
        public string DestinationState { get; private set; }
        public string Name { get; private set; }
    }
}