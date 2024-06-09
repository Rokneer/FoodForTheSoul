using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Variables
    public static UIManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField]
    private List<UI> UserInterfaces;
    private readonly Dictionary<UITypes, GameObject> UIDict = new();
    #endregion Variables

    #region Lifecycle
    private void Awake()
    {
        // Checks if there is only one instance of the script in the scene
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        foreach (UI userInterface in UserInterfaces)
        {
            UIDict[userInterface.type] = userInterface.obj;
        }
    }
    #endregion Lifecycle

    #region Functions
    internal void ShowUI(UITypes type)
    {
        UIDict[type].SetActive(true);
    }

    internal void HideUI(UITypes type)
    {
        UIDict[type].SetActive(false);
    }
    #endregion Functions
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
