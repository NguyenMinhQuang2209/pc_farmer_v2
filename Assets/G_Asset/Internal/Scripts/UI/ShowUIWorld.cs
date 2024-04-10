using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowUIWorld : MonoBehaviour
{
    public TextMeshProUGUI textShow;
    public float showTimer = 1f;
    public float floatSpeed = 1f;
    float showTime = 0f;
    private void Update()
    {
        transform.position = new(transform.position.x, transform.position.y + floatSpeed * Time.deltaTime, transform.position.z);
    }
    public void ShowUIWorldInit(Vector3 pos, string txt, Color color, float showTimer = -1f)
    {
        showTime = showTimer > 0 ? showTimer : this.showTimer;
        textShow.color = color;
        textShow.text = txt;
        transform.position = pos;
        Invoke(nameof(DeactiveUI), showTime);
    }
    public void DeactiveUI()
    {
        textShow.text = "";
        gameObject.SetActive(false);
        ShowUIWorldController.instance.AddPoolingItem(this);
    }
}
