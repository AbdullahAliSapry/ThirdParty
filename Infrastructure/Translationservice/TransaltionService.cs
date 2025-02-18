using DeepL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Translationservice
{
    public class TransaltionService : ITransaltionService
    {
        private readonly Translator _translator;

        public TransaltionService(IConfiguration configuration)
        {
            var apiKey = configuration["TransaltionService:Key"];
            if (string.IsNullOrEmpty(apiKey))
                throw new Exception("Translation key is missing in configuration");

            _translator = new Translator(apiKey);
        }

        public async Task<string> TranslateTextAsync(string text, string targetLang = "ar")
        {
            try
            {
                var result = await _translator.TranslateTextAsync(text, null, targetLang);
                Console.WriteLine("Translation Success");
                return result.Text;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in translation: {ex.Message}");
            }
        }
    }
}
