using GalliumPlusApi.CompatibilityHelpers;
using GalliumPlusApi.ModelDecorators;
using Microsoft.VisualBasic;
using Modeles;
using System.ComponentModel.DataAnnotations;

namespace GalliumPlusApi.Dto
{
    public class UserSummary
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Role { get; set; } = -1;
        public string Year { get; set; } = string.Empty;
        public decimal? Deposit { get; set; } = null;
        public bool IsMember { get; set; } = false;

        public class AccountMapper : Mapper<Account, UserSummary>
        {
            public override UserSummary FromModel(Account model)
            {
                return new UserSummary
                {
                    Deposit = Format.FloatToMonetary(model.Argent),
                    Email = model.Mail,
                    FirstName = model.Prenom,
                    Id = model.Identifiant,
                    IsMember = model.IsMember,
                    LastName = model.Nom,
                    Role = model.RoleId,
                    Year = model.Formation,
                };
            }

            public override Account ToModel(UserSummary dto)
            {
                return new Account(
                    id: UserIdMapper.Current.GetIdFor(dto.Id),
                    identifiant: dto.Id,
                    nom: dto.LastName,
                    prenom: dto.FirstName,
                    argent: (float)(dto.Deposit ?? -1m),
                    year: dto.Year,
                    isMember: dto.IsMember,
                    mail: dto.Email,
                    role: dto.Role
                );
            }
        }
    }
}