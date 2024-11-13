namespace DashboardAPI.Models
{
    public class CMSKey
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<CMSKeyValue> CMSKeyValues { get; set; } = new List<CMSKeyValue>();
    }
}
