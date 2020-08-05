﻿using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Framework.Configuration
{
    public class DefaultTypeConverter : ITypeConverter
    {
        public virtual bool Support(Type type)
        {
            TypeCode typeCode = Type.GetTypeCode(type);

            switch (typeCode)
            {
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.DateTime:
                case TypeCode.String:
                case TypeCode.Decimal:
                    return true;
                default:
                {
                    if (type == typeof(Version))
                        return true;
                    if (type == typeof(Color))
                        return true;
                    if (type == typeof(Vector2))
                        return true;
                    if (type == typeof(Vector3))
                        return true;
                    if (type == typeof(Vector4))
                        return true;
                    if (type == typeof(Rect))
                        return true;
                    return false;
                }
            }
        }

        public virtual object Convert(Type type, object value)
        {
            TypeCode typeCode = Type.GetTypeCode(type);

            switch (typeCode)
            {
                case TypeCode.Boolean:
                    if (value is string str)
                    {
                        string v = str.Trim().ToLower();
                        if (v.Equals("yes") || v.Equals("true"))
                            return true;
                        if (v.Equals("no") || v.Equals("false"))
                            return false;
                        throw new FormatException();
                    }
                    else
                    {
                        return System.Convert.ChangeType(value, type);
                    }
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.DateTime:
                case TypeCode.String:
                case TypeCode.Decimal:
                    return System.Convert.ChangeType(value, type);
                default:
                {
                    if (type == typeof(Version))
                    {
                        if (value is Version version)
                            return version;

                        if (!(value is string))
                            throw new FormatException(
                                $"This value \"{value}\" cannot be converted to the type \"{type.Name}\"");

                        try
                        {
                            return new Version((string) value);
                        }
                        catch (Exception e)
                        {
                            throw new FormatException(
                                $"This value \"{value}\" cannot be converted to the type \"{type.Name}\"", e);
                        }
                    }

                    if (type == typeof(Color))
                    {
                        {
                            if (value is Color color)
                                return color;
                        }
                        if (!(value is string))
                            throw new FormatException(
                                $"This value \"{value}\" cannot be converted to the type \"{type.Name}\"");

                        try
                        {
                            if (ColorUtility.TryParseHtmlString((string) value, out var color))
                                return color;
                        }
                        catch (Exception e)
                        {
                            throw new FormatException(
                                $"This value \"{value}\" cannot be converted to the type \"{type.Name}\"", e);
                        }
                    }
                    else if (type == typeof(Vector2))
                    {
                        if (value is Vector2 vector2)
                            return vector2;

                        if (!(value is string))
                            throw new FormatException(
                                $"This value \"{value}\" cannot be converted to the type \"{type.Name}\"");

                        try
                        {
                            var val = Regex.Replace(((string) value).Trim(), @"(^\()|(\)$)", "");
                            string[] s = val.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                            if (s.Length == 2)
                                return new Vector2(float.Parse(s[0]), float.Parse(s[1]));
                        }
                        catch (Exception e)
                        {
                            throw new FormatException(
                                $"This value \"{value}\" cannot be converted to the type \"{type.Name}\"", e);
                        }
                    }
                    else if (type == typeof(Vector3))
                    {
                        if (value is Vector3 vector3)
                            return vector3;

                        if (!(value is string))
                            throw new FormatException(
                                $"This value \"{value}\" cannot be converted to the type \"{type.Name}\"");

                        try
                        {
                            var val = Regex.Replace(((string) value).Trim(), @"(^\()|(\)$)", "");
                            string[] s = val.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                            if (s.Length == 3)
                                return new Vector3(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]));
                        }
                        catch (Exception e)
                        {
                            throw new FormatException(
                                $"This value \"{value}\" cannot be converted to the type \"{type.Name}\"", e);
                        }
                    }
                    else if (type == typeof(Vector4))
                    {
                        if (value is Vector4 vector4)
                            return vector4;

                        if (!(value is string))
                            throw new FormatException(
                                $"This value \"{value}\" cannot be converted to the type \"{type.Name}\"");

                        try
                        {
                            var val = Regex.Replace(((string) value).Trim(), @"(^\()|(\)$)", "");
                            string[] s = val.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
                            if (s.Length == 4)
                                return new Vector4(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]),
                                    float.Parse(s[3]));
                        }
                        catch (Exception e)
                        {
                            throw new FormatException(
                                $"This value \"{value}\" cannot be converted to the type \"{type.Name}\"", e);
                        }
                    }
                    else if (type == typeof(Rect))
                    {
                        if (value is Rect rect)
                            return rect;

                        if (!(value is string))
                            throw new FormatException(
                                $"This value \"{value}\" cannot be converted to the type \"{type.Name}\"");

                        try
                        {
                            var val = Regex.Replace(((string) value).Trim(), @"(^\()|(\)$)", "");
                            string[] s = val.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                            if (s.Length == 4)
                                return new Rect(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]),
                                    float.Parse(s[3]));
                        }
                        catch (Exception e)
                        {
                            throw new FormatException(
                                $"This value \"{value}\" cannot be converted to the type \"{type.Name}\"", e);
                        }
                    }

                    throw new FormatException(
                        $"This value \"{value}\" cannot be converted to the type \"{type.Name}\"");
                }
            }
        }
    }
}
