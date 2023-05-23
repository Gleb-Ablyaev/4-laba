using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class CalculatorModel
    {
        [Required(ErrorMessage = "FirstNumber является обязательным.")]
        public int FstNumber { get; set; }
        public int FstNumber2 { get; set; }

        [StringLengthWrapper(5,  1, ErrorMessage = "Длина числа должна быть от 1 до 5 цифр.")]
        public int SndNumber { get; set; }

        public string Operation { get; set; }
        public float Result { get; set; }
    }

    public class StringLengthWrapperAttribute : ValidationAttribute
    {
        private readonly int _maxLength;
        private readonly int _minLength;

        public StringLengthWrapperAttribute(int maxLength)
        {
            _maxLength = maxLength;
        }

        public StringLengthWrapperAttribute(int maxLength, int minLength) : this(maxLength)
        {
            _minLength = minLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string stringValue = value.ToString();
                if (stringValue.Length < _minLength || stringValue.Length > _maxLength)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}