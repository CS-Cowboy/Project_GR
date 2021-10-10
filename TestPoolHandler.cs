
using NUnit.Framework;
using UnityEngine;

namespace com.braineeeeDevs.gr.Tests
{
    public class TestPoolHandler
    {
        public ExampleObject newObject;
        public PoolHandler handler;

        /// <summary>
        /// Creates one instance of "example" from your Resources folder.
        /// </summary>
        /// <returns></returns>
        public ExampleObject CreateObject()
        {
            return (MonoBehaviour.Instantiate(Resources.Load("example")) as GameObject).GetComponent<ExampleObject>();
        }
        [TearDown]
        public void TearDown()
        {
            GameObject.Destroy(handler);
            GameObject.Destroy(newObject);
        }
        [SetUp]
        public void Setup()
        {
            newObject = CreateObject();
            handler = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<PoolHandler>();
        }

        [Test]
        public void TestStoresObject()
        {
            Setup();
            var obj = CreateObject();
            PoolHandler.GiveObject(obj);
            Assert.IsTrue(handler.Count != 0, "PoolHandler has failed to add a new pool for given object.");
            TearDown();
        }
        [Test]
        public void TestRetrieveObject()
        {
            Setup();
            var obj = CreateObject();
            PoolHandler.GiveObject(obj);
            var result = PoolHandler.GetObject(obj.PoolID);
            Assert.IsNotNull(result, "PoolHandler has failed to add a new pool for given object.");
            TearDown();
        }


    }
}