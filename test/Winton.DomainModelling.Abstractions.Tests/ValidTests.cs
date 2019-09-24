// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Winton.DomainModelling
{
    public class ValidTests
    {
        public sealed class Append : ValidTests
        {
            [Fact]
            private void ShouldReturnInvalidIfOtherIsInvalid()
            {
                var validation = new Valid<string>("Valid");

                Validation<string> combined =
                    validation.Append(
                        new Invalid<string>(new ValidationError("Test", "Invalid")),
                        (_, __) => string.Empty);

                combined.Should().BeEquivalentTo(new Invalid<string>(new ValidationError("Test", "Invalid")));
            }

            [Fact]
            private void ShouldReturnValidIfOtherIsValid()
            {
                var validation = new Valid<string>("Valid");

                Validation<List<string>> combined = validation.Append(
                    new Valid<string>("Also valid"),
                    (s1, s2) => new List<string> { s1, s2 });

                combined.Should().BeEquivalentTo(
                    new Valid<IEnumerable<string>>(new List<string> { "Valid", "Also valid" }));
            }
        }

        public sealed class Match : ValidTests
        {
            [Fact]
            private void ShouldNotInvokeOnInvalidFunc()
            {
                var validation = new Valid<string>("Valid");
                var invoked = false;

                validation.Match(
                    _ => string.Empty,
                    _ =>
                    {
                        invoked = true;
                        return string.Empty;
                    });

                invoked.Should().BeFalse();
            }

            [Fact]
            private void ShouldReturnResultOfOnValidFunc()
            {
                var validation = new Valid<string>("Valid");

                string result = validation.Match(x => x, _ => string.Empty);

                result.Should().Be("Valid");
            }
        }

        public sealed class ToResult : ValidTests
        {
            [Fact]
            private void ShouldReturnSuccess()
            {
                var validation = new Valid<string>("Valid");

                Result<string> result = validation.ToResult();

                result.Should().BeEquivalentTo(new Success<string>("Valid"));
            }
        }
    }
}