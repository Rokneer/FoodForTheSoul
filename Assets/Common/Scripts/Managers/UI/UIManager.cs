using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance => _instance;

    [Header("UI")]
    [SerializeField]
    private List<UI> UserInterfaces;
    private readonly Dictionary<UITypes, GameObject> UIDict = new();

    #region Lifecycle
    private void Awake()
    {
        // Checks if there is only one instance of the script in the scene
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        foreach (UI userInterface in UserInterfaces)
        {
            UIDict[userInterface.type] = userInterface.obj;
        }
    }
    #endregion Lifecycle

    internal void ShowUI(UITypes type)
    {
        UIDict[type].SetActive(true);
    }

    internal void HideUI(UITypes type)
    {
        UIDict[type].SetActive(false);
    }
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
    Zoom
}
