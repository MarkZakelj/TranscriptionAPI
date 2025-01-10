namespace TextToSpeech.Utils;

public class AudioValidator
{
    public static bool IsWavAudio(IFormFile file)
    {
        try
        {
            using var stream = file.OpenReadStream();
            using var reader = new BinaryReader(stream);
            string riffId = new(reader.ReadChars(4));
            if (riffId != "RIFF")
                return false;

            reader.ReadUInt32();

            string waveId = new(reader.ReadChars(4));
            if (waveId != "WAVE")
                return false;

            string fmtId = new(reader.ReadChars(4));
            return fmtId == "fmt ";
        }
        catch (Exception)
        {
            return false;
        }
    }
}