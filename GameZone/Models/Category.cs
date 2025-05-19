namespace GameZone.Models
{
    public class Category: BaseEntity
    {
        public List<Games> Games { get; set; }
    }
}
