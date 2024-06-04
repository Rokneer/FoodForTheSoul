using UnityEditor;

[CustomEditor(typeof(IngredientData))]
public class IngredientEditor : PhotoEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        IngredientData ingredient = photoObject as IngredientData;

        if (ingredient.sprite == null)
        {
            return;
        }
        DrawPreviewGUI(ingredient.sprite, "Sprite");
    }
}
