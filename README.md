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
// this looks like how the next method will be processed in the FlowController

var flowInstance = new FlowInstance("..flow definition here..", "");
var nextResult = flowInstance.SendEvent("an expected event name like Submit", "..event data here..");

Assert.IsTrue(nextResult.Successed);
Assert.IsEqual(nextResult.CurrentState.Name == "..expected state name..");

```

#### Test 3: Send an event to the flow and flow triggers an action (which is defined in flow definition)

#### Test 4: Send an event to the flow and flow triggers an action which writes changes flow data (which is defined in flow definition)

#### Test 5: Action throws exception, roll back?, result?

#### Test 6: Events and actions logs created?

#### Test 7: Validations: Proper state name? Next state's name exists?
