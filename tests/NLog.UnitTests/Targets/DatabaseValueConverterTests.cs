using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NLog.Layouts;
using NLog.Targets;
using Xunit;

namespace NLog.UnitTests.Targets
{
    public class DatabaseValueConverterTests
    {
        [Theory]
        [MemberData(nameof(ConvertFromStringTestCases))]
        public void ConvertFromString(string value, DbType dbType, object expected)
        {
            // Arrange
            var databaseValueConverter = new DatabaseValueConverter();
            var databaseParameterInfo = new DatabaseParameterInfo("@test", "test layout");

            // Act
            var result = databaseValueConverter.ConvertFromString(value, dbType, databaseParameterInfo);

            // Assert
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> ConvertFromStringTestCases()
        {
            yield return new object[] { "3", DbType.Int16, (short)3 };
            yield return new object[] { "3", DbType.Int32, 3 };
            yield return new object[] { "3", DbType.Int64, (long)3 };
            yield return new object[] { "3", DbType.UInt16, (ushort)3 };
            yield return new object[] { "3", DbType.UInt32, (uint)3 };
            yield return new object[] { "3", DbType.UInt64, (ulong)3 };
            yield return new object[] { "3", DbType.AnsiString, "3" };
            yield return new object[] { "2:30", DbType.Time, new TimeSpan(0, 2, 30, 0), };
            yield return new object[] { "2018-12-23 22:56", DbType.DateTime, new DateTime(2018, 12, 23, 22, 56, 0), };
            yield return new object[] { new DateTime(2018, 12, 23, 22, 56, 0).ToString(CultureInfo.InvariantCulture), DbType.DateTime, new DateTime(2018, 12, 23, 22, 56, 0), };
            yield return new object[] { "2018-12-23", DbType.Date, new DateTime(2018, 12, 23, 0, 0, 0), };
            yield return new object[] { "3888CCA3-D11D-45C9-89A5-E6B72185D287", DbType.Guid, Guid.Parse("3888CCA3-D11D-45C9-89A5-E6B72185D287") };

        }

    }
}
