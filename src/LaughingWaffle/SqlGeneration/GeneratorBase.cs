﻿using LaughingWaffle.PropertyLoaders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LaughingWaffle.SqlGeneration
{
    /// <summary>
    /// Base class for Sql Generators to inherit from (at the moment this is SQL Server only, but who knows where it might go)
    /// </summary>
    /// <typeparam name="TType">The type to reflect upon and generate corresponding SQL statements.</typeparam>>
    public abstract class GeneratorBase<TType> : ISqlGenerator<TType>
    {
        private readonly Type tType;
        private readonly TypeInfo tTypeInfo;
        private readonly IPropertyLoader _propertyFilter;

        /// <summary>
        /// Expose the table name of the underlying [TableAttribute] on the class.
        /// </summary>
        public string TableName(bool tempTable)
        {
            if (string.IsNullOrEmpty(_tableName))
            {
                var tableAttribute = tTypeInfo.GetCustomAttribute<TableAttribute>();
                _tableName = (tempTable ? "#" : "") + (tableAttribute != null ? tableAttribute.Name : tTypeInfo.Name);
            }
            return _tableName;
        }
        private string _tableName;

        /// <summary>
        /// Base Class Constructor; load up the requisite paramaters. 
        /// </summary>
        public GeneratorBase() : this(new PropertyLoader()) { }

        /// <summary>
        /// Base Class Constructor; load up the requisite paramaters. 
        /// </summary>
        /// <param name="propertyFilter">Inject an instance of a custom IPropertyLoader. Useful if you need to filter the list of properties on your types.</param>
        public GeneratorBase(IPropertyLoader propertyFilter) 
        {
            tType = typeof(TType);
            tTypeInfo = tType.GetTypeInfo();

            _propertyFilter = propertyFilter;
        }

        /// <summary>
        /// Given a class type, generate the Create Sql Statement for it.
        /// </summary>
        /// <remarks></remarks>
        /// <returns>The DDL statements for the table based on the input paramaters.</returns>
        public virtual string CreateTable()
        {
            return CreateTable(true);
        }

        /// <summary>
        /// Given a class type, generate the Create Sql Statement for it.
        /// See also: <seealso cref="ISqlGenerator.CreateTable{TType}(bool)"/>
        /// </summary>
        /// <param name="tempTable">Boolean indicating if this table should be a #TempTable or a real table.</param>
        /// <returns>The DDL statements for the table based on the input paramaters.</returns>
        public virtual string CreateTable(bool tempTable)
        {
            var builder = new StringBuilder();

            var allProperties = _propertyFilter.GetProperties(tType);

            builder.AppendLine($@"CREATE TABLE {TableName(tempTable)} (");

            foreach (var prop in allProperties)
            {
                var name = prop.Name;
                var csharpType = GetCoreType(prop.PropertyType).Name;
                var tsqlType = CsharpTypeToSqlType[csharpType];
                var nulle = Nullable.GetUnderlyingType(prop.PropertyType) != null ? "NULL" : "NOT NULL";
                if (prop == allProperties.Last())
                {
                    builder.AppendLine($"[{name}] [{tsqlType}]{(tsqlType == "nvarchar" ? "(max)" : "")} {nulle}");
                }
                else
                {
                    builder.AppendLine($"[{name}] [{tsqlType}]{(tsqlType == "nvarchar" ? "(max)" : "")} {nulle},");
                }
            }

            builder.Append(")");

            return builder.ToString();
        }

        /// <summary>
        /// Utilizing the injected <see cref="IPropertyLoader" /> enumerate the matching properties and return their names.
        /// </summary>
        /// <returns>
        /// Gets the names of all properties utilizing the provided <see cref="IPropertyLoader"/> instance.
        /// </returns>
        public IEnumerable<string> GetProperties() => _propertyFilter.GetProperties(tType).Select(p => p.Name);

        /// <summary>
        /// A dictionary to convert C#/.NET and Sql Types.
        /// </summary>
        /// <returns>A mapping dictionary between C# types and their Sql Counterparts</returns>
        protected abstract Dictionary<string, string> CsharpTypeToSqlType { get; }

        private static Type GetCoreType(Type type)
        {
            if (type.GetTypeInfo().IsGenericType &&
                type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return Nullable.GetUnderlyingType(type);
            else
                return type;
        }

        /// <summary>
        /// Performs the generation of the merge statement based on the given options.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public abstract string Merge(IUpsertOptions<TType> options);
    }
}