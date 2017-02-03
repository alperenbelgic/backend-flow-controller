# back-end-flow-controller
back end flow controller (bef-c)

## Motivation
- ability to design or view the application flow with a human readable way
- to have full control over what is going on behind the scenes
- separating the API from UI ( or any other consumer )


## Some Pseudo Tests:

#### Test 1: Start a flow
```
var flowController = new FlowController("..flow definition here..");

var flowStartResult = flowController.Start("..application data here..");

Assert.IsTrue(flowStartResult.Successed);

Assert.IsEqual(flowStartResult.CurrentFlowState, "an expected state name like DraftState");
```
