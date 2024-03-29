using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public static CursorController instance;

    [HideInInspector] public string currentCursor = "";
    private List<GameObject> currents = new();
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void ChangeCursor(string newCursor, List<GameObject> newList)
    {
        if (currents != null)
        {
            for (int i = 0; i < currents.Count; i++)
            {
                currents[i].SetActive(false);
            }
        }
        if (currentCursor == newCursor)
        {
            currentCursor = "";
            currents = null;
            return;
        }
        currentCursor = newCursor;
        currents = newList;
        if (currents != null)
        {
            for (int i = 0; i < currents.Count; i++)
            {
                currents[i].SetActive(true);
            }
        }
    }
}
