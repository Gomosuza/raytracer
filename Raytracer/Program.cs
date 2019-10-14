﻿using Microsoft.Extensions.Configuration;
using Microsoft.Xna.Framework.Input;
using Raytracer.Configuration;
using System;
using System.IO;
using System.Linq;

namespace Raytracer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var configuration = LoadConfiguration(args);

            using var g = new RaytracerGame(configuration);
            g.Run();
        }

        private static Settings LoadConfiguration(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(new FileInfo(typeof(Program).Assembly.Location).DirectoryName)
                .AddIniFile("raytracer.ini", true)
                .AddCommandLine(args);

            var config = builder.Build();

            var settings = new Settings();
            config.Bind(settings);
            // haven't found a way to add a converter into the builder yet. need to manually call it
            foreach (var section in config.GetSection("keybindings").GetChildren())
            {
                var key = section.Key;
                var value = section.Value;

                var keyValues = (value.Contains('|') ? value.Split('|') : new[] { value }).Select(k => Enum.Parse<Keys>(k, true)).ToArray();
                settings.Keybindings.ActionKeyMap[key] = keyValues;
            }
            return settings;
        }
    }
}
