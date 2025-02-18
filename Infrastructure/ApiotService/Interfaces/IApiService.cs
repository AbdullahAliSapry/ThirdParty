using Contracts.SharedDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ApiotService.Interfaces
{
    public interface IApiService<T> where T : class
    {

        Task<ApiResponse<T>> GetDataAsync(string endpoint, Dictionary<string, string> parameters = null);

        Task<dynamic> GetDataAsyncDynmic(string endpoint, Dictionary<string, string> parameters = null);


        string BuildUrl(string endpoint,Dictionary<string,string> parameters);
    }
}

