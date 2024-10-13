using FluentValidation;
using Project01.Database;
using Project01.Entities;
using Project01.Helpers;

namespace Project01.Validations {
    public class RoleValidation : AbstractValidator<Role> {
        private readonly MysqlContext context;
        public RoleValidation(MysqlContext context) {
            this.context = context;

            RuleFor(x => x.RoleName)
                .NotNull()
                .WithMessage(x => string.Format(Constant.MESSAGE_NOT_NULL, x.RoleName))
                .NotEmpty()
                .WithMessage(x => string.Format(Constant.MESSAGE_NOT_EMPTY, x.RoleName));
        }
    }
}