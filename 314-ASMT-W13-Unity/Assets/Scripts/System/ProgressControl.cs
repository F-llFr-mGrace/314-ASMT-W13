using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using System;

public class ProgressControl : MonoBehaviour
{
    public UnityEvent<string> OnStartGame;
    public UnityEvent<string> OnChallengeComplete;

    [Header("Start Button")]
    [SerializeField] XrButtonInteractable startButton;
    [SerializeField] GameObject keyIndicatorLight;

    [Header("Drawer Interactable")]
    [SerializeField] DrawerInteractable drawer;
    XRSocketInteractor drawerSocket;

    [Header("Challenge Settings")]
    [SerializeField] string startGameString;
    [SerializeField] string[] challengeStrings;
    private bool startGameBool;
    private int challengeNumber;

    // Start is called before the first frame update
    void Start()
    {
        if (startButton != null)
        {
            startButton.selectEntered.AddListener(StartButtonPressed);
        }
        OnStartGame?.Invoke(startGameString);
        SetDrawerInteractable();
    }
    private void ChallengeComplete()
    {
        challengeNumber ++;
        if(challengeNumber < challengeStrings.Length)
        {
            OnChallengeComplete?.Invoke(challengeStrings[challengeNumber]);
        }
        else if(challengeNumber >= challengeStrings.Length)
        {
            //****ALL CHALLENGES COMPLETE
        }
    }
    private void StartButtonPressed(SelectEnterEventArgs arg0)
    {
        if (!startGameBool)
        {
            startGameBool = true;
            if (keyIndicatorLight != null)
            {
                keyIndicatorLight.SetActive(true);
            }
            if (challengeNumber < challengeStrings.Length)
            {
                OnStartGame?.Invoke(challengeStrings[challengeNumber]);
            }
        }
    }
    private void SetDrawerInteractable()
    {
        if(drawer != null)
        {
            drawer.OnDrawerDetach.AddListener(OnDrawerDetach);
            drawerSocket  = drawer.GetKeySocket;
            if(drawerSocket != null)
            {
                drawerSocket.selectEntered.AddListener(OnDrawerSocketed);
            }
        }
    }

    private void OnDrawerDetach()
    {
        ChallengeComplete();
    }

    private void OnDrawerSocketed(SelectEnterEventArgs arg0)
    {
        ChallengeComplete();
    }
}
