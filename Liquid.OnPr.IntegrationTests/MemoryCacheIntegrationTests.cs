using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentValidation;
using Liquid.OnWindowsClient;
using Liquid.Repository;
using Xunit;
using Xunit.Abstractions;

namespace Liquid.OnPr.IntegrationTests
{
    public class MemoryCacheIntegrationTests
    {
        private static readonly IFixture _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        private readonly ITestOutputHelper _output;
        private EntityUnderTeste _lightModel;

        private readonly MemoryCache _sut;

        public MemoryCacheIntegrationTests(EntityUnderTeste entity, ITestOutputHelper output)
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
