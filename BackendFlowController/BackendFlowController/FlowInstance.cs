using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendFlowController
{
    public class FlowInstance
    {
        public string CurrentState { get; private set; }
        public dynamic FlowData { get; }
        public FlowDefinition FlowDefinition { get; private set; }
        public List<FlowLog> CreatedLogs { private set; get; } = new List<FlowLog>();

        public FlowInstance(FlowDefinition flowDefinition, string currentState, dynamic flowData = null)
        {
            this.FlowDefinition = flowDefinition;
            this.CurrentState = currentState;
            this.FlowData = flowData;

            ValidateFlowDefinition();
            ValidateCurrentState();
        }

        private void ValidateFlowDefinition()
        {
            if (FlowDefinition == null)
            {
                throw new FlowInstanceException();
            }
        }

        private void ValidateCurrentState()
        {
            if (!this.FlowDefinition.States.Any(s => s.Name == this.CurrentState))
            {
                throw new FlowInstanceException();
            }
        }

        public SendEventResult SendEvent(string eventName)
        {
            var currentState = this.FlowDefinition.States.FirstOrDefault(s => s.Name == this.CurrentState);

            var getEventResult = currentState.GetEvent(eventName);

            if (false == getEventResult.Succeeded)
            {
                return new SendEventResult(false);
            }

            var currentEvent = getEventResult.Event;

            foreach (var action in currentEvent.Actions)
            {
                CreatedLogs.Add(
                    new FlowLog()
                    {
                        LogType = "Action_PreExecution",
                        LogMessage = "I am not sure that it is normal to add a field which is not consumed at unit tests, as a tdd practise"
                    });

                AssignFlowDataToAction(action);

                action.Execute();

                AssignActionDataToFlow(action);

                CreatedLogs.Add(
                    new FlowLog()
                    {
                        LogType = "Action_PostExecution",
                        LogMessage = "I am not sure that it is normal to add a field which is not consumed at unit tests, as a tdd practise"
                    });
            }

            var destinationState = currentEvent.DestinationState;

            this.CurrentState = currentState.Name;

            if (!String.IsNullOrWhiteSpace(destinationState))
            {
                CurrentState = destinationState;
            }

            return new SendEventResult(succeeded: true, createdLogs: CreatedLogs);
        }

        private void AssignActionDataToFlow(IAction action)
        {
            if (this.FlowData != null)
            {
                var actionPropererties = action.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(FlowDataAttribute), true).Any());

                foreach (var actionProperty in actionPropererties)
                {
                    var flowData = this.FlowData as IDictionary<string, object>;
                    {
                        var value = actionProperty.GetValue(action);

                        flowData[actionProperty.Name] = value;
                    }
                }
            }
        }

        private void AssignFlowDataToAction(IAction action)
        {
            if (this.FlowData != null)
            {

                var actionPropererties = action.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(FlowDataAttribute), true).Any());

                foreach (var actionProperty in actionPropererties)
                {
                    var flowData = this.FlowData as IDictionary<string, object>;
                    if (flowData.ContainsKey(actionProperty.Name))
                    {
                        var valueInFlow = flowData[actionProperty.Name];

                        if (valueInFlow != null && actionProperty.PropertyType == valueInFlow.GetType())
                        {
                            actionProperty.SetValue(action, valueInFlow);
                        }
                    }

                }
            }
        }
    }
}
