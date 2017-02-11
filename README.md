# backend-flow-controller
backend flow controller (bfc)

## Motivation
- ability to design or view the application flow with a human readable way
- to have full control over what is going on behind the scenes
- separating the API from UI ( or any other consumer )


## Some Pseudo Tests:

#### Test 1: Start a flow
```
// this looks like how the api is consumed by consumer

var flowController = new FlowController("..flow definition here..");

var flowStartResult = flowController.Start("..application data here..");

Assert.IsTrue(flowStartResult.Successed);

Assert.IsEqual(flowStartResult.CurrentFlowState, "an expected state name like DraftState");

var flowInstanceId = flowStartResult.FlowInstanceId;

```

#### Test 2: Send an event to the flow and flow changes its state.
```

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

var result = flowInstance.SendEvent("Event1");

Assert.IsTrue(result.Suceeded);
Assert.IsEqual(flowInstance.CurrentState, "State2");
```

#### Test 3: Send an event to the flow and flow triggers an action (which is defined in flow definition)
 ```
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
        Name = "Event1",
        Actions = new List<Action>()
        {	
        	mockAction.Object
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

```


#### Test 4: Send an event to the flow and flow triggers an action which writes changes flow data (which is defined in flow definition)

#### Test 5: Action throws exception, roll back?, result?

#### Test 6: Events and actions logs created?

#### Test 7: Validations: Proper state name? Next state's name exists?
