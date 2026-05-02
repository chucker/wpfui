// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Shell;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Wpf.Ui.Tray;

/*
 * TODO: Handle closing of the parent window.
 * NOTE
 * The problem is as follows:
 * If the main window is closed with the Debugger or simply destroyed,
 * it will not send WM_CLOSE or WM_DESTROY to its child windows. This
 * way, we can't tell tray to close the icon. Thus, we need to add to
 * the TrayHandler a mechanism that detects that the parent window has
 * been closed and then send
 * Shell32.Shell_NotifyIcon(Shell32.NIM.DELETE, Shell32.NOTIFYICONDATA);
 *
 * In another situation, the TrayHandler can also be forced to close,
 * so there is need to detect from the side somehow if this has happened
 * and remove the icon.
 */

/// <summary>
/// Responsible for managing the icons in the Tray bar.
/// </summary>
internal static class TrayManager
{
    public static bool Register(INotifyIcon notifyIcon)
    {
        if (notifyIcon is null)
        {
            return false;
        }

        return Register(notifyIcon, GetParentSource());
    }

    public static bool Register(INotifyIcon notifyIcon, Window parentWindow)
    {
        if (parentWindow == null)
        {
            return false;
        }

        return Register(notifyIcon, (HwndSource)PresentationSource.FromVisual(parentWindow));
    }

    public static bool Register(INotifyIcon notifyIcon, HwndSource? parentSource)
    {
        if (parentSource is null)
        {
            if (!notifyIcon.IsRegistered)
            {
                return false;
            }

            _ = Unregister(notifyIcon);

            return false;
        }

        if (parentSource.Handle == IntPtr.Zero)
        {
            return false;
        }

        if (notifyIcon.IsRegistered)
        {
            _ = Unregister(notifyIcon);
        }

        notifyIcon.Id = (uint)(TrayData.NotifyIcons.Count + 1);

        notifyIcon.HookWindow = new TrayHandler(
            $"wpfui_th_{parentSource.Handle}_{notifyIcon.Id}",
            parentSource.Handle)
        {
            ElementId = notifyIcon.Id.Value,
        };

        notifyIcon.ShellIconData = new NOTIFYICONDATAW
        {
            uID = notifyIcon.Id.Value,
            uFlags = NOTIFY_ICON_DATA_FLAGS.NIF_MESSAGE,
            uCallbackMessage = PInvoke.WM_TRAYMOUSEMESSAGE,
            hWnd = new HWND(notifyIcon.HookWindow.Handle),
            dwState = NOTIFY_ICON_STATE.NIS_SHAREDICON,
        };

        if (!string.IsNullOrEmpty(notifyIcon.TooltipText))
        {
            notifyIcon.ShellIconData = notifyIcon.ShellIconData with
            {
                szTip = notifyIcon.TooltipText,
                uFlags = notifyIcon.ShellIconData.uFlags | NOTIFY_ICON_DATA_FLAGS.NIF_TIP,
            };
        }

        ReloadHicon(notifyIcon);

        notifyIcon.HookWindow.AddHook(notifyIcon.WndProc);

        _ = PInvoke.Shell_NotifyIcon(NOTIFY_ICON_MESSAGE.NIM_ADD, notifyIcon.ShellIconData);

        TrayData.NotifyIcons.Add(notifyIcon);

        notifyIcon.IsRegistered = true;

        return true;
    }

    public static bool ModifyIcon(INotifyIcon notifyIcon)
    {
        if (!notifyIcon.IsRegistered)
        {
            return true;
        }

        ReloadHicon(notifyIcon);

        return PInvoke.Shell_NotifyIcon(NOTIFY_ICON_MESSAGE.NIM_MODIFY, notifyIcon.ShellIconData);
    }

    public static bool ModifyToolTip(INotifyIcon notifyIcon)
    {
        if (!notifyIcon.IsRegistered)
        {
            return true;
        }

        notifyIcon.ShellIconData = notifyIcon.ShellIconData with
        {
            szTip = notifyIcon.TooltipText,
            uFlags = notifyIcon.ShellIconData.uFlags | NOTIFY_ICON_DATA_FLAGS.NIF_TIP,
        };

        return PInvoke.Shell_NotifyIcon(NOTIFY_ICON_MESSAGE.NIM_MODIFY, notifyIcon.ShellIconData);
    }

    /// <summary>
    /// Tries to remove the <see cref="INotifyIcon"/> from the shell.
    /// </summary>
    public static bool Unregister(INotifyIcon notifyIcon)
    {
        if (!notifyIcon.Id.HasValue || !notifyIcon.IsRegistered)
        {
            return false;
        }

        _ = PInvoke.Shell_NotifyIcon(NOTIFY_ICON_MESSAGE.NIM_DELETE, notifyIcon.ShellIconData);

        notifyIcon.IsRegistered = false;

        return true;
    }

    /// <summary>
    /// Gets application source.
    /// </summary>
    private static HwndSource? GetParentSource()
    {
        Window mainWindow = Application.Current.MainWindow;

        if (mainWindow == null)
        {
            return null;
        }

        return (HwndSource)PresentationSource.FromVisual(mainWindow);
    }

    private static void ReloadHicon(INotifyIcon notifyIcon)
    {
        IntPtr hIcon = IntPtr.Zero;

        if (notifyIcon.Icon is not null)
        {
            hIcon = Hicon.FromSource(notifyIcon.Icon);
        }

        if (hIcon == IntPtr.Zero)
        {
            hIcon = Hicon.FromApp();
        }

        if (hIcon != IntPtr.Zero)
        {
            notifyIcon.ShellIconData = notifyIcon.ShellIconData with
            {
                hIcon = new HICON(hIcon),
                uFlags = notifyIcon.ShellIconData.uFlags | NOTIFY_ICON_DATA_FLAGS.NIF_ICON,
            };
        }
    }
}