using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class BackButton : MonoBehaviour
    {
        private Button _button;


    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(BackButtonClick);
    }

    private void BackButtonClick()
    {
        CommonUIManager.Instance.SwitchPanel(CommonPanelType.Exit);
    }
}

