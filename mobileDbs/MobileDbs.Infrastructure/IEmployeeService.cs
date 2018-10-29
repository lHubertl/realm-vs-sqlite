using MobileDbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MobileDbs.Infrastructure
{
    public interface IEmployeeService
    {
        Task<IDataResponse<IList<EmployeeModel>>> GenerateRecord(int count);
        Task<IDataResponse<IEnumerable<EmployeeModel>>> ReadAllRecords();
        Task<IDataResponse<IEnumerable<EmployeeModel>>> ReadById(string guid);
        Task<IResponse> UpdateAllRecords(IList<EmployeeModel> records);
        Task<IResponse> DeleteAllRecords(IList<EmployeeModel> records);
        Task<IResponse> ClearAll();
        Task<IDataResponse<IEnumerable<EmployeeModel>>> ReadByPredicate(Expression<Func<EmployeeModel, bool>> predicate);
    }
}
