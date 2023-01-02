namespace FlipsideWeShare.Models;
public class BountyReference
{
    public Guid Id { get; private set; }

    public BountyReference(Guid id)
    {
        Id = id;
    }
}
