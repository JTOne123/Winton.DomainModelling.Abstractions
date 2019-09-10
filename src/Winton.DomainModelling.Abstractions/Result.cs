// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Winton.DomainModelling
{
    /// <summary>
    ///     Represents the result of an operation which may succeed or fail.
    /// </summary>
    /// <remarks>
    ///     Use <c>Result&lt;Unit&gt;</c> for operations that would otherwise be <c>void</c>.
    /// </remarks>
    /// <typeparam name="T">
    ///     The type of data encapsulated by the result.
    /// </typeparam>
    public abstract class Result<T>
    {
        /// <summary>
        ///     Invokes another result generating function which takes as input the error of this result
        ///     if it is a failure.
        /// </summary>
        /// <remarks>
        ///     If this result is a success then this is a no-op and the original success is retained.
        ///     This is useful for handling errors.
        /// </remarks>
        /// <param name="onFailure">
        ///     The function that is invoked if this result is a failure.
        /// </param>
        /// <returns>
        ///     If this result is a failure, then the result of the <paramref>onFailure</paramref> function;
        ///     otherwise the original error.
        /// </returns>
        public abstract Result<T> Catch(Func<Error, Result<T>> onFailure);

        /// <summary>
        ///     Invokes another result generating function which takes as input the error of this result
        ///     if it is a failure.
        /// </summary>
        /// <remarks>
        ///     If this result is a success then this is a no-op and the original success is retained.
        ///     This is useful for handling errors.
        /// </remarks>
        /// <param name="onFailure">
        ///     The function that is invoked if this result is a failure.
        /// </param>
        /// <returns>
        ///     If this result is a failure, then the result of the <paramref>onFailure</paramref> function;
        ///     otherwise the original error.
        /// </returns>
        public abstract Task<Result<T>> Catch(Func<Error, Task<Result<T>>> onFailure);

        /// <summary>
        ///     Combines this result with another.
        ///     If both are successful then <paramref>combineData</paramref> is invoked;
        ///     else if either is a failure then <paramref>combineErrors</paramref> is invoked.
        /// </summary>
        /// <typeparam name="TOther">
        ///     The type of data in the other result.
        /// </typeparam>
        /// <typeparam name="TCombined">
        ///     The type of data in the combined result.
        /// </typeparam>
        /// <param name="other">
        ///     The other result to be combined with this one.
        /// </param>
        /// <param name="combineData">
        ///     The function that is invoked to combine the data when both of the results are successful.
        /// </param>
        /// <param name="combineErrors">
        ///     The function that is invoked to combine the errors when either of the results is a failure.
        /// </param>
        /// <returns>
        ///     A new <see cref="Result{T}" />.
        /// </returns>
        public abstract Result<TCombined> Combine<TOther, TCombined>(
            Result<TOther> other,
            Func<T, TOther, TCombined> combineData,
            Func<Error, Error, Error> combineErrors);

        /// <summary>
        ///     Used to match on whether this result is a success or a failure.
        /// </summary>
        /// <typeparam name="TOut">
        ///     The type that is returned.
        /// </typeparam>
        /// <param name="onSuccess">
        ///     The function that is invoked if this result represents a success.
        /// </param>
        /// <param name="onFailure">
        ///     The function that is invoked if this result represents a failure.
        /// </param>
        /// <returns>
        ///     A value that is mapped from either the data or the error.
        /// </returns>
        public abstract TOut Match<TOut>(Func<T, TOut> onSuccess, Func<Error, TOut> onFailure);

        /// <summary>
        ///     Invokes the specified action if the result is a failure and returns the original result.
        /// </summary>
        /// <remarks>
        ///     If this result is a success then this is a no-op and the original success is retained.
        ///     This is useful for publishing domain model notifications when an operation fails.
        /// </remarks>
        /// <param name="onFailure">
        ///     The action that will be invoked if this result is a failure.
        /// </param>
        /// <returns>
        ///     The original result.
        /// </returns>
        public abstract Result<T> OnFailure(Action onFailure);

        /// <summary>
        ///     Invokes the specified action if the result is a failure and returns the original result.
        /// </summary>
        /// <remarks>
        ///     If this result is a success then this is a no-op and the original success is retained.
        ///     This is useful for publishing domain model notifications when an operation fails.
        /// </remarks>
        /// <param name="onFailure">
        ///     The action that will be invoked if this result is a failure.
        /// </param>
        /// <returns>
        ///     The original result.
        /// </returns>
        public abstract Result<T> OnFailure(Action<Error> onFailure);

        /// <summary>
        ///     Invokes the specified action if the result is a failure and returns the original result.
        /// </summary>
        /// <remarks>
        ///     If this result is a success then this is a no-op and the original success is retained.
        ///     This is useful for publishing domain model notifications when an operation fails.
        /// </remarks>
        /// <param name="onFailure">
        ///     The asynchronous action that will be invoked if this result is a failure.
        /// </param>
        /// <returns>
        ///     The original result.
        /// </returns>
        public abstract Task<Result<T>> OnFailure(Func<Task> onFailure);

        /// <summary>
        ///     Invokes the specified action if the result is a failure and returns the original result.
        /// </summary>
        /// <remarks>
        ///     If this result is a success then this is a no-op and the original success is retained.
        ///     This is useful for publishing domain model notifications when an operation fails.
        /// </remarks>
        /// <param name="onFailure">
        ///     The asynchronous action that will be invoked if this result is a failure.
        /// </param>
        /// <returns>
        ///     The original result.
        /// </returns>
        public abstract Task<Result<T>> OnFailure(Func<Error, Task> onFailure);

        /// <summary>
        ///     Invokes the specified action if the result is a success and returns the original result.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op and the original failure is retained.
        ///     This is useful for publishing domain model notifications when an operation succeeds.
        /// </remarks>
        /// <param name="onSuccess">
        ///     The action that will be invoked if this result represents a success.
        /// </param>
        /// <returns>
        ///     The original result.
        /// </returns>
        public abstract Result<T> OnSuccess(Action onSuccess);

        /// <summary>
        ///     Invokes the specified action if the result is a success and returns the original result.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op and the original failure is retained.
        ///     This is useful for publishing domain model notifications when an operation succeeds.
        /// </remarks>
        /// <param name="onSuccess">
        ///     The action that will be invoked if this result represents a success.
        /// </param>
        /// <returns>
        ///     The original result.
        /// </returns>
        public abstract Result<T> OnSuccess(Action<T> onSuccess);

        /// <summary>
        ///     Invokes the specified action if the result is a success and returns the original result.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op and the original failure is retained.
        ///     This is useful for publishing domain model notifications when an operation succeeds.
        /// </remarks>
        /// <param name="onSuccess">
        ///     The asynchronous action that will be invoked if this result represents a success.
        /// </param>
        /// <returns>
        ///     The original result.
        /// </returns>
        public abstract Task<Result<T>> OnSuccess(Func<Task> onSuccess);

        /// <summary>
        ///     Invokes the specified action if the result is a success and returns the original result.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op and the original failure is retained.
        ///     This is useful for publishing domain model notifications when an operation succeeds.
        /// </remarks>
        /// <param name="onSuccess">
        ///     The asynchronous action that will be invoked if this result represents a success.
        /// </param>
        /// <returns>
        ///     The original result.
        /// </returns>
        public abstract Task<Result<T>> OnSuccess(Func<T, Task> onSuccess);

        /// <summary>
        ///     Projects a successful result's data from one type to another.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op.
        /// </remarks>
        /// <typeparam name="TOut">
        ///     The type of data in the new result.
        /// </typeparam>
        /// <param name="selector">
        ///     The function that is invoked to select the data.
        /// </param>
        /// <returns>
        ///     A new result containing either; the output of the <paramref>selector</paramref> function
        ///     if this result is a success, otherwise the original failure.
        /// </returns>
        public abstract Result<TOut> Select<TOut>(Func<T, TOut> selector);

        /// <summary>
        ///     Projects a successful result's data from one type to another.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op.
        /// </remarks>
        /// <typeparam name="TOut">
        ///     The type of data in the new result.
        /// </typeparam>
        /// <param name="selector">
        ///     The asynchronous function that is invoked to select the data.
        /// </param>
        /// <returns>
        ///     A new result containing either; the output of the <paramref>selector</paramref> function
        ///     if this result is a success, otherwise the original failure.
        /// </returns>
        public abstract Task<Result<TOut>> Select<TOut>(Func<T, Task<TOut>> selector);

        /// <summary>
        ///     Projects a failed result's error from one type to another.
        /// </summary>
        /// <remarks>
        ///     If this result is a success then this is a no-op.
        /// </remarks>
        /// <param name="selector">
        ///     The function that is invoked to select the error.
        /// </param>
        /// <returns>
        ///     A new result containing either; the output of the <paramref>selector</paramref> function
        ///     if this result is a failure, otherwise the original success.
        /// </returns>
        public abstract Result<T> SelectError(Func<Error, Error> selector);

        /// <summary>
        ///     Projects a failed result's error from one type to another.
        /// </summary>
        /// <remarks>
        ///     If this result is a success then this is a no-op.
        /// </remarks>
        /// <param name="selector">
        ///     The asynchronous function that is invoked to select the error.
        /// </param>
        /// <returns>
        ///     A new result containing either; the output of the <paramref>selector</paramref> function
        ///     if this result is a failure, otherwise the original success.
        /// </returns>
        public abstract Task<Result<T>> SelectError(Func<Error, Task<Error>> selector);

        /// <summary>
        ///     Invokes another result generating function which takes as input the data of this result
        ///     if it is a success.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op and the original failure is retained.
        ///     This is useful for chaining serial operations together that return results.
        /// </remarks>
        /// <typeparam name="TOut">
        ///     The type of data in the new result.
        /// </typeparam>
        /// <param name="onSuccess">
        ///     The function that is invoked if this result is a success.
        /// </param>
        /// <returns>
        ///     If this result is a success, then the result of <paramref>onSuccess</paramref> function;
        ///     otherwise the original error.
        /// </returns>
        public abstract Result<TOut> Then<TOut>(Func<T, Result<TOut>> onSuccess);

        /// <summary>
        ///     Invokes another result generating function which takes as input the data of this result
        ///     if it is a success.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op and the original failure is retained.
        ///     This is useful for chaining serial operations together that return results.
        /// </remarks>
        /// <typeparam name="TOut">
        ///     The type of data in the new result.
        /// </typeparam>
        /// <param name="onSuccess">
        ///     The asynchronous function that is invoked if this result is a success.
        /// </param>
        /// <returns>
        ///     If this result is a success, then the result of <paramref>onSuccess</paramref> function;
        ///     otherwise the original error.
        /// </returns>
        public abstract Task<Result<TOut>> Then<TOut>(Func<T, Task<Result<TOut>>> onSuccess);
    }
}