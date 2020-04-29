// Copyright (c) Avanade Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentValidation;
using Liquid.Repository;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Liquid.OnAzure.IntegrationTests
{
    public class AzureRedisIntegrationTests : IClassFixture<EntityUnderTeste>
    {
        private const string DefaultConnectionString = "liquid.redis.cache.windows.net:6380,password=8SGTpwkAuEzaTOKZj2BU9Lk9DALtV2LWbrA19gYPGAI=,ssl=True,abortConnect=False";
        private static readonly IFixture _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        private readonly ITestOutputHelper _output;
        private EntityUnderTeste _lightModel;

        private readonly AzureRedis _sut;

        public AzureRedisIntegrationTests(EntityUnderTeste entity, ITestOutputHelper output)
        {
            _output = output;
            Workbench.Instance.Reset();
            _sut = new AzureRedis(new AzureRedisConfiguration
            {
                Configuration = DefaultConnectionString,
                SlidingExpirationSeconds = 120,
                AbsoluteExpirationRelativeToNowSeconds = 3600,
            });
            _lightModel = entity;
            //_fakeRedisCache = Substitute.For<IDistributedCache>();
        }

        [Fact]
        public void ShouldThrowExceptionWhenConfigurationIsNull()
        {
            Assert.ThrowsAny<ArgumentNullException>(() => new AzureRedis(null));
        }


        [Fact]
        public Task ShouldThrowNullExceptionWhenTrySetNullEntity()
        {
            _lightModel = null;
            string str = null;

            return Assert.ThrowsAnyAsync<ArgumentNullException>(() => _sut.SetAsync<EntityUnderTeste>(str, _lightModel));
        }

        [Fact]
        public async Task ShouldSetAndGetEntitySuccesfully()
        {
            //Arrange
            _lightModel.Description = "Entitade modelo";
            _lightModel.id = _fixture.Create<string>();

            //Act/
            await _sut.SetAsync<EntityUnderTeste>(_lightModel.id, _lightModel);

            //Assert
            EntityUnderTeste target = await _sut.GetAsync<EntityUnderTeste>(_lightModel.id);
            Assert.NotNull(target);
        }


        [Fact]
        public async Task ShouldSetAndGetEqualsEntity()
        {
            //Arrange
            EntityUnderTeste expected = new EntityUnderTeste
            {
                Description = _fixture.Create<string>().ToLower(CultureInfo.CurrentCulture),
                id = _fixture.Create<string>(),
                Id = _fixture.Create<Guid>(),
            };

            //Act/
            await _sut.SetAsync<EntityUnderTeste>(expected.id, expected);

            //Assert
            EntityUnderTeste actual = await _sut.GetAsync<EntityUnderTeste>(expected.id);
            Assert.Equal(expected, actual, new CustomComparer<EntityUnderTeste>());
        }

    }

    public class CustomComparer<T> : IEqualityComparer<T>
    {
        public bool Equals(T expected, T actual)
        {
            var props = typeof(T).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                var expectedValue = prop.GetValue(expected, null);
                var actualValue = prop.GetValue(actual, null);
                if (!expectedValue.Equals(actualValue))
                {
                    throw new EqualException($"A value of \"{expectedValue}\" for property \"{prop.Name}\"",
                        $"A value of \"{actualValue}\" for property \"{prop.Name}\"");
                }
            }

            return true;
        }

        public int GetHashCode(T parameterValue)
        {
            return Tuple.Create(parameterValue).GetHashCode();
        }
    }

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
