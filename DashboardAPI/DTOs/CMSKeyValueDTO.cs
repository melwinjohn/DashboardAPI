namespace DashboardAPI.DTOs
{
    public class CMSKeyValueDTO
    {
        public int ID { get; set; }
        public int KeyID { get; set; }
        public int LangID { get; set; }
        public string Value { get; set; } = string.Empty;
        public string KeyName { get; set; } = string.Empty;
        public string LanguageName { get; set; } = string.Empty;
    }
}
