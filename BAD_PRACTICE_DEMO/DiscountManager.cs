
using System;
using System.Configuration;
using System.Collections.Generic;
using Autofac;
using BAD_PRACTICE_DEMO.AccountDiscountCalculatorFactory;
using BAD_PRACTICE_DEMO.LoyaltyDiscountCalculator;
using BAD_PRACTICE_DEMO.Utils;
using System.Linq;
using System.Collections.Specialized;

namespace BAD_PRACTICE_DEMO
{
    public class DiscountManager
    {
        private readonly IAccountDiscountCalculatorFactory _factory;
        private readonly ILoyaltyDiscountCalculator _loyaltyDiscountCalculator;


        public DiscountManager(IAccountDiscountCalculatorFactory factory, ILoyaltyDiscountCalculator loyaltyDiscountCalculator)
        {
            _factory = factory;
            _loyaltyDiscountCalculator = loyaltyDiscountCalculator;
        }

        public decimal ApplyDiscount(decimal price, AccountStatus accountStatus, int timeOfHavingAccountInYears)
        {
            decimal priceAfterDiscount = 0;
            priceAfterDiscount = _factory.GetAccountDiscountCalculator(accountStatus).ApplyDiscount(price);
            priceAfterDiscount = _loyaltyDiscountCalculator.ApplyDiscount(priceAfterDiscount, timeOfHavingAccountInYears);

            return priceAfterDiscount;
        }

        /* Using autoFac
        ==> Advantages:

        It’s simple

        Strong-typed configuration – mistake in configuration (wrong type definition) will cause an error at the compile time

        Returning an objects from the factory is very fast -they are already created

        ==> Disadvantages:

        Won’t work in a multi-threaded environment when returned objects have a state*/

        public decimal ApplyDiscountUsingAutoFac(decimal price, AccountStatus accountStatus, int timeOfHavingAccountInYears)
        {
            var discountsDictionary = new Dictionary<AccountStatus, IAccountDiscountCalculator>
            {
                {AccountStatus.NotRegistered, new NotRegisteredDiscountCalculator()},
                {AccountStatus.SimpleCustomer, new SimpleCustomerDiscountCalculator()},
                {AccountStatus.ValuableCustomer, new ValuableCustomerDiscountCalculator()},
                {AccountStatus.MostValuableCustomer, new MostValuableCustomerDiscountCalculator()}
            };

            var factory = new DictionarableAccountDiscountCalculatorFactory(discountsDictionary);
            var builder = new ContainerBuilder();
            builder.RegisterType<DictionarableAccountDiscountCalculatorFactory>().As<IAccountDiscountCalculatorFactory>()
            .WithParameter("discountsDictionary", discountsDictionary)
            .SingleInstance();

            decimal priceAfterDiscount = _factory.GetAccountDiscountCalculator(accountStatus).ApplyDiscount(price);
            priceAfterDiscount = _loyaltyDiscountCalculator.ApplyDiscount(priceAfterDiscount, timeOfHavingAccountInYears);
            return priceAfterDiscount;
        }


        /* Using Lazy
        ==> Advantages:

        Faster app start

        Doesn’t keep the object in memory until you really need it

        Strong-typed configuration – mistake in configuration (wrong type definition) will cause an error at the compile time

        ==> Disadvantages:

        First object return from the factory will be slower

        Won’t work in a multi-threaded environment when returned objects have a state */
        public decimal ApplyDiscountUsingLazy(decimal price, AccountStatus accountStatus, int timeOfHavingAccountInYears)
        {
            var discountsDictionary = new Dictionary<AccountStatus, Lazy<IAccountDiscountCalculator>>
            {
                {AccountStatus.NotRegistered, new Lazy<IAccountDiscountCalculator>(()=> new NotRegisteredDiscountCalculator()) },
                {AccountStatus.SimpleCustomer, new Lazy<IAccountDiscountCalculator>(() => new SimpleCustomerDiscountCalculator())},
                {AccountStatus.ValuableCustomer, new Lazy<IAccountDiscountCalculator>(() => new ValuableCustomerDiscountCalculator())},
                {AccountStatus.MostValuableCustomer, new Lazy<IAccountDiscountCalculator>(() => new MostValuableCustomerDiscountCalculator())}
            };

            var factory = new DictionarableLazyAccountDiscountCalculatorFactory(discountsDictionary);
            decimal priceAfterDiscount = _factory.GetAccountDiscountCalculator(accountStatus).ApplyDiscount(price);
            priceAfterDiscount = _loyaltyDiscountCalculator.ApplyDiscount(priceAfterDiscount, timeOfHavingAccountInYears);
            return priceAfterDiscount;
        }


