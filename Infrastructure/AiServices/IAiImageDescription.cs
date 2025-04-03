using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AiServices
{
    public interface IAiImageDescription
    {


        Task<string> GetImageDescriptionAsync(string imageUrl);
    }
}
