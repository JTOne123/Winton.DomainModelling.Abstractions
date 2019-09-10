// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Winton.DomainModelling
{
    /// <inheritdoc />
    /// <summary>
    ///     A result indicating a failure.
    /// </summary>
    public sealed class Failure<T> : Result<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Failure{T}" /> class.
        /// </summary>
        /// <param name="error">The error that caused the result to be a failure.</param>
        /// <returns>A new instance of <see cref="Failure{T}" />.</returns>
        public Failure(Error error)
        {
            Error = error;
        }

        /// <summary>
        ///     Gets the error that caused the failure.
        /// </summary>
        public Error Error { get; }

        /// <inheritdoc />
        public override Result<T> Catch(Func<Error, Result<T>> onFailure)
        {
            return onFailure(Error);
        }

        /// <inheritdoc />
        public override Task<Result<T>> Catch(Func<Error, Task<Result<T>>> onFailure)
        {
            return onFailure(Error);
        }

        /// <inheritdoc />
        public override Result<TCombined> Combine<TOther, TCombined>(
            Result<TOther> other,
            Func<T, TOther, TCombined> combineData,
            Func<Error, Error, Error> combineErrors)
        {
            return other.Match<Result<TCombined>>(
                otherData => new Failure<TCombined>(Error),
                otherError => new Failure<TCombined>(combineErrors(Error, otherError)));
        }

        /// <inheritdoc />
        public override TOut Match<TOut>(Func<T, TOut> onSuccess, Func<Error, TOut> onFailure)
        {
            return onFailure(Error);
        }

        /// <inheritdoc />
        public override Result<T> OnFailure(Action onFailure)
        {
            return OnFailure(_ => onFailure());
        }

        /// <inheritdoc />
        public override Result<T> OnFailure(Action<Error> onFailure)
        {
            onFailure(Error);
            return this;
        }

        /// <inheritdoc />
        public override Task<Result<T>> OnFailure(Func<Task> onFailure)
        {
            return OnFailure(_ => onFailure());
        }

        /// <inheritdoc />
        public override async Task<Result<T>> OnFailure(Func<Error, Task> onFailure)
        {
            await onFailure(Error);
            return this;
        }

        /// <inheritdoc />
        public override Result<T> OnSuccess(Action onSuccess)
        {
            return this;
        }

        /// <inheritdoc />
        public override Result<T> OnSuccess(Action<T> onSuccess)
        {
            return this;
        }

        /// <inheritdoc />
        public override Task<Result<T>> OnSuccess(Func<Task> onSuccess)
        {
            return Task.FromResult<Result<T>>(this);
        }

        /// <inheritdoc />
        public override Task<Result<T>> OnSuccess(Func<T, Task> onSuccess)
        {
            return Task.FromResult<Result<T>>(this);
        }

        /// <inheritdoc />
        public override Result<TOut> Select<TOut>(Func<T, TOut> selector)
        {
            return new Failure<TOut>(Error);
        }

        /// <inheritdoc />
        public override Task<Result<TOut>> Select<TOut>(Func<T, Task<TOut>> selector)
        {
            return Task.FromResult<Result<TOut>>(new Failure<TOut>(Error));
        }

        /// <inheritdoc />
        public override Result<T> SelectError(Func<Error, Error> selector)
        {
            return new Failure<T>(selector(Error));
        }

        /// <inheritdoc />
        public override async Task<Result<T>> SelectError(Func<Error, Task<Error>> selector)
        {
            return new Failure<T>(await selector(Error));
        }

        /// <inheritdoc />
        public override Result<TOut> Then<TOut>(Func<T, Result<TOut>> onSuccess)
        {
            return new Failure<TOut>(Error);
        }

        /// <inheritdoc />
        public override Task<Result<TOut>> Then<TOut>(Func<T, Task<Result<TOut>>> onSuccess)
        {
            return Task.FromResult<Result<TOut>>(new Failure<TOut>(Error));
        }
    }
}