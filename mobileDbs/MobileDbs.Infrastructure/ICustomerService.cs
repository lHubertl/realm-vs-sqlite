using MobileDbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MobileDbs.Infrastructure
{
    public interface ICustomerService
    {
        Task<IDataResponse<IList<CustomerModel>>> GenerateRecord(int count);
        Task<IDataResponse<IEnumerable<CustomerModel>>> ReadAllRecords();
        Task<IDataResponse<IEnumerable<CustomerModel>>> ReadById(string guid);
        Task<IResponse> UpdateAllRecords(IList<CustomerModel> records);
        Task<IResponse> UpdateRecord(CustomerModel record);
        Task<IResponse> DeleteRecord(CustomerModel record);
        Task<IResponse> DeleteAllRecords(IList<CustomerModel> records);
        Task<IResponse> ClearAll();
        Task<IDataResponse<IEnumerable<CustomerModel>>> ReadByPredicate(Expression<Func<CustomerModel, bool>> predicate);
    }
}
