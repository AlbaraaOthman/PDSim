using UnityEditor;
using UnityEngine;
using PDSim.Connection;
using PDSim.Editor;
using PDSim.Utils;
using PDSim.PlanningModel;
using PDSim.Simulation;

[InitializeOnLoad]
public static class PdSimServerPoller
{
    private static double lastPollTime;
    private const double PollInterval = 5.0;

    private static byte[] previousProblem;
    private static byte[] previousPlan;

    private const string simName = "CustomYaml";

    static PdSimServerPoller()
    {
        EditorApplication.update += Update;
        Debug.Log("PDSim server poller started.");
    }

    private static void Update()
    {
        if (EditorApplication.timeSinceStartup - lastPollTime < PollInterval)
            return;

        lastPollTime = EditorApplication.timeSinceStartup;

        PollServer();
    }

    private static void PollServer()
    {
        Debug.Log("Polling PDSim server...");

        var problemRequest = new ProtobufRequest("problem");
        var problemResponse = problemRequest.Connect();

        var planRequest = new ProtobufRequest("plan");
        var planResponse = planRequest.Connect();

        if (problemResponse == null || planResponse == null)
        {
            Debug.LogWarning("Problem or plan not received.");
            return;
        }

        // First successful poll
        if (previousProblem == null || previousPlan == null)
        {
            previousProblem = problemResponse;
            previousPlan = planResponse;

            Debug.Log("Initial problem and plan stored.");
            return;
        }

        bool problemChanged =
            !ByteArraysEqual(previousProblem, problemResponse);

        bool planChanged =
            !ByteArraysEqual(previousPlan, planResponse);

        if (problemChanged || planChanged)
        {
            Debug.Log("PDSim data has changed!");

            if (problemChanged)
                Debug.Log("Problem changed.");

            if (planChanged)
                Debug.Log("Plan changed.");


            AssetUtils.CreateFolders(simName);

            var reader = new ProtobufReader();

            reader.Read(
                problemResponse,
                planResponse,
                simName
            );

            var simulationDataRoot = AssetUtils.GetSimulationDataPath(simName);

            var problemPath = simulationDataRoot + "/PdSimProblem.asset";

            var instancePath = simulationDataRoot + "/PdSimInstance.asset";

            var newProblem = AssetDatabase.LoadAssetAtPath<PdSimProblem>(problemPath);
            var newInstance = AssetDatabase.LoadAssetAtPath<PdSimInstance>(instancePath);

            var manager = Object.FindObjectOfType<PdSimManager>();


            if (manager == null)
            {
                Debug.LogWarning("No PdSimSimulationManager found in the scene. Please add one to manage the simulation.");
            }
            else
            {
                manager.problemModel = newProblem;
                manager.problemInstance = newInstance;
            }

            EditorUtility.SetDirty(manager);

            manager.SetUpObjects();

            Debug.Log("New PDSim assets created.");

            previousProblem = problemResponse;
            previousPlan = planResponse;
        }
        else
        {
            Debug.Log("No change.");
        }
    }

    private static bool ByteArraysEqual(byte[] a, byte[] b)
    {
        if (a.Length != b.Length)
            return false;

        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i])
                return false;
        }

        return true;
    }

}