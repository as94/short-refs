namespace ShortRefs.Domain.Models.References
{
    public interface IReferenceEncoder
    {
        string Encode(int id);

        int Decode(string str);
    }
}
