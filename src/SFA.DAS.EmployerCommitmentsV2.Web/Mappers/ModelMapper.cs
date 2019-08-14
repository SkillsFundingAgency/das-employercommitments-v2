using System;
using SFA.DAS.Commitments.Shared.Interfaces;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers
{
    public class ModelMapper : IModelMapper
    {
        private readonly IServiceProvider _serviceProvider;

        public ModelMapper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T Map<T>(object source) where T : class
        {
            Type[] typeArgs = { source.GetType(), typeof(T) };
            var mapperType = typeof(IMapper<,>).MakeGenericType(typeArgs);
            var mapper = _serviceProvider.GetService(mapperType);

            if (mapper == null)
            {
                throw new ArgumentException("Unable to find mapper");
            }
            
            var mapMethod = mapper.GetType().GetMethod("Map");
            var result = mapMethod.Invoke(mapper, new[] { source });

            return result as T;
        }
    }

    public interface IModelMapper
    {
        T Map<T>(object source) where T : class;
    }

}
