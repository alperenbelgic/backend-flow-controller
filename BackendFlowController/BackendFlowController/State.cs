using System.Collections.Generic;

namespace BackendFlowController
{
    public class State
    {
        public State()
        {
        }

        public List<Event> Events { get; set; }
        public string Name { get; set; }
    }
}