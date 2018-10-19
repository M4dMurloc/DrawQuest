using UnityEngine;
using UnityEditor;

public class ClearSavedImage
{

    [MenuItem("Tools/SimpleDrawingCanvas/ClearSavedImage")]
    private static void ClearSavedImageEntry()
    {
        System.IO.File.Delete(Application.persistentDataPath + "/" + DrawingCanvas.FILENAME);
    }
}
