// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Winton.DomainModelling
{
    public class ValidatorTests
    {
        public sealed class Validate : ValidatorTests
        {
            public static IEnumerable<object[]> TestCases => new List<object[]>
            {
                new object[]
                {
                    string.Empty,
                    new Invalid<string>(
                        new ValidationError(
                            "Test",
                            new List<string>
                            {
                                "The password must have a length of at least 3.",
                                "The password must contain at least 1 special character.",
                                "The password must contain at least 1 letter."
                            }))
                },
                new object[]
                {
                    "Test",
                    new Invalid<string>(
                        new ValidationError("Test", "The password must contain at least 1 special character."))
                },
                new object[]
                {
                    "Test!",
                    new Valid<string>("Test!")
                }
            };

            [Theory]
            [MemberData(nameof(TestCases))]
            private void ShouldReturnTheCorrectValidation(string data, Validation<string> expected)
            {
                Validation<string> validation = new Validator<string>(data)
                    .Expect(s => s.Length > 3, "Test", "The password must have a length of at least 3.")
                    .Expect(
                        s => Regex.IsMatch(s, "[!\"£$%^&*()_+?#]"),
                        "Test",
                        "The password must contain at least 1 special character.")
                    .Expect(s => Regex.IsMatch(s, "[A-Za-z]"), "Test", "The password must contain at least 1 letter.")
                    .Validate();

                validation.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
            }

            [Fact]
            private void TestingApi()
            {
                Foo.Create()
                   .Should()
                   .BeEquivalentTo(
                       new Invalid<Foo>(
                           new ValidationError(
                               new Dictionary<string, IEnumerable<string>>
                               {
                                   {
                                       "Bars[0].Age",
                                       new List<string> { "Age cannot be negative." }
                                   },
                                   {
                                       "Bars[1].Age",
                                       new List<string> { "Age cannot be negative." }
                                   }
                               })));
            }

            private struct Age
            {
                private readonly int value;

                internal Age(int value)
                {
                    this.value = value;
                }

                public static implicit operator int(Age age)
                {
                    return age.value;
                }

                internal static Validation<Age> Create(int age)
                {
                    return age < 0
                        ? new Invalid<Age>(new ValidationError("Age cannot be negative."))
                        : new Valid<Age>(new Age(age)) as Validation<Age>;
                }
            }

            private class Bar
            {
                internal Bar(string name, Age age)
                {
                    this.Name = name;
                    this.Age = age;
                }

                public Age Age { get; }

                public string Name { get; }

                internal static Validation<Bar> Create(Validation<Age> age, string name)
                {
                    return new Func<string, Age, Bar>((s, a) => new Bar(s, a))
                           .Apply(new Valid<string>(name))
                           .Apply(age.Nest(nameof(Age)));
                }
            }

            private class Foo
            {
                internal Foo(IEnumerable<Bar> bars)
                {
                    this.Bars = bars;
                }

                public IEnumerable<Bar> Bars { get; }

                internal static Validation<Foo> Create()
                {
                    var bars = Enumerable.Range(-10, 2).Select(i => Bar.Create(Age.Create(i), "Name"));

                    return new Func<IEnumerable<Bar>, Foo>(bs => new Foo(bs))
                        .Apply(bars.Nest(nameof(Bars)).ValidateAll());
                }
            }
        }
    }
}