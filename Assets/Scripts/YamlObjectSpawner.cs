using System.Collections.Generic;
using UnityEngine;
using YamlDotNet.Serialization;

namespace PDSim.Simulation
{
    public class YamlObjectSpawner : MonoBehaviour
    {
        public TextAsset yamlFile;

        // One generic prefab for all YAML objects
        public GameObject objectPrefab;

        // Existing PDDL objects
        public Transform problemObjectsParent;

        // Separate parent for YAML-only objects
        public Transform yamlObjectsParent;

        public void SpawnObjects()
        {
            if (yamlFile == null || objectPrefab == null ||
                problemObjectsParent == null)
            {
                Debug.LogError("Missing YAML spawner references.");
                return;
            }

            // Create YAML Objects container if missing
            if (yamlObjectsParent == null)
            {
                GameObject container = new GameObject("YAML Objects");
                yamlObjectsParent = container.transform;
            }

            // Prevent accidentally cloning an entire container
            if (objectPrefab == problemObjectsParent.gameObject ||
                objectPrefab == yamlObjectsParent.gameObject)
            {
                Debug.LogError("Object Prefab cannot be a parent container.");
                return;
            }

            var deserializer = new DeserializerBuilder().Build();

            var yamlData = deserializer.Deserialize<
                Dictionary<string, Dictionary<string, string>>
            >(yamlFile.text);

            if (yamlData == null)
                return;

            foreach (var item in yamlData)
            {
                string objectName = item.Key;

                // Skip existing PDDL objects
                if (problemObjectsParent.Find(objectName) != null)
                    continue;

                // Skip previously spawned YAML objects
                if (yamlObjectsParent.Find(objectName) != null)
                    continue;

                // Spawn new YAML-only object
                GameObject newObject = Instantiate(
                    objectPrefab,
                    yamlObjectsParent
                );

                newObject.name = objectName;

                Debug.Log($"Spawned YAML object: {objectName}");
            }
        }

        private void Start()
        {
            SpawnObjects();

            InformationReader reader =
                FindObjectOfType<InformationReader>();

            if (reader != null)
            {
                reader.ReadYaml();
            }
        }
    }
}