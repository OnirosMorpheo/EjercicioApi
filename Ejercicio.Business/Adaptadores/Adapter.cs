
namespace Ejercicio.Business.Adaptadores
{
    using Autofac;
    using AutoMapper;
    using System;
    using System.Collections.Generic;

    public abstract class Adapter<DtoGet, Model> : IAdapter<DtoGet, Model> where DtoGet : class, new() where Model : class, new()
    {
        protected MapperConfiguration configMap;
        protected IMapper mapper;
        protected readonly ILifetimeScope lifeTimeScope;
        protected abstract Action<IMapperConfigurationExpression> configExpresion { get; }

        public Adapter(ILifetimeScope lifeTimeScope)
        {
            this.lifeTimeScope = lifeTimeScope;
            this.configMap = new MapperConfiguration(configExpresion);
            this.mapper = this.configMap.CreateMapper();
        }

        public DtoGet ToDto(Model obj)
        {
            return mapper.Map<DtoGet>(obj);
        }

        public Model ToModel(DtoGet obj)
        {
            return mapper.Map<Model>(obj);
        }

        public IEnumerable<DtoGet> ToIEnumerableDto(IEnumerable<Model> lista)
        {
            return mapper.Map<IEnumerable<DtoGet>>(lista);
        }

        public IEnumerable<Model> ToIEnumerableModel(IEnumerable<DtoGet> lista)
        {
            return mapper.Map<IEnumerable<Model>>(lista);
        }

    }
}
