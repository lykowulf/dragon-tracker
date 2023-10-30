using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TestTracker2020.Extensions;

namespace TestTracker2020.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }

        [Display(Name = "Select Image")]
        [NotMapped]
        [DataType(DataType.Upload)]
        [MaxFileSize(2 * 1024 * 1024)]
        [AllowedExtension(new string[] { ".jpg", ".png", ".doc", ".docx", ".xls", ".xlsx", ".pdf", ".txt"})]
        public IFormFile FormFile { get; set; }

        public string FileName { get; set; }
        
        public byte[] FileData { get; set; }


        public string ContextType { get; set; }



        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
        public string UserId { get; set; }


        public virtual BTUser User { get; set; }
    }
}
