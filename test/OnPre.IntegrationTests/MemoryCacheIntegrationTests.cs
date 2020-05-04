// Copyright (c) Avanade Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
using System;
using System.Collections.Generic;
namespace Liquid.OnPre.IntegrationTests
{
    using System.Reflection;
    using System.Threading.Tasks;
    using AutoFixture;
    using AutoFixture.AutoNSubstitute;
    using FluentValidation;
    using Liquid.OnWindowsClient;
    using Liquid.Repository;
    using Xunit;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    public class MemoryCacheIntegrationTests : IClassFixture<EntityUnderTesting>
    {
        private static readonly IFixture _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        private readonly ITestOutputHelper _output;
        private EntityUnderTesting _lightModel;

        private readonly MemoryCache _sut;

        public MemoryCacheIntegrationTests(EntityUnderTesting entity, ITestOutputHelper output)
        {
            _output = output;
            Workbench.Instance.Reset();
            _sut = new MemoryCache(new MemoryCacheConfiguration
            {
                SlidingExpirationSeconds = 120,
                AbsoluteExpirationRelativeToNowSeconds = 120,
            });
            _lightModel = entity;
            //_fakeRedisCache = Substitute.For<IDistributedCache>();
        }

        [Fact]
        public void ShouldThrowExceptionWhenConfigurationIsNull()
        {
            Assert.ThrowsAny<ArgumentNullException>(() => new MemoryCache(null));
        }


        [Fact]
        public Task ShouldThrowNullExceptionWhenTrySetNullEntity()
        {
            _lightModel = null;
            string str = null;

            return Assert.ThrowsAnyAsync<ArgumentNullException>(() => _sut.SetAsync<EntityUnderTesting>(str, _lightModel));
        }

        [Fact]
        public async Task ShouldSetAndGetEntitySuccesfully()
        {
            //Arrange
            _lightModel.Description = "Entitade modelo";
            _lightModel.id = Guid.NewGuid().ToString();

            //Act/
            await _sut.SetAsync<EntityUnderTesting>(_lightModel.id, _lightModel);

            //Assert
            EntityUnderTesting target = await _sut.GetAsync<EntityUnderTesting>(_lightModel.id);
            Assert.NotNull(target);
        }

        //[Fact]
        //public async Task ShouldSetAndGetEqualsEntity()
        //{
        //    //Arrange
        //    EntityUnderTeste expected = new EntityUnderTeste
        //    {
        //        Description = _fixture.Create<string>().ToLower(CultureInfo.CurrentCulture),
        //        id = _fixture.Create<string>(),
        //        Id = _fixture.Create<Guid>(),
        //    };

        //    //Act/
        //    await _sut.SetAsync<EntityUnderTeste>(expected.id, expected);

        //    //Assert
        //    EntityUnderTeste actual = await _sut.GetAsync<EntityUnderTeste>(expected.id);
        //    Assert.Equal(expected, actual, new CustomComparer<EntityUnderTeste>());
        //}

    }

    public class EntityUnderTesting : LightModel<EntityUnderTesting>
    {

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
