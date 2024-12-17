using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IframeButton : MonoBehaviour
{
    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(AddIframeButtonClicked);
    }

    public void AddIframeButtonClicked()
    {
        Time.timeScale = 0;
        string jsCode = @"
        function getUrlParameter(name) {
            name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
            var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
            var results = regex.exec(location.search);
            return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
        };

        function redirect() {
        var userId = getUrlParameter('userId');
        var contentId = getUrlParameter('contentId');
        var redirectUrl = 'https://play.q-u-i-l-t.com/#feedback';
        redirectUrl = `${redirectUrl}?userId=${userId}&contentId=${contentId}`
         window.top.location.href = redirectUrl;
        }
         redirect();";       
        Application.ExternalEval(jsCode);
    }
}
