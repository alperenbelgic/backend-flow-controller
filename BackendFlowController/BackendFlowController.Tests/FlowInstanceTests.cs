using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendFlowController.Tests
{
    [TestFixtureAttribute]
    public class FlowInstanceTests
    {
        [TestAttribute]
        public void TestTest()
        {
            Assert.AreEqual(1, 1);
        }

        [TestAttribute]
        public void Event_Changes_Flow_Instances_State()
        {

            var flowDefinition = new FlowDefinition()
            {
                States = new List<State>()
                {
                    new State()
                    {
                        Name = "State1",
                        Events = new List<Event>()
                        {
                            new Event()
                            {
                                Name = "Event1",
                                DestinationState = "State2"
                            }
                        }
                    },
                    new State()
                    {
                        Name = "State2"
                    }
                }
            };

            var flowInstance = new FlowInstance()
            {
                CurrentState = "State1",
                FlowDefinition = flowDefinition
            };

            SendEventResult result = flowInstance.SendEvent("Event1");

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(flowInstance.CurrentState, "State2");

        }
    }
}
