using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace Portal.DomainClasses.Models
{
    public class WebsiteVisitor
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public System.DateTime Date { get; set; }
        [Required]
        [Display(Name = "Host Address")]
        public string HostAddress { get; set; }
        [Required]
        [Display(Name = "Host Name")]
        public string HostName { get; set; }
        [Required]
        public string Browser { get; set; }
        [Required]
        public string Url { get; set; }
        [Display(Name = "Url Referrer")]
        public string UrlReferrer { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
