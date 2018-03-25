using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendFlowController.Tests
{
    [TestFixture]
    public class FlowInstanceTests
    {
        [Test]
        public void TestTest()
        {
            Assert.AreEqual(1, 1);
        }

        [Test]
        public void Event_Changes_Flow_Instances_State()
        {

            var flowDefinition = new FlowDefinition(
                                                         states: new List<State>()
                                                         {
                                                             new State(
                                                                         name: "State1",
                                                                         events: new List<Event>()
                                                                         {
                                                                             new Event(
                                                                                         name : "Event1",
                                                                                         destinationState : "State2"
                                                                                      )
                                                                         }
                                                                       ),
                                                             new State(name: "State2")
                                                         }
                                                    );

            var flowInstance = new FlowInstance(
                                                 currentState: "State1",
                                                 flowDefinition: flowDefinition
                                                );

            SendEventResult result = flowInstance.SendEvent("Event1");

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual("State2", flowInstance.CurrentState);

        }

        [Test]
        public void Event_Changes_Flow_Instances_State_2()
        {

            var flowDefinition = new FlowDefinition(
                                                    states: new List<State>()
                                                    {
                                                        new State(
                                                                    name:"StateA",
                                                                    events:
                                                                    new List<Event>()
                                                                    {
                                                                        new Event(
                                                                                    name : "EventA",
                                                                                    destinationState: "StateB"
                                                                                 )
                                                                    }
                                                                   ),
                                                        new State(name:"StateB")
                                                     }
                                                    );

            var flowInstance = new FlowInstance(
                                                currentState: "StateA",
                                                flowDefinition: flowDefinition
                                                );

            SendEventResult result = flowInstance.SendEvent("EventA");

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual("StateB", flowInstance.CurrentState);
        }

        [Test]
        public void Undefined_Event_Returns_Unsuccessful_Result()
        {
            var flowDefinition = new FlowDefinition(
                                                        states: new List<State>()
                                                        {
                                                            new State(
                                                                        name: "StateA",
                                                                        events:
                                                                        new List<Event>()
                                                                        {
                                                                            new Event(name : "EventA")
                                                                        }
                                                                )
                                                        }
                );

            var flowInstance = new FlowInstance(
                                                currentState: "StateA",
                                                flowDefinition: flowDefinition
                                                );

            SendEventResult result = flowInstance.SendEvent("EventB");

            Assert.AreEqual(false, result.Succeeded);
        }

        [Test]
        public void Flow_Instance_Construction_Wrong_State_Name_Throws_Error()
        {
            var flowDefinition = new FlowDefinition(
                                                       states: new List<State>()
                                                       {
                                                                new State(
                                                                            name: "StateA",
                                                                            events:
                                                                            new List<Event>()
                                                                            {
                                                                                new Event(name : "EventA")
                                                                            }
                                                                    )
                                                       }

                                                    );

            FlowInstance flowInstance;


            var ex = Assert.Throws<FlowInstanceException>(
                () =>
                {
                    flowInstance = new FlowInstance(currentState: "StateB", flowDefinition: flowDefinition);
                });
        }

        [Test]
        public void Flow_Instance_Construction_Missing_Flow_Definition_Throws_Error()
        {
            FlowInstance flowInstance;
            var ex = Assert.Throws<FlowInstanceException>(
                () =>
                {
                    flowInstance = new FlowInstance(flowDefinition: null, currentState: "StateB");
                });
        }


        [Test]
        public void Event_Does_Not_Change_The_State()
        {
            var flowDefinition = new FlowDefinition(
                                                        states: new List<State>()
                                                        {
                                                            new State(
                                                                        name:"State1",
                                                                        events:
                                                                        new List<Event>()
                                                                        {
                                                                            new Event(name : "Event1")
                                                                        }
                                                                      )
                                                        }

                                                   );

            var flowInstance = new FlowInstance(
                                                currentState: "State1",
                                                flowDefinition: flowDefinition
                                                );

            var result = flowInstance.SendEvent("Event1");

            Assert.AreEqual("State1", flowInstance.CurrentState);

        }
        [Test]
        public void Event_Triggers_Action()
        {
            var mockAction = new Mock<IAction>();

            var flowDefinition = new FlowDefinition(
                                                    states: new List<State>()
                                                    {
                                                        new State(
                                                                    name:"State1",
                                                                    events:
                                                                    new List<Event>()
                                                                    {
                                                                        new Event(
                                                                                    name : "Event1",
                                                                                    actions : new List<IAction>()
                                                                                    {
                                                                                        mockAction.Object
                                                                                    }
                                                                                  )
                                                                    }
                                                            )
                                                    }
                                                   );

            var flowInstance = new FlowInstance(
                                                  currentState: "State1",
                                                  flowDefinition: flowDefinition
                                                );

            var result = flowInstance.SendEvent("Event1");
            mockAction.Verify(action => action.Execute());

        }

        [Test]
        public void Action_Execution_Creates_Logs()
        {
            var mockAction = new Mock<IAction>();

            var flowDefinition = new FlowDefinition(
                                                    states: new List<State>()
                                                    {
                                                        new State(
                                                                    name:"State1",
                                                                    events:
                                                                    new List<Event>()
                                                                    {
                                                                        new Event(
                                                                                    name : "Event1",
                                                                                    actions : new List<IAction>()
                                                                                    {
                                                                                        mockAction.Object
                                                                                    }
                                                                                  )
                                                                    }
                                                                  )
                                                    }
                                                    );

            var flowInstance = new FlowInstance(
                                                    currentState: "State1",
                                                    flowDefinition: flowDefinition
                                                );


            var result = flowInstance.SendEvent("Event1");

            Assert.IsTrue(result.CreatedLogs.Any(cl => cl.LogType == "Action_PostExecution"));
        }

        [Test]
        public void There_Should_Be_No_Action_Log_If_No_Action_Executed()
        {
            var flowDefinition = new FlowDefinition(
                                                    states: new List<State>()
                                                    {
                                                        new State(
                                                            name: "State1",
                                                            events:
                                                                    new List<Event>()
                                                                    {
                                                                        new Event(name : "Event1")
                                                                    }
                                                            )
                                                    }
                                                   );

            var flowInstance = new FlowInstance(
                                                currentState: "State1",
                                                flowDefinition: flowDefinition
                                                );
            {

            };

            var result = flowInstance.SendEvent("Event1");

            Assert.IsFalse(result.CreatedLogs.Any(cl => cl.LogType == "ActionLog"));
        }


        public class FlowDataReadingFakeAction : IAction
        {
            [FlowData]
            public string SomeFlowData { get; set; }

            [FlowData]
            public int SomeFlowDataInt { get; set; }

            public string TestingField { get; set; }
            public int TestingFieldInt { get; private set; }

            public void Execute()
            {
                TestingField = SomeFlowData;
                TestingFieldInt = SomeFlowDataInt;
            }
        }
        [Test]
        public void Action_Can_Read_Flow_Data()
        {
            string testText = "bebek";
            int testInt = 1;

            var flowDataReadingAction = new FlowDataReadingFakeAction();

            var flowDefinition = new FlowDefinition(
                                                    states: new List<State>()
                                                    {
                                                        new State(
                                                                    name:"State1",
                                                                    events:
                                                                    new List<Event>()
                                                                    {
                                                                        new Event(
                                                                                    name : "Event1",
                                                                                    actions : new List<IAction>()
                                                                                    {
                                                                                        flowDataReadingAction
                                                                                    }
                                                                                  )
                                                                    }
                                                            )
                                                    }
                                                   );

            dynamic flowData = new ExpandoObject();


            flowData.SomeFlowData = testText; ;
            flowData.SomeFlowDataInt = testInt;

            var flowInstance = new FlowInstance(
                                                  currentState: "State1",
                                                  flowDefinition: flowDefinition,
                                                  flowData: flowData
                                                );

            var result = flowInstance.SendEvent("Event1");

            Assert.AreEqual(testText, flowDataReadingAction.TestingField);
            Assert.AreEqual(testInt, flowDataReadingAction.TestingFieldInt);

        }

        public class FlowDataWritingFakeAction : IAction
        {
            [FlowData]
            public string SomeFlowData { get; set; }

            [FlowData]
            public int SomeFlowDataInt { get; set; }

            public string TestingField { get; set; }
            public int TestingFieldInt { get; set; }

            public void Execute()
            {
                SomeFlowData = TestingField;
                SomeFlowDataInt = TestingFieldInt;
            }
        }

        [Test]
        public void Action_Can_Write_Flow_Data()
        {
            string stringTestData = "stringTestData";
            int intTestData = 54;

            var flowDataWritingFakeAction = new FlowDataWritingFakeAction()
            {
                TestingFieldInt = intTestData,
                TestingField = stringTestData
            };

            var flowDefinition = new FlowDefinition(
                                                    states: new List<State>()
                                                    {
                                                        new State(
                                                                    name:"State1",
                                                                    events:
                                                                    new List<Event>()
                                                                    {
                                                                        new Event(
                                                                                    name : "Event1",
                                                                                    actions : new List<IAction>()
                                                                                              {
                                                                                                  flowDataWritingFakeAction
                                                                                              }
                                                                                   )
                                                                    }
                                                             )
                                                    }
                                                   );

            dynamic flowData = new ExpandoObject();
            flowData.SomeFlowData = null as string;
            flowData.SomeFlowDataInt = 0;

            var flowInstance = new FlowInstance(flowDefinition, "State1", flowData: flowData);

            var sendEventResult = flowInstance.SendEvent("Event1");

            Assert.AreEqual(stringTestData, flowData.SomeFlowData);
            Assert.AreEqual(intTestData, flowData.SomeFlowDataInt);
        }




    }
}
