using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour
{
    public Transform player;
    public RectTransform slider;//2dUI
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        follow();
    }
    void follow()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(player.position);
        slider.position = screenPos + offset;
    }
}


