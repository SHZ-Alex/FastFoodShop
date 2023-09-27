using System.ComponentModel.DataAnnotations;

namespace FastFoodShop.Web.Utility;

public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly int _maxFileSize;
    private const int HalfMegaBiteSize = 4194304;
    
    public MaxFileSizeAttribute(int maxFileSize)
    {
        _maxFileSize = maxFileSize;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            if (file.Length > _maxFileSize * HalfMegaBiteSize)
            {
                return new ValidationResult($"Maximum allowed file size is {_maxFileSize} MB.");
            }
        }

        return ValidationResult.Success;
    }

}