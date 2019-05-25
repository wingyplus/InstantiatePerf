using System.Runtime.InteropServices;
using NUnit.Framework;
using Unity.Entities;
using Unity.PerformanceTesting;
using UnityEditor;
using UnityEngine;

namespace Tests
{
    public class InstantiatePerfTests
    {
        private GameObject _cubePrefab;

        [OneTimeSetUp]
        public void SetUp()
        {
            _cubePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Cube.prefab");
        }

        [Test, Performance]
        public void BenchmarkEntity_Instantiate()
        {
            var cubeEcsPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(_cubePrefab, World.Active);
            var manager = World.Active.EntityManager;

            Measure
                .Method(() => manager.Instantiate(cubeEcsPrefab))
                .MeasurementCount(10000)
                .GC()
                .Run();
        }

        [Test, Performance]
        public void BenchmarkGameObject_Instantiate()
        {
            Measure
                .Method(() => GameObject.Instantiate(_cubePrefab, Vector3.zero, Quaternion.identity))
                .MeasurementCount(10000)
                .GC()
                .Run();
        }

        [Test, Category("sizeof")]
        public void TestSizeof_Entity()
        {
            Debug.Log($"{Marshal.SizeOf<Entity>()}");
        }
        
        [Test, 
         Category("sizeof"), 
         Ignore("cannot use Marshal.SizeOf with managed type.")]
        public void TestSizeof_GameObject()
        {
            Debug.Log($"{Marshal.SizeOf<GameObject>()}");
        }
    }
}