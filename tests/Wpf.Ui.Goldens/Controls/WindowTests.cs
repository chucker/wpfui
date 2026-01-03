// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Chucker.Lib.WpfGoldens;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui.Markup;

using Xunit;

namespace Wpf.Ui.Goldens.Controls;

public class WindowTests 
{
    public static IEnumerable<TheoryDataRow<ApplicationTheme>> NonFluentTestParameters =>
        from theme in new[] { ApplicationTheme.Light, ApplicationTheme.Dark }
        select new TheoryDataRow<ApplicationTheme>(theme);

    [WpfTheory]
    [MemberData(nameof(NonFluentTestParameters))]
    public void TestNonFluent(ApplicationTheme theme)
    {
        var window = new Window();

        window.Resources.MergedDictionaries.Add(new ControlsDictionary());
        window.Resources.MergedDictionaries.Add(new ThemesDictionary { Theme = theme });

        window.Title = "Hello";
        
        window.Show();

        WpfGolden.FromCurrentTest()
            .Build()
//            .WithSize(500, 500)
            .Render(window)
            .AssertIsWithinTolerance(0);
    }

    public static IEnumerable<TheoryDataRow<ApplicationTheme, WindowBackdropType, WindowCornerPreference>> FluentWithTitleBarAndExtendsContentParameters =>
        from theme in new[] { ApplicationTheme.Light, ApplicationTheme.Dark }
        from backdropType in Enum.GetValues<WindowBackdropType>() 
        from cornerPreference in Enum.GetValues<WindowCornerPreference>()
        select new TheoryDataRow<ApplicationTheme, WindowBackdropType, WindowCornerPreference>(theme, backdropType, cornerPreference);

    [WpfTheory]
    [MemberData(nameof(FluentWithTitleBarAndExtendsContentParameters))]
    public void TestFluentWithTitleBarAndExtendsContent(ApplicationTheme theme,
        WindowBackdropType backdropType,
        WindowCornerPreference cornerPreference)
    {
        var window = new FluentWindow
        {
            WindowBackdropType = backdropType,
            WindowCornerPreference = cornerPreference,
            ExtendsContentIntoTitleBar = true
        };

        window.Resources.MergedDictionaries.Add(new ControlsDictionary());
        window.Resources.MergedDictionaries.Add(new ThemesDictionary { Theme = theme });

        window.Content = new TitleBar { Title = "Hello" };
        
        window.Show();

        WpfGolden.FromCurrentTest()
            .Build()
//            .WithSize(500, 500)
            .Render(window)
            .AssertIsWithinTolerance(0);
    }

    public static IEnumerable<TheoryDataRow<ApplicationTheme, WindowCornerPreference>> FluentTestParameters =>
        from theme in new[] { ApplicationTheme.Light, ApplicationTheme.Dark }
        from cornerPreference in Enum.GetValues<WindowCornerPreference>()
        select new TheoryDataRow<ApplicationTheme, WindowCornerPreference>(theme, cornerPreference);

    [WpfTheory]
    [MemberData(nameof(FluentTestParameters))]
    public void TestFluentWithTitleBar(ApplicationTheme theme,
        WindowCornerPreference cornerPreference)
    {
        var window = new FluentWindow
        {
            WindowCornerPreference = cornerPreference,
            ExtendsContentIntoTitleBar = false
        };

        window.Resources.MergedDictionaries.Add(new ControlsDictionary());
        window.Resources.MergedDictionaries.Add(new ThemesDictionary { Theme = theme });

        window.Content = new TitleBar { Title = "Hello" };
        
        window.Show();

        WpfGolden.FromCurrentTest()
            .Build()
//            .WithSize(500, 500)
            .Render(window)
            .AssertIsWithinTolerance(0);
    }
}