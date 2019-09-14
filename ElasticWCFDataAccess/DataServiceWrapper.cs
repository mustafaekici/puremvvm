using KarmasisDomain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarmasisDomain;
using System.ServiceModel;
using ServiceLibrary;
using System.ServiceModel.Description;

namespace ElasticWCFDataAccess
{
    public class DataServiceWrapper : TicketRepository
    {
        string connection;
        IService1 channel;
        public DataServiceWrapper(string connection)
        {
            this.connection = connection;
        }
        public void CreateTicket(KarmasisDomain.Ticket entity)
        {
            ServiceLibrary.Ticket t = new ServiceLibrary.Ticket();
            //TODO need mapper=>
            t.Description = entity.Description;
            t.ID = entity.ID;
            t.ProductName = entity.ProductName;
            t.CustomerName = entity.CustomerName;
            t.TimeCreated = entity.TimeCreated;
            t.Subject = entity.Subject;
            byte[] pass_byte = Encoding.ASCII.GetBytes(entity.ProductName);
            int code = 0;
            foreach (var item in pass_byte)
            {
                code += item;
            }

            t.ProductNameKey = code;
            channel.CreateTicket(t);
         
        }
        public bool Login(KarmasisDomain.User user)
        {
            ChannelFactory<IService1> channelFactory = null;
            try
            {

                EndpointAddress Serviceaddress = new EndpointAddress(connection);
                BasicHttpBinding httpBinding = new BasicHttpBinding();

                ChannelFactory<IService1> myChannelFactory = new ChannelFactory<IService1>(httpBinding, Serviceaddress);

                channel = myChannelFactory.CreateChannel();

            }
            catch (TimeoutException)
            {
                //Timeout error
                if (channelFactory != null)
                    channelFactory.Abort();

                throw;
            }
            catch (FaultException)
            {
                if (channelFactory != null)
                    channelFactory.Abort();

                throw;
            }
            catch (CommunicationException)
            {
                //Communication error    
                if (channelFactory != null)
                    channelFactory.Abort();

                throw;
            }
            catch (Exception)
            {
                if (channelFactory != null)
                    channelFactory.Abort();

                throw;
            }
            //custom authentication not by wcf
            return channel.CheckLogin(new ServiceLibrary.User() { UserName = user.UserName, Password = user.Pass });
        }
        public async Task<IEnumerable<KarmasisDomain.Ticket>> GetAllTicketsAsync()
        {
            var result = await channel.GetAllTicketsAsync();
            return result.Select(t =>
        new KarmasisDomain.Ticket
        {
            ID = t.ID,
            Subject=t.Subject,
            ProductName=t.ProductName,
            SolvedBy=t.SolvedBy,
            Solved=t.Solved,
            TimeCreated=t.TimeCreated,
            Description = t.Description,
            CustomerName = t.CustomerName,

        }).AsEnumerable();

        }
        public async Task<bool> SolvedTicketAsync(int id, string username,bool solved)
        {
            int? code = 0;
            if (solved)
            {
                byte[] pass_byte = Encoding.ASCII.GetBytes(username);
                foreach (var item in pass_byte)
                {
                    code += item;
                }
            }
            else
            {
                code = null;
            }
            return await channel.SolvedTicketAsync(id, username,code,solved);
        }
        public async Task<bool> DeleteTicketAsync(int searchID)
        {
            return await channel.DeleteTicketAsync(searchID);
        }
        public async Task<IEnumerable<KarmasisDomain.ChartEntity>> TopProblemProductsAsync()
        {
            var result = await channel.TopProblemProductsAsync();
            return result.Select(t =>
            new KarmasisDomain.ChartEntity
            {
                Definition = t.Definition,
                Value = t.Value
            }).AsEnumerable();

        }
        public async Task<IEnumerable<KarmasisDomain.ChartEntity>> TopSolvedByAsync()
        {
            var result = await channel.TopSolvedByAsync();
            return result.Select(t =>
            new KarmasisDomain.ChartEntity
            {
                Definition = t.Definition,
                Value = t.Value
            }).AsEnumerable();
        }

        public bool CheckDb()
        {
            return channel.CheckDb();
        }
    }
}
