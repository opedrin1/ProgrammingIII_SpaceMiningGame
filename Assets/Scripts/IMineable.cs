
public interface IMineable
{
    float ValuePerUnit { get; }
    
    bool IsDepleted { get; }
    
    float Mine(float amount);
}