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
    [SerializeField] private List<Building_Position_Center_Item> center_positions_item = new();
    [SerializeField] private float raycastDistance = 0.12f;

    [SerializeField] private LayerMask checkMask;
    [SerializeField] private int buildMask = 7;
    [SerializeField] private LayerMask colliderMask;

    Dictionary<string, bool> raycastChecks = new();
    Dictionary<BuildingPositionName, GameObject> dic_positionItems = new();
    Dictionary<BuildingCenterPositionName, Sprite> dic_centerpositionItems = new();

    bool canBuild = true;

    SpriteRenderer spriteRender;


    private void Start()
    {
        SetupCache();
        if (TryGetComponent<SpriteRenderer>(out spriteRender))
        {

        }
    }

    public void BuildingInit()
    {
        gameObject.layer = buildMask;
        SetupCache();

        if (TryGetComponent<SpriteRenderer>(out spriteRender))
        {

        }

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
        if (position_items != null && position_items.Count > 0)
        {
            for (int i = 0; i < position_items.Count; i++)
            {
                Building_Position_Item item = position_items[i];
                dic_positionItems[item.name] = item.item;
            }
        }

        if (center_positions_item != null && center_positions_item.Count > 0)
        {
            for (int i = 0; i < center_positions_item.Count; i++)
            {
                Building_Position_Center_Item item = center_positions_item[i];
                dic_centerpositionItems[item.name] = item.sprite;
            }
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
        if (position_items != null && position_items.Count > 0)
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


        if (center_positions_item != null && center_positions_item.Count > 0)
        {
            int positionIndex = 0;

            if (raycastChecks[Left])
                positionIndex += 1;
            if (raycastChecks[Right])
                positionIndex += 2;
            if (raycastChecks[Top])
                positionIndex += 4;
            if (raycastChecks[Down])
                positionIndex += 8;

            switch (positionIndex)
            {
                case 0:
                    spriteRender.sprite = dic_centerpositionItems[BuildingCenterPositionName.Not_All];
                    break;
                case 1:
                    spriteRender.sprite = dic_centerpositionItems[BuildingCenterPositionName.Left];
                    break;
                case 2:
                    spriteRender.sprite = dic_centerpositionItems[BuildingCenterPositionName.Right];
                    break;
                case 3:
                    spriteRender.sprite = dic_centerpositionItems[BuildingCenterPositionName.Left_Right];
                    break;
                case 4:
                    spriteRender.sprite = dic_centerpositionItems[BuildingCenterPositionName.Top];
                    break;
                case 5:
                    spriteRender.sprite = dic_centerpositionItems[BuildingCenterPositionName.Top_Left];
                    break;
                case 6:
                    spriteRender.sprite = dic_centerpositionItems[BuildingCenterPositionName.Top_Right];
                    break;
                case 7:
                    spriteRender.sprite = dic_centerpositionItems[BuildingCenterPositionName.Top_Left_Right];
                    break;
                case 8:
                    spriteRender.sprite = dic_centerpositionItems[BuildingCenterPositionName.Bottom];
                    break;
                case 9:
                    spriteRender.sprite = dic_centerpositionItems[BuildingCenterPositionName.Bottom_Left];
                    break;
                case 10:
                    spriteRender.sprite = dic_centerpositionItems[BuildingCenterPositionName.Bottom_Right];
                    break;
                case 11:
                    spriteRender.sprite = dic_centerpositionItems[BuildingCenterPositionName.Bottom_Left_Right];
                    break;
                case 12:
                    spriteRender.sprite = dic_centerpositionItems[BuildingCenterPositionName.Top_Bottom];
                    break;
                case 13:
                    spriteRender.sprite = dic_centerpositionItems[BuildingCenterPositionName.Top_Left_Bottom];
                    break;
                case 14:
                    spriteRender.sprite = dic_centerpositionItems[BuildingCenterPositionName.Top_Right_Bottom];
                    break;
                case 15:
                    spriteRender.sprite = dic_centerpositionItems[BuildingCenterPositionName.All];
                    break;
            }

        }
    }

}
[System.Serializable]
public class Building_Position_Item
{
    public GameObject item;
    public BuildingPositionName name;
}
[System.Serializable]
public class Building_Position_Center_Item
{
    public Sprite sprite;
    public BuildingCenterPositionName name;
}