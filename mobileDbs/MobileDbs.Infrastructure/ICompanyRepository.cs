using MobileDbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MobileDbs.Infrastructure
{
    public interface ICompanyRepository
    {
        Task<IResponse> CreateAsync(CompanyModel company);
        Task<IResponse> CreateAsync(IEnumerable<CompanyModel> companiesList);
        Task<IDataResponse<IEnumerable<CompanyModel>>> ReadAsync();
        Task<IDataResponse<CompanyModel>> ReadAsync(string guid);
        Task<IDataResponse<IEnumerable<CompanyModel>>> ReadAsync(Expression<Func<CompanyModel, bool>> predicate);
        Task<IResponse> UpdateAsync(CompanyModel company);
        Task<IResponse> UpdateAsync(IEnumerable<CompanyModel> companiesList);
        Task<IResponse> DeleteAsync(CompanyModel company);
        Task<IResponse> DeleteAsync(IEnumerable<CompanyModel> companiesList);
        Task<IResponse> ClearAll();
    }
}
