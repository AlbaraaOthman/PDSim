using System.Collections.Generic;
using UnityEngine;
using YamlDotNet.Serialization;
using TMPro;
namespace PDSim.Simulation
{
    public class InformationReader : MonoBehaviour
    {
        // Drag your YAML file here in the Unity Inspector
        public TextAsset yamlFile;

        // All YAML data
        private Dictionary<string, Dictionary<string, string>> yamlData;

        private void Awake()
        {
            if (yamlFile == null)
            {
                Debug.LogError("No YAML file assigned to InformationReader.");
                return;
            }

        }

        private float LocationParser(string locationString)
        {
            if (locationString.StartsWith("n"))
            {
                return float.Parse(locationString.Substring(1));
            } else
            {
                return float.Parse(locationString);
            }
        }

        public void ReadYaml()
        {
            var deserializer = new DeserializerBuilder().Build();

            yamlData = deserializer.Deserialize<
                Dictionary<string, Dictionary<string, string>>
            >(yamlFile.text);

            // Print every item in the YAML
            foreach (var item in yamlData)
            {
                string itemName = item.Key;

                string colour = GetProperty(item.Value, "colour");
                string dimension = GetProperty(item.Value, "dimension");
                string descriptor = GetProperty(item.Value, "descriptior");
                string initialLocation = GetProperty(item.Value, "initial_location");

                // Debug.Log(
                //     "Item: " + itemName +
                //     " | Colour: " + colour +
                //     " | Dimension: " + dimension +   
                //     " | Descriptor: " + descriptor
                // );

                GameObject problemObjects = GameObject.Find("Problem Objects");
                GameObject yamlObjects = GameObject.Find("YAML Objects");

                Transform itemTransform = null;

                // Search planning objects first
                if (problemObjects != null)
                {
                    itemTransform = problemObjects.transform.Find(itemName);
                }

                // Search additional YAML objects
                if (itemTransform == null && yamlObjects != null)
                {
                    itemTransform = yamlObjects.transform.Find(itemName);
                }

                if (itemTransform == null)
                {
                    Debug.LogWarning("Could not find Problem Object: " + itemName);
                    continue;
                }

                GameObject itemObject = itemTransform.gameObject;
                

                GameObject mainBlock = itemObject.transform.Find("Cube").gameObject;
                
                
                // Set the colour of the item


                string[] split_colour = colour.Split('_');

                float r_float = float.Parse(split_colour[1]) / 255f;
                float g_float = float.Parse(split_colour[2]) / 255f;
                float b_float = float.Parse(split_colour[3]) / 255f;

                Color currColour = new Color(r_float, g_float, b_float);

                MeshRenderer gameObjectRenderer = mainBlock.GetComponent<MeshRenderer>();

                Material newMaterial = new Material(Shader.Find("Standard"))
                {
                    color = currColour
                };
                gameObjectRenderer.material = newMaterial;

                // Set the dimension of the item
                string[] split_dimension = dimension.Split('_');
                float dimensionX = float.Parse(split_dimension[1]);
                float dimensionZ = float.Parse(split_dimension[2]);
                float dimensionY = float.Parse(split_dimension[3]);


                mainBlock.transform.localScale = new Vector3(dimensionX, dimensionY, dimensionZ);

                    
                string[] split_location = initialLocation.Split('_');
                Debug.Log("Initial Location: " + initialLocation);

                float locationX = LocationParser(split_location[1]);
                float locationZ = LocationParser(split_location[2]);
                float locationY = LocationParser(split_location[3]);

                mainBlock.transform.localPosition = new Vector3(locationX, locationY + dimensionY / 2f, locationZ);

                
                // Set the descriptor of the item

                TextMeshPro textMesh = mainBlock.transform.Find("Item_Label").GetComponent<TextMeshPro>();
                textMesh.text = descriptor;




                DisableLocationMeshes();
            }
        }

    private void DisableLocationMeshes()
    {
        GameObject problemObjects = GameObject.Find("Problem Objects");

        if (problemObjects == null)
        {
            Debug.LogWarning("Could not find Problem Objects.");
            return;
        }

        foreach (Transform child in problemObjects.transform)
        {
            // Only affect loc_ objects
            if (!child.name.StartsWith("loc_"))
                continue;

            // Disable all renderers on this location
            MeshRenderer[] renderers =
                child.GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer renderer in renderers)
            {
                renderer.enabled = false;
            }
        }
    }


        private string GetProperty(
            Dictionary<string, string> item,
            string property)
        {
            if (item.TryGetValue(property, out string value))
            {
                return value;
            }

            return null;
        }

        // Get the colour of an item
        public string GetColour(string itemName)
        {
            if (yamlData.TryGetValue(itemName, out var item))
            {
                return GetProperty(item, "colour");
            }

            return null;
        }

        // Get the dimension of an item
        public string GetDimension(string itemName)
        {
            if (yamlData.TryGetValue(itemName, out var item))
            {
                return GetProperty(item, "dimension");
            }

            return null;
        }

        // Get the descriptor of an item
        public string GetDescriptor(string itemName)
        {
            if (yamlData.TryGetValue(itemName, out var item))
            {
                return GetProperty(item, "descriptior");
            }

            return null;
        }
    }
}