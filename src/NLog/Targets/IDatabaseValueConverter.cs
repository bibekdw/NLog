using System.Data;

namespace NLog.Targets
{
    /// <summary>
    /// Convert values
    /// </summary>
    public interface IDatabaseValueConverter
    {
        /// <summary>
        /// convert layout value to parameter value
        /// </summary>
        object ConvertFromString(string value, DbType dbType, string parseFormat);

        /// <summary>
        /// Convert rawvalue to parameter value
        /// </summary>
        /// <param name="rawValue"></param>
        /// <param name="dbType"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        object ConvertFromObject(object rawValue, DbType dbType, string format);
    }
}