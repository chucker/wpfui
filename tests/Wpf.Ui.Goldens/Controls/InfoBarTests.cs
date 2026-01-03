// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Chucker.Lib.WpfGoldens;

using System;
using System.Collections.Generic;
using System.Linq;

using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui.Markup;

using Xunit;

namespace Wpf.Ui.Goldens.Controls;

public class InfoBarTests
{
    public static IEnumerable<TheoryDataRow<ApplicationTheme, InfoBarSeverity, bool>> TestParameters =>
        from theme in new[] { ApplicationTheme.Light, ApplicationTheme.Dark }
        from severity in Enum.GetValues<InfoBarSeverity>()
        from isClosable in new[] { true, false }
        select new TheoryDataRow<ApplicationTheme, InfoBarSeverity, bool>(theme, severity, isClosable);

    [WpfTheory]
    [MemberData(nameof(TestParameters))]
    public void TestImageSnapshot(ApplicationTheme theme,
        InfoBarSeverity severity,
        bool isClosable)
    {
        var control = new Wpf.Ui.Controls.InfoBar
        { 
            IsOpen = true,
            IsClosable = isClosable,
            Message = "Message", 
            Title = "Title",
            Severity = severity
        };

        control.Resources.MergedDictionaries.Add(new ControlsDictionary());
        control.Resources.MergedDictionaries.Add(new ThemesDictionary { Theme = theme });

        WpfGolden.FromCurrentTest()
            .Build()
            .WithSize(640, 100)
            .Render(control)
            .AssertIsWithinTolerance(0);
    }
}