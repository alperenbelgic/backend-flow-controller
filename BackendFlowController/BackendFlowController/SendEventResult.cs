using System.Collections.Generic;

namespace BackendFlowController
{
    public class SendEventResult
    {
        public SendEventResult()
        {
            CreatedLogs = new List<FlowLog>();
        }
        
        public bool? Succeeded { get; set; }
        
        public List<FlowLog> CreatedLogs { get; set; }
    }
}
