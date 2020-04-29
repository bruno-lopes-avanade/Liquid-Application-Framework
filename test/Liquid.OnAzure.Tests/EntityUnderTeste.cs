using System;
using FluentValidation;
using Liquid.Repository;

namespace Liquid.OnAzure.Tests
{
    public class EntityUnderTeste : LightModel<EntityUnderTeste>
    {
        /// <summary>
        /// Identifies entity car
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Describes the entity object
        /// </summary>
        public string Description { get; set; }

        /// <inheritdoc/>
        public override void Validate()
        {
            RuleFor(i => i.Description).NotEmpty().WithErrorCode("DESCRIPTION_MUSTNOT_BE_EMPTY");
        }
    }
}