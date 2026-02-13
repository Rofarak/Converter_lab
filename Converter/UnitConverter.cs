using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter
{
    /// <summary>
    /// Unit conversion class
    /// </summary>
    public static class UnitConverter
    {
        // ============ LENGTH ============
        private static readonly Dictionary<string, double> lengthUnits = new Dictionary<string, double>
        {
            { "Meters (m)", 1.0 },
            { "Kilometers (km)", 0.001 },
            { "Centimeters (cm)", 100.0 }
        };

        // ============ MASS ============
        private static readonly Dictionary<string, double> massUnits = new Dictionary<string, double>
        {
            { "Kilograms (kg)", 1.0 },
            { "Grams (g)", 1000.0 },
            { "Tons (t)", 0.001 }
        };

        // ============ TEMPERATURE ============
        private static readonly List<string> temperatureUnits = new List<string>
        {
            "Celsius (°C)",
            "Fahrenheit (°F)",
            "Kelvin (K)"
        };

        /// <summary>
        /// Get units for selected category
        /// </summary>
        public static string[] GetUnits(string category)
        {
            switch (category)
            {
                case "Length":
                    return new List<string>(lengthUnits.Keys).ToArray();
                case "Mass":
                    return new List<string>(massUnits.Keys).ToArray();
                case "Temperature":
                    return temperatureUnits.ToArray();
                default:
                    return new string[0];
            }
        }

        /// <summary>
        /// Convert value
        /// </summary>
        public static double Convert(double value, string fromUnit, string toUnit, string category)
        {
            if (category == "Temperature")
            {
                return ConvertTemperature(value, fromUnit, toUnit);
            }
            else
            {
                return ConvertLinear(value, fromUnit, toUnit, category);
            }
        }

        /// <summary>
        /// Linear conversion (length, mass)
        /// </summary>
        private static double ConvertLinear(double value, string fromUnit, string toUnit, string category)
        {
            Dictionary<string, double> units = category == "Length" ? lengthUnits : massUnits;

            if (!units.ContainsKey(fromUnit) || !units.ContainsKey(toUnit))
                throw new ArgumentException("Unknown unit");

            // Convert to base unit (meters or kilograms)
            double baseValue = value / units[fromUnit];

            // Convert from base to target
            return baseValue * units[toUnit];
        }

        /// <summary>
        /// Temperature conversion
        /// </summary>
        private static double ConvertTemperature(double value, string fromUnit, string toUnit)
        {
            double celsius;

            // Convert to Celsius
            if (fromUnit == "Celsius (°C)")
                celsius = value;
            else if (fromUnit == "Fahrenheit (°F)")
                celsius = (value - 32) * 5 / 9;
            else if (fromUnit == "Kelvin (K)")
                celsius = value - 273.15;
            else
                throw new ArgumentException("Unknown temperature unit");

            // Convert from Celsius to target unit
            if (toUnit == "Celsius (°C)")
                return celsius;
            else if (toUnit == "Fahrenheit (°F)")
                return celsius * 9 / 5 + 32;
            else if (toUnit == "Kelvin (K)")
                return celsius + 273.15;
            else
                throw new ArgumentException("Unknown temperature unit");
        }

        /// <summary>
        /// Get unit symbol
        /// </summary>
        public static string GetUnitSymbol(string unitName)
        {
            if (string.IsNullOrEmpty(unitName))
                return "";

            int startIndex = unitName.LastIndexOf('(') + 1;
            int endIndex = unitName.LastIndexOf(')');

            if (startIndex > 0 && endIndex > startIndex)
            {
                return unitName.Substring(startIndex, endIndex - startIndex);
            }
            return "";
        }
    }
}
