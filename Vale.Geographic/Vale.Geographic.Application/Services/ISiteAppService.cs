using Vale.Geographic.Application.Dto;
using System.Collections.Generic;
using System;

namespace Vale.Geographic.Application.Services
{
    public interface ISiteAppService
    {
        IEnumerable<SiteAsCountryDto> GetAll(string unitNameFilter);
        SiteAsCountryDto GetById(Guid id);
        SiteAsCountryDto GetByCode(string codeSite);
        string GetSiteIdByCode(string codeSite);
    }
}