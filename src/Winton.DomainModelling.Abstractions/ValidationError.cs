// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Winton.DomainModelling
{
    /// <inheritdoc cref="Error" />
    /// <summary>
    ///     A collection of validation errors.
    ///     The error is a dictionary where each key can have many values
    ///     and each value describes a validation issue with that key.
    /// </summary>
    /// <remarks>
    ///     It is anticipated that keys will represent a single property on a particular domain entity
    ///     and that a single entity will be validated per request,
    ///     such that a key could be set using <c>nameof(Property)</c>.
    ///     However, this class places no restrictions on how the keys can be used, so each application
    ///     has the freedom to chose its own convention for transmitting validation errors.
    ///     For instance if a particular request involves validation of multiple entities then the
    ///     keys could be namespaced using the entity names.
    /// </remarks>
    public class ValidationError : Error, IReadOnlyDictionary<string, IEnumerable<string>>
    {
        private readonly IReadOnlyDictionary<string, IEnumerable<string>> _errors;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValidationError" /> class from a
        ///     <see cref="IReadOnlyDictionary{TKey,TValue}" />.
        /// </summary>
        /// <param name="errors">The validation errors for each field that is invalid.</param>
        /// <returns>A new instance of <see cref="ValidationError" />.</returns>
        public ValidationError(IReadOnlyDictionary<string, IEnumerable<string>> errors)
            : base("One or more validation errors occurred.", string.Empty)
        {
            _errors = errors;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValidationError" /> class from a
        ///     single error.
        /// </summary>
        /// <param name="key">The key under which to store the error message.</param>
        /// <param name="value">The error message for the specified key.</param>
        /// <returns>A new instance of <see cref="ValidationError" />.</returns>
        public ValidationError(string key, string value)
            : this(new Dictionary<string, IEnumerable<string>> { { key, new List<string> { value } } })
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValidationError" /> class from a
        ///     collection of errors for a single key.
        /// </summary>
        /// <param name="key">The key under which to store the error message.</param>
        /// <param name="values">The error messages for the specified key.</param>
        /// <returns>A new instance of <see cref="ValidationError" />.</returns>
        public ValidationError(string key, IEnumerable<string> values)
            : this(new Dictionary<string, IEnumerable<string>> { { key, values } })
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValidationError" /> class from a
        ///     single error message.
        /// </summary>
        /// <param name="value">The error message.</param>
        /// <returns>A new instance of <see cref="ValidationError" />.</returns>
        public ValidationError(string value)
            : this(string.Empty, value)
        {
        }

        /// <inheritdoc />
        public int Count => _errors.Count;

        /// <inheritdoc />
        public IEnumerable<string> Keys => _errors.Keys;

        /// <inheritdoc />
        public IEnumerable<IEnumerable<string>> Values => _errors.Values;

        /// <inheritdoc />
        public IEnumerable<string> this[string key] => _errors[key];

        /// <summary>
        ///     Implicitly casts a <see cref="Dictionary{TKey,TValue}" /> to a <see cref="ValidationError" />.
        /// </summary>
        /// <param name="errors">The dictionary of field level validation errors.</param>
        /// <returns>A new instance of <see cref="ValidationError" />.</returns>
        public static implicit operator ValidationError(Dictionary<string, IEnumerable<string>> errors)
        {
            return new ValidationError(errors);
        }

        /// <summary>
        ///     Adds another <see cref="ValidationError" /> to this one by merging the field level errors.
        /// </summary>
        /// <param name="other">The other validation error to append.</param>
        /// <returns>This error with the others appended.</returns>
        public ValidationError Append(ValidationError other)
        {
            return _errors
                .Concat(other._errors)
                .GroupBy(error => error.Key, error => error.Value)
                .ToDictionary(group => group.Key, group => group.SelectMany(x => x).Distinct());
        }

        /// <inheritdoc />
        public bool ContainsKey(string key)
        {
            return _errors.ContainsKey(key);
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<string, IEnumerable<string>>> GetEnumerator()
        {
            return _errors.GetEnumerator();
        }

#pragma warning disable 1591
#pragma warning disable SA1600 // Elements should be documented
        public ValidationError Nest(string key)
        {
            string Key(KeyValuePair<string, IEnumerable<string>> pair)
            {
                return string.IsNullOrWhiteSpace(pair.Key) ? key : $"{key}.{pair.Key}";
            }

            return new ValidationError(
                this
                    .Select(pair => new KeyValuePair<string, IEnumerable<string>>(Key(pair), pair.Value))
                    .ToDictionary(pair => pair.Key, pair => pair.Value));
        }

        public ValidationError Nest(string key, int index)
        {
            return Nest($"{key}[{index}]");
        }
#pragma warning restore SA1600 // Elements should be documented
#pragma warning restore 1591

        /// <summary>
        ///     Creates a <see cref="Dictionary{TKey,TValue}" /> from the errors contained in this class,
        ///     using the specified <paramref name="elementSelector" /> to create the values.
        /// </summary>
        /// <param name="elementSelector">The selector used to create the values.</param>
        /// <typeparam name="TValue">The type of value to be created.</typeparam>
        /// <returns>The new <see cref="Dictionary{TKey,TValue}" />.</returns>
        public Dictionary<string, TValue> ToDictionary<TValue>(Func<IEnumerable<string>, TValue> elementSelector)
        {
            return _errors.ToDictionary(e => e.Key, e => elementSelector(e.Value));
        }

        /// <inheritdoc />
        public bool TryGetValue(string key, out IEnumerable<string> value)
        {
            return _errors.TryGetValue(key, out value);
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_errors).GetEnumerator();
        }
    }
}