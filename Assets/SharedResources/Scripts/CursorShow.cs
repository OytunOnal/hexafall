using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CursorShow : MonoBehaviour
{
    public Transform cursorImg;

    private void Start()
    {
#if UNITY_EDITOR
        cursorImg.gameObject.SetActive(true);
#else
        Destroy(gameObject);
#endif
        
    }
#if UNITY_EDITOR
    private void Update()
    {
        Vector3 cursorPos = Input.mousePosition;
        
        cursorImg.position = cursorPos;
        if (Input.GetMouseButtonDown(0))
            cursorImg.localScale = Vector3.one / 2;
        if (Input.GetMouseButtonUp(0))
            cursorImg.localScale = Vector3.one;


    }

#endif
}