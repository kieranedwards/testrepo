using Autofac;
using SagePay.InitalRequest;
using SagePay.Interfaces;

namespace SagePay
{
    public class DependencyConfiguration : Module
    {
        protected override void Load(ContainerBuilder builder)
        {           
            builder.Register(c => new TransactionParameterValidator()).As<ITransactionParameterValidator>();
            builder.Register(c => new TransactionRequestBuilderFactory(c.Resolve<ITransactionParameterValidator>(), c.Resolve<IPaymentListenerUrlProvider>())).As<ITransactionRequestBuilderFactory>();
            builder.Register(c => new TransactionClient()).As<ITransactionClient>();
        }
    }
}
