using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SelectScreenManager : MonoBehaviour
{
    private bool[] playersReady;
    [SerializeField] private float countdownTimer;
    [SerializeField] private TMP_Text centerText;
    [SerializeField] private GameObject[] playerUI;
    [SerializeField] private GameObject[] models;

    private List<int> playerIndices = new List<int>();
    private int[] characterSelectIndexes;

    private Color[] uiColors;

    private PlayerInput[] pis;
    private bool lockState;

    [SerializeField] private AudioClip[] charSelectSFX;
    [SerializeField] private AudioClip menuArrows;
    [SerializeField] private AudioClip readyClip;
    [SerializeField] private AudioClip joinClip;
    [SerializeField] private AudioSource quieterAudio;

    private void Start()
    {
        pis = new PlayerInput[4];
        uiColors = new Color[4];
        for (int i = 0; i < 4; i++)
        {
            uiColors[i] = playerUI[i].transform.GetChild(0).GetComponent<Image>().color;
        }
        playersReady = new bool[4];
        characterSelectIndexes = new int[4];

        foreach(GameObject pui in playerUI)
        {
            pui.SetActive(false);
        }
    }

    public void AddPlayer(int pid)
    {
        if (!playerIndices.Contains(pid))
        {
            playerIndices.Add(pid);
        }
        playerUI[pid].SetActive(true);
        models[pid].transform.GetChild(characterSelectIndexes[pid]).gameObject.SetActive(true);
        quieterAudio.GetComponent<AudioSource>().PlayOneShot(joinClip);
        Unready(pid);
        StopCountdown();
    }

    public void RemovePlayer(int pid)
    {
        playerIndices.Remove(pid);
        playerUI[pid].SetActive(false);
        for(int i= 0; i < 4; i++)
        {
            models[pid].transform.GetChild(characterSelectIndexes[i]).gameObject.SetActive(false);
        }
        StopCountdown();
    }

    public void Ready(int i)
    {
        playersReady[i] = true;
        playerUI[i].transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Ready!";
        playerUI[i].transform.GetChild(0).GetComponent<Image>().color = uiColors[i];

        Camera.main.GetComponent<AudioSource>().PlayOneShot(readyClip);
        Camera.main.GetComponent<AudioSource>().PlayOneShot(charSelectSFX[characterSelectIndexes[i]]);
        CheckAllReady();
    }

    private void CheckAllReady()
    {
        if (playerIndices.Count < 2) return;
        foreach (int idx in playerIndices)
        {
            if (!playersReady[idx]) return;
        }
        StartCoroutine(StartGame());
    }

    public void Unready(int pid)
    {
        playersReady[pid] = false;
        playerUI[pid].transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Ready?";
        playerUI[pid].transform.GetChild(0).GetComponent<Image>().color = uiColors[pid] * .75f;

        StopCountdown();
    }

    public void SetCharacterIndex(int pid, int v)
    {
        if (lockState || v == 0 || playersReady[pid]) return;

        Camera.main.GetComponent<AudioSource>().PlayOneShot(menuArrows);
        models[pid].transform.GetChild(characterSelectIndexes[pid]).gameObject.SetActive(false);
        
        int newVal = characterSelectIndexes[pid] + v;
        if (newVal > 3) characterSelectIndexes[pid] = 0;
        else if (newVal < 0) characterSelectIndexes[pid] = 3;
        else characterSelectIndexes[pid] = newVal;

        models[pid].transform.GetChild(characterSelectIndexes[pid]).gameObject.SetActive(true);
    }

    private IEnumerator StartGame()
    {
        lockState = true;
        GameInfo.playerIndices = playerIndices;
        GameInfo.characterSelectIndexes = characterSelectIndexes;
        float timer = countdownTimer;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            centerText.text = ((int)Mathf.Ceil(timer)).ToString();
            yield return null;
        }

        GameInfo.characterSelectIndexes = characterSelectIndexes;
        GameInfo.playerIndices = playerIndices;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void StopCountdown()
    {
        lockState = false;
        StopAllCoroutines();
        centerText.text = "Select Your Character";
    }
}
