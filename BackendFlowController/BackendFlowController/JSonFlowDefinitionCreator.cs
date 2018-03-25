using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendFlowController
{
    public class JSonFlowDefinitionCreator : IFlowDefinitionCreator
    {
        private string jsonContent;

        public JSonFlowDefinitionCreator(string jsonContent)
        {
            this.jsonContent = jsonContent;
        }

        public FlowDefinition Create()
        {
            IJsonDeserialiser jsonDeserialiser = new NewtonJsonDeserialiser();
            var flowDefinitionInJson = jsonDeserialiser.Deserialise<FlowDefinitionJsonModel>(this.jsonContent);

            List<State> states = CreateStates(flowDefinitionInJson.States);

            return new FlowDefinition(states);
        }

        private List<State> CreateStates(List<StateJsonModel> statesInJson)
        {
            var states = new List<State>();

            foreach (var stateInJson in statesInJson)
            {
                List<Event> events = CreateEvents(stateInJson.Events);



                var state = new State(stateInJson.Name, events: events);

                states.Add(state);
            }

            return states;
        }

        private List<Event> CreateEvents(List<EventJsonModel> eventsInJson)
        {
            if (eventsInJson != null)
            {
                var events = new List<Event>();

                foreach (var eventInJson in eventsInJson)
                {
                    List<IAction> actions = CreateActions(eventInJson.Actions);

                    var @event = new Event(eventInJson.Name, actions: actions);

                    events.Add(@event);
                }

                return events;
            }
            else
            {
                return new List<Event>();
            }
        }

        private List<IAction> CreateActions(List<ActionJsonModel> actionsInJson)
        {
            if (actionsInJson != null)
            {
                var actions = new List<IAction>();

                foreach (var item in actionsInJson)
                {
                    var action = new DummyAction();

                    actions.Add(action);
                }

                return actions;
            }
            else
            {
                return new List<IAction>();
            }
        }
        public class DummyAction : IAction
        {
            public void Execute()
            {
                throw new NotImplementedException();
            }
        }
    }

    public interface IJsonDeserialiser
    {
        T Deserialise<T>(string jsonContent);
    }

    public class NewtonJsonDeserialiser : IJsonDeserialiser
    {
        public T Deserialise<T>(string jsonContent)
        {
            return JsonConvert.DeserializeObject<T>(jsonContent);
        }
    }

    public class FlowDefinitionJsonModel
    {
        public virtual List<StateJsonModel> States { get; set; }
    }

    public class StateJsonModel
    {
        public virtual string Name { get; set; }

        public virtual List<EventJsonModel> Events { get; set; }
    }

    public class EventJsonModel
    {
        public virtual string Name { get; set; }

        public virtual List<ActionJsonModel> Actions { get; set; }
    }

    public class ActionJsonModel
    {
    }
}
