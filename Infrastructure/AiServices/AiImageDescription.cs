
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.AiServices
{
    public class AiImageDescription : IAiImageDescription
    {
        private readonly ConfigAi _configs;

        public AiImageDescription(IOptions<ConfigAi> options)
        {
            _configs = options.Value;
        }

        public async Task<string> GetImageDescriptionAsync(string imageUrl)
        {   


            

            return "test";


        }
    
    
    }

    public class ConfigAi
    {
        public string Key { get; set; } = null!;
    }
}
