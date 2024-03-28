using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingItem : MonoBehaviour
{

    public static string Left = "Left";
    public static string Right = "Right";
    public static string Top = "Top";
    public static string Down = "Down";

    [SerializeField] private List<Building_Position_Item> position_items = new();
    [SerializeField] private float raycastDistance = 0.12f;

    [SerializeField] private LayerMask checkMask;
    [SerializeField] private int buildMask;
    [SerializeField] private LayerMask colliderMask;

    Dictionary<string, bool> raycastChecks = new();
    Dictionary<BuildingPositionName, GameObject> dic_positionItems = new();

    bool canBuild = true;


    private void Start()
    {
        SetupCache();
    }

    public void BuildingInit()
    {
        gameObject.layer = buildMask;
        SetupCache();

        if (TryGetComponent<Interactible>(out var interact))
        {
            interact.enabled = true;
        }

        ReloadSpineMain();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;
        canBuild = ((1 << layer) & colliderMask) == 0;
    }

    public void SetupCache()
    {
        for (int i = 0; i < position_items.Count; i++)
        {
            Building_Position_Item item = position_items[i];
            dic_positionItems[item.name] = item.item;
        }
    }

    public bool CanBuild()
    {
        return canBuild;
    }

    public void ReloadSpine()
    {
        Vector2 pos = transform.position;
        raycastChecks?.Clear();
        CheckRayCast(pos, transform.right * -1f, Left);
        CheckRayCast(pos, transform.right, Right);
        CheckRayCast(pos, transform.up, Top);
        CheckRayCast(pos, transform.up * -1f, Down);

        CheckActivePosition();
    }

    public void ReloadSpineMain()
    {
        Vector2 pos = transform.position;
        raycastChecks?.Clear();
        CheckRayCastMainSpine(pos, transform.right * -1f, Left);
        CheckRayCastMainSpine(pos, transform.right, Right);
        CheckRayCastMainSpine(pos, transform.up, Top);
        CheckRayCastMainSpine(pos, transform.up * -1f, Down);

        CheckActivePosition();
    }

    public void CheckRayCast(Vector2 start, Vector2 target, string pos)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(start, target, raycastDistance, checkMask);
        raycastChecks[pos] = hits.Length > 1;
    }

    public void CheckRayCastMainSpine(Vector2 start, Vector2 target, string pos)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(start, target, raycastDistance, checkMask);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject != this)
            {
                if (hits[i].collider.gameObject.TryGetComponent<BuildingItem>(out var item))
                {
                    item.ReloadSpine();
                }
            }
        }
        raycastChecks[pos] = hits.Length > 1;
    }

    public void CheckActivePosition()
    {
        dic_positionItems[BuildingPositionName.Top_Left].SetActive(!raycastChecks[Top]);
        dic_positionItems[BuildingPositionName.Top_Right].SetActive(!raycastChecks[Top]);
        dic_positionItems[BuildingPositionName.Top_Center].SetActive(!raycastChecks[Top]);

        dic_positionItems[BuildingPositionName.Bottom_Left].SetActive(!raycastChecks[Down]);
        dic_positionItems[BuildingPositionName.Bottom_Center].SetActive(!raycastChecks[Down]);
        dic_positionItems[BuildingPositionName.Bottom_Right].SetActive(!raycastChecks[Down]);

        dic_positionItems[BuildingPositionName.Left_Center].SetActive(!raycastChecks[Left]);
        dic_positionItems[BuildingPositionName.Right_Center].SetActive(!raycastChecks[Right]);

        if (raycastChecks[Left])
        {
            dic_positionItems[BuildingPositionName.Top_Left].SetActive(false);
            dic_positionItems[BuildingPositionName.Bottom_Left].SetActive(false);
        }

        if (raycastChecks[Right])
        {
            dic_positionItems[BuildingPositionName.Top_Right].SetActive(false);
            dic_positionItems[BuildingPositionName.Bottom_Right].SetActive(false);
        }
    }

}
[System.Serializable]
public class Building_Position_Item
{
    public GameObject item;
    public BuildingPositionName name;
}
