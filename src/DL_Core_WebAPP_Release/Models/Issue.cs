namespace DL_Core_WebAPP_Release.Models
{
    public class Issue
    {
        public int IssueID { get; set; }
        public string IssuedOn { get; set; }
        public string DueOn { get; set; }

        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        public string UserId { get; set; }

    }
}
