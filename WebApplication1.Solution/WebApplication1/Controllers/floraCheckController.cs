using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Api.DTOs;
using System.Drawing;
using System.Web.Helpers;
using System.IO;
using WebApplication1.Api.Errors;
using System.Net.Http;
//using static System.Net.Mime.MediaTypeNames;

namespace WebApplication1.Api.Controllers
{
    public class FloraCheckController : ApiBaseController
    {
       
        private string ConvertImageToBase64(Stream imageStream)
        {
            using (var memoryStream = new MemoryStream())
            {
                imageStream.CopyTo(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }
        private string GetImageType(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    using (var image = Image.FromStream(stream))
                    {
                        // Return the format of the image (e.g., Jpeg, Png)
                        return image.RawFormat.ToString(); // This will return something like "Jpeg", "Png", etc.
                    }
                }
            }
            catch (Exception)
            {
                return null;  // Return null if the image format is invalid or the file can't be processed
            }
        }


        private const string GEMINI_API_URL = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash-exp:generateContent";
        const string GEMINI_API_KEY = "AIzaSyCR85ak-D4tA95eTIW6KoIQ-9jzThxh1CE"; // Replace with your API key
        [HttpPost("predict")]
        public async Task<IActionResult> Predict([FromForm] PredictionRequestDto request)
        {
            if (request.file == null || request.file.Length == 0)
                return BadRequest("No image provided.");

            if (!request.file.ContentType.StartsWith("image/"))
                return BadRequest("Uploaded file is not an image.");

            try
            {
                using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(50) }) // Set 50 seconds timeout
                {
                    client.BaseAddress = new Uri("https://fastflorocheck.onrender.com");

                    using (var content = new MultipartFormDataContent())
                    {
                        // Attach the image file
                        var fileContent = new StreamContent(request.file.OpenReadStream());
                        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(request.file.ContentType);
                        content.Add(fileContent, "file", request.file.FileName);

                        // Forward the request to Flask
                        var response = await client.PostAsync("/predict", content);

                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            string cleanedResult = result.Replace("\"", " ").Replace("\r", "");

                            var apiUrl = $"{GEMINI_API_URL}?key={GEMINI_API_KEY}";

                            var jsonContent = $@"
                    {{
                        ""contents"": [
                            {{
                                ""parts"":[{{""text"": ""summerize the Symptoms,Cause and Treatment of {cleanedResult} in 9 lines ,, dont reply by my question ,, or okay ,, reply by your answer only by HTML formatting "" }}]
                            }}
                        ]
                    }}";

                            var newContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                            using (var chatClient = new HttpClient { Timeout = TimeSpan.FromSeconds(50) }) // Chat client timeout
                            {
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

                                var base64Image = ConvertImageToBase64(request.file.OpenReadStream());

                                var combinedResponse = new
                                {
                                    Image = base64Image,
                                    ImageType = GetImageType(request.file),
                                    DiseaseName = cleanedResult,
                                    ChatbotReply = chatbotReply
                                };

                                return Ok(combinedResponse);
                            }
                        }
                        else
                        {
                            var errorMessage = await response.Content.ReadAsStringAsync();
                            var errorResponse = new
                            {
                                code = "PROCESSING_ERROR",
                                err = "Unable to process the request. Please try again later.",
                                details = "Unclear image. Please upload a clear plant leaf image"
                            };
                            return StatusCode((int)response.StatusCode, errorResponse);
                        }
                    }
                }
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                var errorResponse = new
                {
                  
                    err = "Server error: The request timed out."
                };
                // Return a custom error message for timeouts
                return StatusCode( StatusCodes.Status504GatewayTimeout, errorResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    err = "Server error"
                };
                // General error handling
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
    }
}