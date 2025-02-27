namespace SDR.Models.Configurations;

public class RandomSignalDataProviderConfiguration
{
    public float FrequencyMin { get; set; }
    public float FrequencyMax { get; set; }
    public float StrengthMin { get; set; }
    public float StrengthMax { get; set; }
    public int Count { get; set; }
    public int Frequency { get; set; }
}