using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Course
    {

        public Course()
        {
            Students = new List<Student>();
        }

        public long Id { get; set; }

        [StringLength(150)]
        public string Name { get; set; }

        public virtual List<Student> Students { get; set; }


    }
}