        /* Configuration outside code
         
        ==> Advantages:

        Configuration change does not mean change in a code base

        Switching assignment AccountStatus – IAccountDiscountCalculatorFactory implementation can be done via configuration

        ==> Disadvantages:

        Weak-typed configuration – mistake in a configuration (wrong type definition) will cause an error in the runtime

        Won’t work in a multi-threaded environment when returned objects have a state

        You potentially allow deployment team to change behaviour of your code*/
        public decimal ApplyDiscountUsingConfigurationOutsideCode(decimal price, AccountStatus accountStatus, int timeOfHavingAccountInYears)
        {
            //this is mock data 
            //String include [Namespace].[Classname], [AssemblyName]
            var discountDiscountDictionary = new Dictionary<AccountStatus, string>
            {
                {AccountStatus.NotRegistered, "BAD_PRACTICE_DEMO.AccountDiscountCalculatorFactory.NotRegisteredDiscountCalculator, BAD_PRACTICE_DEMO"},
                {AccountStatus.SimpleCustomer, "BAD_PRACTICE_DEMO.AccountDiscountCalculatorFactory.SimpleCustomerDiscountCalculator, BAD_PRACTICE_DEMO"},
                {AccountStatus.ValuableCustomer, "BAD_PRACTICE_DEMO.AccountDiscountCalculatorFactory.ValuableCustomerDiscountCalculator, BAD_PRACTICE_DEMO"},
                {AccountStatus.MostValuableCustomer, "BAD_PRACTICE_DEMO.AccountDiscountCalculatorFactory.MostValuableCustomerDiscountCalculator, BAD_PRACTICE_DEMO"}

            };

            var factory = new ConfigurableAccountDiscountCalculatorFactory(discountDiscountDictionary);
            decimal priceAfterDiscount = factory.GetAccountDiscountCalculator(accountStatus).ApplyDiscount(price);
            priceAfterDiscount = _loyaltyDiscountCalculator.ApplyDiscount(priceAfterDiscount, timeOfHavingAccountInYears);
            return priceAfterDiscount;
        }

        /*Like example below but we use Lazy instead
        ==>Advantages:

        Configuration change does not mean change in a code base

        Faster app start

        Doesn’t keep the object in memory until you really need it

        Switching assignment AccountStatus – IAccountDiscountCalculatorFactory implementation can be done via configuration

        ==> Disadvantages:

        Weak-typed configuration – mistake in a configuration (wrong type definition) will cause an error in the runtime

        Won’t work in a multi-threaded environment when returned objects have a state

        You potentially allow deployment team to change behaviour of your code

        First object return from the factory will be slower*/
        public decimal ApplyDiscountUsingConfigurationOutsideCodeLazyVersion(decimal price, AccountStatus accountStatus, int timeOfHavingAccountInYears)
        {
            //this is mock data 
            //String include [Namespace].[Classname], [AssemblyName]
            var discountDiscountDictionary = new Dictionary<AccountStatus, string>
            {
                {AccountStatus.NotRegistered, "BAD_PRACTICE_DEMO.AccountDiscountCalculatorFactory.NotRegisteredDiscountCalculator, BAD_PRACTICE_DEMO"},
                {AccountStatus.SimpleCustomer, "BAD_PRACTICE_DEMO.AccountDiscountCalculatorFactory.SimpleCustomerDiscountCalculator, BAD_PRACTICE_DEMO"},
                {AccountStatus.ValuableCustomer, "BAD_PRACTICE_DEMO.AccountDiscountCalculatorFactory.ValuableCustomerDiscountCalculator, BAD_PRACTICE_DEMO"},
                {AccountStatus.MostValuableCustomer, "BAD_PRACTICE_DEMO.AccountDiscountCalculatorFactory.MostValuableCustomerDiscountCalculator, BAD_PRACTICE_DEMO"}

            };

            var factory = new ConfigurableLazyAccountDiscountCalculatorFactory(discountDiscountDictionary);
            decimal priceAfterDiscount = factory.GetAccountDiscountCalculator(accountStatus).ApplyDiscount(price);
            priceAfterDiscount = _loyaltyDiscountCalculator.ApplyDiscount(priceAfterDiscount, timeOfHavingAccountInYears);
            return priceAfterDiscount;
        }

