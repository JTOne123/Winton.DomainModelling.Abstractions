// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;

namespace Winton.DomainModelling
{
    /// <summary>
    ///     Represents the the result of a validation operation.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of data that has been validated.
    /// </typeparam>
    // ReSharper disable once InconsistentNaming
    public abstract class Validation<T>
    {
        /// <summary>
        ///     Implicitly casts a <see cref="Validation{T}"/> to a <see cref="Result{T}"/>.
        /// </summary>
        /// <param name="validation">The validation to cast.</param>
        /// <returns>The corresponding <see cref="Result{T}"/>.</returns>
        public static implicit operator Result<T>(Validation<T> validation)
        {
            return validation.ToResult();
        }

        /// <summary>
        ///     Appends another validation onto this one.
        ///     If both are valid then <paramref>combine</paramref> is invoked;
        ///     else if either is invalid then the errors are appended.
        /// </summary>
        /// <typeparam name="TOther">
        ///     The type of data in the other validation.
        /// </typeparam>
        /// <typeparam name="TCombined">
        ///     The type of data in the combined validation.
        /// </typeparam>
        /// <param name="other">
        ///     The other result to be combined with this one.
        /// </param>
        /// <param name="combine">
        ///     The function that is invoked to combine the data when both of the validations are valid.
        /// </param>
        /// <returns>
        ///     A new <see cref="Validation{T}" />.
        /// </returns>
        public abstract Validation<TCombined> Append<TOther, TCombined>(
            Validation<TOther> other,
            Func<T, TOther, TCombined> combine);

        /// <summary>
        ///     Used to match on whether this valid or invalid.
        /// </summary>
        /// <typeparam name="TOut">
        ///     The type that is returned.
        /// </typeparam>
        /// <param name="onValid">
        ///     The function that is invoked if this represents valid data.
        /// </param>
        /// <param name="onInvalid">
        ///     The function that is invoked if this represents invalid data.
        /// </param>
        /// <returns>
        ///     A value that is mapped from either the data or the error.
        /// </returns>
        public abstract TOut Match<TOut>(Func<T, TOut> onValid, Func<ValidationError, TOut> onInvalid);

        /// <summary>
        ///     Converts this <see cref="Validation{T}" /> into a <see cref="Result{T}" />.
        /// </summary>
        /// <remarks>
        ///     If this is valid then it will be converted to <see cref="Success{T}" />;
        ///     otherwise it will be converted to <see cref="Failure{T}" />.
        /// </remarks>
        /// <returns>The corresponding <see cref="Result{T}" />.</returns>
        public abstract Result<T> ToResult();
    }
}