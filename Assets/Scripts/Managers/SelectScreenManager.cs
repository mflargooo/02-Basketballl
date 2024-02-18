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

    private Color[] uiColors;

    private bool lockState;

    [SerializeField] private AudioClip[] charSelectSFX;
    [SerializeField] private AudioClip menuArrows;
    [SerializeField] private AudioClip readyClip;
    [SerializeField] private AudioClip joinClip;
    [SerializeField] private AudioSource quieterAudio;
    [SerializeField] private AudioSource charSFXSource;

    private void Awake()
    {
        lockState = false;

        uiColors = new Color[4];
        for (int i = 0; i < 4; i++)
        {
            uiColors[i] = playerUI[i].transform.GetChild(0).GetComponent<Image>().color;
        }
        playersReady = new bool[4] { false, false, false, false };

        foreach (GameObject pui in playerUI)
        {
            pui.SetActive(false);
        }

        foreach(GameObject obj in GameInfo.playerInputObjs)
        {
            if (!obj) continue;

            PlayerInput input = obj.GetComponent<PlayerInput>();
            playerIndices.Add(input.playerIndex);
            input.SwitchCurrentActionMap("UI");
            input.ActivateInput();
            ActivatePlayer(input.playerIndex);
            Unready(input.playerIndex);
        }
    }

    public void AddPlayer(int pid)
    {
        if (!playerIndices.Contains(pid))
        {
            playerIndices.Add(pid);
        }
        quieterAudio.GetComponent<AudioSource>().PlayOneShot(joinClip);
        ActivatePlayer(pid);
        Unready(pid);
        StopCountdown();
    }

    public void ActivatePlayer(int pid) 
    {
        playerUI[pid].SetActive(true);
        models[pid].transform.GetChild(GameInfo.characterSelectIndexes[pid]).gameObject.SetActive(true);
    }

    public void RemovePlayer(int pid)
    {
        playerIndices.Remove(pid);
        GameInfo.playerInputObjs[pid] = null;
        DeactivatePlayer(pid);
        StopCountdown();
    }

    public void DeactivatePlayer(int pid)
    {
        playerUI[pid].SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            models[pid].transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void Ready(int pid)
    {
        if (playersReady[pid]) return;

        playersReady[pid] = true;
        playerUI[pid].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        playerUI[pid].transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        playerUI[pid].transform.GetChild(1).gameObject.SetActive(false);
        playerUI[pid].transform.GetChild(2).gameObject.SetActive(false);
        playerUI[pid].transform.GetChild(0).GetComponent<Image>().color = uiColors[pid];

        Camera.main.GetComponent<AudioSource>().PlayOneShot(readyClip);
        charSFXSource.PlayOneShot(charSelectSFX[GameInfo.characterSelectIndexes[pid]]);
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
        playerUI[pid].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        playerUI[pid].transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        playerUI[pid].transform.GetChild(1).gameObject.SetActive(true);
        playerUI[pid].transform.GetChild(2).gameObject.SetActive(true);
        playerUI[pid].transform.GetChild(0).GetComponent<Image>().color = uiColors[pid] * .25f;

        StopCountdown();
    }

    public void SetCharacterIndex(int pid, int v)
    {
        if (lockState || v == 0 || playersReady[pid]) return;

        Camera.main.GetComponent<AudioSource>().PlayOneShot(menuArrows);
        models[pid].transform.GetChild(GameInfo.characterSelectIndexes[pid]).gameObject.SetActive(false);
        
        int newVal = GameInfo.characterSelectIndexes[pid] + v;
        if (newVal > 3) GameInfo.characterSelectIndexes[pid] = 0;
        else if (newVal < 0) GameInfo.characterSelectIndexes[pid] = 3;
        else GameInfo.characterSelectIndexes[pid] = newVal;

        models[pid].transform.GetChild(GameInfo.characterSelectIndexes[pid]).gameObject.SetActive(true);
    }

    private IEnumerator StartGame()
    {
        lockState = true;
        GameInfo.playerIndices = playerIndices;
        float timer = countdownTimer;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            centerText.text = ((int)Mathf.Ceil(timer)).ToString();
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void StopCountdown()
    {
        lockState = false;
        StopAllCoroutines();
        centerText.text = "Select Your Character";
    }
}
