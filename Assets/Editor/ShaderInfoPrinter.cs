using UnityEditor;
using UnityEngine;

public class ShaderInfoPrinter
{
    [MenuItem("Tools/Print URP Shader Info")]
    public static void Print()
    {
        Shader lit = Shader.Find("Universal Render Pipeline/Lit");
        if (lit != null)
        {
            string path = AssetDatabase.GetAssetPath(lit);
            string guid = AssetDatabase.AssetPathToGUID(path);
            Debug.Log($"Shader: {lit.name}, Path: {path}, GUID: {guid}");
        }
        else
        {
            Debug.LogError("Could not find Universal Render Pipeline/Lit shader.");
        }
    }
}
