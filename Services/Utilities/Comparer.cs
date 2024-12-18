using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Services.Utilities
{
    public static class Comparer
    {
        public static (bool IsMatched, string Diff, JObject Object) Diff(JToken jToken,
            JToken baseJToken)
        {
            var diff = new JObject();
            if (jToken is null || baseJToken is null)
            {
                return (false, "NoNullAllowedException", diff);
            }

            if (JToken.DeepEquals(jToken, baseJToken)) return (true, string.Empty, diff);

            switch (jToken.Type)
            {
                case JTokenType.Object:
                {
                    var current = jToken as JObject;
                    var target = baseJToken as JObject;
                    var addedKeys = current!.Properties().Select(c => c.Name)
                        .Except(target!.Properties().Select(c => c.Name));
                    var removedKeys = target.Properties().Select(c => c.Name)
                        .Except(current.Properties().Select(c => c.Name));
                    var unchangedKeys = current.Properties()
                        .Where(c => JToken.DeepEquals(c.Value, baseJToken[c.Name]))
                        .Select(c => c.Name);

                    var enumerable = addedKeys.ToList();
                    foreach (var addedKey in addedKeys)
                    {
                        diff[addedKey] = new JObject
                        {
                            ["+"] = jToken[addedKey]
                        };
                    }

                    foreach (var removedKey in removedKeys)
                    {
                        diff[removedKey] = new JObject
                        {
                            ["-"] = baseJToken[removedKey]
                        };
                    }

                    var potentiallyModifiedKeys = current.Properties().Select(c => c.Name)
                        .Except(addedKeys).Except(unchangedKeys);
                    foreach (var k in potentiallyModifiedKeys)
                    {
                        var foundDiff = Diff(current[k], target[k]);
                        if (foundDiff.Object.HasValues) diff[k] = foundDiff.Object;
                    }
                }
                    break;
                case JTokenType.Array:
                {
                    var current = jToken as JArray;
                    var target = baseJToken as JArray;
                    var plus = new JArray(current!.Except(target!, new JTokenEqualityComparer()));
                    var minus = new JArray(target.Except(current, new JTokenEqualityComparer()));
                    if (plus.HasValues) diff["+"] = plus;
                    if (minus.HasValues) diff["-"] = minus;
                }
                    break;
                default:
                    diff["+"] = jToken;
                    diff["-"] = baseJToken;
                    break;
            }

            return (true, JsonConvert.SerializeObject(diff), diff);
        }
    }
}