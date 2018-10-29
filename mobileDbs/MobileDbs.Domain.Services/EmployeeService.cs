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
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IDataResponse<IList<EmployeeModel>>> GenerateRecord(int count)
        {
            List<EmployeeModel> employees = new List<EmployeeModel>();
            Random rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                employees.Add(new EmployeeModel
                {
                    Guid = Guid.NewGuid().ToString(),
                    Name = StringExtension.GenerateName(6),
                    Age = rnd.Next(30, 67),
                    Position = "Xam Developer"
                });
            }

            await _employeeRepository.CreateAsync(employees);

            return new DataResponse<IList<EmployeeModel>>(employees, true);
        }

        public Task<IDataResponse<IEnumerable<EmployeeModel>>> ReadAllRecords()
        {
            return _employeeRepository.ReadAsync();
        }

        public Task<IDataResponse<IEnumerable<EmployeeModel>>> ReadById(string guid)
        {
            return _employeeRepository.ReadAsync(item => item.Guid == guid);
        }

        public Task<IResponse> UpdateAllRecords(IList<EmployeeModel> records)
        {
            if (records == null)
                return null;

            return _employeeRepository.UpdateAsync(records);
        }

        public Task<IResponse> DeleteAllRecords(IList<EmployeeModel> records)
        {
            if (records == null)
                return null;

            return _employeeRepository.DeleteAsync(records);
        }

        public Task<IResponse> ClearAll()
        {
            return _employeeRepository.ClearAll();
        }

        public Task<IDataResponse<IEnumerable<EmployeeModel>>> ReadByPredicate(Expression<Func<EmployeeModel, bool>> predicate)
        {
            return _employeeRepository.ReadAsync(predicate);
        }
    }
}
