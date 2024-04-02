using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferenceController : MonoBehaviour
{
    public static PreferenceController instance;

    public static string PLAYER_TAG = "Player";

    [HideInInspector] public Transform player;
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
    public Transform GetPlayer()
    {
        return GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
    }
}
