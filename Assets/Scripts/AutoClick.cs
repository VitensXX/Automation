using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class AutoClick : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern int SetCursorPos(int x, int y);
    [DllImport("user32.dll")]
    static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);

    //这个枚举同样来自user32.dll
    [Flags]
    enum MouseEventFlag : uint
    {
        Move = 0x0001,
        LeftDown = 0x0002,
        LeftUp = 0x0004,
        RightDown = 0x0008,
        RightUp = 0x0010,
        MiddleDown = 0x0020,
        MiddleUp = 0x0040,
        XDown = 0x0080,
        XUp = 0x0100,
        Wheel = 0x0800,
        VirtualDesk = 0x4000,
        Absolute = 0x8000
    }

    public bool EnableAutoClick { get; set; }//是否启动自动点击


    List<Vector2> _clickPositions = new List<Vector2>();
    public void ResetPositions()
    {
        _clickPositions.Clear();
    }

    public void AddPosition(Vector2 pos)
    {
        _clickPositions.Add(pos);
    }

    // Start is called before the first frame update
    void Start()
    {
        EnableAutoClick = false;
    }

    float _interval = 3f;
    float _tick = 0;
    bool _clickFirst = true;

    bool hasMouseDown;
    int _clickIndex = 0;
    // Update is called once per frame
    void Update()
    {
        if (!EnableAutoClick || _clickPositions.Count == 0)
        {
            return;
        }

        _tick += Time.deltaTime;
        if (_tick > _interval)
        {
            _tick = 0;

            Vector2 pos = _clickPositions[_clickIndex++ % _clickPositions.Count];
            SetCursorPos(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

            ////点击第一个位子
            //if (_clickFirst)
            //{
            //    _clickFirst = false;
            //    SetCursorPos(10, 20);
            //    Debug.LogError("第一个");
            //}
            //else
            //{
            //    _clickFirst = true;
            //    SetCursorPos(300, 20);
            //    //mouse_event(MouseEventFlag.LeftDown, 0, 0, 0, UIntPtr.Zero);
            //    //mouse_event(MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);
            //    Debug.LogError("第二个");
            //}
            mouse_event(MouseEventFlag.LeftDown, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);
        }
        //}
    }
}
