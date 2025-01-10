namespace TextToSpeech.Models;

public class TranscriptionResponse
{
    public required bool Success { get; set; }
    public required string Message { get; set; }
    public string? Language { get; set; }
    public string? Transcription { get; set; }
}