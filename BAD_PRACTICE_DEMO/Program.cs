using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAD_PRACTICE_DEMO.AccountDiscountCalculatorFactory;
using BAD_PRACTICE_DEMO.LoyaltyDiscountCalculator;
using BAD_PRACTICE_DEMO.Utils;

namespace BAD_PRACTICE_DEMO
{
    class Program
    {
        static void Main(string[] args)
        {
            decimal price = 50000;
            AccountStatus accountStatus = AccountStatus.SimpleCustomer;
            int time = 4;

            DiscountManager discountManager = new DiscountManager(new DefaultAccountDiscountCalculatorFactory(), new DefaultLoyaltyDiscountCalculator());

            decimal priceAfterDiscount = discountManager.ApplyDiscount(price, accountStatus, time);
            Console.WriteLine(priceAfterDiscount);

            Console.ReadKey();

        }
    }
}
