using System.Collections.Generic;

namespace BackendFlowController
{
    public class SendEventResult
    {
        public SendEventResult(bool succeeded, List<FlowLog> createdLogs = null)
        {
            this.Succeeded = succeeded;
            CreatedLogs = createdLogs ?? new List<FlowLog>();
        }

        public bool Succeeded { get; private set; }

        public List<FlowLog> CreatedLogs { get; private set; }
    }
}
