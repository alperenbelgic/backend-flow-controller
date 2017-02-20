using Moq;
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
        
         [TestAttribute]
        public void Event_Changes_Flow_Instances_State_2()
        {

            var flowDefinition = new FlowDefinition()
            {
                States = new List<State>()
                {
                    new State()
                    {
                        Name = "StateA",
                        Events = new List<Event>()
                        {
                            new Event()
                            {
                                Name = "EventA",
                                DestinationState = "StateB"
                            }
                        }
                    },
                    new State()
                    {
                        Name = "StateB"
                    }
                }
            };

            var flowInstance = new FlowInstance()
            {
                CurrentState = "StateA",
                FlowDefinition = flowDefinition
            };

            SendEventResult result = flowInstance.SendEvent("EventA");

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(flowInstance.CurrentState, "StateB");

        }

        [TestAttribute]
        public void Event_Triggers_Action()
        {


            var mockAction = new Mock<IAction>();


            var flowDefinition = new FlowDefinition()
            {
                States = new List<State>()
  {
    new State()
    {
      Name = "State1",
      Events = new List<Event>()
      {
          new Event() {
        Name = "Event1",
        Actions = new List<IAction>()
        {
            mockAction.Object
        }
          }
      }
    }
  }
            };

            var flowInstance = new FlowInstance()
            {
                CurrentState = "State1",
                FlowDefinition = flowDefinition
            };

            var result = flowInstance.SendEvent("Event1");
            mockAction.Verify(action => action.Execute());



        }
    }
}
