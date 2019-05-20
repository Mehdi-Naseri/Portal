using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace Portal.DomainClasses.Models
{
    public class Upload
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        [Display(Name = "Uploaded By")]
        public string UploadedBy { get; set; }
        [Required]
        [StringLength(255)]
        [Display(Name = "File Name")]
        public string FileName { get; set; }
        [Required]
        [StringLength(255)]
        [Display(Name = "Random Name")]
        public string RandomName { get; set; }
        [Required]
        [Display(Name = "Upload DateTime")]
        public System.DateTime UploadDateTime { get; set; }
        [Required]
        [Display(Name = "Content Length")]
        public int ContentLength { get; set; }
        [Required]
        [StringLength(25)]
        [Display(Name = "Content Type")]
        public string ContentType { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
