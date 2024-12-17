using UnityEngine;
using UnityEngine.UI;

public class CloseWebViewButton : MonoBehaviour
{
    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(CloseWebView);
    }

    public void CloseWebView()
    {
        AudioListener.volume = 0;
        // Call the JavaScript function to close the web view
        Application.ExternalEval("closeWebView();");
    }
}