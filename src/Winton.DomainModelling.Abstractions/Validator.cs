// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Winton.DomainModelling
{
    /// <summary>
    ///     This class is a kind of builder for <see cref="Validation{T}"/>.
    ///     It exposes a fluent API that allows clients to define a set of
    ///     expectations for some data.
    ///     If all of the expectations are met then it will create an instance
    ///     of <see cref="Valid{T}"/>; otherwise it will return <see cref="Invalid{T}"/>
    ///     containing the error messages for each expectation that failed.
    /// </summary>
    /// <typeparam name="T">The type of data to be validated.</typeparam>
    public sealed class Validator<T>
    {
        private readonly T _data;
        private readonly List<Expectation> _expectations;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Validator{T}" /> class.
        /// </summary>
        /// <param name="data">The data to be validated.</param>
        public Validator(T data)
        {
            _data = data;
            _expectations = new List<Expectation>();
        }

        /// <summary>
        ///     Adds a validation condition that asserts some invariant of the data
        ///     being validated. If the predicate returns <c>false</c> then the data
        ///     is deemed invalid and the error message is generated for the specified key.
        /// </summary>
        /// <param name="predicate">The predicate contains the invariant.</param>
        /// <param name="key">
        ///     The key under which to place the error message if the data doesn't meet the predicates condition.
        /// </param>
        /// <param name="message">The message to use in the case that the data is invalid.</param>
        /// <returns>An instance of itself after the expectation has been added.</returns>
        public Validator<T> Expect(Predicate<T> predicate, string key, string message)
        {
            _expectations.Add(new Expectation(predicate, key, message));
            return this;
        }

        /// <summary>
        ///     Runs all of the expectations.
        ///     If any fail then it will create an <see cref="Invalid{T}"/> instance containing
        ///     the errors for each failed expectation; otherwise it will create a <see cref="Valid{T}"/>.
        /// </summary>
        /// <returns>The <see cref="Invalid{T}"/> if any expectations fail; otherwise <see cref="Valid{T}"/>.</returns>
        public Validation<T> Validate()
        {
            Dictionary<string, IEnumerable<string>> errors = _expectations
                .Where(invariant => !invariant.Predicate(_data))
                .GroupBy(invariant => invariant.Key)
                .ToDictionary(g => g.Key, g => g.Select(p => p.Message));
            return errors.Any() ? new Invalid<T>(errors) : new Valid<T>(_data) as Validation<T>;
        }

        private sealed class Expectation
        {
            public Expectation(Predicate<T> predicate, string key, string message)
            {
                Predicate = predicate;
                Key = key;
                Message = message;
            }

            public string Message { get; }

            public string Key { get; }

            public Predicate<T> Predicate { get; }
        }
    }
}