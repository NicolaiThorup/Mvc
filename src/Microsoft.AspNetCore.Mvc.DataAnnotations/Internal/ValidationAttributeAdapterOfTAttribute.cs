// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

namespace Microsoft.AspNetCore.Mvc.DataAnnotations.Internal
{
    /// <summary>
    /// An implementation of <see cref="IClientModelValidator"/> which understands data annotation attributes.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    public abstract class ValidationAttributeAdapter<TAttribute> : IClientModelValidator
        where TAttribute : ValidationAttribute
    {
        private readonly IStringLocalizer _stringLocalizer;
        /// <summary>
        /// Create a new instance of <see cref="ValidationAttributeAdapter{TAttribute}"/>.
        /// </summary>
        /// <param name="attribute">The <typeparamref name="TAttribute"/> instance to validate.</param>
        /// <param name="stringLocalizer">The <see cref="IStringLocalizer"/>.</param>
        public ValidationAttributeAdapter(TAttribute attribute, IStringLocalizer stringLocalizer)
        {
            Attribute = attribute;
            _stringLocalizer = stringLocalizer;
        }

        /// <summary>
        /// Gets the <typeparamref name="TAttribute"/> instance.
        /// </summary>
        public TAttribute Attribute
        {
            get;
        }

        /// <inheritdoc />
        public abstract IEnumerable<ModelClientValidationRule> GetClientValidationRules(
            ClientModelValidationContext context);

        /// <summary>
        /// Gets the error message formatted using the <see cref="Attribute"/>.
        /// </summary>
        /// <param name="modelMetadata">The <see cref="ModelMetadata"/> associated with the model annotated with
        /// <see cref="Attribute"/>.</param>
        /// <param name="arguments">The value arguments which will be used in constructing the error message.</param>
        /// <returns>Formatted error string.</returns>
        protected virtual string GetErrorMessage(ModelMetadata modelMetadata, params object[] arguments)
        {
            if (modelMetadata == null)
            {
                throw new ArgumentNullException(nameof(modelMetadata));
            }

            if (_stringLocalizer != null &&
                !string.IsNullOrEmpty(Attribute.ErrorMessage) &&
                string.IsNullOrEmpty(Attribute.ErrorMessageResourceName) &&
                Attribute.ErrorMessageResourceType == null)
            {
                return _stringLocalizer[Attribute.ErrorMessage, arguments];
            }

            return Attribute.FormatErrorMessage(modelMetadata.GetDisplayName());
        }
    }
}