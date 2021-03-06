﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAD_PRACTICE_DEMO.Utils;

namespace BAD_PRACTICE_DEMO.AccountDiscountCalculatorFactory
{
    public class DefaultAccountDiscountCalculatorFactory : IAccountDiscountCalculatorFactory
    {
        public IAccountDiscountCalculator GetAccountDiscountCalculator(AccountStatus accountStatus)
        {
            IAccountDiscountCalculator calculator;
            switch(accountStatus)
            {
                case AccountStatus.NotRegistered:
                    calculator = new NotRegisteredDiscountCalculator();
                    break;
                case AccountStatus.SimpleCustomer:
                    calculator = new SimpleCustomerDiscountCalculator();
                    break;
                case AccountStatus.ValuableCustomer:
                    calculator = new ValuableCustomerDiscountCalculator();
                    break;
                case AccountStatus.MostValuableCustomer:
                    calculator = new MostValuableCustomerDiscountCalculator();
                    break;
                default:
                    throw new NotImplementedException();
            }
            return calculator;

        }
    }
}
