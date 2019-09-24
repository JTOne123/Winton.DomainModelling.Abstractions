// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Winton.DomainModelling
{
    public class ValidationErrorTests
    {
        public sealed class Append : ValidationErrorTests
        {
            public static IEnumerable<object[]> TestCases => new List<object[]>
            {
                new object[]
                {
                    new ValidationError("Test", "Error"),
                    new ValidationError("Test", "Other error"),
                    new ValidationError("Test", new List<string> { "Error", "Other error" })
                },
                new object[]
                {
                    new ValidationError("Key 1", new List<string> { "Error", "Other error" }),
                    new ValidationError("Key 2", "Error"),
                    new ValidationError(
                        new Dictionary<string, IEnumerable<string>>
                        {
                            { "Key 1", new List<string> { "Error", "Other error" } },
                            { "Key 2", new List<string> { "Error" } }
                        })
                },
                new object[]
                {
                    new ValidationError(
                        new Dictionary<string, IEnumerable<string>>
                        {
                            { "Key 1", new List<string> { "Error", "Other error" } },
                            { "Key 2", new List<string> { "Error" } }
                        }),
                    new ValidationError(
                        new Dictionary<string, IEnumerable<string>>
                        {
                            { "Key 1", new List<string> { "Error", "Other error" } },
                            { "Key 2", new List<string> { "Error" } }
                        }),
                    new ValidationError(
                        new Dictionary<string, IEnumerable<string>>
                        {
                            { "Key 1", new List<string> { "Error", "Other error" } },
                            { "Key 2", new List<string> { "Error" } }
                        })
                },
                new object[]
                {
                    new ValidationError(
                        new Dictionary<string, IEnumerable<string>>
                        {
                            { "Key 1", new List<string> { "Error", "Other error" } },
                            { "Key 2", new List<string> { "Error" } }
                        }),
                    new ValidationError(
                        new Dictionary<string, IEnumerable<string>>
                        {
                            { "Key 2", new List<string> { "Other error" } },
                            { "Key 3", new List<string> { "Foo" } }
                        }),
                    new ValidationError(
                        new Dictionary<string, IEnumerable<string>>
                        {
                            { "Key 1", new List<string> { "Error", "Other error" } },
                            { "Key 2", new List<string> { "Error", "Other error" } },
                            { "Key 3", new List<string> { "Foo" } }
                        })
                }
            };

            [Fact]
            private void ShouldNotModifyTheOriginalError()
            {
                var error = new ValidationError("Test", "Error");

                error.Append(new ValidationError("Test", "Another error."));

                error.Should().BeEquivalentTo(new ValidationError("Test", "Error"));
            }

            [Theory]
            [MemberData(nameof(TestCases))]
            private void ShouldReturnAnErrorContainingTheCombinedMessages(
                ValidationError error,
                ValidationError other,
                ValidationError expected)
            {
                ValidationError combined = error.Append(other);

                combined.Should().BeEquivalentTo(expected);
            }
        }

        public sealed class ToDictionary : ValidationErrorTests
        {
            [Fact]
            private void ShouldNotModifyOriginalError()
            {
                var error = new ValidationError("Test", "Error");

                error.ToDictionary(v => v);

                error.Should().BeEquivalentTo(new ValidationError("Test", "Error"));
            }
        }
    }
}