using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DragWindow : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern bool MoveWindow(
      IntPtr hWnd,      // 窗口句柄。
      int X,          // 水平位置。
      int Y,          // 垂直位置。
      int nWidth,     // 窗口的宽度。
      int nHeight,    // 窗口的高度。
      bool bRepaint   // 重绘标识。（TRUE）
    );

    [DllImport("user32.dll")]
    static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    IntPtr hWnd;
    private const int WM_ACTIVATE = 0x0006;
    private readonly IntPtr WA_ACTIVE = new IntPtr(1);

    // Start is called before the first frame update
    void Start()
    {
        hWnd = GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    int x, y;
    public void OnClickTest()
    {
        x += 10;
        y += 10;
        MoveWindow(hWnd, x, y, 300, 300, true);
        SendMessage(hWnd, WM_ACTIVATE, WA_ACTIVE, IntPtr.Zero);
    }
}
