using UnityEngine;
using UnityEditor;
public class Test : MonoBehaviour
{
    [MenuItem("Tools/Half Scale All Selected Objects")]
    private static void HalfScaleSelected()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            Undo.RecordObject(obj.transform, "Half Scale");
            Vector3 currentScale = obj.transform.localScale;
            obj.transform.localScale = currentScale * 0.5f;
        }

        Debug.Log("Selected objects scaled to half.");
    }
}
