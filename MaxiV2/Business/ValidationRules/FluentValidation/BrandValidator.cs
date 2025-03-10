﻿using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation {
    public class BrandValidator : AbstractValidator<Brand> {
        public BrandValidator() {
            RuleFor(b => b.Name).NotEmpty();
            RuleFor(b => b.Name).Length(2, 40);
        }
    }
}
