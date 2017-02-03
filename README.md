# back-end-flow-controller
back end flow controller (bef-c)xx

- ability to design or view the application flow
- to have full control over what is going on behind the scenes
- separating the API from UI ( or any other consumer )


Some Pseudo Test:

<code>
var flowController = new FlowController("..flow definition here..");

var flowStartResult = flowController.Start("..data here..");

Assert.IsTrue(flowStartResult.Successed);

Assert.IsEqual(flowStartResult.CurrentFlowState == "expected state name like DraftState");

</code>
