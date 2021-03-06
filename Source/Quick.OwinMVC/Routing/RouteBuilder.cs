﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Quick.OwinMVC.Routing
{
    public static class RouteBuilder
    {
        private static readonly Regex paramRegex = new Regex(@":(?<name>[A-Za-z0-9_\.]*)", RegexOptions.Compiled);

        public static Regex RouteToRegex(string route)
        {
            var parts = route.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).AsEnumerable();

            parts = parts.Select(part => !paramRegex.IsMatch(part) ?
                part :
                string.Join("",
                    paramRegex.Matches(part)
                        .Cast<Match>()
                        .Where(match => match.Success)
                        .Select(match => string.Format(
                            "(?<{0}>.+?)",
                            match.Groups["name"].Value.Replace(".", @"\.")
                            )
                        )
                    )
                );

            return new Regex("^/" + string.Join("/", parts) + "$", RegexOptions.Compiled);
        }
    }
}
