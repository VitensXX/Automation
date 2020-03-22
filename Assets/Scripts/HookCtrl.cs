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


    private const int WH_KEYBOARD_LL = 13;
    private delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
    private const int WM_KEYDOWN = 0x0100;
    int idHook;

    //安装钩子
    private void StartHook()
    {
        HookProc lpfn = new HookProc(Hook);

        idHook = SetWindowsHookEx(WH_KEYBOARD_LL, lpfn, GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName), 0);

        if (idHook > 0)
        {
            Debug.LogError("钩子[" + idHook + "]安装成功");
        }
        else
        {
            Debug.LogError("钩子安装失败");
            UnhookWindowsHookEx(idHook);
        }
    }
    //卸载钩子
    public void StopHook()
    {
        Debug.LogError("卸载钩子 "+ idHook);
        bool result = UnhookWindowsHookEx(idHook);
        Debug.LogError("卸载结果 "+result);
        if (result)
        {
            idHook = 0;
        }
    }

    private int Hook(int nCode, IntPtr wParam, IntPtr lParam)
    {
        try
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if ((KeyCode)vkCode == KeyCode.Escape)
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                }
            }
            return CallNextHookEx(idHook, nCode, wParam, lParam);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
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
        if(idHook != 0)
        {
            StopHook();
        }
    }
}
