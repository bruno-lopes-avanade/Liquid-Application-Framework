using Liquid.Base.Domain;
using Liquid.Domain;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Liquid.Sample.CarRegistry
{
    /// <summary>
    /// Services of Car
    /// </summary>
    public class CarService : LightService
    {
        /// <summary>
        /// Get a record of Car
        /// </summary>
        /// <param name="id">Car Id</param>
        /// <returns></returns>
        public async Task<DomainResponse> GetAsync(string id)
        {
            Telemetry.TrackEvent("Get Record");
            var record = await Cache.GetAsync<Car>(id).ConfigureAwait(true);
            if (string.IsNullOrEmpty(record?.id))
            {
                record = await Repository.GetByIdAsync<Car>(id);
                if(!string.IsNullOrEmpty(record?.id))
                    await Cache.SetAsync(record.id, record).ConfigureAwait(false);
            }
            return Response(record);
        }

        /// <summary>
        /// Get All records of Car
        /// </summary>
        /// <returns></returns>
        public async Task<DomainResponse> GetAllAsync()
        {
            Telemetry.TrackEvent("GetAll Records");
            var records = await Repository.GetAsync<Car>(x => true).ConfigureAwait(true);
            records.ToList().ForEach(async c =>
            {
                await Cache.SetAsync(c.id, c).ConfigureAwait(false);
            });
            return Response(records);
        }

        /// <summary>
        /// Save a record of Car
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public async Task<DomainResponse> SaveAsync(CarVM viewModel)
        {
            var model = new Car();
            model.MapFrom(viewModel);
            Telemetry.TrackEvent("Save Record");
            var records = await Repository.AddOrUpdateAsync(model).ConfigureAwait(true);
            await Cache.SetAsync(records.id, model).ConfigureAwait(false);
            return Response(records);
        }

        /// <summary>
        /// Save a record using a worker
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public async Task<DomainResponse> SaveWorker(LegacyCarVM viewModel)
        {
            var car = CarFactory.Create(viewModel);
            Telemetry.TrackEvent("Save Record");

            var query = Repository.GetAsync<Car>(x => x.id == viewModel.Id).Result;

            var carReturn = query.AsEnumerable().FirstOrDefault();

            if (carReturn != null)
            {
                car.id = carReturn.id;
            }

            var records = await Repository.AddOrUpdateAsync(car).ConfigureAwait(true);
            await Cache.SetAsync(car.id, car).ConfigureAwait(false);
            return Response(records);
        }

        /// <summary>
        /// Deletes a record of Car
        /// </summary>
        /// <param name="id">Identifies the car to be removed</param>
        /// <returns>An empty <see cref="DomainResponse"/></returns>
        public async Task<DomainResponse> DeleteAsync(Guid id)
        {
            Telemetry.TrackEvent("Delete Record");
            await Repository.DeleteAsync<Car>(id.ToString()).ConfigureAwait(true);
            await Cache.RemoveAsync(id.ToString()).ConfigureAwait(false);
            return Response(new DomainResponse());
        }
    }
}
