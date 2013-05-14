using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IoC_ServiceLocator
{
    class Program
    {
        static void Main(string[] args)
        {
            const int someRuntimeValue = 99999;

            IPaymentProcessor paymentProcessor = IoC.Resolve<IPaymentProcessor>(someRuntimeValue);

            bool result = paymentProcessor.Charge(15000);

            Console.WriteLine(result ? "Payment success" : "Payment failed");

            Console.ReadLine();
        }
    }
}
