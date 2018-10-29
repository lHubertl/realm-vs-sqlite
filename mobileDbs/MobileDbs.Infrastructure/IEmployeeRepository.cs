using MobileDbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MobileDbs.Infrastructure
{
    public interface IEmployeeRepository
    {
        Task<IResponse> CreateAsync(EmployeeModel employee);
        Task<IResponse> CreateAsync(IEnumerable<EmployeeModel> employeesList);
        Task<IDataResponse<IEnumerable<EmployeeModel>>> ReadAsync();
        Task<IDataResponse<EmployeeModel>> ReadAsync(string guid);
        Task<IDataResponse<IEnumerable<EmployeeModel>>> ReadAsync(Expression<Func<EmployeeModel, bool>> predicate);
        Task<IResponse> UpdateAsync(EmployeeModel employee);
        Task<IResponse> UpdateAsync(IEnumerable<EmployeeModel> employeesList);
        Task<IResponse> DeleteAsync(EmployeeModel employee);
        Task<IResponse> DeleteAsync(IEnumerable<EmployeeModel> employeesList);
        Task<IResponse> ClearAll();
    }
}
