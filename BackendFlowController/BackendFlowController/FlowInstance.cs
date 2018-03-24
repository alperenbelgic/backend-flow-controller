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
        }

        public SendEventResult SendEvent(string eventName)
        {
            var currentStateInDefinition = this.FlowDefinition.States.FirstOrDefault(s => s.Name == this.CurrentState);

            var sentEventInDefinitionResult = currentStateInDefinition.GetEvent(eventName);

            if (false == sentEventInDefinitionResult.Succeeded)
            {
                return new SendEventResult(false);
            }

            var sentEventInDefinition = sentEventInDefinitionResult.Event;

            var destinationState = sentEventInDefinition.DestinationState;

            foreach (var action in sentEventInDefinition.Actions)
            {
                CreatedLogs.Add(
                    new FlowLog()
                    {
                        LogType = "Action_PreExecution",
                        LogMessage = "I am not sure that it is normal to add a field which is not consumed at unit tests, as a tdd practise"
                    });

                AssignFlowDataToAction(action);

                action.Execute();

                CreatedLogs.Add(
                    new FlowLog()
                    {
                        LogType = "Action_PostExecution",
                        LogMessage = "I am not sure that it is normal to add a field which is not consumed at unit tests, as a tdd practise"
                    });
            }

            this.CurrentState = currentStateInDefinition.Name;
            if (!String.IsNullOrWhiteSpace(destinationState))
            {
                CurrentState = destinationState;
            }

            return new SendEventResult(succeeded: true, createdLogs: CreatedLogs);
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

                        if (actionProperty.PropertyType == valueInFlow.GetType())
                        {
                            actionProperty.SetValue(action, valueInFlow);
                        }
                    }

                }
            }
        }
    }
}
