using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    #region Variables
    [Header("UI")]
    [SerializeField]
    private List<UI> UserInterfaces;
    private readonly Dictionary<UITypes, GameObject> UIDict = new();
    #endregion

    #region Lifecycle
    private void Awake()
    {
        foreach (UI userInterface in UserInterfaces)
        {
            UIDict[userInterface.type] = userInterface.obj;
        }
    }
    #endregion

    #region Functions
    internal void ShowUI(UITypes type)
    {
        UIDict[type].SetActive(true);
    }

    internal void HideUI(UITypes type)
    {
        UIDict[type].SetActive(false);
    }
    #endregion
}

[System.Serializable]
public class UI
{
    public GameObject obj;

    public UITypes type;

    public UI(GameObject UIObj, UITypes UIType)
    {
        obj = UIObj;
        type = UIType;
    }
}

public enum UITypes
{
    None,
    Camera,
    Battery,
    Ingredients,
    EquipedRecipe,
    Recipes,
    Zoom,
    Score,
}
