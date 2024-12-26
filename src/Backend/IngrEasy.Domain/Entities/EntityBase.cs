namespace IngrEasy.Domain;

public class EntityBase
{
    public bool Active { get; set; } = true;
    
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    
    public int Id { get; set; }

}