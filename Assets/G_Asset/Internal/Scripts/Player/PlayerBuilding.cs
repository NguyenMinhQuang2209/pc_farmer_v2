using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilding : MonoBehaviour
{
    [SerializeField] private float offset = 0.16f;
    private PlayerInput playerInput;

    private Transform preview_item = null;
    private Vector2 pos = Vector2.zero;
    private Vector2 rot = Vector2.zero;
    private BuildingItem building_item = null;
    private Dictionary<string, BuildingItem> building_item_poolings = new();
    private BuildingItem store_item = null;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    private void Update()
    {
        FollowMouse();
        if (preview_item != null)
        {
            preview_item.position = pos;
            preview_item.rotation = Quaternion.Euler(rot);

            if (playerInput.onFoot.Build.triggered)
            {
                if (building_item.CanBuild())
                {
                    Building();
                }
            }
        }
    }
    public void FollowMouse()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePosition);
        pos.x = Mathf.Round(worldPos.x / offset) * offset;
        pos.y = Mathf.Round(worldPos.y / offset) * offset;
    }

    public void ChangeBuildingItem(BuildingItem item)
    {
        if (preview_item != null)
        {
            preview_item.gameObject.SetActive(false);
            preview_item = null;
        }
        store_item = item;
        if (item != null)
        {
            string name = item.name;
            if (building_item_poolings.ContainsKey(name))
            {
                building_item = building_item_poolings[name];
            }
            else
            {
                BuildingItem tempItem = Instantiate(item, pos, Quaternion.Euler(rot));
                if (tempItem.TryGetComponent<Interactible>(out var interactible))
                {
                    interactible.enabled = false;
                }
                building_item = tempItem;
                building_item_poolings[name] = tempItem;
            }
            preview_item = building_item.transform;
        }
        else
        {
            building_item = null;
        }
    }
    public void Building()
    {
        if (store_item != null)
        {
            BuildingItem buildingItem = Instantiate(store_item, pos, Quaternion.Euler(rot));
            buildingItem.BuildingInit();
        }
    }
}
