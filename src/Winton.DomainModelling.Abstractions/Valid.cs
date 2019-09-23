// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Winton.DomainModelling
{
    /// <inheritdoc />
    /// <summary>
    ///     A valid outcome of a validation.
    /// </summary>
    public sealed class Valid<T> : Validation<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Valid{T}" /> class.
        /// </summary>
        /// <param name="data">The data encapsulated by the result.</param>
        /// <returns>A new instance of <see cref="Valid{T}" />.</returns>
        public Valid(T data)
        {
            Data = data;
        }

        /// <summary>
        ///     Gets the data.
        /// </summary>
        public T Data { get; }

        /// <inheritdoc />
        public override Validation<TCombined> Append<TOther, TCombined>(
            Validation<TOther> other,
            Func<T, TOther, TCombined> combine)
        {
            return other.Match<Validation<TCombined>>(
                otherData => new Valid<TCombined>(combine(Data, otherData)),
                otherError => new Invalid<TCombined>(otherError));
        }

        /// <inheritdoc />
        public override TOut Match<TOut>(Func<T, TOut> onValid, Func<ValidationError, TOut> onInvalid)
        {
            return onValid(Data);
        }

        /// <inheritdoc />
        public override Validation<TOut> Select<TOut>(Func<T, TOut> selector)
        {
            return new Valid<TOut>(selector(Data));
        }

        /// <inheritdoc />
        public override async Task<Validation<TOut>> Select<TOut>(Func<T, Task<TOut>> selector)
        {
            return new Valid<TOut>(await selector(Data));
        }

        /// <inheritdoc />
        public override Result<T> ToResult()
        {
            return new Success<T>(Data);
        }
    }
}