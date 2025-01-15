using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Api.DTOs;
using WebApplication1.Api.Errors;
using WebApplication1.Core.Repositries;

namespace WebApplication1.Api.Controllers
{
    public class EviroAnalysisController : ApiBaseController
    {

        public EviroAnalysisController(IGenericRepositry<object> repo)
        {
            this.repo = repo;
        }
        private const string GEMINI_API_URL = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent";
        const string GEMINI_API_KEY = "AIzaSyCR85ak-D4tA95eTIW6KoIQ-9jzThxh1CE"; // Replace with your API key
        private readonly IGenericRepositry<object> repo;

        [HttpPost("/analysisBot")]
        public async Task<IActionResult> eviroAnalysis([FromBody] EviroAnalysisRequestDto request)
        {
            try
            {
                if (request.plantName == null || !(request.plantName is string))
                {
                    return BadRequest(new ApiErrorResponse(500, "You must add a valid plant name"));
                }
                var apiUrl = $"{GEMINI_API_URL}?key={GEMINI_API_KEY}";


                var jsonContent = $@"
                        {{
                            ""contents"": [
                                {{
                                    ""parts"":[{{""text"": ""give me an ideal percentage of each 1.temprature {request.Temperature},2.Humidity ,,{request.Humidity} , 3.soilMoisture {request.soilMoisture} ,4.light intensity{request.lightIntensity} ,, for a healthy soil to produce a healthy {request.plantName} give a response in short answer on each one,if user input is not a plant name give him short message = {request.plantName} is not a valid plant name "" }}]
                                }}
                            ]
                        }}";

                var newContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                using (var chatClient = new HttpClient())
                {
                    // Include the API key in the URL, not in Authorization header
                    var chatbotResponse = await chatClient.PostAsync(apiUrl, newContent);

                    if (!chatbotResponse.IsSuccessStatusCode)
                    {
                        var chatbotError = await chatbotResponse.Content.ReadAsStringAsync();
                        return StatusCode((int)chatbotResponse.StatusCode, $"Chatbot API Error: {chatbotError}");
                    }

                    var chatbotResult = await chatbotResponse.Content.ReadAsStringAsync();
                    var chatbotReply = JsonDocument.Parse(chatbotResult)
                        .RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();
                    if (chatbotReply.Contains("is not a valid plant name") || chatbotReply.Contains("Not found"))
                        return BadRequest(new ApiErrorResponse(400, $"{request.plantName} is not valid plant name "));



                    var temperatureSentence = Regex.Match(chatbotReply, @"\*\*Temperature:\*\* (.+)");
                    var humiditySentence = Regex.Match(chatbotReply, @"\*\*Humidity:\*\* (.+)");
                    var soilMoistureSentence = Regex.Match(chatbotReply, @"\*\*Soil Moisture:\*\* (.+)");
                    var lightIntensitySentence = Regex.Match(chatbotReply, @"\*\*Light Intensity:\*\* (.+)");
                    string Temperature = temperatureSentence.Success ? temperatureSentence.Groups[1].Value : "Not found";
                    string Humidity = humiditySentence.Success ? humiditySentence.Groups[1].Value : "Not found";
                    string soilMoisture = soilMoistureSentence.Success ? soilMoistureSentence.Groups[1].Value : "Not found";
                    string lightIntensity = lightIntensitySentence.Success ? lightIntensitySentence.Groups[1].Value : "Not found";

                    var response = new
                    {
                        Temperature,
                        Humidity,
                        soilMoisture,
                        lightIntensity
                    };

                    return Ok(response);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);

                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }

        }



        [HttpGet("")]
        public async Task<IActionResult> getEviroAnalysisData()
        {
            var data = await repo.getEviroAnalysisDataAsync();
            if (data == null)
                return BadRequest(new ApiErrorResponse(400, "An error occured"));
            return Ok(data);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> getAnalysisById(int id)
        {
            var data = await repo.getAnalysisByPlantIdAsync(id);
            if (data == null)
                return BadRequest(new ApiErrorResponse(401, "Data Not Found"));
            return Ok(data);
        }
    }
}
