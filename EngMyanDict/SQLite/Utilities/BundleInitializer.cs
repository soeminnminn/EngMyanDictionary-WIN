﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;
using SQLitePCL;
using System.Runtime.InteropServices;

namespace Microsoft.Data.SQLite.Utilities
{
    internal static class BundleInitializer
    {
        public static void Initialize()
        {
            /*Assembly assembly;
            try
            {
                assembly = Assembly.Load(
                    new AssemblyName(
                        "SQLitePCLRaw.batteries_v2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8226ea5df37bcae9"));
            }
            catch
            {
                return;
            }

            assembly.GetType("SQLitePCL.Batteries_V2").GetTypeInfo().GetDeclaredMethod("Init")
                .Invoke(null, null);*/

            
            Settings.BaseDirectoryForDynamicLoadNativeLibrary = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            raw.SetProvider(new SQLite3Provider_PInvoke());
        }
    }
}
