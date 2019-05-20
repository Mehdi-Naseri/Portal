using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace Portal.DomainClasses.Models
{
    public class WeblogPost
    {
        public WeblogPost()
        {
            this.WeblogComments = new HashSet<WeblogComment>();
        }

        [Key]
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter post DateTime.")]
        [Display(Name = "DateTime")]
        public System.DateTime PostDateTime { get; set; }
        [Required(ErrorMessage = "Please enter writer name.")]
        public string Writer { get; set; }
        [Required(ErrorMessage = "Please enter post title.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter post content.")]
        [Display(Name = "Post Content")]
        public string PostContent { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }

        public virtual ICollection<WeblogComment> WeblogComments { get; set; }

    }
}
