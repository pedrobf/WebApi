using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Student
    {
        public long Id { get; set; }
        public long CourseId { get; set; }

        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(150), EmailAddress]
        public string Email { get; set; }

        [StringLength(1)]
        [Required]
        public string Gender { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid Date")]
        [Display(Name = "Birthday")]
        public DateTime Birthday { get; set; }
    }
}