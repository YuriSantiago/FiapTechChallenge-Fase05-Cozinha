using Core.Requests.Update;
using FluentValidation;

namespace Core.Validators
{
    public class PedidoUpdateStatusRequestValidator : AbstractValidator<PedidoUpdateStatusRequest>
    {

        public PedidoUpdateStatusRequestValidator()
        {
            RuleFor(x => x.PedidoId)
                 .NotEmpty().WithMessage("O ID do pedido é obrigatório.")
                 .GreaterThan(0).WithMessage("O ID do pedido deve ser maior que zero.");

            RuleFor(x => x.Status)
                 .NotEmpty().WithMessage("O status do pedido não pode estar vazio.")
                 .MaximumLength(50).WithMessage("O status deve ter no máximo 50 caracteres");

        }

    }
}
