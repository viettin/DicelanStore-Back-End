namespace dicelanStore.Models.dicelanApp
{
    public class ErrorResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}
