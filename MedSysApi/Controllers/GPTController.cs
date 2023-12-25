using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

            return Ok(answer);
        }

    }
}
