using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LaughingWaffle.PropertyLoaders
{
    /// <summary>
    /// Define the contract of the property loader
    /// </summary>
    public interface IPropertyLoader
    {
        /// <summary>
        /// Determine and filter (as appropriate) the list of properties for an instance of <see cref="ISqlGenerator"/>.
        /// </summary>
        /// <param name="tType"></param>
        /// <returns></returns>
        IEnumerable<PropertyInfo> GetProperties(Type tType);
    }
}