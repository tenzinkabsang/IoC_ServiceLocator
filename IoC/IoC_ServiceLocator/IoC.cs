using System;
using System.Linq;
using System.Reflection;

using Ninject;
using Ninject.Parameters;

namespace IoC_ServiceLocator
{
    public class IoC
    {
        private static readonly IKernel NinjectKernel;
        private const string KEY = "metadata_Key";

        // Tells the C# compiler not to mark type as beforefieldinit
        static IoC()
        {
            NinjectKernel = new StandardKernel(new MyModule(KEY));
        }

        public static T Resolve<T>(params object[] values)
        {
            // If no parameters then simply call Get.
            if (values == null || !values.Any())
                return NinjectKernel.Get<T>();

            // If we got this far, it means that the class we're
            // trying to initialize requires ctr arguments.
            // Get the registered type from the binding using the metadata info
            Type bindedType = NinjectKernel.GetBindings(typeof(T))
                                           .Select(b => b.Metadata.Get<Type>(KEY))
                                           .FirstOrDefault();

            if(bindedType == null)
                throw new ArgumentException(string.Format("{0} is not registered", typeof(T).FullName));

            ConstructorInfo[] allConstructors = bindedType.GetConstructors();

            // Find the first constructor matching the number of arguments and types passed in.
            ConstructorInfo matchedConstructor = 
                allConstructors.FirstOrDefault(c => IsMatch(c.GetParameters(), values));

            if (matchedConstructor == null)
                return default(T);

            IParameter[] arguments = GetConstructorArguments(matchedConstructor, values);

            return NinjectKernel.Get<T>(arguments);
        }

        private static ConstructorArgument[] GetConstructorArguments(ConstructorInfo ctr, params object[] values)
        {
            return ctr.GetParameters()
                      .Zip(values, (ctrParam, value) => new ConstructorArgument(ctrParam.Name, value))
                      .ToArray();
        }


        private static bool IsMatch(ParameterInfo[] parameterTypes, params object[] values)
        {
            // If the # of ctr parameters does not match the # of args - return false.
            if (parameterTypes.Length != values.Length)
                return false;

            // Ensure that param type matches argument type
            for (int i = 0; i < parameterTypes.Length; i++)
            {
                if (parameterTypes[i].ParameterType != values[i].GetType())
                    return false;
            }
            return true;
        }
    }
}