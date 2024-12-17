using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    [SerializeField] private Image muteImage;
    [SerializeField] private Sprite mutedSprite;
    [SerializeField] private Sprite soundSprite;

    [SerializeField] private bool isSoundOpen = true;

    private string _sceneName;

    private Button _button;
    private void Awake()
    {
        _sceneName = SceneManager.GetActiveScene().name;
        isSoundOpen = PlayerPrefs.GetInt(_sceneName + "Sound", 1) == 1;
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnMuteButtonClicked);

        if (isSoundOpen)
        {
            OpenSounds();
        }
        else
        {
            CloseSounds();
        }
    }

    //private void OnEnable()
    //{
    //}

    public void OnMuteButtonClicked()
    {
        if (!isSoundOpen)
        {
            OpenSounds();
        }
        else
        {
            CloseSounds();
        }
    }

    private void CloseSounds()
    {
        muteImage.sprite = mutedSprite;
        PlayerPrefs.SetInt(_sceneName + "Sound", 0);
        AudioListener.volume = 0;
        isSoundOpen = false;
    }

    private void OpenSounds()
    {
        muteImage.sprite = soundSprite;
        PlayerPrefs.SetInt(_sceneName + "Sound", 1);
        AudioListener.volume = 1;
        isSoundOpen = true;
    }
}
