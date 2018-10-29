using MobileDbs.Domain.Models;
using MobileDbs.Infrastructure.Helpers;
using MobileDbs.Infrastructure.Realm.Dto;
using MobileDbs.Infrastructure.Realm.Mapper;
using MobileDbs.Infrastructure.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MobileDbs.Infrastructure.Realm
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly RealmManager _realmManager;

        public EmployeeRepository(RealmManager realmManager)
        {
            _realmManager = realmManager;
        }

        public async Task<IResponse> CreateAsync(EmployeeModel employee)
        {
            var employeeDto = employee.ToDto();

            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new Response(false, "Realm instance can't be null");

            await realmInstance.WriteAsync(realm =>
            {
                realm.Add(employeeDto, false);

            });
            return new Response(true);
        }

        public async Task<IResponse> CreateAsync(IEnumerable<EmployeeModel> employeesList)
        {
            var employeesDto = employeesList.ToDto();

            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new Response(false, "Realm instance can't be null");

            await realmInstance.WriteAsync(realm =>
            {
                foreach (var employeeDto in employeesDto)
                {
                    realm.Add(employeeDto, false);
                }

            });
            return new Response(true);
        }

        public async Task<IDataResponse<IEnumerable<EmployeeModel>>> ReadAsync()
        {
            IEnumerable<EmployeeModelDto> resultDto;
            IEnumerable<EmployeeModel> result;

            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new DataResponse<IEnumerable<EmployeeModel>>(null, false, "Realm instance can't be null");

            resultDto = realmInstance.All<EmployeeModelDto>();
            result = resultDto.ToModel();

            return new DataResponse<IEnumerable<EmployeeModel>>(result, result != null);
        }

        public async Task<IDataResponse<EmployeeModel>> ReadAsync(string guid)
        {
            EmployeeModelDto resultDto;
            EmployeeModel result;

            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new DataResponse<EmployeeModel>(null, false, "Realm instance can't be null");

            resultDto = realmInstance.All<EmployeeModelDto>().Where(item => item.Guid == guid).FirstOrDefault();
            result = resultDto.ToModel();

            return new DataResponse<EmployeeModel>(result, result != null);
        }

        public async Task<IDataResponse<IEnumerable<EmployeeModel>>> ReadAsync(Expression<Func<EmployeeModel, bool>> predicate)
        {
            IEnumerable<EmployeeModelDto> resultDto;
            IEnumerable<EmployeeModel> result;

            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new DataResponse<IEnumerable<EmployeeModel>>(null, false, "Realm instance can't be null");

            var afterParameter = Expression.Parameter(typeof(EmployeeModelDto), predicate.Name);
            var visitor = new ExpressionExtension(predicate, afterParameter); ;
            var newPredicate = Expression.Lambda<Func<EmployeeModelDto, bool>>(visitor.Visit(predicate.Body), afterParameter);

            resultDto = realmInstance.All<EmployeeModelDto>()
                                     .Where(newPredicate);
            result = resultDto.ToModel();

            return new DataResponse<IEnumerable<EmployeeModel>>(result, result != null);
        }

        public async Task<IResponse> UpdateAsync(EmployeeModel employee)
        {
            var employeeDto = employee.ToDto();

            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new Response(false, "Realm instance can't be null");

            await realmInstance.WriteAsync(realm =>
            {
                realm.Add(employeeDto, true);
            });

            return new Response(true);
        }

        public async Task<IResponse> UpdateAsync(IEnumerable<EmployeeModel> employeesList)
        {
            var employeesDto = employeesList.ToDto();

            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new Response(false, "Realm instance can't be null");

            await realmInstance.WriteAsync(realm =>
            {
                foreach (var employeeDto in employeesDto)
                {
                    realm.Add(employeeDto, true);
                }
            });

            return new Response(true);
        }
        public async Task<IResponse> DeleteAsync(EmployeeModel employee)
        {
            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new Response(false, "Realm instance can't be null");

            using (var trans = realmInstance.BeginWrite())
            {
                var employeeRealm = realmInstance.Find<EmployeeModelDto>(employee.Guid);
                realmInstance.Remove(employeeRealm);
                trans.Commit();
            }

            return new Response(true);
        }

        public async Task<IResponse> DeleteAsync(IEnumerable<EmployeeModel> employeesList)
        {
            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new Response(false, "Realm instance can't be null");

            using (var trans = realmInstance.BeginWrite())
            {
                foreach (var employee in employeesList)
                {
                    var item = realmInstance.Find<EmployeeModelDto>(employee.Guid);
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
                realmInstance.RemoveAll<EmployeeModelDto>();
                trans.Commit();
            }

            return new Response(true);
        }
    }
}
