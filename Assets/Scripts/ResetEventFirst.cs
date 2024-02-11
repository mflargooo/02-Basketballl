using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResetEventFirst : MonoBehaviour
{
    [SerializeField] private EventSystem es;
    [SerializeField] private GameObject playButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!es.firstSelectedGameObject) es.firstSelectedGameObject = playButton;
    }
}
