using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAD_PRACTICE_DEMO.Utils;

namespace BAD_PRACTICE_DEMO.AccountDiscountCalculatorFactory
{
    public class ConfigurableAccountDiscountCalculatorFactory : IAccountDiscountCalculatorFactory
    {

        Dictionary<AccountStatus, IAccountDiscountCalculator> _discountsDictionary;

        public ConfigurableAccountDiscountCalculatorFactory(Dictionary<AccountStatus, string> discountsDictionary)
        {
            this._discountsDictionary = convertStringDictionaryToObjectDictionary(discountsDictionary);
        }

        public IAccountDiscountCalculator GetAccountDiscountCalculator(AccountStatus accountStatus)
        {
            IAccountDiscountCalculator calculator;
            if (!_discountsDictionary.TryGetValue(accountStatus, out calculator))
            {
                throw new NotImplementedException("There is no implementation of IAccountDiscountCalculatorFactory interface for given Account Status");
            }
            return calculator;
        }

        private Dictionary<AccountStatus, IAccountDiscountCalculator> convertStringDictionaryToObjectDictionary(Dictionary<AccountStatus, string> dict)
        {
            return dict.ToDictionary(x => x.Key, x => (IAccountDiscountCalculator)Activator.CreateInstance(Type.GetType(x.Value)));
        }
    }
}
