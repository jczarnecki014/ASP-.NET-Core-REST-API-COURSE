using FluentValidation;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;

namespace RestaurantAPI.Models.Validators
{
    public class RestaurantQueryValidator: AbstractValidator<RestaurantQuery>
    {
        public int[] allowedPageSizes = new[] {5,10,15};
        public string[] allowedSortByColumnsNames = new[]{nameof(RestaurantDTO.name),nameof(RestaurantDTO.City),nameof(RestaurantDTO.description)};
        public RestaurantQueryValidator()
        {
            RuleFor(r=>r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r=>r.PageSize).Custom((value,context) => {
                if(!allowedPageSizes.Contains(value))
                { 
                    context.AddFailure("PageSize",$"PageSize must in [{String.Join(",",allowedPageSizes)}]");
                }
            });
            RuleFor(r=>r.SortBy)
            .Must(value=> !String.IsNullOrEmpty(value) || allowedSortByColumnsNames.Contains(value))
            .WithMessage($"Property SortBy must be in [{String.Join(",",allowedSortByColumnsNames)}]  ");
        }

    }
}
