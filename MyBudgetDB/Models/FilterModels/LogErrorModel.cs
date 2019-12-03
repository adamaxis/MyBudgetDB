using System;
using System.ComponentModel.DataAnnotations;

namespace MyBudgetDB.Models.FilterModels
{
    public class LogErrorModel
    {
        public int Id { get; set; }
        public int StatusCode { get; set; }
        public DateTime TimeOfError { get; set; }
        [StringLength(128)]
        public string RequestId { get; set; }
        [StringLength(128)]
        public string ExceptionMessage { get; set; }
        [StringLength(4096)]
        public string StackTrace { get; set; }
    }
}
