using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CustomEditor(typeof(CowController))]
public class CowNavMeshUtility : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CowController cow = (CowController)target;

        if (GUILayout.Button("Reposition Cow On NavMesh"))
        {
            RepositionCow(cow);
        }

        if (GUILayout.Button("Reposition ALL Cows In Scene"))
        {
            CowController[] cows = FindObjectsByType<CowController>(FindObjectsSortMode.None);
            int repositioned = 0;

            foreach (var c in cows)
            {
                if (RepositionCow(c))
                    repositioned++;
            }

            Debug.Log($"Repositioned {repositioned} cows on NavMesh.");
        }
    }

    private bool RepositionCow(CowController cow)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(cow.transform.position, out hit, 5f, NavMesh.AllAreas))
        {
            Undo.RecordObject(cow.transform, "Reposition Cow on NavMesh");
            cow.transform.position = hit.position;
            return true;
        }
        else
        {
            Debug.LogWarning($"Cow '{cow.name}' could not be repositioned: no NavMesh nearby.");
            return false;
        }
    }
}