// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Winton.DomainModelling
{
    /// <inheritdoc cref="Error" />
    /// <summary>
    ///     An error indicating that there some data is invalid.
    /// </summary>
    public class ValidationError : Error, IReadOnlyDictionary<string, IEnumerable<string>>
    {
        private readonly IReadOnlyDictionary<string, IEnumerable<string>> _errors;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValidationError" /> class.
        /// </summary>
        /// <param name="errors">The validation errors for each field that is invalid.</param>
        /// <returns>A new instance of <see cref="ValidationError" />.</returns>
        public ValidationError(IReadOnlyDictionary<string, IEnumerable<string>> errors)
            : base("One or more validation errors occurred.", string.Empty)
        {
            _errors = errors;
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
        ///     Implicitly casts a <see cref="Dictionary{TKey,TValue}"/> to a <see cref="ValidationError"/>.
        /// </summary>
        /// <param name="errors">The dictionary of field level validation errors.</param>
        /// <returns>A new instance of <see cref="ValidationError"/>.</returns>
        public static implicit operator ValidationError(Dictionary<string, IEnumerable<string>> errors)
        {
            return new ValidationError(errors);
        }

        /// <summary>
        ///     Appends another <see cref="ValidationError"/> to this one by merging the field level errors.
        /// </summary>
        /// <param name="other">The other validation error to append.</param>
        /// <returns>This error with the others appended.</returns>
        public ValidationError Append(ValidationError other)
        {
            return _errors
                .Concat(other._errors)
                .GroupBy(error => error.Key, error => error.Value)
                .ToDictionary(group => group.Key, group => group.SelectMany(x => x));
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