using System.Collections.Generic;

namespace BackendFlowController
{
    public class FlowDefinition
    {
        public FlowDefinition(List<State> states)
        {
            this.States = states;
        }

        public List<State> States { get; private set; }
    }
}