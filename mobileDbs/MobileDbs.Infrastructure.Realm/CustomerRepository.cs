using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MobileDbs.Domain.Models;
using MobileDbs.Infrastructure.Helpers;
using MobileDbs.Infrastructure.Realm.Dto;
using MobileDbs.Infrastructure.Realm.Mapper;
using MobileDbs.Infrastructure.Responses;

namespace MobileDbs.Infrastructure.Realm
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly RealmManager _realmManager;

        public CustomerRepository(RealmManager realmManager)
        {
            _realmManager = realmManager;
        }

        public async Task<IResponse> CreateAsync(CustomerModel customer)
        {
            var customerDto = customer.ToDto();

            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new Response(false, "Realm instance can't be null");

            await realmInstance.WriteAsync(realm =>
            {
               realm.Add(customerDto, false);
               
            });
            return new Response(true);
        }

        public async Task<IResponse> CreateAsync(IEnumerable<CustomerModel> customersList)
        {
            var customersDto = customersList.ToDto();

            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new Response(false, "Realm instance can't be null");

            await realmInstance.WriteAsync(realm =>
            {
                foreach (var customerDto in customersDto)
                {
                    realm.Add(customerDto, false);
                }

            });
            return new Response(true);
        }

        public async Task<IDataResponse<IEnumerable<CustomerModel>>> ReadAsync()
        {
            IEnumerable<CustomerModelDto> resultDto;
            IEnumerable<CustomerModel> result;

            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new DataResponse<IEnumerable<CustomerModel>>(null, false, "Realm instance can't be null");

            resultDto = realmInstance.All<CustomerModelDto>();
            result = resultDto.ToModel();

            return new DataResponse<IEnumerable<CustomerModel>>(result, result != null);
        }

        public async Task<IDataResponse<CustomerModel>> ReadAsync(string guid)
        {
            CustomerModelDto resultDto;
            CustomerModel result;

            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new DataResponse<CustomerModel>(null, false, "Realm instance can't be null");

            resultDto = realmInstance.All<CustomerModelDto>().Where(item => item.Guid == guid).FirstOrDefault();
            result = resultDto.ToModel();

            return new DataResponse<CustomerModel>(result, result != null);
        }

        public async Task<IDataResponse<IEnumerable<CustomerModel>>> ReadAsync(Expression<Func<CustomerModel, bool>> predicate)
        {
            IEnumerable<CustomerModelDto> resultDto;
            IEnumerable<CustomerModel> result = null;

            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new DataResponse<IEnumerable<CustomerModel>>(null, false, "Realm instance can't be null");
           
            var afterParameter = Expression.Parameter(typeof(CustomerModelDto), predicate.Name);
            var visitor = new ExpressionExtension(predicate, afterParameter); ;
            var newPredicate = Expression.Lambda<Func<CustomerModelDto, bool>>(visitor.Visit(predicate.Body), afterParameter);
            resultDto = realmInstance.All<CustomerModelDto>()
                                        .Where(newPredicate);
            result = resultDto.ToModel();
            
            return new DataResponse<IEnumerable<CustomerModel>>(result, result != null);

        }

        public async Task<IResponse> UpdateAsync(CustomerModel customer)
        {
            var customerDto = customer.ToDto();

            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new Response(false, "Realm instance can't be null");

            await realmInstance.WriteAsync(realm =>
            {
                realm.Add(customerDto, true);

            });
            return new Response(true);

        }

        public async Task<IResponse> UpdateAsync(IEnumerable<CustomerModel> customersList)
        {
            var customersDto = customersList.ToDto();

            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new Response(false, "Realm instance can't be null");

            await realmInstance.WriteAsync(realm =>
            {
                foreach (var customerDto in customersDto)
                {
                    realm.Add(customerDto, true);
                }

            });
            return new Response(true);
        }
        public async Task<IResponse> DeleteAsync(CustomerModel customer)
        {
            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new Response(false, "Realm instance can't be null");

            using (var trans = realmInstance.BeginWrite())
            {
                var customerRealm = realmInstance.Find<CustomerModelDto>(customer.Guid);
                realmInstance.Remove(customerRealm);
                trans.Commit();
            }

            return new Response(true);
        }

        public async Task<IResponse> DeleteAsync(IEnumerable<CustomerModel> customersList)
        {
            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new Response(false, "Realm instance can't be null");

            using (var trans = realmInstance.BeginWrite())
            {
                foreach (var customer in customersList)
                {
                    var item = realmInstance.Find<CustomerModelDto>(customer.Guid);
                    realmInstance.Remove(item);
                }

                trans.Commit();
            }
            return new Response(true);
        }

        public async Task<IResponse> ClearAll()
        {
            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new Response(false, "Realm instance can't be null");

            using (var trans = realmInstance.BeginWrite())
            {
                realmInstance.RemoveAll<CustomerModelDto>();
                trans.Commit();
            }

            return new Response(true);
        }
    }
}
