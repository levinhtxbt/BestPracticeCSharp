using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAD_PRACTICE_DEMO.Utils;

namespace BAD_PRACTICE_DEMO.LoyaltyDiscountCalculator
{
    public class DefaultLoyaltyDiscountCalculator : ILoyaltyDiscountCalculator
    {
        public decimal ApplyDiscount(decimal price, int timeOfHavingAccountInYears)
        {
            decimal discountForLoyaltyInPercentage = (timeOfHavingAccountInYears > Constants.MAXIMUM_DISCOUNT_FOR_LOYALTY) ?
                (decimal)Constants.MAXIMUM_DISCOUNT_FOR_LOYALTY / 100 : (decimal)timeOfHavingAccountInYears / 100;
            return price - (discountForLoyaltyInPercentage * price);
        }
    }
}
