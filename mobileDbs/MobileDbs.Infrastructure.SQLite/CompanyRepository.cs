using MobileDbs.Domain.Models;
using MobileDbs.Infrastructure.Helpers;
using MobileDbs.Infrastructure.Responses;
using MobileDbs.Infrastructure.SQLite.Dto;
using MobileDbs.Infrastructure.SQLite.Mapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MobileDbs.Infrastructure.SQLite
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly SQLiteManager _sqliteManager;

        public CompanyRepository(SQLiteManager sqliteManager)
        {
            _sqliteManager = sqliteManager;
        }

        public async Task<IResponse> CreateAsync(CompanyModel company)
        {
            var companyDto = company.ToDto();

            if (_sqliteManager.DataBase == null)
                return new Response(false);

            await _sqliteManager.DataBase.InsertAsync(companyDto);

            return new Response(true);
        }

        public async Task<IResponse> CreateAsync(IEnumerable<CompanyModel> companiesList)
        {
            var companiesDto = companiesList.ToDto();
            if (_sqliteManager.DataBase == null)
                return new Response(false);

            await _sqliteManager.DataBase.InsertAllAsync(companiesDto);

            return new Response(true);
        }

        public async Task<IDataResponse<IEnumerable<CompanyModel>>> ReadAsync()
        {
            if (_sqliteManager.DataBase == null)
                return null;

            List<CompanyModelDto> resultDto;
            IEnumerable<CompanyModel> result;


            resultDto = await _sqliteManager.DataBase.Table<CompanyModelDto>().ToListAsync();
            result = resultDto.ToModel();
            return new DataResponse<IEnumerable<CompanyModel>>(result, true);
        }

        public async Task<IDataResponse<CompanyModel>> ReadAsync(string guid)
        {
            if (_sqliteManager.DataBase == null)
                return null;

            CompanyModelDto resultDto;
            CompanyModel result;

            resultDto = await _sqliteManager.DataBase.Table<CompanyModelDto>().Where(i => i.Guid == guid).FirstOrDefaultAsync();
            result = resultDto.ToModel();
            return new DataResponse<CompanyModel>(result, true);
        }

        public async Task<IDataResponse<IEnumerable<CompanyModel>>> ReadAsync(Expression<Func<CompanyModel, bool>> predicate)
        {
            if (_sqliteManager.DataBase == null)
                return null;

            List<CompanyModelDto> resultDto;
            IEnumerable<CompanyModel> result;
            if (predicate == null)
            {
                resultDto = await _sqliteManager.DataBase.Table<CompanyModelDto>().ToListAsync();
                result = resultDto.ToModel();
                return new DataResponse<IEnumerable<CompanyModel>>(result, true);
            }

            var afterParameter = Expression.Parameter(typeof(CompanyModelDto), predicate.Name);
            var visitor = new ExpressionExtension(predicate, afterParameter); ;
            var newPredicate = Expression.Lambda<Func<CompanyModelDto, bool>>(visitor.Visit(predicate.Body), afterParameter);
          
            resultDto = await _sqliteManager.DataBase.Table<CompanyModelDto>()
                .Where(newPredicate)
                .ToListAsync();

            result = resultDto.ToModel();

            return new DataResponse<IEnumerable<CompanyModel>>(result, true);
        }

        public async Task<IResponse> UpdateAsync(CompanyModel company)
        {
            var companyDto = company.ToDto();

            if (_sqliteManager.DataBase == null)
                return new Response(false);

            await _sqliteManager.DataBase.UpdateAsync(companyDto);

            return new Response(true);
        }

        public async Task<IResponse> UpdateAsync(IEnumerable<CompanyModel> companiesList)
        {
            var companiesDto = companiesList.ToDto();
            if (_sqliteManager.DataBase == null)
                return new Response(false);

            await _sqliteManager.DataBase.UpdateAllAsync(companiesDto);

            return new Response(true);
        }

        public async Task<IResponse> DeleteAsync(CompanyModel company)
        {
            var companyDto = company.ToDto();
            if (_sqliteManager.DataBase == null)
                return new Response(false);

            await _sqliteManager.DataBase.DeleteAsync(companyDto);

            return new Response(true);
        }

        public async Task<IResponse> DeleteAsync(IEnumerable<CompanyModel> companiesList)
        {
            var companiesDto = companiesList.ToDto();

            if (_sqliteManager.DataBase == null)
                return new Response(false);

            await _sqliteManager.DataBase.RunInTransactionAsync((connection) =>
            {
                foreach (var item in companiesDto)
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
                connection.DeleteAll<CompanyModelDto>();
            });

            return new Response(true);
        }
    }
}
