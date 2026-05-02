// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Windows.Win32;

internal static partial class PInvoke
{
    /// <summary>
    /// This is the hard-coded message value used by WinForms for Shell_NotifyIcon.
    /// It's relatively safe to reuse.
    /// </summary>
    internal const uint WM_TRAYMOUSEMESSAGE = 0x800; // WM_USER + 1024
}
