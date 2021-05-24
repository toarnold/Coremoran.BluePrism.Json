using System;
using System.Data;

namespace Coremoran.BluePrism.Json
{
    public static class BluePrismCollectionExtension
    {
        public static bool IsSingleRow(this DataTable table) => Convert.ToBoolean(table.ExtendedProperties[SpecialValues.SingleRowPropertyName] ?? false);
        public static void SetSingleRow(this DataTable table, bool singleRow) => table.ExtendedProperties[SpecialValues.SingleRowPropertyName] = singleRow;
    }
}
