// 
// Copyright (c) 2004-2018 Jaroslaw Kowalski <jaak@jkowalski.net>, Kim Christensen, Julian Verdurmen
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions 
// are met:
// 
// * Redistributions of source code must retain the above copyright notice, 
//   this list of conditions and the following disclaimer. 
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution. 
// 
// * Neither the name of Jaroslaw Kowalski nor the names of its 
//   contributors may be used to endorse or promote products derived from this
//   software without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF 
// THE POSSIBILITY OF SUCH DAMAGE.
// 

#if !SILVERLIGHT && !__IOS__ && !__ANDROID__

using System;
using System.Data;
using System.Globalization;

namespace NLog.Targets
{
    /// <summary>
    /// Convert values for the database target
    /// </summary>
    internal class DatabaseValueConverter : IDatabaseValueConverter
    {
        /// <inheritdoc />
        public object ConvertFromString(string value, DbType dbType, DatabaseParameterInfo parameterInfo)
        {

            if (value == null)
            {
                return null;
            }

            var format = parameterInfo.Format;

            switch (dbType)
            {
                case DbType.String:
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength: //todo truncate?
                    return value;
            }

            var trimmedValue = value.Trim();


            switch (dbType)
            {
                case DbType.Boolean:
                    return bool.Parse(trimmedValue);
                case DbType.Decimal:
                case DbType.Currency:
                case DbType.VarNumeric:
                    return decimal.Parse(trimmedValue);
                case DbType.Double:
                    return double.Parse(trimmedValue);
                case DbType.Single:
                    return float.Parse(trimmedValue);
                case DbType.Time:
                    return TimeSpan.Parse(trimmedValue);
                case DbType.DateTime:
                case DbType.DateTime2:
                case DbType.Date:
                    if (string.IsNullOrEmpty(format))
                        return DateTime.Parse(trimmedValue, CultureInfo.InvariantCulture);
                    else
                        return DateTime.ParseExact(trimmedValue, format, null);
                case DbType.DateTimeOffset:
                    if (string.IsNullOrEmpty(format))
                        return DateTimeOffset.Parse(trimmedValue, CultureInfo.InvariantCulture);
                    else
                        return DateTimeOffset.ParseExact(trimmedValue, format, null);
                case DbType.Guid:
#if NET3_5
                    return new Guid(trimmedValue);
#else
                    return string.IsNullOrEmpty(format) ? Guid.Parse(trimmedValue) : Guid.ParseExact(trimmedValue, format);
#endif
                case DbType.Byte:
                    return byte.Parse(trimmedValue);
                case DbType.SByte:
                    return sbyte.Parse(trimmedValue);
                case DbType.Int16:
                    return short.Parse(trimmedValue);
                case DbType.Int32:
                    return int.Parse(trimmedValue);
                case DbType.Int64:
                    return long.Parse(trimmedValue);
                case DbType.UInt16:
                    return ushort.Parse(trimmedValue);
                case DbType.UInt32:
                    return uint.Parse(trimmedValue);
                case DbType.UInt64:
                    return ulong.Parse(trimmedValue);
                case DbType.Binary:
                    return ConvertToBinary(trimmedValue);
                default:
                    return value;
            }
        }

        /// <inheritdoc />
        public object ConvertFromObject(object rawValue, DbType dbType, DatabaseParameterInfo parameterInfo)
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
#endif