using System;

namespace _04Zaporozhets
{
    /// <summary>
    /// Represents the Western Zodiac signs.
    /// </summary>
    public enum WesternZodiac
    {
        Capricorn = 0,
        Aquarius,
        Pisces,
        Aries,
        Taurus,
        Gemini,
        Cancer,
        Leo,
        Virgo,
        Libra,
        Scorpio,
        Sagittarius
    }

    /// <summary>
    /// Represents the Chinese Zodiac signs.
    /// </summary>
    public enum ChineseZodiac
    {
        Rat = 0,
        Ox,
        Tiger,
        Rabbit,
        Dragon,
        Snake,
        Horse,
        Goat,
        Monkey,
        Rooster,
        Dog,
        Pig
    }

    /// <summary>
    /// Provides extension methods for working with Zodiac signs based on a given date.
    /// </summary>
    public static class ZodiacUtils
    {
        /// <summary>
        /// Gets the Western Zodiac sign based on the provided date.
        /// </summary>
        /// <param name="dateTime">The date to determine the Western Zodiac sign for.</param>
        /// <returns>The Western Zodiac sign for the given date.</returns>
        public static WesternZodiac GetWesternZodiacSign(this DateTime dateTime)
        {
            int[] pivotDays = { 21, 20, 22, 21, 22, 22, 24, 24, 24, 24, 23, 23 };

            int monthIndex = dateTime.Month - 1;
            int offset = dateTime.Day < pivotDays[monthIndex] ? 0 : 1;
            int index = (monthIndex + offset) % 12;

            return (WesternZodiac)index;
        }

        /// <summary>
        /// Gets the Chinese Zodiac sign based on the provided date.
        /// </summary>
        /// <param name="dateTime">The date to determine the Chinese Zodiac sign for.</param>
        /// <returns>The Chinese Zodiac sign for the given date.</returns>
        public static ChineseZodiac GetChineseZodiacSign(this DateTime dateTime)
        {
            int index = (dateTime.Year - 4) % 12;

            return (ChineseZodiac)index;
        }

        /// <summary>
        /// Validates the provided date to ensure it falls within a valid range.
        /// </summary>
        /// <param name="dateTime">The date to validate.</param>
        /// <param name="propertyName">The name of the property being validated.</param>
        public static void ValidateDate(this DateTime dateTime, BaseBindable baseBindable, string propertyName)
        {
            if (dateTime.Year < 1900 || dateTime.Year > DateTime.Now.Year)
            {
                baseBindable.AddPropertyError(propertyName, "Date is out of valid range");
            }
        }
    }
}
