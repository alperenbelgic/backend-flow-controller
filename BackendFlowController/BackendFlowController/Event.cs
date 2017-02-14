using System.Collections.Generic;

namespace BackendFlowController
{
    public class Event
    {
        public Event()
        {
        }

        public List<IAction> Actions = new List<IAction>();
        public string DestinationState { get; set; }
        public string Name { get; set; }
    }
}