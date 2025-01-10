using OpenAI;
using OpenAI.Audio;
using TextToSpeech.Models;
using TextToSpeech.Utils;

namespace TextToSpeech.Services;

public class WhisperTranscriptionService : ITranscriptionService
{
    private readonly OpenAIClient _client;
    private readonly AudioValidator _audioValidator;

    public WhisperTranscriptionService(IConfiguration configuration)
    {
        _client = new OpenAIClient(configuration.GetValue<string>("OPENAI_API_KEY"));
        _audioValidator = new AudioValidator();
    }

    public async Task<TranscriptionResponse> TranscribeAudioAsync(IFormFile? audioFile)
    {
        if (audioFile == null)
            return new TranscriptionResponse
            {
                Success = false, Message = "The file is empty"
            };

        if (!AudioValidator.IsWavAudio(audioFile))
            return new TranscriptionResponse
            {
                Success = false, Message = "The file is not in a proper WAV format"
            };

        var whisperClient = _client.GetAudioClient("whisper-1");
        await using var stream = audioFile.OpenReadStream();
        var options = new AudioTranscriptionOptions()
        {
            ResponseFormat = AudioTranscriptionFormat.Verbose,
        };

        try
        {
            AudioTranscription response = await whisperClient.TranscribeAudioAsync(
                stream,
                audioFile.FileName,
                options
            );
            // var response = new AudioTranscriptionMock();
            if (response.Text == "")
                return new TranscriptionResponse
                {
                    Success = false,
                    Message = "No speech recognised in the audio"
                };

            return new TranscriptionResponse
            {
                Success = true,
                Message = "Transcription done",
                Language = response.Language,
                Transcription = response.Text
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            return new TranscriptionResponse
            {
                Success = false,
                Message = "Whisper Client not working"
            };
        }
    }
}