using System.Collections.Generic;

namespace BackendFlowController
{
    public class FlowDefinition
    {
        public FlowDefinition(List<State> states)
        {
            this.States = states;
        }

        List<State> States { get; set; }

        public List<State> GetStates()
        {
            return new List<State>(this.States);
        }
    }
}