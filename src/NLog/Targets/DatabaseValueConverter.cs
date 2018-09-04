using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace NLog.Targets
{
    class DatabaseValueConverter : IDatabaseValueConverter
    {
        /// <inheritdoc />
        public DatabaseValueConverter()
        {
            
        }

        /// <summary>
        /// convert layout value to parameter value
        /// </summary>
        public object ConvertFromString(string value, DbType dbType, string parseFormat)
        {
            switch (dbType)
            {
                case DbType.String:
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength: //todo truncate?
                    return value;
                case DbType.Boolean:
                    return bool.Parse(value);
                case DbType.Decimal:
                case DbType.Currency:
                case DbType.VarNumeric:
                    return decimal.Parse(value);
                case DbType.Double:
                    return double.Parse(value);
                case DbType.Single:
                    return float.Parse(value);
                case DbType.DateTime:
                case DbType.DateTime2:
                case DbType.Date:
                case DbType.Time:
                    var dateFormat = string.IsNullOrEmpty(parseFormat) ? "yyyy/MM/dd HH:mm:ss.fff" : parseFormat;
                    return DateTime.ParseExact(value, dateFormat, null);
                case DbType.DateTimeOffset:
                    var dateOffsetFormat = string.IsNullOrEmpty(parseFormat) ? "yyyy/MM/dd HH:mm:ss.fff zzz" : parseFormat;
                    return DateTimeOffset.ParseExact(value, dateOffsetFormat, null);
                case DbType.Guid:
#if NET3_5
                    return new Guid(value);
#else
                    return string.IsNullOrEmpty(parseFormat) ? Guid.Parse(value) : Guid.ParseExact(value, parseFormat);
#endif
                case DbType.Byte:
                    return byte.Parse(value);
                case DbType.SByte:
                    return sbyte.Parse(value);
                case DbType.Int16:
                    return short.Parse(value);
                case DbType.Int32:
                    return int.Parse(value);
                case DbType.Int64:
                    return long.Parse(value);
                case DbType.UInt16:
                    return ushort.Parse(value);
                case DbType.UInt32:
                    return uint.Parse(value);
                case DbType.UInt64:
                    return ulong.Parse(value);
                case DbType.Binary:
                    return ConvertToBinary(value);
                default:
                    return value;
            }
        }

        /// <inheritdoc />
        public object ConvertFromObject(object rawValue, DbType dbType, string format)
        {
            // todo
            return rawValue;
        }

        /// <summary>
        /// convert layout value to parameter value
        /// </summary>
        private static object ConvertToBinary(string value)
        {
            var byteCount = value.Length / 2;
            var buffer = new byte[byteCount];
            for (int i = 0; i < byteCount; i++)
            {
                buffer[i] = byte.Parse(value.Substring(i * 2, 2), NumberStyles.AllowHexSpecifier);
            }
            return buffer;
        }
    }
}
