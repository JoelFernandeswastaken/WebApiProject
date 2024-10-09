using Mapster;
using Organization.Application.Common.DTO.Request;
using Organization.Application.CompanyModule.Commands.DeleteCompany;
using Organization.Application.CompanyModule.Commands.UpdateCompany;
using Organization.Application.EmployeeModule.Commands.AddEmployee;
using Organization.Application.EmployeeModule.Commands.UpdateEmployee;
using Organization.Application.EmployeeModule.Queries.GetEmployees;
using Organization.Application.UserModule.Commands.RegisterUser;
using Organization.Application.UserModule.Queries.GetUserByEmail;
using Organization.Application.UserModule.Queries.LoginUser;
using Organization.Domain.Employee;
using System.Web.Helpers;

namespace Organization.Presentation.Api.Common.Mappings
{
    public class GlobalMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig typeAdapterConfig) 
        {
            typeAdapterConfig.NewConfig<(string id, EmployeeRequest employeeRequest), UpdateEmployeeCommand>()
               .Map(dest => dest.id, src => src.id)
               .Map(dest => dest, src => src.employeeRequest)
               .Map(dest => dest.CompanyID, src => src.employeeRequest.CompanyID);

            typeAdapterConfig.NewConfig<(string id, CompanyRequest companyRequest), UpdateCompanyCommand>()
                .Map(dest => dest.Id, src => src.id)
                .Map(dest => dest, src => src.companyRequest);

            typeAdapterConfig.NewConfig<(string id, bool deleteAssociation), DeleteCompanyCommand>()
                .Map(dest => dest.id, src => src.id)
                .Map(dest => dest.deleteAssociations, src => src.deleteAssociation);

            typeAdapterConfig.NewConfig<RegisterUserRequest, RegisterUserCommand>()
                .Map(dest => dest.username, src => src.Username)
                .Map(dest => dest.password, src => src.Password)
                .Map(dest => dest.email, src => src.Email);

            typeAdapterConfig.NewConfig<GetUserByEmailRequest, GetUserByEmailQuery>()
                .Map(dest => dest.email, src => src.Email);

            typeAdapterConfig.NewConfig<LoginUserRequest, LoginUserQuery>()
                .Map(dest => dest.email, src => src.Email)
                .Map(dest => dest.password, src => src.Password);
        }
    }
}
