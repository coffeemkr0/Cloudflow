using Cloudflow.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Cloudflow.WcfServiceLibrary
{
    [ServiceContract]
    public interface IAgentService
    {
        [WebInvoke(UriTemplate = "/GetAgentStatus", Method = "GET", ResponseFormat = WebMessageFormat.Json), CorsEnabled]
        [OperationContract]
        AgentStatus GetAgentStatus();

        [WebInvoke(UriTemplate = "/EnableJob", Method = "GET", ResponseFormat = WebMessageFormat.Json), CorsEnabled]
        [OperationContract]
        void EnableJob();

        [WebGet(UriTemplate = "GetData/{value}", ResponseFormat = WebMessageFormat.Json), CorsEnabled]
        [OperationContract]
        string GetData(string value);

        [WebInvoke(UriTemplate = "/GetDataUsingDataContract", Method = "POST", ResponseFormat = WebMessageFormat.Json), CorsEnabled]
        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "Cloudflow.WcfServiceLibrary.ContractType".
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
