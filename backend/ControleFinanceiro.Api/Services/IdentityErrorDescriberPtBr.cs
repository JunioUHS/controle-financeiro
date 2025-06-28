using Microsoft.AspNetCore.Identity;

namespace ControleFinanceiro.Api.Services
{
    public class IdentityErrorDescriberPtBr : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
            => new IdentityError { Code = nameof(DuplicateUserName), Description = $"O usu�rio '{userName}' j� est� em uso." };

        public override IdentityError DuplicateEmail(string email)
            => new IdentityError { Code = nameof(DuplicateEmail), Description = $"O e-mail '{email}' j� est� em uso." };

        public override IdentityError PasswordTooShort(int length)
            => new IdentityError { Code = nameof(PasswordTooShort), Description = $"A senha deve ter pelo menos {length} caracteres." };

        public override IdentityError PasswordRequiresNonAlphanumeric()
            => new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "A senha deve conter ao menos um caractere especial." };

        public override IdentityError PasswordRequiresDigit()
            => new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "A senha deve conter ao menos um d�gito ('0'-'9')." };

        public override IdentityError PasswordRequiresLower()
            => new IdentityError { Code = nameof(PasswordRequiresLower), Description = "A senha deve conter ao menos uma letra min�scula ('a'-'z')." };

        public override IdentityError PasswordRequiresUpper()
            => new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "A senha deve conter ao menos uma letra mai�scula ('A'-'Z')." };
    }
}