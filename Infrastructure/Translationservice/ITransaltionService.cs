using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Translationservice
{
    public interface ITransaltionService
    {


        Task<string> TranslateTextAsync(string text,string targetlang="ar");
    }
}
