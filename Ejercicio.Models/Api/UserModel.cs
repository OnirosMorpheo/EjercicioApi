
namespace Ejercicio.Models.Api
{
    using Newtonsoft.Json;
    using System;

    public class UserModel : IModel<Guid>
    {
        [JsonIgnore]
        public Guid Id { get {
                return this.Uid;
            }
            set {
                this.Uid = value;
            } 
        }
        public Guid Uid { get; set; }
        public string Name { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}
