
namespace Ejercicio.Models.Api
{
    using System;

    public class UserModel
    {
        public Guid Uid { get; set; }
        public string Name { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}
