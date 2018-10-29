using MobileDbs.Domain.Models;
using MobileDbs.Infrastructure.SQLite.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileDbs.Infrastructure.SQLite.Mapper
{
    static class MapperProvider
    {
        #region ToModel

        public static CustomerModel ToModel(this CustomerModelDto dto)
        {
            var model = new CustomerModel
            {
                Age = dto.Age,
                Guid = dto.Guid,
                IsActive = dto.IsActive,
                LastVisit = dto.LastVisit,
                Name = dto.Name,
                Salary = dto.Salary,
            };

            return model;
        }

        public static EmployeeModel ToModel(this EmployeeModelDto dto)
        {
            var model = new EmployeeModel
            {
                Age = dto.Age,
                Guid = dto.Guid,
                Name = dto.Name,
                CustomerId = dto.CustomerId,
                Position = dto.Position
            };

            return model;
        }

        public static CompanyModel ToModel(this CompanyModelDto dto)
        {
            var model = new CompanyModel
            {
                Guid = dto.Guid,
                Name = dto.Name,
                Co_FounderName = dto.Co_FounderName,
                FounderName = dto.FounderName
            };

            return model;
        }

        #endregion

        #region ToDto

        public static CustomerModelDto ToDto(this CustomerModel model)
        {
            var dto = new CustomerModelDto
            {
                Age = model.Age,
                Guid = model.Guid,
                IsActive = model.IsActive,
                LastVisit = model.LastVisit,
                Name = model.Name,
                Salary = model.Salary,
            };

            return dto;
        }

        public static EmployeeModelDto ToDto(this EmployeeModel model)
        {
            var dto = new EmployeeModelDto
            {
                Age = model.Age,
                Guid = model.Guid,
                Name = model.Name,
                CustomerId = model.CustomerId,
                Position = model.Position
            };

            return dto;
        }

        public static CompanyModelDto ToDto(this CompanyModel model)
        {
            var dto = new CompanyModelDto
            {
                Guid = model.Guid,
                Name = model.Name,
                Co_FounderName = model.Co_FounderName,
                FounderName = model.FounderName
            };

            return dto;
        }

        #endregion

        #region IEnumerable<ToModel>

        public static IEnumerable<CustomerModelDto> ToDto(this IEnumerable<CustomerModel> models)
        {
            foreach (var model in models)
            {
                yield return model.ToDto();
            }
        }

        public static IEnumerable<EmployeeModelDto> ToDto(this IEnumerable<EmployeeModel> models)
        {
            foreach (var model in models)
            {
                yield return model.ToDto();
            }
        }

        public static IEnumerable<CompanyModelDto> ToDto(this IEnumerable<CompanyModel> models)
        {
            foreach (var model in models)
            {
                yield return model.ToDto();
            }
        }

        #endregion

        #region IEnumerable<ToDto>

        public static IEnumerable<CustomerModel> ToModel(this IEnumerable<CustomerModelDto> models)
        {
            foreach (var model in models)
            {
                yield return model.ToModel();
            }
        }

        public static IEnumerable<EmployeeModel> ToModel(this IEnumerable<EmployeeModelDto> models)
        {
            foreach (var model in models)
            {
                yield return model.ToModel();
            }
        }

        public static IEnumerable<CompanyModel> ToModel(this IEnumerable<CompanyModelDto> models)
        {
            foreach (var model in models)
            {
                yield return model.ToModel();
            }
        }

        #endregion
    }
}
