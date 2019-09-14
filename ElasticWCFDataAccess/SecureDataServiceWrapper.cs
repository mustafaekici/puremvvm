using KarmasisDomain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarmasisDomain;
using ServiceLibrary;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace ElasticWCFDataAccess
{
    public class SecureDataServiceWrapper: TicketRepository
    {
        IService1 channel;
        string connection;
        public SecureDataServiceWrapper(string connection)
        {
            this.connection = connection;
        }

        public bool CheckDb()
        {
            throw new NotImplementedException();
        }

        public void CreateTicket(KarmasisDomain.Ticket ticket)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTicketAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<KarmasisDomain.Ticket>> GetAllTicketsAsync()
        {
            throw new NotImplementedException();
        }

        public bool Login(KarmasisDomain.User user)
        {
            ChannelFactory<IService1> channelFactory = null;
            try
            {

                EndpointAddress Serviceaddress = new EndpointAddress(connection);
                BasicHttpBinding httpBinding = new BasicHttpBinding();
                httpBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                httpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

                ChannelFactory<IService1> myChannelFactory =
                    new ChannelFactory<IService1>(httpBinding, Serviceaddress);
                var defaultCredentials = myChannelFactory.Endpoint.Behaviors.Find<ClientCredentials>();

                myChannelFactory.Credentials.UserName.UserName = "youhaveto";
                myChannelFactory.Credentials.UserName.Password = "SecurityTokenException";

                //ClientCredentials CC = new ClientCredentials();
                //CC.UserName.UserName = "h";
                //CC.UserName.Password = "p";
                // myChannelFactory.Endpoint.Behaviors.Remove(defaultCredentials); //remove default ones
                // myChannelFactory.Endpoint.Behaviors.Add(CC); //add required on

                // Create a channel.
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
            // authentication by wcf
            return channel.CheckLogin(new ServiceLibrary.User() { UserName = user.UserName, Password = user.Pass });
        }

        public Task<bool> SolvedTicketAsync(int iD, string username, bool solved)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<KarmasisDomain.ChartEntity>> TopProblemProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<KarmasisDomain.ChartEntity>> TopSolvedByAsync()
        {
            throw new NotImplementedException();
        }
    }
}
