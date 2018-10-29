using MobileDbs.Domain.Models;
using MobileDbs.Infrastructure;
using MobileDbs.Infrastructure.Helpers;
using MobileDbs.Infrastructure.Responses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MobileDbs.Domain.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<IDataResponse<IList<CompanyModel>>> GenerateRecord(int count)
        {
            List<CompanyModel> companies = new List<CompanyModel>();
            for (int i = 0; i < count; i++)
            {
                companies.Add(new CompanyModel
                {
                    Guid = Guid.NewGuid().ToString(),
                    Name = StringExtension.GenerateName(6),
                    FounderName = StringExtension.GenerateName(8),
                    Co_FounderName = StringExtension.GenerateName(5)
                });
            }

            await _companyRepository.CreateAsync(companies);

            return new DataResponse<IList<CompanyModel>>(companies, true);
        }

        public Task<IDataResponse<IEnumerable<CompanyModel>>> ReadAllRecords()
        {
            return _companyRepository.ReadAsync();
        }

        public Task<IDataResponse<IEnumerable<CompanyModel>>> ReadById(string guid)
        {
            return _companyRepository.ReadAsync(item => item.Guid == guid);
        }

        public Task<IResponse> UpdateAllRecords(IList<CompanyModel> records)
        {
            if (records == null)
                return null;
            return _companyRepository.UpdateAsync(records);
        }

        public Task<IResponse> DeleteAllRecords(IList<CompanyModel> records)
        {
            if (records == null)
                return null;

            return _companyRepository.DeleteAsync(records);
        }

        public Task<IResponse> ClearAll()
        {
            return _companyRepository.ClearAll();
        }

        public Task<IDataResponse<IEnumerable<CompanyModel>>> ReadByPredicate(Expression<Func<CompanyModel, bool>> predicate)
        {
            return _companyRepository.ReadAsync(predicate);
        }
    }
}
