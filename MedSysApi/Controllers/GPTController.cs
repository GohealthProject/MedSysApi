using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenAI_API;
using OpenAI_API.Completions;

namespace MedSysApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GPTController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> GetAIBasedResult(string SearchText)
        {
            string APIKey = "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";
            string answer = string.Empty;

            var openai = new OpenAIAPI(APIKey);
            CompletionRequest req = new CompletionRequest()
            {
                Prompt = SearchText,
                Model = OpenAI_API.Models.Model.DavinciText,
                MaxTokens = 200
            };

            var result = await openai.Completions.CreateCompletionAsync(req);

            foreach (var choice in result.Completions)
            {
                answer = choice.Text;
            }

            var json = JsonConvert.SerializeObject(answer);

            return Ok(json);
        }

        [HttpPost("tttt")]
        public async Task<string> testt(string SearchText)
        {
            string aaa = "abbcc";
            return aaa;
        }
    }
}
