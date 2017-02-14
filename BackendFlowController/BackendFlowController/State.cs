using System.Collections.Generic;

namespace BackendFlowController
{
    public class State
    {
        public State()
        {
        }

        public List<Event> Events = new List<Event>();
        public string Name { get; set; }
    }
}