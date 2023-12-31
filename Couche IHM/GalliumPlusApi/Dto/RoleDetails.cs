using Modeles;

namespace GalliumPlusApi.Dto
{
    public class RoleDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public uint Permissions { get; set; }

        public RoleDetails()
        {
            this.Id = -1;
            this.Name = String.Empty;
            this.Permissions = 0;
        }

        public class Mapper : Mapper<Role, RoleDetails>
        {
            public override Role ToModel(RoleDetails dto)
            {
                return new Role(dto.Id, dto.Name);
            }
        }
    }
}