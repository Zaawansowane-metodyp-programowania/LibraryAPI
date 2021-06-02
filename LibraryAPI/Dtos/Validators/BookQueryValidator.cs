using System;
using System.Linq;
using FluentValidation;
using LibraryAPI.Models;

namespace LibraryAPI.Dtos.Validators
{
    public class BookQueryValidator : AbstractValidator<BookQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15 };
        private string[] allowedSortByColumnName =
            { nameof(Book.BookName),nameof(Book.BookDescription),nameof(Book.Category),nameof(Book.PublisherName),nameof(Book.PublishDate)};

        public BookQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must in [{string.Join(",", allowedPageSizes)}]");
                }

            });

            RuleFor(r => r.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnName.Contains(value))
                .WithMessage($"Sort by is optional, or must be in [{string.Join(",", allowedSortByColumnName)}]");
        }
    }
}
