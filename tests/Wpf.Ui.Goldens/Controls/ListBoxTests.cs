// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Bogus;
using Chucker.Lib.WpfGoldens;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui.Markup;
using Xunit;

namespace Wpf.Ui.Goldens.Controls;

public class ListBoxTests
{
    private class Person
    {
        public string FirstName { get; set; }
    }

    private readonly Faker<Person> _personFaker;

    public ListBoxTests()
    {
        _personFaker = new Faker<Person>()
            .RuleFor(p => p.FirstName, x => x.Person.FirstName);
    }
    
    public static IEnumerable<TheoryDataRow<ApplicationTheme>> TestParameters =>
        from theme in new[] { ApplicationTheme.Light, ApplicationTheme.Dark }
        select new TheoryDataRow<ApplicationTheme>(theme);
    
    [WpfTheory]
    [MemberData(nameof(TestParameters))]
    public void TestListView(ApplicationTheme theme)
    {
        var control = new ListBox();
        
        control.Resources.MergedDictionaries.Add(new ControlsDictionary());
        control.Resources.MergedDictionaries.Add(new ThemesDictionary { Theme = theme });

        control.ItemsSource = _personFaker.Generate(10);
        
        WpfGolden.FromCurrentTest()
            .Build()
            .WithSize(640, 480)
            .Render(control)
            .AssertIsWithinTolerance(0);
    }
}