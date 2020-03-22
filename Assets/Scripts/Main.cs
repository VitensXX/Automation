using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    OnCloseSelf();
        //}
    }

    public void OnCloseSelf()
    {
        Debug.LogError("Close");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
