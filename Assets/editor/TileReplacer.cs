using UnityEditor;
using UnityEngine;

public class TileReplacer : MonoBehaviour
{
    // Assign your grass prefab in the inspector or load it via Resources in the code
    public GameObject grassPrefab;

    [MenuItem("Tools/Replace Tile Objects With Grass Prefab")]
    private static void ReplaceTileObjectsWithGrass()
    {
        // Get the selected object in the hierarchy
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject == null)
        {
            Debug.LogWarning("Please select the 'Tile' parent object in the hierarchy.");
            return;
        }

        // Ensure the selected object has child tiles to replace
        Transform[] tileChildren = selectedObject.GetComponentsInChildren<Transform>();

        if (tileChildren.Length == 0)
        {
            Debug.LogWarning("No child tiles found to replace.");
            return;
        }

        // Load the grass prefab from the Resources folder (place your prefab in Resources/Prefabs/Grass)
        GameObject grassPrefab = Resources.Load<GameObject>("Prefabs/Tiles/Grass");

        if (grassPrefab == null)
        {
            Debug.LogError("Grass prefab not found in Resources/Prefabs folder.");
            return;
        }

        // Iterate through each child tile and replace it with the grass prefab
        foreach (Transform childTile in tileChildren)
        {
            if (childTile != selectedObject.transform) // Skip the parent object itself
            {
                // Replace the tile with the grass prefab
                GameObject newGrass = PrefabUtility.InstantiatePrefab(grassPrefab) as GameObject;
                newGrass.transform.position = childTile.position;
                newGrass.transform.SetParent(selectedObject.transform, true);

                // Optionally, you can destroy the original child tile object
                DestroyImmediate(childTile.gameObject);
            }
        }

        Debug.Log("Tile objects replaced with grass prefab successfully!");
    }
}
