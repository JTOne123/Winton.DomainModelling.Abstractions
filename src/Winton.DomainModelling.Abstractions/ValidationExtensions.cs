// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;

namespace Winton.DomainModelling
{
    /// <summary>
    ///     Extensions for specific types of <see cref="Validation{T}" />.
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        ///     Takes a func that is encapsulated in a <see cref="Validation{T}"/> and partially applies
        ///     the data that is encapsulated in a <see cref="Validation{T}"/> to the first argument of the func.
        /// </summary>
        /// <remarks>
        ///     If the validations are valid then the func is invoked with the data, otherwise any errors are combined.
        /// </remarks>
        /// <param name="func">The func to invoke with valid data.</param>
        /// <param name="validation">
        ///     The validation containing the data to be applied to the <paramref name="func" />.
        /// </param>
        /// <typeparam name="TIn">The type of the first argument to the <paramref name="func" />.</typeparam>
        /// <typeparam name="TOut">The type of data returned from the <paramref name="func" />.</typeparam>
        /// <returns>
        ///     A <see cref="Validation{T}" /> containing either the result of the valid func invoked on the valid data,
        ///     or the combination of the validation errors.
        /// </returns>
        public static Validation<TOut> Apply<TIn, TOut>(
            this Validation<Func<TIn, TOut>> func,
            Validation<TIn> validation)
        {
            return func.Append(validation, (f, x) => f(x));
        }

        /// <summary>
        ///     Takes a func and partially applies the data that is encapsulated in a
        ///     <see cref="Validation{T}"/> to the first argument of the func.
        /// </summary>
        /// <remarks>
        ///     If the validation is valid then the func is invoked with the data, otherwise the original error is returned.
        /// </remarks>
        /// <param name="func">The func to invoke with valid data.</param>
        /// <param name="validation">
        ///     The validation containing the data to be applied to the <paramref name="func" />.
        /// </param>
        /// <typeparam name="TIn">The type of the first argument to the <paramref name="func" />.</typeparam>
        /// <typeparam name="TOut">The type of data returned from the <paramref name="func" />.</typeparam>
        /// <returns>
        ///     A <see cref="Validation{T}" /> containing either result of the func invoked on the valid data,
        ///     or the original invalid data of the argument.
        /// </returns>
        public static Validation<TOut> Apply<TIn, TOut>(
            this Func<TIn, TOut> func,
            Validation<TIn> validation)
        {
            return new Valid<Func<TIn, TOut>>(func).Apply(validation);
        }

        /// <summary>
        ///     Takes a func and partially applies the data that is encapsulated in a
        ///     <see cref="Validation{T}"/> to the first argument of the func.
        /// </summary>
        /// <remarks>
        ///     If the validation is valid then the func is invoked with the data, otherwise the original error is returned.
        /// </remarks>
        /// <param name="func">The func to invoke with valid data.</param>
        /// <param name="validation">
        ///     The validation containing the data to be applied to the <paramref name="func" />.
        /// </param>
        /// <typeparam name="TIn1">The type of the first argument to the <paramref name="func" />.</typeparam>
        /// <typeparam name="TIn2">The type of the second argument to the <paramref name="func" />.</typeparam>
        /// <typeparam name="TOut">The type of data returned from the <paramref name="func" />.</typeparam>
        /// <returns>
        ///     A <see cref="Validation{T}" /> containing either the partially applied func,
        ///     or the validation errors.
        /// </returns>
        public static Validation<Func<TIn2, TOut>> Apply<TIn1, TIn2, TOut>(
            this Func<TIn1, TIn2, TOut> func,
            Validation<TIn1> validation)
        {
            return new Valid<Func<TIn1, TIn2, TOut>>(func).Apply(validation);
        }

        /// <summary>
        ///     Takes a func that is encapsulated in a <see cref="Validation{T}"/> and partially applies
        ///     the data that is encapsulated in a <see cref="Validation{T}"/> to the first argument of the func.
        /// </summary>
        /// <remarks>
        ///     If the validation is valid then the func is invoked with the data, otherwise the original error is returned.
        /// </remarks>
        /// <param name="func">The func to invoke with valid data.</param>
        /// <param name="validation">
        ///     The validation containing the data to be applied to the <paramref name="func" />.
        /// </param>
        /// <typeparam name="TIn1">The type of the first argument to the <paramref name="func" />.</typeparam>
        /// <typeparam name="TIn2">The type of the second argument to the <paramref name="func" />.</typeparam>
        /// <typeparam name="TOut">The type of data returned from the <paramref name="func" />.</typeparam>
        /// <returns>
        ///     A <see cref="Validation{T}" /> containing either the partially applied func,
        ///     or the combination of the validation errors.
        /// </returns>
        public static Validation<Func<TIn2, TOut>> Apply<TIn1, TIn2, TOut>(
            this Validation<Func<TIn1, TIn2, TOut>> func,
            Validation<TIn1> validation)
        {
            return func.Append<TIn1, Func<TIn2, TOut>>(validation, (f, x1) => x2 => f(x1, x2));
        }
    }
}