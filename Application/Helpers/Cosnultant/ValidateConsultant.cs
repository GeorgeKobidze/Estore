using Domain.Infrastructure.DTO.Common;
using Domain.Infrastructure.Interface;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Application.Helpers.Cosnultant
{
    public class ValidateConsultant
    {
        private readonly Consultant _consultant;
        private readonly IUnitOfWork<Consultant> _unitOfWork;

        public ValidateConsultant(Consultant consultant, IUnitOfWork<Consultant> unitOfWork)
        {
            _consultant = consultant;
            _unitOfWork = unitOfWork;
        }


        public ServiceResponse<string> Validate()
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();


            //Dob Validation

            if ((DateTime.Now.Year - _consultant.DateOfBirth.Year) < 18)
            {
                serviceResponse.Succes = false;
                serviceResponse.Message.Add($"Person is under 18");
            }

            //FirstName and LastName Validation

            if (_consultant.FirstName.Length > 0 && _consultant.LastName.Length > 0)
            {
                if (!Regex.IsMatch(_consultant.FirstName, @"^[a-zA-Z]+$") && !Regex.IsMatch(_consultant.FirstName, @"^[ა-ჰ]+$"))
                {
                    serviceResponse.Succes = false;
                    serviceResponse.Message.Add($"First Name is invalid ");
                }

                if (!Regex.IsMatch(_consultant.LastName, @"^[a-zA-Z]+$") && !Regex.IsMatch(_consultant.LastName, @"^[ა-ჰ]+$"))
                {
                    serviceResponse.Succes = false;
                    serviceResponse.Message.Add($"Last Name is invalid");
                }
            }
            else
            {
                serviceResponse.Succes = false;
                serviceResponse.Message.Add($"First Name or Last Name is empty");
            }



            // Identification Validation
            if (_consultant.Identification.Length == 0 || _consultant.Identification.Length != 11)
            {
                serviceResponse.Succes = false;
                serviceResponse.Message.Add($"Person  Identification is empty or lenght is less than 11");
            }
            else
            {
                if (_unitOfWork.Repository.Where(x => x.Identification == _consultant.Identification && x.Uid != _consultant.Uid).Any())
                {
                    serviceResponse.Succes = false;
                    serviceResponse.Message.Add($"Person with Identification - {_consultant.Identification} is already Registred");
                }

                else if (Regex.Matches(_consultant.Identification, @"[0-9]").Count != 11)
                {
                    serviceResponse.Succes = false;
                    serviceResponse.Message.Add($"Person Identification Conatain letter(s)");
                }
            }



            // Gender Validation
            if (!(_consultant.Gender.ToUpper() == "male".ToUpper() || _consultant.Gender.ToUpper() == "female".ToUpper() ))
                
            {
                serviceResponse.Succes = false;
                serviceResponse.Message.Add($"Gender {_consultant.Gender} is invalid");
            }

            if (_consultant.ConsultantUid != null) 
            {
                if (!_unitOfWork.Repository.Where(e => e.Uid == _consultant.ConsultantUid).Any())
                {
                    serviceResponse.Succes = false;
                    serviceResponse.Message.Add($"ConsultantUid {_consultant.ConsultantUid} is invalid");
                }
            }


            return serviceResponse;

        }
    }
}
