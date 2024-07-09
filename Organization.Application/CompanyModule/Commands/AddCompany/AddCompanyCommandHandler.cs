using MediatR;
using Organization.Application.Common.DTO.Request;
using Organization.Application.Common.Exceptions;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Domain.Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.CompanyModule.Commands.AddCompany
{
    public class AddCompanyCommandHandler : IRequestHandler<AddCompanyCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISender _sender;
        public AddCompanyCommandHandler(IUnitOfWork unitOfWork, ISender sender)
        {
            _sender = sender;
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(AddCompanyCommand companyRequest, CancellationToken cancellationToken)
        {
            bool isCompanyNameExists = await _unitOfWork.Companies.IsExistingAsync(companyRequest.Name);
            if (isCompanyNameExists)
                throw new DuplicateException($"Company with name {companyRequest.Name} alredy exists.");

            string guid = Guid.NewGuid().ToString().Replace("/", "_").Replace("+", "-").Substring(0, 22);

            _unitOfWork.BeginTransaction();
            var id = await _unitOfWork.Companies.AddAsync(new Company()
            {
                Id = guid,
                Name = companyRequest.Name,
                Address = companyRequest.Address,
                Country = companyRequest.Country
            }); //Alter stored procedure to return id of company added
            _unitOfWork.CommitAndCloseConnection();

            return id;
        }
    }
}
