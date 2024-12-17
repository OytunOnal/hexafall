using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    private Button _button;


    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnRestartButtonClicked);
    }
    // Start is called before the first frame update
    public void OnRestartButtonClicked()
    {
        Time.timeScale = 1;
        //AnalyticsController.Instance.LogReplay(FindObjectOfType<TOKGameController>().GetCurrentLevel().ToString());
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName,LoadSceneMode.Single);
    }
}
