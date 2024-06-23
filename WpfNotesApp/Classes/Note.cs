namespace WpfNotesApp.Classes
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsReady { get; set; }
        public DateTime LastModified { get; set; }
    }
}
