using System.Collections.Generic;
using System.Linq;

namespace BackendFlowController
{
    public class State
    {
        public State(string name, List<Event> events = null)
        {
            this.Name = name;
            this.Events = events ?? new List<Event>();
        }

        List<Event> Events;
        public GetEventResult GetEvent(string eventName)
        {
            var _event = Events.FirstOrDefault(e => e.Name == eventName);

            if (_event != null)
            {
                return new GetEventResult(true, _event);
            }
            else
            {
                return new GetEventResult(false);
            }
        }

        public string Name { get; private set; }
    }

    public class GetEventResult
    {
        public GetEventResult(bool succeeded, Event _event = null)
        {
            this.Event = _event;
            this.Succeeded = succeeded;
        }

        public bool Succeeded { get; private set; }
        public Event Event { get; private set; }
    }
}