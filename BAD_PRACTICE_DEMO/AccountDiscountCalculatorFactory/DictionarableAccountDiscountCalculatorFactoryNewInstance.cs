using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAD_PRACTICE_DEMO.Utils;

namespace BAD_PRACTICE_DEMO.AccountDiscountCalculatorFactory
{
    public class DictionarableAccountDiscountCalculatorFactoryNewInstance : IAccountDiscountCalculatorFactory
    {
        private readonly Dictionary<AccountStatus, Type> _discountsDictionary;

        public DictionarableAccountDiscountCalculatorFactoryNewInstance(Dictionary<AccountStatus, Type> discountsDictionary)
        {
            _discountsDictionary = discountsDictionary;
            CheckIfAllValuesFromDictImplementsProperInterface();
        }

        private void CheckIfAllValuesFromDictImplementsProperInterface()
        {
            foreach (var item in _discountsDictionary)
            {
                if (!typeof(IAccountDiscountCalculator).IsAssignableFrom(item.Value))
                {
                    throw new ArgumentException("The type: " + item.Value.FullName + "does not implement IAccountDiscountCalculatorFactory interface!");
                }
            }
        }

        public IAccountDiscountCalculator GetAccountDiscountCalculator(AccountStatus accountStatus)
        {
            Type calculator;

            if (!_discountsDictionary.TryGetValue(accountStatus, out calculator))
            {
                throw new NotImplementedException("There is no implementation of IAccountDiscountCalculatorFactory interface for given Account Status");
            }
            return (IAccountDiscountCalculator)Activator.CreateInstance(calculator);
        }
    }
}
