namespace DashboardAPI.Models
{
    public class CMSKeyValue
    {
        public int ID { get; set; }
        public int KeyID { get; set; }
        public int LangID { get; set; }
        public string Value { get; set; } = string.Empty;

        public virtual CMSKey CMSKey { get; set; } = null!;
        public virtual LanguageMaster Language { get; set; } = null!;
    }
}
