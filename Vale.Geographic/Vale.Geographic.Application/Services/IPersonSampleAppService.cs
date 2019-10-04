using System.Collections.Generic;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Services;

namespace Vale.Geographic.Application.Services
{
    public interface IPersonSampleAppService
    {
        IPersonSampleService personSampleService { get; set; }

        void Delete(int id);
        PersonSampleDto GetById(int id);
        PersonSampleDto Insert(PersonSampleDto obj);
        PersonSampleDto Update(int id, PersonSampleDto obj);
        IEnumerable<PersonSampleDto> Get(bool? active, IFilterParameters request, out int total);
        IEnumerable<PersonSampleDto> GetAll(IFilterParameters parameters, out int total);
    }
}