        /* new Instance
         ==> Advantages:

        It’s simple

        Strong-typed configuration – mistake in configuration (wrong type definition) will cause an error at the compile time

        Works like a charm in a multi-threaded environment when returned objects have a state

        ==> Disadvantages:

        Objects are returned slower from the factory - every call to the factory for an object will create a new instance

        If given in configuration type does not implement IAccountDiscountCalculator , an error will occur in the runtime

        Caller is responsible for managing a life cycle of an object after factory will return it*/
        public decimal ApplyDiscountUsingDictionaryButNewInstance(decimal price, AccountStatus accountStatus, int timeOfHavingAccountInYears)
        {
            //this is mock data 
            //String include [Namespace].[Classname], [AssemblyName]
            var discountsDictionary = new Dictionary<AccountStatus, Type>
            {
              {AccountStatus.NotRegistered, typeof(NotRegisteredDiscountCalculator)},
              {AccountStatus.SimpleCustomer, typeof(SimpleCustomerDiscountCalculator)},
              {AccountStatus.ValuableCustomer, typeof(ValuableCustomerDiscountCalculator)},
              {AccountStatus.MostValuableCustomer, typeof(MostValuableCustomerDiscountCalculator)}
              //{AccountStatus.MostValuableCustomerExtended, typeof(MostValuableCustomerDiscountCalculatorExtend)} this class not implement interface IAccountDiscountCalculator 
            };

            var factory = new DictionarableAccountDiscountCalculatorFactoryNewInstance(discountsDictionary);
            decimal priceAfterDiscount = factory.GetAccountDiscountCalculator(accountStatus).ApplyDiscount(price);
            priceAfterDiscount = _loyaltyDiscountCalculator.ApplyDiscount(priceAfterDiscount, timeOfHavingAccountInYears);
            return priceAfterDiscount;
        }


        /*
         ==> Advantages:

        Configuration change does not mean change in a code base

        Switching assignment AccountStatus – IAccountDiscountCalculatorFactory implementation can be done via configuration

        Works like a charm in a multi-threaded environment when returned objects have a state

        ==> Disadvantages:

        Weak-typed configuration – mistake in a configuration will cause an error in a runtime

        You potentially allow deployment team to change behaviour of your code

        Objects are returned slower from the factory - every call to the factory for an object will create a new instance

        Caller is responsible for managing a life cycle of an object after the factory will return it*/
        public decimal ApplyDiscountUsingConfigurationOutsiteCodeFromConfigurationFile(decimal price, AccountStatus accountStatus, int timeOfHavingAccountInYears)
        {
            //this is mock data 
            //String include [Namespace].[Classname], [AssemblyName]
            var collection = ConfigurationManager.GetSection("DiscountCalculatorsConfiguration") as NameValueCollection;
            var discountsDictionary = collection.AllKeys.ToDictionary(k => k, k => collection[k]);

            var factory = new ConfigurableAccountDiscountCalculatorFactoryFromConfigurationFile(discountsDictionary);
            decimal priceAfterDiscount = factory.GetAccountDiscountCalculator(accountStatus).ApplyDiscount(price);
            priceAfterDiscount = _loyaltyDiscountCalculator.ApplyDiscount(priceAfterDiscount, timeOfHavingAccountInYears);
            return priceAfterDiscount;
        }


    }




}
