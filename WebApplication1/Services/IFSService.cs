using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data.DTOs;

namespace WebApplication1.API.Services
{
    public interface IFSService
    {
        Task<IEnumerable<SampleContent>> GetSampleContents();
        Task<string> SaveRecord(SampleContent newRecord);
    }
}
