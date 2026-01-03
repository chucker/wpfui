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

public class BadgeTests
{
    public static IEnumerable<TheoryDataRow<ApplicationTheme, ControlAppearance, object>> TestParameters =>
        from theme in new[] { ApplicationTheme.Light, ApplicationTheme.Dark }
        from appearance in Enum.GetValues<ControlAppearance>()
        from content in new object[] { "1", 1234, "NEW" }
        select new TheoryDataRow<ApplicationTheme, ControlAppearance, object>(theme, appearance, content);

    [WpfTheory]
    [MemberData(nameof(TestParameters))]
    public void TestImageSnapshot(ApplicationTheme theme,
        ControlAppearance appearance,
        object content)
    {
        var control = new Badge()
        {
            Appearance = appearance,
            Content = content
        };

        control.Resources.MergedDictionaries.Add(new ControlsDictionary());
        control.Resources.MergedDictionaries.Add(new ThemesDictionary { Theme = theme });

        WpfGolden.FromCurrentTest()
            .Build()
            .WithSize(100, 100)
            .Render(control)
            .AssertIsWithinTolerance(0);
    }
}