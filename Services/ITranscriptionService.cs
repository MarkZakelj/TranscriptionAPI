using TextToSpeech.Models;

namespace TextToSpeech.Services;

public interface ITranscriptionService
{
    Task<TranscriptionResponse> TranscribeAudioAsync(IFormFile audioFile);
}