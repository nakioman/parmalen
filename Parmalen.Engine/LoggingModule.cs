using System.Linq;
using System.Reflection;
using Autofac.Core;
using log4net;

namespace Parmalen.Engine
{
    public class LoggingModule : Autofac.Module
    {
        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Preparing += OnComponentPreparing;
            registration.Activated += (sender, e) => InjectLoggerProperties(e.Instance);
        }

        private static void OnComponentPreparing(object sender, PreparingEventArgs e)
        {
            var t = e.Component.Target.Activator.LimitType;
            e.Parameters = e.Parameters.Union(
                new[]
                {
                new ResolvedParameter((p, i) => IsLogger(p), (p, i) => GetLogger(t.FullName))
                });
        }

        private static void InjectLoggerProperties(object instance)
        {
            var instanceType = instance.GetType();

            var properties = instanceType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => IsLogger(p) && p.CanWrite && p.GetIndexParameters().Length == 0);

            foreach (var propToSet in properties)
            {
                propToSet.SetValue(instance, GetLogger(instanceType.FullName), null);
            }
        }

        private static ILog GetLogger(string name)
        {
            return LogManager.GetLogger(name);
        }

        private static bool IsLogger(ParameterInfo p)
        {
            return p.ParameterType == typeof(ILog);
        }

        private static bool IsLogger(PropertyInfo p)
        {
            return p.PropertyType == typeof(ILog);
        }
    }
}