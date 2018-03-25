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

        List<IAction> Actions { get; set; }

        public List<IAction> GetActions()
        {
            return new List<IAction>(this.Actions);
        }

        public string DestinationState { get; private set; }
        public string Name { get; private set; }
    }
}