using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Subject
    {

        public long Id { get; set; }
        public long CourseId { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
    }
}