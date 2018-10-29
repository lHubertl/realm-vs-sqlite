using MobileDbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MobileDbs.Infrastructure
{
    public interface ICustomerRepository
    {
        Task<IResponse> CreateAsync(CustomerModel customer);
        Task<IResponse> CreateAsync(IEnumerable<CustomerModel> customersList);
        Task<IDataResponse<IEnumerable<CustomerModel>>> ReadAsync();
        Task<IDataResponse<CustomerModel>> ReadAsync(string guid);
        Task<IDataResponse<IEnumerable<CustomerModel>>> ReadAsync(Expression<Func<CustomerModel, bool>> predicate);
        Task<IResponse> UpdateAsync(CustomerModel customer);
        Task<IResponse> UpdateAsync(IEnumerable<CustomerModel> customersList);
        Task<IResponse> DeleteAsync(CustomerModel customer);
        Task<IResponse> DeleteAsync(IEnumerable<CustomerModel> customersList);
        Task<IResponse> ClearAll();
    }
}
