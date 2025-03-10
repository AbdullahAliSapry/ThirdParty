using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class CommissionScheme
    {

        public int Id { get; set; }

        public UserType UserType { get; set; }
        public double LowerLimit { get; set; }

        public double UpperLimit { get; set; }

        public double CommissionRate { get; set; }

        public DateTime StartDate { get; set; }  

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        public ValidationResult ValidateLimits()
        {
            if (LowerLimit >= UpperLimit)
            {
                return new 
                    ValidationResult("LowerLimit should be less than UpperLimit.");
            }
            return ValidationResult.Success;
        }

    }



    public enum UserType
    {
        Normal,
        Markter,
    }




}
