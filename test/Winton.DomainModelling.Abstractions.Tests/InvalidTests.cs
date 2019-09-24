// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Winton.DomainModelling
{
    public class InvalidTests
    {
        public sealed class Append : ValidTests
        {
            [Fact]
            private void ShouldReturnInvalidIfOtherIsValid()
            {
                var validation = new Invalid<string>(new ValidationError("Test", "Error"));

                Validation<string> combined = validation.Append(
                    new Valid<string>("Valid"),
                    (_, __) => string.Empty);

                combined.Should().BeEquivalentTo(new Invalid<string>(new ValidationError("Test", "Error")));
            }

            [Fact]
            private void ShouldReturnInvalidWithCombinedErrorsIfOtherIsInvalid()
            {
                var validation = new Invalid<string>(new ValidationError("Test", "Error"));

                Validation<string> combined =
                    validation.Append(
                        new Invalid<string>(new ValidationError("Test", "Another error")),
                        (_, __) => string.Empty);

                combined.Should().BeEquivalentTo(
                    new Invalid<string>(new ValidationError("Test", new List<string> { "Error", "Another error" })));
            }
        }

        public sealed class Match : ValidTests
        {
            [Fact]
            private void ShouldNotInvokeOnValidFunc()
            {
                var validation = new Invalid<string>(new ValidationError("Test", "Error"));
                var invoked = false;

                validation.Match(
                    _ =>
                    {
                        invoked = true;
                        return string.Empty;
                    },
                    _ => string.Empty);

                invoked.Should().BeFalse();
            }

            [Fact]
            private void ShouldReturnResultOfOnInvalidFunc()
            {
                var validation = new Invalid<string>(new ValidationError("Test", "Error"));

                string result = validation.Match(_ => string.Empty, _ => "Error");

                result.Should().Be("Error");
            }
        }

        public sealed class ToResult : ValidTests
        {
            [Fact]
            private void ShouldReturnFailure()
            {
                var validation = new Invalid<string>(new ValidationError("Test", "Error"));

                Result<string> result = validation.ToResult();

                result.Should().BeEquivalentTo(new Failure<string>(new ValidationError("Test", "Error")));
            }
        }
    }
}