using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MobileDbs.Domain.Models;
using MobileDbs.Infrastructure.Helpers;
using MobileDbs.Infrastructure.Responses;
using MobileDbs.Infrastructure.SQLite.Dto;
using MobileDbs.Infrastructure.SQLite.Mapper;

namespace MobileDbs.Infrastructure.SQLite
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly SQLiteManager _sqliteManager;

        public CustomerRepository(SQLiteManager sqliteManager)
        {
            _sqliteManager = sqliteManager;
        }

        public async Task<IResponse> CreateAsync(CustomerModel customer)
        {
            var customerDto = customer.ToDto();

            if (_sqliteManager.DataBase == null)
                return new Response(false);

            await _sqliteManager.DataBase.InsertAsync(customerDto);
           
            return new Response(true);
        }

        public async Task<IResponse> CreateAsync(IEnumerable<CustomerModel> customersList)
        {
            var customersDto = customersList.ToDto();
            if (_sqliteManager.DataBase == null)
                return new Response(false);
            
            await _sqliteManager.DataBase.InsertAllAsync(customersDto);

            return new Response(true);
        }

        public async Task<IDataResponse<IEnumerable<CustomerModel>>> ReadAsync()
        {
            if (_sqliteManager.DataBase == null)
                return null;

            List<CustomerModelDto> resultDto;

            resultDto = await _sqliteManager.DataBase.Table<CustomerModelDto>().ToListAsync();

            var result = resultDto.ToModel();
            return new DataResponse<IEnumerable<CustomerModel>>(result, true);
        }

        public async Task<IDataResponse<CustomerModel>> ReadAsync(string guid)
        {
            if (_sqliteManager.DataBase == null)
                return null;

            CustomerModelDto resultDto;
            resultDto = await _sqliteManager.DataBase.Table<CustomerModelDto>().Where(i => i.Guid == guid).FirstOrDefaultAsync();

            var result = resultDto.ToModel();
            return new DataResponse<CustomerModel>(result, true);
        }

        public async Task<IDataResponse<IEnumerable<CustomerModel>>> ReadAsync(Expression<Func<CustomerModel, bool>> predicate)
        {
            if (_sqliteManager.DataBase == null)
                return null;

            List<CustomerModelDto> resultDto = null;
            IEnumerable<CustomerModel> result;
            if (predicate == null)
            {
                resultDto = await _sqliteManager.DataBase.Table<CustomerModelDto>().ToListAsync();
                result = resultDto.ToModel();
                return new DataResponse<IEnumerable<CustomerModel>>(result, true);
            }

            var afterParameter = Expression.Parameter(typeof(CustomerModelDto), predicate.Name);
            var visitor = new ExpressionExtension(predicate, afterParameter); ;
            var newPredicate = Expression.Lambda<Func<CustomerModelDto, bool>>(visitor.Visit(predicate.Body), afterParameter);
            resultDto = await _sqliteManager.DataBase.Table<CustomerModelDto>()
                                                        .Where(newPredicate)
                                                        .ToListAsync();

            result = resultDto.ToModel();
            return new DataResponse<IEnumerable<CustomerModel>>(result,true);
        }

        public async Task<IResponse> UpdateAsync(CustomerModel customer)
        {
            var customerDto = customer.ToDto();

            if (_sqliteManager.DataBase == null)
                return new Response(false);

            await _sqliteManager.DataBase.UpdateAsync(customerDto);
            
            return new Response(true);
        }

        public async Task<IResponse> UpdateAsync(IEnumerable<CustomerModel> customersList)
        {
            var customersDto = customersList.ToDto();

            if (_sqliteManager.DataBase == null)
                return new Response(false);

            await _sqliteManager.DataBase.UpdateAllAsync(customersDto);
            
            return new Response(true);
        }

        public async Task<IResponse> DeleteAsync(CustomerModel customer)
        {
            var customerDto = customer.ToDto();

            if (_sqliteManager.DataBase == null)
                return new Response(false);

            await _sqliteManager.DataBase.DeleteAsync(customerDto);
            
            return new Response(true);
        }

        public async Task<IResponse> DeleteAsync(IEnumerable<CustomerModel> customersList)
        {
            var customersDto = customersList.ToDto();

            if (_sqliteManager.DataBase == null)
                return new Response(false);

            await _sqliteManager.DataBase.RunInTransactionAsync((connection) =>
            {
                foreach (var item in customersDto)
                {
                    connection.Delete(item);
                }
            });

            return new Response(true);
        }

        public async Task<IResponse> ClearAll()
        {
            if (_sqliteManager.DataBase == null)
                return new Response(false);
            await _sqliteManager.DataBase.RunInTransactionAsync((connection) =>
            {
                connection.DeleteAll<CustomerModelDto>();
            });

            return new Response(true);
        }
        
    }
}
