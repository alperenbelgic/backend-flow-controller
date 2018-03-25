using Microsoft.CodeAnalysis.CSharp.Scripting;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendFlowController.Tests
{
    [TestFixture]
    public class JsonFlowDefinitionCreatorTests
    {


        //var o = JsonConvert.DeserializeObject(content);



        [Test]
        public void Flow_Definition_With_No_State()
        {
            string jsonContent = @"
{
    ""States"":[]
}
";
            var creator = new JSonFlowDefinitionCreator(jsonContent);
            var definition = creator.Create();

            Assert.AreEqual(0, definition.GetStates().Count);
        }

        [Test]
        public void Flow_Definition_With_One_State()
        {
            string jsonContent = @"
{
    ""States"":[
        {
            
        }        
     ]
}
";
            var creator = new JSonFlowDefinitionCreator(jsonContent);
            var definition = creator.Create();

            Assert.AreEqual(1, definition.GetStates().Count);

        }

        [Test]
        public void Flow_Definition_With_State_Name()
        {
            string jsonContent =
@"
{
    ""States"":[
        {
            ""Name"": ""name of first state""
        }        
     ]
}
";

            var creator = new JSonFlowDefinitionCreator(jsonContent);
            var definition = creator.Create();

            Assert.AreEqual("name of first state", definition.GetStates().First().Name);
        }


        [Test]
        public void Flow_Definition_With_State_And_Event()
        {
            string jsonContent =
@"
{
    ""States"":[
        {
            ""Name"": ""name of first state"",
            ""Events"": [
                {
                
                }
            ]
        }        
     ]
}
";
            var creator = new JSonFlowDefinitionCreator(jsonContent);
            var definition = creator.Create();

            Assert.AreEqual(1, definition.GetStates().First().GetEvents().Count);
        }

        [Test]
        public void Flow_Definition_With_State_And_Event_And_Event_Name()
        {
            string jsonContent =
@"
{
    ""States"":[
        {
            ""Name"": ""name of first state"",
            ""Events"": [
                {
                    ""Name"": ""name of the event""
                }
            ]
        }        
     ]
}
";
            var creator = new JSonFlowDefinitionCreator(jsonContent);
            var definition = creator.Create();

            Assert.AreEqual("name of the event", definition.GetStates().First().GetEvents().First().Name);
        }

        [Test]
        public void Flow_Definition_With_State_And_Event_And_1_Action()
        {
            string jsonContent =
@"
{
    ""States"":[
        {
            ""Name"": ""name of first state"",
            ""Events"": [
                {
                    ""Name"": ""name of the event"",
                    ""Actions"": [
                        {

                        }
                    ]
                }
            ]
        }        
     ]
}
";
            var creator = new JSonFlowDefinitionCreator(jsonContent);
            var definition = creator.Create();

            Assert.AreEqual(1, definition.GetStates().First().GetEvents().First().GetActions().Count);
        }


        public string A;
        public string B;

        /*[Test]*/
        public void RoslynPOC()
        {
            Assert.AreEqual(1, 1);

            StreamReader sr = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2017\FlowController\backend-flow-controller\BackendFlowController\BackendFlowController.Tests\SampleFlowDefinitionFile.json");
            var content = sr.ReadToEnd();

            //var o = JsonConvert.DeserializeObject(content);
            B = A = "texthere";


            {

                var script = CSharpScript.Create<string>(code: "A" as string, globalsType: typeof(JsonFlowDefinitionCreatorTests));
                script.Compile();

                var b = script.RunAsync(this);
                b.Wait();
                var r = b.Result.ReturnValue;
                Assert.AreEqual(A, r);
            }

            {

                var script = CSharpScript.Create<string>(code: "B" as string, globalsType: typeof(JsonFlowDefinitionCreatorTests));
                script.Compile();

                var b = script.RunAsync(this);
                b.Wait();
                var r = b.Result.ReturnValue;
                Assert.AreEqual(A, r);
            }


            {
                //CSharpScript.Create()
            }
        }



    }
}
