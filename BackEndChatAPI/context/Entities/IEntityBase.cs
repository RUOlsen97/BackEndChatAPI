namespace BackEndChatAPI.context.Entities
{
    public interface IEntityBase
    {
        DateTime CreatedOn { get; set; }
        DateTime ModifiedOn { get; set; }
    }
}
