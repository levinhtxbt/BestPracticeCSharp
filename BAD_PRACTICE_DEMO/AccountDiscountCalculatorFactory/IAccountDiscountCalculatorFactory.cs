using BAD_PRACTICE_DEMO.Utils;

namespace BAD_PRACTICE_DEMO.AccountDiscountCalculatorFactory
{
    public interface IAccountDiscountCalculatorFactory
    {
        IAccountDiscountCalculator GetAccountDiscountCalculator(AccountStatus accountStatus);
    }
}