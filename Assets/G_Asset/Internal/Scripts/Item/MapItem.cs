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
        Debug.Log(item.mapName);
    }
    public void MapItemInit(MapItemInit item)
    {
        this.item = item;
        nameTxt.text = item.mapName + "\n" + "(" + item.GetMode() + ")";
    }
}
