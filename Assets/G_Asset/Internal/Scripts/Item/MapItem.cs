using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapItem : MonoBehaviour
{
    public TextMeshProUGUI nameTxt;
    public Button clickBtn;
    private MapItemInit item = null;
    private void Start()
    {
        clickBtn.onClick.AddListener(() =>
        {
            MovePosition();
        });
    }
    public void MovePosition()
    {
        if (item.spawnPosition != null)
        {
            Transform player = PreferenceController.instance.GetPlayer();
            if (player != null)
            {
                player.transform.position = item.spawnPosition.position;
                CursorController.instance.ChangeCursor("", null);
            }
        }
    }
    public void MapItemInit(MapItemInit item)
    {
        this.item = item;
        nameTxt.text = item.mapName + "\n" + "(" + item.GetMode() + ")";
    }
}
