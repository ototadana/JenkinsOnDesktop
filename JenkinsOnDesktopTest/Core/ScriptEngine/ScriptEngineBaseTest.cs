using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Threading;
using XPFriend.JenkinsOnDesktop.Core.Folder;

namespace XPFriend.JenkinsOnDesktop.Core.ScriptEngine
{
    [TestClass]
    public class ScriptEngineBaseTest
    {
        private CultureInfo defaultUICulture;

        [TestInitialize]
        public void Setup()
        {
            defaultUICulture = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            TestUtil.UpdateWorkspaceFolder(GetType().Name);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Thread.CurrentThread.CurrentUICulture = defaultUICulture;
        }

        [TestMethod]
        public void TestScriptError()
        {
            // setup
            BusinessesFolder.Initialize(BusinessesFolder.CheckJobStatus);

            try
            {
                // when
                BusinessScriptEngine.Execute(BusinessesFolder.CheckJobStatus, null, new Hashtable());
                Assert.Fail("ここにはこない");
            }
            catch (ApplicationException e)
            {
                // then
                Console.WriteLine(e.Message);
                Assert.IsTrue(e.Message.StartsWith("Cannot process command because of "));
            }
        }

        [TestMethod]
        public void TestErrorTheScriptMustReturnObject()
        {
            // setup
            BusinessesFolder.Initialize(BusinessesFolder.TimeKeeping);
            string script = Path.Combine(BusinessesFolder.GetFolder(BusinessesFolder.TimeKeeping), "main.ps1");
            TestUtil.Replace(script, "    $newReport\r\n}", "}");

            try
            {
                // when
                BusinessScriptEngine.Execute(BusinessesFolder.TimeKeeping, null, new Hashtable());
                Assert.Fail("ここにはこない");
            }
            catch (ApplicationException e)
            {
                // then
                Console.WriteLine(e.Message);
                Assert.AreEqual("the script must return a instance of Hashtable", e.Message);
            }
        }
    }
}
