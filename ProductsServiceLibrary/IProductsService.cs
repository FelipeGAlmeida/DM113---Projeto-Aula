using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Net.Security;

namespace Products {
    // OBSERVAÇÃO: Você pode usar o comando "Renomear" no menu "Refatorar" para alterar o nome da interface "IService1" no arquivo de código e configuração ao mesmo tempo.
    [ServiceContract(Namespace = "http://localhost/productsService/01",Name = "IProductsService")]
    public interface IProductsService {

        // Get all products
        //[OperationContract]
        [OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign)]
        List<ProductData> ListProducts();

        // Get the details of a single product
        //[OperationContract]
        [OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign)]
        ProductData GetProduct(string productCode);

        // Get the current stock for a product
        //[OperationContract]
        [OperationContract(ProtectionLevel = ProtectionLevel.Sign)]
        int CurrentStock(string productCode);

        // Add stock for a product
        //[OperationContract]
        [OperationContract(ProtectionLevel = ProtectionLevel.Sign)]
        bool AddStock(string productCode, decimal quantity);

        // TODO: Adicione suas operações de serviço aqui
    }

    [ServiceContract(Namespace = "http://localhost/productsService/02",Name = "IProductsService")]
    public interface IProductsServiceV2 {

        // Get all products
        //[OperationContract]
        [OperationContract]
        List<ProductData> ListProducts();

        // Get the product numbers of matching products
        [OperationContract]
        List<ProductData> ListMatchingProducts(string match);

        // Get the details of a single product
        //[OperationContract]
        [OperationContract]
        ProductData GetProduct(string productCode);

        // Get the current stock for a product
        //[OperationContract]
        [OperationContract()]
        int CurrentStock(string productCode);

        // Add stock for a product
        //[OperationContract]
        [OperationContract]
        bool AddStock(string productCode, decimal quantity);

        // TODO: Adicione suas operações de serviço aqui
    }


    // Use um contrato de dados como ilustrado no exemplo abaixo para adicionar tipos compostos a operações de serviço.
    [DataContract]
    public class ProductData {

        [DataMember(Order = 0)]
        public string Name;

        [DataMember(Order = 1)]
        public string Code;

        [DataMember(Order = 2)]
        public decimal Price;
    }
}
