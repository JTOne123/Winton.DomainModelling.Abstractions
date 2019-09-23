// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Winton.DomainModelling
{
    /// <inheritdoc />
    /// <summary>
    ///     An invalid outcome of a validation.
    /// </summary>
    public sealed class Invalid<T> : Validation<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Invalid{T}" /> class.
        /// </summary>
        /// <param name="error">The error that caused the validation to be invalid.</param>
        /// <returns>A new instance of <see cref="Invalid{T}" />.</returns>
        public Invalid(ValidationError error)
        {
            Error = error;
        }

        /// <summary>
        ///     Gets the validation error.
        /// </summary>
        public ValidationError Error { get; }

        /// <inheritdoc />
        public override Validation<TCombined> Append<TOther, TCombined>(
            Validation<TOther> other,
            Func<T, TOther, TCombined> combineData)
        {
            return other.Match<Validation<TCombined>>(
                otherData => new Invalid<TCombined>(Error),
                otherError => new Invalid<TCombined>(Error.Add(otherError)));
        }

        /// <inheritdoc />
        public override TOut Match<TOut>(Func<T, TOut> onValid, Func<ValidationError, TOut> onInvalid)
        {
            return onInvalid(Error);
        }

        /// <inheritdoc />
        public override Validation<TOut> Select<TOut>(Func<T, TOut> selector)
        {
            return new Invalid<TOut>(Error);
        }

        /// <inheritdoc />
        public override Task<Validation<TOut>> Select<TOut>(Func<T, Task<TOut>> selector)
        {
            return Task.FromResult<Validation<TOut>>(new Invalid<TOut>(Error));
        }

        /// <inheritdoc />
        public override Result<T> ToResult()
        {
            return new Failure<T>(Error);
        }
    }
}