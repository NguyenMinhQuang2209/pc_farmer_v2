using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferenceController : MonoBehaviour
{
    public static PreferenceController instance;

    public static string PLAYER_TAG = "Player";

    public Transform player;
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
        player = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
    }

}
