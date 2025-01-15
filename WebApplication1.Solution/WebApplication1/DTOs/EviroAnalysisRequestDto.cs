namespace WebApplication1.Api.DTOs
{
    public class EviroAnalysisRequestDto
    {
        public int Temperature { get; set; }
        public int Humidity { get; set; }
        public int soilMoisture { get; set; }
        public int lightIntensity { get; set; }
        public string plantName { get; set; }
    }
}
