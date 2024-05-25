using UnityEditor;
using UnityEngine;

public class PhotoEditor : Editor
{
    protected PhotoObjectData photoObject;

    private void OnEnable()
    {
        photoObject = target as PhotoObjectData;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawPreviewGUI(photoObject.model, "Model");
    }

    protected void DrawPreviewGUI(Object asset, string label)
    {
        Texture2D texture = AssetPreview.GetAssetPreview(asset);
        if (texture != null)
        {
            GUILayout.Label($"{label} Preview");
            GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));
            GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
        }
    }
}
