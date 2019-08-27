﻿namespace FeatureFlagger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::FeatureFlagger.Domain;

    public static class FeatureFlaggerExtensions
    {
        public static bool IsEnabled(this IFeatureFlagger featureFlagger)
        {
            var behaviours = FeatureFlagger.Behaviours.ToList();
            var flags = GetFlags(featureFlagger);

            return !(from flag in flags
                     let behaviour =
                         behaviours.FirstOrDefault(
                             b =>
                             b.GetType()
                                 .Name.ToUpperInvariant()
                                 .Contains(flag.Name.ToUpperInvariant()))
                     let func = behaviour.Behaviour()
                     where func(flag.Properties) == false
                     select flag).Any();
        }

        private static IEnumerable<Flag> GetFlags(IFeatureFlagger featureFlagger)
        {
            var featureName =
                featureFlagger.GetType().Name
                    .Replace("FeatureFlagger", string.Empty);

            var feature = Read(featureName);

            // add the feature name to each flag as a property
            // (as long as it's not been added already).
            feature.Flags.ForEach(
                f =>
                    {
                        if (!f.Properties.ContainsKey(Constants.Feature))
                        {
                            f.Add(Constants.Feature, feature.Name);
                        }
                    });

            return feature.Flags;
        }

        private static Feature Read(string featureName)
        {
            return
                FeatureFlagger.Features.ToList()
                    .Find(
                        f =>
                        f.Name.Equals(
                            featureName,
                            StringComparison.OrdinalIgnoreCase));
        }
    }
}
