using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;

namespace MicroFlow
{
    public sealed class ValidationResult
    {
        private readonly List<ValidationError> _errors = new List<ValidationError>();

        public bool HasErrors
        {
            get { return _errors.Count > 0; }
        }

        [NotNull]
        public ReadOnlyCollection<ValidationError> Errors
        {
            get { return _errors.AsReadOnly(); }
        }

        public void AddError([NotNull] string errorMessage)
        {
            errorMessage.AssertNotNullOrEmpty("Error message cannot be null or empty");
            _errors.Add(new ValidationError(errorMessage));
        }

        public void AddError([NotNull] IFlowNode node, [NotNull] string errorMessage)
        {
            node.AssertNotNull("node != null");
            errorMessage.AssertNotNullOrEmpty("Error message cannot be null or empty");

            _errors.Add(new ValidationError(node, errorMessage));
        }

        public void AddError(IActivityDescriptor descriptor, [NotNull] string errorMessage)
        {
            descriptor.AssertNotNull("descriptor != null");
            errorMessage.AssertNotNullOrEmpty("Error message cannot be null or empty");

            _errors.Add(new ValidationError(descriptor.Id, descriptor.Name, errorMessage));
        }

        public void TakeErrorsFrom([NotNull] ValidationResult validationResult)
        {
            validationResult.AssertNotNull("validationResult != null");

            _errors.AddRange(validationResult.Errors);
            validationResult.ClearErrors();
        }

        public void ClearErrors()
        {
            _errors.Clear();
        }
    }
}