
namespace Ejercicio.Persistencia.Metadata
{
    using System;

    public class IdentityAttribute : Attribute
    {
        public bool Identity { get; }


        public IdentityAttribute(bool isIdentity) {
            this.Identity = isIdentity;
        }
    }
}
