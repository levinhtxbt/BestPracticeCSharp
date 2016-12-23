namespace BAD_PRACTICE_DEMO.LoyaltyDiscountCalculator
{
    public interface ILoyaltyDiscountCalculator
    {
        decimal ApplyDiscount(decimal price, int timeOfHavingAccountInYears);
    }


}