using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiForTest.Models
{
    public class Skill
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [Range(1,10, ErrorMessage = "Level must be in range 1-10")]
        public byte Level { get; set; }
    }
}