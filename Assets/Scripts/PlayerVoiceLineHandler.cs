using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVoiceLineHandler : MonoBehaviour
{
    [SerializeField] private AudioClip[] CantUseVoiceLines;
    private int CantUseVoiceLineIndex = 0;

    private AudioSource audioSource;

    private static PlayerVoiceLineHandler _instance;

    public static PlayerVoiceLineHandler Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        _instance = this;
    }

    public void CantUseThat()
    {
        CantUseVoiceLineIndex++;
        if(CantUseVoiceLineIndex >= CantUseVoiceLines.Length)
        {
            CantUseVoiceLineIndex = 0;
        }

        audioSource.clip = CantUseVoiceLines[CantUseVoiceLineIndex];
        audioSource.Play();
    }
}
