// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Bogus;
using Chucker.Lib.WpfGoldens;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui.Markup;
using Xunit;

namespace Wpf.Ui.Goldens.Controls;

public class ListViewTests
{
    private class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
    }

    private readonly Faker<Person> _personFaker;

    public ListViewTests()
    {
        _personFaker = new Faker<Person>()
            .RuleFor(p => p.FirstName, x => x.Person.FirstName)
            .RuleFor(p => p.LastName, x => x.Person.LastName)
            .RuleFor(p => p.Company, x => x.Company.CompanyName());
    }

    public static IEnumerable<TheoryDataRow<ApplicationTheme>> TestParameters =>
        from theme in new[] { ApplicationTheme.Light, ApplicationTheme.Dark }
        select new TheoryDataRow<ApplicationTheme>(theme);

    [WpfTheory]
    [MemberData(nameof(TestParameters))]
    public void TestListView(ApplicationTheme theme)
    {
        var control = new ListView();

        control.Resources.MergedDictionaries.Add(new ControlsDictionary());
        control.Resources.MergedDictionaries.Add(new ThemesDictionary { Theme = theme });

        control.ItemsSource = _personFaker.Generate(10);

        WpfGolden.FromCurrentTest()
            .Build()
            .WithSize(640, 480)
            .Render(control)
            .AssertIsWithinTolerance(0);
    }

    [WpfTheory]
    [MemberData(nameof(TestParameters))]
    public void TestListViewWithItemTemplate(ApplicationTheme theme)
    {
        var control = new ListView();

        control.Resources.MergedDictionaries.Add(new ControlsDictionary());
        control.Resources.MergedDictionaries.Add(new ThemesDictionary { Theme = theme });
        
        control.Resources.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri($"/Wpf.Ui.Goldens;component/Resources/{theme}ItemTemplates.xaml", UriKind.RelativeOrAbsolute)
        });

        control.ItemTemplate = (DataTemplate)control.FindResource("ListViewItemTemplate");

        control.ItemsSource = _personFaker.Generate(10);

        WpfGolden.FromCurrentTest()
            .Build()
            .WithSize(640, 480)
            .Render(control)
            .AssertIsWithinTolerance(0);
    }

    [WpfTheory]
    [MemberData(nameof(TestParameters))]
    public void TestListViewInFluentWindow(ApplicationTheme theme)
    {
        var control = new ListView { ItemsSource = _personFaker.Generate(10) };

        var window = new FluentWindow { Content = control };

        window.Resources.MergedDictionaries.Add(new ControlsDictionary());
        window.Resources.MergedDictionaries.Add(new ThemesDictionary { Theme = theme });

        window.Show();

        WpfGolden.FromCurrentTest()
            .Build()
            .WithSize(640, 480)
            .Render(window)
            .AssertIsWithinTolerance(0);
    }
}