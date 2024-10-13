using FluentValidation;
using Project01.Database;
using Project01.Entities;
using Project01.Helpers;

namespace Project01.Validations {
    public class UserValidation : AbstractValidator<User> {
        private readonly MysqlContext context;
        public UserValidation(MysqlContext context) {
            this.context = context;

            RuleFor(x => x.Email)
                .NotNull()
                .WithMessage(x => string.Format(Constant.MESSAGE_NOT_NULL, x.Email))
                .NotEmpty()
                .WithMessage(x => string.Format(Constant.MESSAGE_NOT_EMPTY, x.Email));

            RuleFor(x => x.Password)
                .NotNull()
                .WithMessage(x => string.Format(Constant.MESSAGE_NOT_NULL, x.Password))
                .NotEmpty()
                .WithMessage(x => string.Format(Constant.MESSAGE_NOT_EMPTY, x.Password));
        }
    }
}