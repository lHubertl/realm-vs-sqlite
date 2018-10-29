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
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IDataResponse<IList<CustomerModel>>> GenerateRecord(int count)
        {
            var companies = new List<CustomerModel>();
            Random rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                companies.Add(new CustomerModel
                {
                    Guid = Guid.NewGuid().ToString(),
                    Name = StringExtension.GenerateName(6),
                    Age = rnd.Next(30, 67),
                    IsActive = rnd.Next(0, 2) == 0,
                    LastVisit = new DateTimeOffset(new DateTime(rnd.Next(2000, 2018), rnd.Next(1, 11), rnd.Next(1, 11))),
                    Salary = rnd.NextDouble() * 999999
                });
            }

            await _customerRepository.CreateAsync(companies);

            return new DataResponse<IList<CustomerModel>>(companies, true);
        }
        public Task<IDataResponse<IEnumerable<CustomerModel>>> ReadAllRecords()
        {
            return _customerRepository.ReadAsync();
        }

        public Task<IDataResponse<IEnumerable<CustomerModel>>> ReadById(string guid)
        {
            return _customerRepository.ReadAsync(item => item.Guid == guid);
        }

        public Task<IResponse> UpdateAllRecords(IList<CustomerModel> records)
        {
            if (records == null)
                return null;

            return _customerRepository.UpdateAsync(records);
        }
        public Task<IResponse> UpdateRecord(CustomerModel record)
        {
            if (record == null)
                return null;

            return _customerRepository.UpdateAsync(record);
        }

        public Task<IResponse> DeleteRecord(CustomerModel record)
        {
            if (record == null)
                return null;

            return _customerRepository.DeleteAsync(record);
        }

        public Task<IResponse> DeleteAllRecords (IList<CustomerModel> records)
        {
            if (records == null)
                return null;

            return _customerRepository.DeleteAsync(records);
        }

        public Task<IResponse> ClearAll()
        {
            return _customerRepository.ClearAll();
        }

        public Task<IDataResponse<IEnumerable<CustomerModel>>> ReadByPredicate(Expression<Func<CustomerModel, bool>> predicate)
        {
            return _customerRepository.ReadAsync(predicate);
        }
    }
}
