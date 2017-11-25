using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace LaughingWaffle.SqlGeneration
{
    public class PropertyLoader : IPropertyLoader
    {
        /// <summary>
        /// Default Property Load/Filter. Pulls all properties that have .CanWrite.
        /// </summary>
        /// <param name="tType">The type to scan properties of and apply the filter to.</param>
        /// <returns>An IEnumerable of Property Info objects from the original type.</returns>
        public IEnumerable<PropertyInfo> GetProperties(Type tType) => tType.GetProperties()
                    .Where(q => q.CanWrite) // write props
                  //.Where(x => !x.GetAccessors()[0].IsVirtual) // non-virtual props
                    ;
    }
}