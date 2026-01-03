// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Chucker.Lib.WpfGoldens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui.Markup;
using Xunit;

namespace Wpf.Ui.Goldens.Controls;

public class ButtonTests
{
    public static IEnumerable<TheoryDataRow<ApplicationTheme, ControlAppearance, string>> TestParameters =>
        from theme in new[] { ApplicationTheme.Light, ApplicationTheme.Dark }
        from appearance in Enum.GetValues<ControlAppearance>()
        from content in new[] { "OK", "Cancel" }
        select new TheoryDataRow<ApplicationTheme, ControlAppearance, string>(theme, appearance, content);

    [WpfTheory]
    [MemberData(nameof(TestParameters))]
    public void TestImageSnapshot(ApplicationTheme theme,
        ControlAppearance appearance,
        string content)
    {
        var control = new Wpf.Ui.Controls.Button { Appearance = appearance, Content = content };

        control.Resources.MergedDictionaries.Add(new ControlsDictionary());
        control.Resources.MergedDictionaries.Add(new ThemesDictionary { Theme = theme });

        WpfGolden.FromCurrentTest()
            .Build()
            .WithSize(640, 100)
            .Render(control)
            .AssertIsWithinTolerance(0);
    }

    [WpfTheory]
    [MemberData(nameof(TestParameters))]
    public void TestWithIcon(ApplicationTheme theme,
        ControlAppearance appearance,
        string content)
    {
        var control = new Button
        {
            Appearance = appearance, Content = content, Icon = new SymbolIcon(SymbolRegular.Rocket16)
        };

        control.Resources.MergedDictionaries.Add(new ControlsDictionary());
        control.Resources.MergedDictionaries.Add(new ThemesDictionary { Theme = theme });

        WpfGolden.FromCurrentTest()
            .Build()
            .WithSize(640, 100)
            .Render(control)
            .AssertIsWithinTolerance(0);
    }

    public static IEnumerable<TheoryDataRow<ApplicationTheme, string, string>> AccentColorTestParameters =>
        from theme in new[] { ApplicationTheme.Light, ApplicationTheme.Dark }
        from color in new[]
        {
            nameof(Colors.MediumPurple), nameof(Colors.Teal), nameof(Colors.LimeGreen), nameof(Colors.Orange)
        }
        from content in new[] { "OK", "Close" }
        select new TheoryDataRow<ApplicationTheme, string, string>(theme, color, content);

    [WpfTheory]
    [MemberData(nameof(AccentColorTestParameters))]
    public void TestWithAccentColor(ApplicationTheme theme, string colorName, string content)
    {
        var accentColor = (Color)typeof(Colors)
            .GetProperty(colorName, BindingFlags.Static | BindingFlags.Public)!
            .GetValue(null)!;

        // var accentColor = Colors.MediumPurple;
        //
        // UiApplication.Current.Resources.MergedDictionaries.Add(new ControlsDictionary());
        // UiApplication.Current.Resources.MergedDictionaries.Add(new ThemesDictionary { Theme = theme });
        //
        // ApplicationAccentColorManager.Apply(accentColor);

        var control = new Button
        {
            Appearance = ControlAppearance.Primary,
            //IsDefault = true,
            Content = content,
            Icon = new SymbolIcon(SymbolRegular.Rocket16)
        };

        control.Resources.MergedDictionaries.Add(new ControlsDictionary());
        control.Resources.MergedDictionaries.Add(new ThemesDictionary { Theme = theme });
        
        ApplicationAccentColorManager.Apply(accentColor, applicationTheme: theme);
        ApplicationThemeManager.Apply(control);

        // var window = new FluentWindow { Content = control, };
        //
        // control.Width = 100;
        // control.Height = 30;
        //
        // window.Width = 600;
        // window.Height = 400;
        //
        // UiApplication.Current.MainWindow = window;
        //
        // window.ShowDialog();

        // var control = new System.Windows.Controls.Button()
        // {
        //     IsDefault = true,
        //     Content = content,
        // };
        
        WpfGolden.FromCurrentTest()
            .Build()
            .WithSize(120, 40)
            .Render(control)
            .AssertIsWithinTolerance(0);
    }
}