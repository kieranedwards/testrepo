using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SagePay.InitalRequest;

namespace SagePay.Interfaces
{
    public interface ITransactionRequestBuilderFactory
    {
        ITransactionRequestBuilder CreateBuilder();
    }
}
