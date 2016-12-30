using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAD_PRACTICE_DEMO.Utils;

namespace BAD_PRACTICE_DEMO.AccountDiscountCalculatorFactory
{
    public class ConfigurableLazyAccountDiscountCalculatorFactory : IAccountDiscountCalculatorFactory
    {
        private Dictionary<AccountStatus, Lazy<IAccountDiscountCalculator>> _discountsDictionary;

        public ConfigurableLazyAccountDiscountCalculatorFactory(Dictionary<AccountStatus, string> discountsDictionary)
        {
            _discountsDictionary = convertStringDictionaryToObjectDictionary(discountsDictionary);
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


        private Dictionary<AccountStatus, Lazy<IAccountDiscountCalculator>> convertStringDictionaryToObjectDictionary(Dictionary<AccountStatus, string> dict)
        {
            return dict.ToDictionary(x => x.Key,
                x => new Lazy<IAccountDiscountCalculator>(() => (IAccountDiscountCalculator)Activator.CreateInstance(Type.GetType(x.Value))));
        }

    }
}
