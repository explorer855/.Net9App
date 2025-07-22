namespace Domain.Entities
{
    public abstract class Entity
    {
        protected Entity()
        { }

        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
