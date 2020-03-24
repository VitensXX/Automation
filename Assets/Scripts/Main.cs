using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    private AutoClick _autoClick;

    private void OnEnable()
    {
        HookCtrl.DoOnKeyDown += DoOnKeydonw;
        HookCtrl.DoOnMouseLeftButtonDown += ListenMouseLeftButtonDown;
    }

    private void OnDisable()
    {
        HookCtrl.DoOnKeyDown -= DoOnKeydonw;
        HookCtrl.DoOnMouseLeftButtonDown -= ListenMouseLeftButtonDown;
    }

    void OnESC()
    {
        //if (_autoClick.EnableAutoClick)
        //{
        //    _autoClick.EnableAutoClick = false;
        //    Debug.LogError("暂停自动点击");
        //    return;
        //}

        Debug.LogError("程序退出");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    //bool _startAutoClick = false;
    void OnStartAutoClick()
    {
        _autoClick.EnableAutoClick = !_autoClick.EnableAutoClick;
        Debug.LogError("F8 start click state "+_autoClick.EnableAutoClick);
    }

    bool _enableMouseListener = false;

    //开启监听并记录点击
    void OnBeginRecordClick()
    {
        if (!_enableMouseListener)
        {
            _autoClick.ResetPositions();
            _enableMouseListener = true;
        }
        Debug.LogError("start record");
    }

    //结束点击的监听与记录
    void OnEndRecordClick()
    {
        _enableMouseListener = false;
        Debug.LogError("end record");
    }

    void ListenMouseLeftButtonDown(Vector2 position)
    {

        if (!_enableMouseListener)
            return;

        Debug.LogError("Click ");

        _autoClick.AddPosition(position);

    }

    private void Start()
    {
        _autoClick = GetComponent<AutoClick>();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    Debug.LogError("FFFFFFFFFFFFFF");
        //    //OnCloseSelf();
        //}
    }


    void DoOnKeydonw(int vkCode)
    {
        //ESC按键的响应
        if (vkCode == 27)//ESC
        {
            OnESC();
        }
        else if (vkCode == 119)//F8
        {
            OnStartAutoClick();
        }
        //F9
        else if (vkCode == 120)//F9
        {
            OnBeginRecordClick();
        }
        //F10
        else if (vkCode == 121)//F10
        {
            OnEndRecordClick();
        }
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

    public void OnTestClick()
    {
        Debug.LogError("test Click");
    }
}
