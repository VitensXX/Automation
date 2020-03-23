using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class HookCtrl : MonoBehaviour
{
    //建立钩子
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    private static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, uint threadId);

    //移除钩子
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    private static extern bool UnhookWindowsHookEx(int idHook);

    //把消息传递到下一个监听
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    private static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);


    public delegate void HookOnKeyDownESC();//按键响应ESC操作
    public static HookOnKeyDownESC DoOnKeyDownESC;

    public delegate void HookOnKeyDownF9();//按键F9响应
    public static HookOnKeyDownF9 DoOnKeyDownF9;

    public delegate void HookOnKeyDownF10();//按键F10响应
    public static HookOnKeyDownF10 DoOnKeyDownF10;

    public delegate void HookOnMouseLeftButtonDown();//鼠标左键响应
    public static HookOnMouseLeftButtonDown DoOnMouseLeftButtonDown;


    private const int WH_KEYBOARD_LL = 13;
    private const int WH_KEYBOARD = 2;
    private delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

    private const int WH_MOUSE_LL = 14;
    private const int WH_MOUSE = 7;


    private const int WM_KEYDOWN = 0x0100;
    private const int WM_SYSKEYDOWN = 0x0104;
    private const int WM_LBUTTONDOWN = 0x0201;
    //WM_MOUSEMOVE 0x0200
    //WM_LBUTTONUP 0x0202


    int idHookKeyBoard;
    int idHookMouse;


    public struct KeyMSG

    {

        public int vkCode;

        public int scanCode;

        public int flags;

        public int time;

        public int dwExtraInfo;

    }

    //安装钩子
    private void StartHook()
    {
        HookProc lpfn = new HookProc(KeyboardHook);

        idHookKeyBoard = SetWindowsHookEx(WH_KEYBOARD_LL, lpfn, GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName), 0);

        HookProc lpfn2 = new HookProc(MouseHook);
        idHookMouse = SetWindowsHookEx(WH_MOUSE_LL, lpfn2, GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName), 0);

        if (idHookKeyBoard > 0)
        {
            Debug.Log("键盘钩子[" + idHookKeyBoard + "]安装成功");
        }
        else
        {
            Debug.Log("键盘钩子安装失败");
            UnhookWindowsHookEx(idHookKeyBoard);
        }

        if(idHookMouse > 0)
        {
            Debug.Log("鼠标钩子[" + idHookMouse + "]安装成功");
        }
        else
        {
            Debug.Log("鼠标钩子安装失败");
            UnhookWindowsHookEx(idHookMouse);
        }
    }
    //卸载钩子
    public void StopHook()
    {
        if(idHookKeyBoard != 0)
        {
            Debug.Log("开始卸载键盘钩子 "+ idHookKeyBoard);
            bool resultKeyBoard = UnhookWindowsHookEx(idHookKeyBoard);
            Debug.Log("卸载键盘钩子结果 "+resultKeyBoard);
            if (resultKeyBoard)
            {
                idHookKeyBoard = 0;
            }
        }

        if(idHookMouse != 0)
        {
            Debug.Log("开始卸载鼠标钩子 " + idHookMouse);
            bool resultMouse = UnhookWindowsHookEx(idHookMouse);
            Debug.Log("卸载鼠标钩子结果 " + resultMouse);
            if (resultMouse)
            {
                idHookMouse = 0;
            }
        }
    }

    private int KeyboardHook(int nCode, IntPtr wParam, IntPtr lParam)
    {
        Debug.LogError("KeyboardHook " + wParam + " "+lParam);
        try
        {
            //键盘按下响应
            if (nCode >= 0 && wParam == (IntPtr)WH_KEYBOARD)
            {
                //KeyMSG m = (KeyMSG)Marshal.PtrToStructure(lParam, typeof(KeyMSG));

                int vkCode = Marshal.ReadInt32(lParam);
                //ESC按键的响应
                if ((KeyCode)vkCode == KeyCode.Escape)
                {
                    Debug.LogError("ESC!!!!!!!!!!!" + lParam);
                    DoOnKeyDownESC?.Invoke();
                }
                //F9
                else if ((KeyCode)vkCode == KeyCode.F9)
                {
                    Debug.LogError("F9!!!!!!!!!!!");
                    DoOnKeyDownF9?.Invoke();
                }
                //F10
                else if ((KeyCode)vkCode == KeyCode.F10)
                {
                    Debug.LogError("F10!!!!!!!!!!!");
                    DoOnKeyDownF10?.Invoke();
                }
            }

            return CallNextHookEx(idHookKeyBoard, nCode, wParam, lParam);
        }
        catch (Exception ex)
        {
            //Debug.LogError(ex.Message);
            return 0;
        }
    }

    private int MouseHook(int nCode, IntPtr wParam, IntPtr lParam)
    {
        //Debug.LogError("MouseHook " + wParam + " " + lParam);
        try
        {
            //鼠标左键点击
            if (nCode >= 0 && wParam == (IntPtr)WM_LBUTTONDOWN)
            {
                Debug.LogError("MouseDown "+lParam);
                DoOnMouseLeftButtonDown?.Invoke();
            }

            return CallNextHookEx(idHookKeyBoard, nCode, wParam, lParam);
        }
        catch (Exception ex)
        {
            //Debug.LogError(ex.Message);
            return 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartHook();
    }

    private void OnDestroy()
    {
        StopHook();
    }
}
