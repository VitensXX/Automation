using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    private void OnEnable()
    {
        HookCtrl.DoOnKeyDownESC += OnESC;
        HookCtrl.DoOnKeyDownF9 += OnBeginRecordClick;
        HookCtrl.DoOnKeyDownF10 += OnEndRecordClick;
        HookCtrl.DoOnMouseLeftButtonDown += ListenMouseLeftButtonDown;
    }

    private void OnDisable()
    {
        HookCtrl.DoOnKeyDownESC -= OnESC;
        HookCtrl.DoOnKeyDownF9 -= OnBeginRecordClick;
        HookCtrl.DoOnKeyDownF10 -= OnEndRecordClick;
        HookCtrl.DoOnMouseLeftButtonDown -= ListenMouseLeftButtonDown;
    }

    void OnESC()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    bool _enableMouseListener = false;

    //开启监听并记录点击
    void OnBeginRecordClick()
    {
        _enableMouseListener = true;
        Debug.LogError("start record");
    }

    //结束点击的监听与记录
    void OnEndRecordClick()
    {
        _enableMouseListener = false;
        Debug.LogError("end record");
    }

    void ListenMouseLeftButtonDown()
    {
        if (!_enableMouseListener)
            return;

        Debug.LogError("Click ");
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    Debug.LogError("FFFFFFFFFFFFFF");
        //    //OnCloseSelf();
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
