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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly SQLiteManager _sqliteManager;

        public EmployeeRepository(SQLiteManager sqliteManager)
        {
            _sqliteManager = sqliteManager;
        }

        public async Task<IResponse> CreateAsync(EmployeeModel employee)
        {
            var employeeDto = employee.ToDto();
            if (_sqliteManager.DataBase == null)
                return new Response(false);

            await _sqliteManager.DataBase.InsertAsync(employeeDto);

            return new Response(true);
        }

        public async Task<IResponse> CreateAsync(IEnumerable<EmployeeModel> employeesList)
        {
            var employeesDto = employeesList.ToDto();
            if (_sqliteManager.DataBase == null)
                return new Response(false);

            await _sqliteManager.DataBase.InsertAllAsync(employeesDto);

            return new Response(true);
        }

        public async Task<IDataResponse<IEnumerable<EmployeeModel>>> ReadAsync()
        {
            if (_sqliteManager.DataBase == null)
                return null;

            List<EmployeeModelDto> resultDto;

            resultDto = await _sqliteManager.DataBase.Table<EmployeeModelDto>().ToListAsync();
            var result = resultDto.ToModel();
            return new DataResponse<IEnumerable<EmployeeModel>>(result, true);
        }

        public async Task<IDataResponse<EmployeeModel>> ReadAsync(string guid)
        {
            if (_sqliteManager.DataBase == null)
                return null;

            EmployeeModelDto resultDto;
            resultDto = await _sqliteManager.DataBase.Table<EmployeeModelDto>().Where(i => i.Guid == guid).FirstOrDefaultAsync();
            var result = resultDto.ToModel();
            return new DataResponse<EmployeeModel>(result, true);
        }

        public async Task<IDataResponse<IEnumerable<EmployeeModel>>> ReadAsync(Expression<Func<EmployeeModel, bool>> predicate)
        {
            if (_sqliteManager.DataBase == null)
                return null;

            List<EmployeeModelDto> resultDto;
            IEnumerable<EmployeeModel> result;
            if (predicate == null)
            {
                resultDto = await _sqliteManager.DataBase.Table<EmployeeModelDto>().ToListAsync();
                result = resultDto.ToModel();
                return new DataResponse<IEnumerable<EmployeeModel>>(result, true);
            }

            var afterParameter = Expression.Parameter(typeof(EmployeeModelDto), predicate.Name);
            var visitor = new ExpressionExtension(predicate, afterParameter); ;
            var newPredicate = Expression.Lambda<Func<EmployeeModelDto, bool>>(visitor.Visit(predicate.Body), afterParameter);

            resultDto = await _sqliteManager.DataBase.Table<EmployeeModelDto>()
                                                     .Where(newPredicate)
                                                     .ToListAsync();

            result = resultDto.ToModel();

            return new DataResponse<IEnumerable<EmployeeModel>>(result, true);
        }

        public async Task<IResponse> UpdateAsync(EmployeeModel employee)
        {
            var employeeDto = employee.ToDto();
            if (_sqliteManager.DataBase == null)
                return new Response(false);

            await _sqliteManager.DataBase.UpdateAsync(employeeDto);

            return new Response(true);
        }

        public async Task<IResponse> UpdateAsync(IEnumerable<EmployeeModel> employeesList)
        {
            var employeesDto = employeesList.ToDto();

            if (_sqliteManager.DataBase == null)
                return new Response(false);

            await _sqliteManager.DataBase.UpdateAllAsync(employeesDto);

            return new Response(true);
        }

        public async Task<IResponse> DeleteAsync(EmployeeModel employee)
        {
            var employeeDto = employee.ToDto();

            if (_sqliteManager.DataBase == null)
                return new Response(false);

            await _sqliteManager.DataBase.DeleteAsync(employeeDto);

            return new Response(true);
        }

        public async Task<IResponse> DeleteAsync(IEnumerable<EmployeeModel> employeesList)
        {
            var employeesDto = employeesList.ToDto();

            if (_sqliteManager.DataBase == null)
                return new Response(false);

            await _sqliteManager.DataBase.RunInTransactionAsync((connection) =>
            {
                foreach (var item in employeesDto)
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
                connection.DeleteAll<EmployeeModelDto>();
            });

            return new Response(true);
        }
    }
}
