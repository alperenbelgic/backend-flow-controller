using System.Collections.Generic;

namespace BackendFlowController
{
    public class SendEventResult
    {
        public bool? Succeeded { get; set; }
        
        public List<FlowLog> CreatedLogs { get; set; }
    }
}
