using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Emails
{
    public class EmailModel
    {
        [Required]
        public List<string> ToAddresses { get; set; }

        public List<string> CCs { get; set; }

        public List<string> BCCs { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public string Subject { get; set; }

        public EmailAttachment Attachment { get; set; }

        public EmailModel()
        {
            ToAddresses = new List<string>();
            CCs = new List<string>();
            BCCs = new List<string>();
        }
    }
    public class EmailAttachment
    {
        public EmailAttachment(string fileName, byte[] file)
        {
            FileName = fileName;
            File = file;
        }

        public EmailAttachment()
        {

        }

        public string FileType { get; set; }
        public string FileName { get; set; }
        public byte[] File { get; set; }
    }
}
