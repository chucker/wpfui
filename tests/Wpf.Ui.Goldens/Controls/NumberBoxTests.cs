// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Chucker.Lib.WpfGoldens;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui.Markup;
using Xunit;

namespace Wpf.Ui.Goldens.Controls;

public class NumberBoxTests
{
    public static IEnumerable<TheoryDataRow<ApplicationTheme, NumberBoxSpinButtonPlacementMode, float>> TestParameters =>
        from theme in new[] { ApplicationTheme.Light, ApplicationTheme.Dark }
        from spinButtonPlacementMode in Enum.GetValues<NumberBoxSpinButtonPlacementMode>()
        from value in new[] { 0f, 1.23f, 1_000f }
        select new TheoryDataRow<ApplicationTheme, NumberBoxSpinButtonPlacementMode, float>(theme, spinButtonPlacementMode, value);

    [WpfTheory]
    [MemberData(nameof(TestParameters))]
    public void TestNumberBoxImageSnapshot(
        ApplicationTheme theme,
        NumberBoxSpinButtonPlacementMode spinButtonPlacementMode,
        double value)
    {
        var control = new NumberBox { SpinButtonPlacementMode = spinButtonPlacementMode, Value = value };

        control.Resources.MergedDictionaries.Add(new ControlsDictionary());
        control.Resources.MergedDictionaries.Add(new ThemesDictionary { Theme = theme });

        WpfGolden.FromCurrentTest()
            .Build()
            .WithSize(200, 100)
            .Render(control)
            .AssertIsWithinTolerance(0);
    }
}