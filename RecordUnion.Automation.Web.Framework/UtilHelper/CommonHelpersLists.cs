using System;
using System.Collections.Generic;

namespace RecordUnion.Automation.Web.Framework.UtilHelper
{
    public static class CommonHelpersLists
    {
        public static IList<String> MergeLists(this IList<String> initial, IList<String> ToAdd)
        {
            foreach(String str in ToAdd)
            {
                initial.Add(str);
            }

            return initial;
        }
    }
}
