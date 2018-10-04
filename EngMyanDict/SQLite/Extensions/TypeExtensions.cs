// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Reflection;

namespace System
{
    internal static class TypeExtensions
    {
        public static Type UnwrapEnumType(this Type type)
        {
            return type.GetTypeInfo().IsEnum ? Enum.GetUnderlyingType(type) : type;
        }

        public static Type UnwrapNullableType(this Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }
    }
}
