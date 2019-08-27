﻿namespace FeatureFlagger.ConfigurationReaders
{
    using System;
    using System.Collections.Generic;
    using System.Composition;
    using System.Configuration;
    using System.Linq;
    using FeatureFlagger.Domain;

    [Export(typeof(IConfigurationReader)),
        ExportMetadata(Constants.Reader, Constants.Config)]
    public class ConfigReader : IConfigurationReader
    {
        // NOTE: this method is the same for StoreReader.
        public Feature Read(string featureName, IEnumerable<Feature> features)
        {
            return
                features.ToList()
                    .Find(
                        f =>
                        f.Name.Equals(
                            featureName,
                            StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Feature> ReadAll()
        {
            return (List<Feature>)ConfigurationManager.GetSection("features");
        }
    }
}
