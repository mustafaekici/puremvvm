using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        void CreateTicket(Ticket ticket);

        [OperationContract]
        Task<IEnumerable<Ticket>> GetAllTicketsAsync();

        [OperationContract]
        bool CheckLogin(User user);

        [OperationContract]
        Task<bool> SolvedTicketAsync(int id,string username,int? usercode,bool solved);
        [OperationContract]
        Task<bool> DeleteTicketAsync(int id);

        [OperationContract]
        Task<IEnumerable<ChartEntity>> TopProblemProductsAsync();

        [OperationContract]
        Task<IEnumerable<ChartEntity>> TopSolvedByAsync();
        [OperationContract]
        bool CheckDb();
    }

    [DataContract]
    public class Ticket
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]

        public string ProductName { get; set; } // name of the product related to the request [DataMember] public string SolvedBy { get; set; }// name of the technical support employee who solved the issue [DataMember] public bool Solved { get; set; } [DataMember] public DateTime TimeCreated { get; set; }
        [DataMember]
        public string SolvedBy { get; set; }// name of the technical support employee who solved the issue [DataMember] public bool Solved { get; set; } [DataMember] public DateTime TimeCreated { get; set; }
        [DataMember]
        public bool Solved { get; set; }
        [DataMember]
        public DateTime TimeCreated { get; set; }
        [DataMember]
        public int ProductNameKey { get; set; }

        [DataMember]
        public int UserNameKey { get; set; }

    }

    [DataContract]
    public class User
    {
       
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
  
    }
   
    [DataContract]
    public class ChartEntity 
    {
        [DataMember]
        public string Definition { get; set; }
        [DataMember]
        public long Value { get; set; }
    }

}
