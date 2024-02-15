using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floating : MonoBehaviour
{
    public float amplitude = 0.02f;
    public float frequency = 1f;

    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = startPos;
        newPos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = newPos;

    }
}