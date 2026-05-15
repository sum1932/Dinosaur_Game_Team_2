using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class MaterialUpgraderTool
{
    [MenuItem("Tools/Upgrade External Materials")]
    public static void Upgrade()
    {
        string path = "Assets/External";
        string[] guids = AssetDatabase.FindAssets("t:Material", new[] { path });
        
        Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");
        if (urpLit == null)
        {
            Debug.LogError("Could not find Universal Render Pipeline/Lit shader.");
            return;
        }

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
            
            if (mat != null && mat.shader.name == "Standard")
            {
                Debug.Log($"Upgrading material: {assetPath}");
                
                // Store old values
                Texture mainTex = mat.GetTexture("_MainTex");
                Color color = mat.GetColor("_Color");
                Texture bumpMap = mat.HasProperty("_BumpMap") ? mat.GetTexture("_BumpMap") : null;
                float metallic = mat.HasProperty("_Metallic") ? mat.GetFloat("_Metallic") : 0f;
                float smoothness = mat.HasProperty("_Glossiness") ? mat.GetFloat("_Glossiness") : 0.5f;

                // Change shader
                mat.shader = urpLit;

                // Apply values to new properties
                mat.SetTexture("_BaseMap", mainTex);
                mat.SetColor("_BaseColor", color);
                if (bumpMap != null) mat.SetTexture("_BumpMap", bumpMap);
                mat.SetFloat("_Metallic", metallic);
                mat.SetFloat("_Smoothness", smoothness);

                EditorUtility.SetDirty(mat);
            }
        }
        
        AssetDatabase.SaveAssets();
        Debug.Log("Material upgrade complete.");
    }
}
