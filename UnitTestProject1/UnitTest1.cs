using System;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BExpansion;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestExpansion()
        {
            var array = new string[] {"s", "ss"};
            Assert.AreEqual("s ss", array.Join(" "));
            
            Assert.AreEqual("C:\\Windows\\", "C:\\Windows".AddDirectorySeparatorChar());

            var r = new Random(-1);
            Assert.AreEqual("g11074", r.GenerateRandomString());

            Assert.AreEqual("te\0st", "te\"st".MakeFileNameSafe());
        }

        [TestMethod]
        public void TestUtils()
        {
            var filename = "temp";
            var contents = "Test Contents ☺☻♥";

            Util.WriteFile(filename, contents, Encoding.UTF8);

            Assert.AreEqual(contents, Util.ReadFile(filename, Encoding.UTF8));

            Assert.IsTrue(Util.GenerateRandomString().Length > 1);

            Assert.AreEqual(String.Empty, Util.SendWebRequest("https://servc.eu/p/sslf/empty"));
        }

        [TestMethod]
        public void TestSafeThreadManager()
        {
            var safeThreadManager = new SafeThreadManager();
            safeThreadManager.RunThread(() =>
            {
                //System.Threading.Thread.Sleep(10000);
                Util.WriteFile("temp2", "this is a test file created from another thread.", Encoding.UTF8);
            });
            safeThreadManager.RunThread(() =>
            {
                //System.Threading.Thread.Sleep(10000);
                Util.WriteFile("temp3", "this is a test file created from another thread.", Encoding.UTF8);
            });

            var hasexcepted = false;
            // Test with a simple System.IndexOutOfRangeException
            safeThreadManager.RunThread(() => { var l = new int[] { 0, 1 }; l[5].ToString(); }, (ex) => { hasexcepted = true; });
            safeThreadManager.WaitForFinish();
            Assert.IsTrue(hasexcepted);
        }

        [TestMethod]
        public void LoopedListTest()
        {
            var looped = new BExpansion.Collections.LoopedList<string>();
            
            looped.Add("test1");
            looped.Add("test2");
            looped.Add("test3");

            Assert.AreEqual("test1", looped[3]);
            Assert.AreEqual("test2", looped[4]);
            Assert.AreEqual("test3", looped[5]);
            
            Assert.AreEqual("test1", looped[30]);
        }
    }
}
