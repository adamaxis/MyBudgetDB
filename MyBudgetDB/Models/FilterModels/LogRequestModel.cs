using System;
using System.ComponentModel.DataAnnotations;

namespace MyBudgetDB.Models.FilterModels
{
    public class LogRequestModel
    {
        public Guid RequestId { get; set; }
        [StringLength(128)]
        public string Request { get; set; }
        [StringLength(128)]
        public string Response { get; set; }
    }
}
