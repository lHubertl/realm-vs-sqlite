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
    public class CompanyRepository : ICompanyRepository
    {
        private readonly RealmManager _realmManager;

        public CompanyRepository(RealmManager realmManager)
        {
            _realmManager = realmManager;
        }
        public async Task<IResponse> CreateAsync(CompanyModel company)
        {
            var companyDto = company.ToDto();

            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new Response(false, "Realm instance can't be null");

            await realmInstance.WriteAsync(realm =>
            {
                realm.Add(companyDto, false);

            });
            return new Response(true);
        }

        public async Task<IResponse> CreateAsync(IEnumerable<CompanyModel> companiesList)
        {
            var companiesDto = companiesList.ToDto();

            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new Response(false, "Realm instance can't be null");

            await realmInstance.WriteAsync(realm =>
            {
                foreach (var companyDto in companiesDto)
                {
                    realm.Add(companyDto, false);
                }

            });
            return new Response(true);
        }

        public async Task<IDataResponse<IEnumerable<CompanyModel>>> ReadAsync()
        {
            IEnumerable<CompanyModelDto> resultDto;
            IEnumerable<CompanyModel> result;
            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new DataResponse<IEnumerable<CompanyModel>>(null, false, "Realm instance can't be null");

            resultDto = realmInstance.All<CompanyModelDto>();
            result = resultDto.ToModel();

            return new DataResponse<IEnumerable<CompanyModel>>(result, result != null);
        }

        public async Task<IDataResponse<CompanyModel>> ReadAsync(string guid)
        {
            CompanyModelDto resultDto;
            CompanyModel result;

            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new DataResponse<CompanyModel>(null, false, "Realm instance can't be null");

            resultDto = realmInstance.All<CompanyModelDto>().Where(item => item.Guid == guid).FirstOrDefault();
            result = resultDto.ToModel();

            return new DataResponse<CompanyModel>(result, result != null);
        }

        public async Task<IDataResponse<IEnumerable<CompanyModel>>> ReadAsync(Expression<Func<CompanyModel, bool>> predicate)
        {
            IEnumerable<CompanyModelDto> resultDto;
            IEnumerable<CompanyModel> result;

            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new DataResponse<IEnumerable<CompanyModel>> (null, false, "Realm instance can't be null");

            var afterParameter = Expression.Parameter(typeof(CompanyModelDto), predicate.Name);
            var visitor = new ExpressionExtension(predicate, afterParameter); ;
            var newPredicate = Expression.Lambda<Func<CompanyModelDto, bool>>(visitor.Visit(predicate.Body), afterParameter);

            resultDto = realmInstance.All<CompanyModelDto>()
                                     .Where(newPredicate);
            result = resultDto.ToModel();

            return new DataResponse<IEnumerable<CompanyModel>>(result, result != null);
        }

        public async Task<IResponse> UpdateAsync(CompanyModel company)
        {
            var companyDto = company.ToDto();

            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new Response(false, "Realm instance can't be null");

            await realmInstance.WriteAsync(realm =>
            {
                realm.Add(companyDto, true);

            });
            return new Response(true);

        }

        public async Task<IResponse> UpdateAsync(IEnumerable<CompanyModel> companiesList)
        {
            var companiesDto = companiesList.ToDto();

            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new Response(false, "Realm instance can't be null");

            await realmInstance.WriteAsync(realm =>
            {
                foreach (var companyDto in companiesDto)
                {
                    realm.Add(companyDto, true);
                }

            });
            return new Response(true);
        }
        public async Task<IResponse> DeleteAsync(CompanyModel company)
        {
            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new Response(false, "Realm instance can't be null");

            using (var trans = realmInstance.BeginWrite())
            {
                var companyRealm = realmInstance.Find<CompanyModelDto>(company.Guid);
                realmInstance.Remove(companyRealm);
                trans.Commit();
            }

            return new Response(true);
        }

        public async Task<IResponse> DeleteAsync(IEnumerable<CompanyModel> companiesList)
        {
            var realmInstance = await _realmManager.GetInstanceAsync();
            if (realmInstance == null)
                return new Response(false, "Realm instance can't be null");

            using (var trans = realmInstance.BeginWrite())
            {
                foreach (var company in companiesList)
                {
                    var item = realmInstance.Find<CompanyModelDto>(company.Guid);
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
                realmInstance.RemoveAll<CompanyModelDto>();
                trans.Commit();
            }

            return new Response(true);
        }
    }
}
