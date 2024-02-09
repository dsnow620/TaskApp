namespace TaskApp.Models
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string AssignedTo { get; set; }
        public DateTime DueDt { get; set; }
    }
}
