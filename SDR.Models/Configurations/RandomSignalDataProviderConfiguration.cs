namespace SDR.Models.Configurations;

public class RandomSignalDataProviderConfiguration
{
    public byte FrequencyMin { get; set; }
    public byte FrequencyMax { get; set; }
    public sbyte StrengthMin { get; set; }
    public sbyte StrengthMax { get; set; }
    public int Count { get; set; }
    public int Frequency { get; set; }
}