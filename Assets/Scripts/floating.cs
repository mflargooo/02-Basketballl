using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floating : MonoBehaviour
{
    [SerializeField] float amplitude = 0.02f;
    [SerializeField] float frequency = 1f;
    [SerializeField] private float rotateSpeed;

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

        transform.Rotate(rotateSpeed * Vector3.up * Time.deltaTime);

    }
}