using Ninject.Modules;

namespace IoC_ServiceLocator
{
    public class MyModule : NinjectModule
    {
        private readonly string _key;

        public MyModule(string key)
        {
            _key = key;
        }

        public override void Load()
        {
            Bind<IPaymentProcessor>().To<CreditCardPaymentProcessor>()
                                     .WithMetadata(_key, typeof(CreditCardPaymentProcessor));
        }
    }


    public interface IPaymentProcessor
    {
        bool Charge(decimal amount);
    }

    public class CreditCardPaymentProcessor : IPaymentProcessor
    {
        private readonly int _accountNumber;

        // In real app the parameter could be any data that can only
        // be accessed at runtime.
        public CreditCardPaymentProcessor(int accountNumber)
        {
            _accountNumber = accountNumber;
        }

        public bool Charge(decimal amount)
        {
            // simulate payment charge successful
            return true;
        }
    }
}