using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public static MapController instance;
    public GameObject map_ui_container;
    public Transform map_content;
    public Scrollbar map_slider;
    public float defaultMapValue = 0f;
    public MapItem mapItem;
    public int previousCaveIndex = 0;

    [SerializeField] private List<CaveInteract> mapItems = new();
    private Dictionary<int, GameObject> storeObjects = new();
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        map_ui_container.SetActive(true);
        SpawnMapItem();
        map_ui_container.SetActive(false);
    }
    public void InteractWithMap(int index)
    {
        storeObjects[previousCaveIndex].SetActive(true);
        previousCaveIndex = index;
        storeObjects[previousCaveIndex].SetActive(false);
        CursorController.instance.ChangeCursor("Map", new() { map_ui_container });
    }
    public void SpawnMapItem()
    {
        foreach (Transform child in map_content.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < mapItems.Count; i++)
        {
            MapItem tempItem = Instantiate(mapItem, map_content.transform, false);
            tempItem.MapItemInit(mapItems[i]);
            storeObjects[i] = tempItem.gameObject;
            mapItems[i].ChangeCaveIndex(i);
        }

        map_slider.value = defaultMapValue;
    }
}