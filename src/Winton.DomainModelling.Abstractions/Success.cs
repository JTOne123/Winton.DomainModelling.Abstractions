// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Winton.DomainModelling
{
    /// <summary>
    ///     Contains static methods to make working with void results easier.
    /// </summary>
    [SuppressMessage(
        "StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleType",
        Justification = "Generic and non-generic version of the same class.")]
    public static class Success
    {
        /// <summary>
        ///     Creates a <see cref="Success{Unit}" /> to represent a successful result
        ///     that does not contain any data.
        /// </summary>
        /// <returns>
        ///     A new <see cref="Success{Unit}" />.
        /// </returns>
        public static Success<Unit> Unit()
        {
            return new Success<Unit>(DomainModelling.Unit.Value);
        }
    }

    /// <inheritdoc />
    /// <summary>
    ///     A result indicating a success.
    /// </summary>
    [SuppressMessage(
        "StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleType",
        Justification = "Generic and non-generic version of the same class.")]
    public sealed class Success<T> : Result<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Success{T}" /> class.
        /// </summary>
        /// <param name="data">The data encapsulated by the result.</param>
        /// <returns>A new instance of <see cref="Success{T}" />.</returns>
        public Success(T data)
        {
            Data = data;
        }

        /// <summary>
        ///     Gets the data.
        /// </summary>
        public T Data { get; }

        /// <inheritdoc />
        public override Result<T> Catch(Func<Error, Result<T>> onFailure)
        {
            return new Success<T>(Data);
        }

        /// <inheritdoc />
        public override Task<Result<T>> Catch(Func<Error, Task<Result<T>>> onFailure)
        {
            return Task.FromResult<Result<T>>(new Success<T>(Data));
        }

        /// <inheritdoc />
        public override Result<TCombined> Combine<TOther, TCombined>(
            Result<TOther> other,
            Func<T, TOther, TCombined> combineData,
            Func<Error, Error, Error> combineErrors)
        {
            return other.Match<Result<TCombined>>(
                otherData => new Success<TCombined>(combineData(Data, otherData)),
                otherError => new Failure<TCombined>(otherError));
        }

        /// <inheritdoc />
        public override TOut Match<TOut>(Func<T, TOut> onSuccess, Func<Error, TOut> onFailure)
        {
            return onSuccess(Data);
        }

        /// <inheritdoc />
        public override Result<T> OnFailure(Action onFailure)
        {
            return this;
        }

        /// <inheritdoc />
        public override Result<T> OnFailure(Action<Error> onFailure)
        {
            return this;
        }

        /// <inheritdoc />
        public override Task<Result<T>> OnFailure(Func<Task> onFailure)
        {
            return Task.FromResult<Result<T>>(this);
        }

        /// <inheritdoc />
        public override Task<Result<T>> OnFailure(Func<Error, Task> onFailure)
        {
            return Task.FromResult<Result<T>>(this);
        }

        /// <inheritdoc />
        public override Result<T> OnSuccess(Action onSuccess)
        {
            return OnSuccess(_ => onSuccess());
        }

        /// <inheritdoc />
        public override Result<T> OnSuccess(Action<T> onSuccess)
        {
            onSuccess(Data);
            return this;
        }

        /// <inheritdoc />
        public override async Task<Result<T>> OnSuccess(Func<Task> onSuccess)
        {
            return await OnSuccess(_ => onSuccess());
        }

        /// <inheritdoc />
        public override async Task<Result<T>> OnSuccess(Func<T, Task> onSuccess)
        {
            await onSuccess(Data);
            return this;
        }

        /// <inheritdoc />
        public override Result<TOut> Select<TOut>(Func<T, TOut> selector)
        {
            return new Success<TOut>(selector(Data));
        }

        /// <inheritdoc />
        public override async Task<Result<TOut>> Select<TOut>(Func<T, Task<TOut>> selector)
        {
            return new Success<TOut>(await selector(Data));
        }

        /// <inheritdoc />
        public override Result<T> SelectError(Func<Error, Error> selector)
        {
            return new Success<T>(Data);
        }

        /// <inheritdoc />
        public override Task<Result<T>> SelectError(Func<Error, Task<Error>> selector)
        {
            return Task.FromResult<Result<T>>(new Success<T>(Data));
        }

        /// <inheritdoc />
        public override Result<TOut> Then<TOut>(Func<T, Result<TOut>> onSuccess)
        {
            return onSuccess(Data);
        }

        /// <inheritdoc />
        public override Task<Result<TOut>> Then<TOut>(Func<T, Task<Result<TOut>>> onSuccess)
        {
            return onSuccess(Data);
        }
    }
}