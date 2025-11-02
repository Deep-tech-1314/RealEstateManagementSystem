using System;

namespace RealEstateManagementSystem.Services
{
    public static class CurrencyService
    {
        // Rupee symbol
        public const string RupeeSymbol = "₹";
        
        // Convert USD to INR with more reasonable pricing
        // This applies a scaling factor to make prices more reasonable
        // For example, a $500,000 property becomes ₹15,00,000 instead of ₹4,15,00,000
        public static decimal ConvertUsdToInr(decimal usdAmount)
        {
            // Apply a scaling factor to make prices more reasonable
            // For real estate in India, we'll use a more appropriate conversion
            // that accounts for market differences
            
            // For properties under $1000 (rental prices per month)
            if (usdAmount < 1000)
            {
                // Convert to reasonable monthly rent in INR (15-30K range)
                return Math.Round(usdAmount * 25, 2);
            }
            // For properties under $10,000 (low-cost properties)
            else if (usdAmount < 10000)
            {
                // Convert to reasonable low-cost property price (5-50 lakh range)
                return Math.Round(usdAmount * 500, 2);
            }
            // For properties under $100,000 (mid-range properties)
            else if (usdAmount < 100000)
            {
                // Convert to reasonable mid-range property price (50 lakh - 2 crore range)
                return Math.Round(usdAmount * 200, 2);
            }
            // For properties under $1,000,000 (luxury properties)
            else if (usdAmount < 1000000)
            {
                // Convert to reasonable luxury property price (2-10 crore range)
                return Math.Round(usdAmount * 30, 2);
            }
            // For ultra-luxury properties
            else
            {
                // Convert to reasonable ultra-luxury property price (10+ crore range)
                return Math.Round(usdAmount * 15, 2);
            }
        }
        
        // Format price with rupee symbol
        public static string FormatInrPrice(decimal inrAmount)
        {
            return $"{RupeeSymbol}{inrAmount.ToString("N0")}";
        }
        
        // Convert and format in one step
        public static string ConvertAndFormatPrice(decimal usdAmount)
        {
            decimal inrAmount = ConvertUsdToInr(usdAmount);
            return FormatInrPrice(inrAmount);
        }
    }
}