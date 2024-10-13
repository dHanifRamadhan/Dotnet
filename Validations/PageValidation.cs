using FluentValidation;
using Project01.Database;
using Project01.Entities;
using Project01.Helpers;

namespace Project01.Validations {
    public class PageValidation : AbstractValidator<Page> {
        private readonly MysqlContext context;

        public PageValidation(MysqlContext context) {
            this.context = context;

            RuleFor(x => x.PageName)
                .NotNull()
                .WithMessage(x => string.Format(Constant.MESSAGE_NOT_NULL, x.PageName))
                .NotEmpty()
                .WithMessage(x => string.Format(Constant.MESSAGE_NOT_EMPTY, x.PageName))
                .Must(UniqueName)
                .WithMessage(x => string.Format(Constant.MESSAGE_UNIQUE_VALUE, x.PageName))
                .Matches(@"\A\S{3,15}\z")
                .WithMessage(x => string.Format(Constant.MESSAGE_NOT_WHITE_SPACE, x.PageName));
        }

        private bool UniqueName(Page item, string name) {
            var pages = context.Pages.SingleOrDefault(x => x.PageName.Equals(name));
            if (pages == null)
                return true;
            
            return pages.PageCode.Equals(item.PageCode);
        }
    }
}