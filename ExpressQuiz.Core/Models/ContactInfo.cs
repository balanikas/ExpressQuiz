﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressQuiz.Core.Models
{
    public class ContactInfo : Entity
    {
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime Created { get; set; }
    }
}