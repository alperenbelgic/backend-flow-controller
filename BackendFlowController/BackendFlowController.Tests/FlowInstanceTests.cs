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
        public void TestTest_ErrorTest()
        {
            Assert.AreEqual(1, 2);
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
            Assert.AreEqual("State2", flowInstance.CurrentState);

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
            Assert.AreEqual("StateB", flowInstance.CurrentState);
        }
        
        
        [TestAttribute]
        public void Event_Does_Not_Change_The_State()
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
                                Name = "Event1"
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
            
            Assert.AreEqual("State1", flowInstance.CurrentState);

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
                            new Event() 
                            {
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
        
        [TestAttribute]
        public void Action_Execution_Creates_Logs()
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
                            new Event() 
                            {
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
            
            Assert.IsTrue(result.CreatedLogs.Any(cl => cl.LogType == "ActionLog"));
        }
    }
}
