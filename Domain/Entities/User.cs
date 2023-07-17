﻿using PaelystSolution.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaelystSolution.Domain.Entities
{
    [Table("SolutionUser")]
    public class User : AuditableEntity
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }    = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public List<Document> Documents { get; set; }
    }
}
