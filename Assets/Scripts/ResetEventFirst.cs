using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResetEventFirst : MonoBehaviour
{
    [SerializeField] private EventSystem es;
    [SerializeField] private GameObject playButton;

    private void Start()
    {
        es.SetSelectedGameObject(playButton);
    }
    public void SetSelectedGameObject(GameObject obj)
    {
        es.SetSelectedGameObject(obj);
    }
}
