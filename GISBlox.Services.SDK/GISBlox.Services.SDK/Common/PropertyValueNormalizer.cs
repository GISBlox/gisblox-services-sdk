// ------------------------------------------------------------
// Copyright (c) Bartels Online. All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;

namespace GISBlox.Services.SDK.Common
{
   /// <summary>
   /// Normalizes property dictionaries by converting wrapper objects that contain a Value and ValueType
   /// into concrete CLR types. Only intended for use with Conversion API results (ToWkt/ToWkb).
   /// </summary>
   internal static class PropertyValueNormalizer
   {
      /// <summary>
      /// Normalize properties for a list of WKT results.
      /// </summary>
      public static void NormalizeWktList(List<WKT> wkts)
      {
         if (wkts == null) return;
         foreach (var w in wkts)
         {
            NormalizePropertyBags(w?.Properties);
         }
      }

      /// <summary>
      /// Normalize properties for a list of WKB results.
      /// </summary>
      public static void NormalizeWkbList(List<WKB> wkbs)
      {
         if (wkbs == null) return;
         foreach (var w in wkbs)
         {
            NormalizePropertyBags(w?.Properties);
         }
      }

      private static void NormalizePropertyBags(List<Dictionary<string, object>> bags)
      {
         if (bags == null) return;
         foreach (var bag in bags)
         {
            NormalizeDictionary(bag);
         }
      }

      private static void NormalizeDictionary(Dictionary<string, object> dict)
      {
         if (dict == null) return;
         var keys = new List<string>(dict.Keys);
         foreach (var key in keys)
         {
            dict[key] = NormalizeValue(dict[key]);
         }
      }

      private static object NormalizeValue(object value)
      {
         // Unwrap JsonElement to CLR values if present
         value = UnwrapJsonElement(value);

         // Handle wrapper objects with Value/ValueType contract
         if (value is Dictionary<string, object> nested)
         {
            // Try to find keys (case-insensitive): Value and ValueType
            string valueKey = null;
            string typeKey = null;
            foreach (var k in nested.Keys)
            {
               if (valueKey == null && string.Equals(k, "Value", StringComparison.OrdinalIgnoreCase)) valueKey = k;
               if (typeKey == null && string.Equals(k, "ValueType", StringComparison.OrdinalIgnoreCase)) typeKey = k;
            }

            if (valueKey != null && typeKey != null)
            {
               var rawType = nested[typeKey]?.ToString();
               var rawValue = UnwrapJsonElement(nested[valueKey]);
               return ConvertByValueType(rawValue, rawType);
            }

            // Otherwise, recursively normalize nested dictionaries
            NormalizeDictionary(nested);
            return nested;
         }

         // Normalize lists/arrays
         if (value is List<object> list)
         {
            for (int i = 0; i < list.Count; i++)
            {
               list[i] = NormalizeValue(list[i]);
            }
            return list;
         }

         return value;
      }

      private static object UnwrapJsonElement(object value)
      {
         if (value is JsonElement je)
         {
            switch (je.ValueKind)
            {
               case JsonValueKind.Null:
               case JsonValueKind.Undefined:
                  return null;
               case JsonValueKind.True:
                  return true;
               case JsonValueKind.False:
                  return false;
               case JsonValueKind.Number:
                  if (je.TryGetInt64(out long l)) return l;
                  if (je.TryGetDecimal(out decimal m)) return m;
                  return je.GetDouble();
               case JsonValueKind.String:
                  if (je.TryGetDateTime(out var dt)) return dt;
                  return je.GetString();
               case JsonValueKind.Object:
                  {
                     var dict = new Dictionary<string, object>(StringComparer.Ordinal);
                     foreach (var prop in je.EnumerateObject())
                     {
                        dict[prop.Name] = UnwrapJsonElement(prop.Value);
                     }
                     return dict;
                  }
               case JsonValueKind.Array:
                  {
                     var list = new List<object>();
                     foreach (var item in je.EnumerateArray())
                     {
                        list.Add(UnwrapJsonElement(item));
                     }
                     return list;
                  }
            }
         }
         return value;
      }

      private static object ConvertByValueType(object value, string valueType)
      {
         if (string.IsNullOrWhiteSpace(valueType)) return value;
         var t = valueType.Trim();

         // Normalize type token
         t = t.Replace("System.", string.Empty, StringComparison.OrdinalIgnoreCase);

         switch (t.ToLowerInvariant())
         {
            case "string":
               return value?.ToString();
            case "int":
            case "int32":
               return value == null ? 0 : Convert.ToInt32(value, CultureInfo.InvariantCulture);
            case "long":
            case "int64":
               return value == null ? 0L : Convert.ToInt64(value, CultureInfo.InvariantCulture);
            case "double":
               return value == null ? 0d : Convert.ToDouble(value, CultureInfo.InvariantCulture);
            case "decimal":
               return value == null ? 0m : Convert.ToDecimal(value, CultureInfo.InvariantCulture);
            case "float":
            case "single":
               return value == null ? 0f : Convert.ToSingle(value, CultureInfo.InvariantCulture);
            case "bool":
            case "boolean":
               return value != null && Convert.ToBoolean(value, CultureInfo.InvariantCulture);
            case "datetime":
            case "date":
               if (value is DateTime dt) return dt;
               if (value is long ticks) return new DateTime(ticks);
               if (DateTime.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var parsed))
                  return parsed;
               return value;
            case "guid":
               if (value is Guid g) return g;
               if (Guid.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), out var guid))
                  return guid;
               return value;
            case "byte[]":
            case "binary":
            case "bytes":
               if (value is byte[] b) return b;
               var s = Convert.ToString(value, CultureInfo.InvariantCulture);
               try { return Convert.FromBase64String(s); } catch { return value; }
            case "null":
               return null;
            default:
               return value;
         }
      }
   }
}
