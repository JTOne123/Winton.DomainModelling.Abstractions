// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
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
                    new Invalid<string>(new ValidationError("Test", "The password must contain at least 1 special character."))
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
        }
    }
}