interface IManager
{
    public void Clear();
}

public interface IClassHasChain
{
    public void Clear();
}
public interface IPuzzleInteraction
{
    public void Interactive();
    public void ElementHit(ElementType type);
}
public interface IPuzzleBox
{
    public bool pushable { get; }
}