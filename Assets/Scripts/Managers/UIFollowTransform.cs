using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowTransform : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(player.position);
        gameObject.GetComponent<RectTransform>().position = screenPos + offset;
    }
}


