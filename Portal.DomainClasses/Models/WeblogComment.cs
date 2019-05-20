using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace Portal.DomainClasses.Models
{
    public class WeblogComment
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter post DateTime.")]
        [Display(Name = "Comment DateTime")]
        public System.DateTime CommentDateTime { get; set; }
        [Required(ErrorMessage = "Please enter your name.")]
        public string Writer { get; set; }
        [Required(ErrorMessage = "Please write your comment.")]
        public string Comment { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
        [Required]
        public int WeblogPostId { get; set; }
        public virtual WeblogPost WeblogPost { get; set; }

    }
}
