namespace ShortRefs.Domain.Services
{
    public interface IReferenceEncoder
    {
        string Encode(long id);

        long Decode(string str);
    }
}
