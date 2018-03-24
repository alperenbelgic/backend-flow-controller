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
        public List<FlowLog> CreatedLogs = new List<FlowLog>();

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
    }
}
