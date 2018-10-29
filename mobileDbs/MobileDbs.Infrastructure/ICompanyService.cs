using MobileDbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MobileDbs.Infrastructure
{
    public interface ICompanyService
    {
        Task<IDataResponse<IList<CompanyModel>>> GenerateRecord(int count);
        Task<IDataResponse<IEnumerable<CompanyModel>>> ReadAllRecords();
        Task<IDataResponse<IEnumerable<CompanyModel>>> ReadById(string guid);
        Task<IResponse> UpdateAllRecords(IList<CompanyModel> records);
        Task<IResponse> DeleteAllRecords(IList<CompanyModel> records);
        Task<IResponse> ClearAll();
        Task<IDataResponse<IEnumerable<CompanyModel>>> ReadByPredicate(Expression<Func<CompanyModel, bool>> predicate);
    }
}
