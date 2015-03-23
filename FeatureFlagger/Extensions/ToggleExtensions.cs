﻿namespace FeatureFlagger
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class ToggleExtensions
    {
        public static bool IsEnabled(this IToggle toggle, IConfigurationReader reader)
        {
            // use type name to look up feature flags.
            var feature = reader.Read(toggle.GetType().Name);

            // we're going to create an instance of a behaviour type in the assembly.
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName().Name;

            // instantiate a behaviour that matches each flag then test it with the flag's properties.
            // TODO: exception handling.
            return !(from flag in feature.Flags
                        let type = assembly.GetType(string.Format("{0}.{1}Behaviour", assemblyName, flag.Name))
                        let behaviour = (IBehaviour)Activator.CreateInstance(type)
                        let func = behaviour.Behaviour()
                        where func(flag.Properties) == false
                        select flag).Any();
        }
    }
}