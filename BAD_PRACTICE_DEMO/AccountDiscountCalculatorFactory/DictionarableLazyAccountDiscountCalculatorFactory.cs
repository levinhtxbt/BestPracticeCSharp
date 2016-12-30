using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAD_PRACTICE_DEMO.Utils;

namespace BAD_PRACTICE_DEMO.AccountDiscountCalculatorFactory
{
    public class DictionarableLazyAccountDiscountCalculatorFactory : IAccountDiscountCalculatorFactory
    {
        private readonly Dictionary<AccountStatus, Lazy<IAccountDiscountCalculator>> _discountsDictionary;

        public DictionarableLazyAccountDiscountCalculatorFactory(Dictionary<AccountStatus, Lazy<IAccountDiscountCalculator>> discountsDictionary)
        {
            this._discountsDictionary = discountsDictionary;
        }

        public IAccountDiscountCalculator GetAccountDiscountCalculator(AccountStatus accountStatus)
        {

            Lazy<IAccountDiscountCalculator> calculator;
            if (!_discountsDictionary.TryGetValue(accountStatus, out calculator))
            {
                throw new NotImplementedException("There is no implementation of IAccountDiscountCalculatorFactory interface for given Account Status");
            }

            return calculator.Value;
        }
    }
}
