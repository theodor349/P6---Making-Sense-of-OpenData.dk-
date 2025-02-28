﻿using Microsoft.Extensions.DependencyInjection;
using Printers.GeoJson;
using Printers.OutputLog;
using Shared.ComponentInterfaces;

namespace Printers
{
    public static class ConfigureServices
    {
        public static void AddPrinters(this IServiceCollection services)
        {
            services.AddTransient<IPrinter, Printer>();
            services.AddTransient<IGeoJsonPrinter, GeoJsonPrinter>();
            services.AddTransient<IFilePrinter, FilePrinter>();
            services.AddTransient<IOutputLogPrinter, OutputLogPrinter>();
        }
    }
}
