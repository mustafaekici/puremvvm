using ServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace WcfService
{
    public class Service1 : IService1
    {
        public bool CheckLogin(ServiceLibrary.User user)
        {
            CustomUserNameValidator cv = new CustomUserNameValidator();
            try
            {
                cv.Validate(user.UserName, user.Password);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public void CreateTicket(Ticket ticket)
        {
            //var myJson = new
            //{
            //    ID = ticket.ID,
            //    Description = ticket.Description,
            //    CustomerName = ticket.CustomerName,
            //    ProductName = ticket.ProductName,
            //    Subject = ticket.Subject,
            //    TimeCreated = ticket.TimeCreated

            //};
            if (!ValidateIfIdIsAlreadyUsedForIndex(ticket.ID.ToString()))
            {
                var response = ElasticConnection.EsClient().Index(ticket);
            }

        }
        private bool ValidateIfIdIsAlreadyUsedForIndex(string id)
        {
            var result = ElasticConnection.EsClient().Search<Ticket>(s => s
                .Index("tickets")
                .AllTypes()
                .Query(q => q.Term(t => t.Field("_id").Value(id))));
            if (result.Documents.Any())
                return true;
            else
                return false;
        }
        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {

            var response = await ElasticConnection.EsClient().SearchAsync<Ticket>(s => s
                .Index("tickets")
                .Type("ticket")
                .From(0)
                .Size(1000)
                .Query(q => q.MatchAll()));
            return response.Hits.Select(x => { x.Source.ID = int.Parse(x.Id); return x.Source; });

        }
        public async Task<bool> SolvedTicketAsync(int searchid, string username, int? usercode, bool solved)
        {
            var response = await ElasticConnection.EsClient().UpdateAsync<Ticket, UpdatedTicket>(searchid, d => d
               .Index("tickets")
               .Type("ticket")
               .Doc(new UpdatedTicket
               {
                   SolvedBy = solved == false ? string.Empty : username,
                   Solved = solved,
                   UserNameKey = usercode
               }));

            return response.IsValid;
        }
        public async Task<bool> DeleteTicketAsync(int searchid)
        {
            bool status;

            var response = await ElasticConnection.EsClient().DeleteAsync<Ticket>(searchid, d => d
                .Index("tickets")
                .Type("ticket"));

            if (response.IsValid)
            {
                status = true;
            }
            else
            {
                status = false;
            }

            return status;
        }
        public async Task<IEnumerable<ChartEntity>> TopProblemProductsAsync()
        {
            //I couldnt have enough time to investigate right query for elasticsearch
            var result = await ElasticConnection.EsClient().SearchAsync<Ticket>(s => s
             .From(0)
                .Size(1000)
                .Query(q => q.MatchAll())
                .Aggregations(a => a
                .Terms("unique", ss => ss
                 .Field(k => k.ProductNameKey).OrderDescending("totalCount")
                 .Aggregations(r => r
                     .ValueCount("totalCount", v => v
                         .Field(p => p.ProductNameKey))))
                )
            );

            var res = result.Aggs.Terms("unique").Buckets.Select(x => new ChartEntity
            {
                Definition = result.Documents.First(b => b.ProductNameKey.ToString() == x.Key).ProductName,
                Value = x.DocCount.Value
            }).Take(5).AsEnumerable();

            return res;
        }
        public async Task<IEnumerable<ChartEntity>> TopSolvedByAsync()
        {
            //I couldnt have enough time to investigate right query for elasticsearch
            var result = await ElasticConnection.EsClient().SearchAsync<Ticket>(s => s
            .From(0)
                .Size(1000)
                .Query(q => q.MatchAll())
                .Aggregations(a => a
                .Terms("unique", ss => ss
                 .Field(k => k.UserNameKey).OrderDescending("totalCount")
                 .Aggregations(r => r
                     .ValueCount("totalCount", v => v
                         .Field(p => p.UserNameKey))))

                )
            );

            var res = result.Aggs.Terms("unique").Buckets.Select(x => new ChartEntity
            {
                Definition = result.Documents.First(b => b.UserNameKey.ToString() == x.Key).SolvedBy,
                Value = x.DocCount.Value
            }).Take(5).AsEnumerable();
            return res;
        }

        public bool CheckDb()
        {
            var response = ElasticConnection.EsClient().Search<Ticket>(s => s
              .Index("tickets")
              .Type("ticket")
              .From(0)
              .Size(1)
              .Query(q => q.MatchAll()));

            if (response.Hits.Count <= 0)
            {
                return true;
            }
            else
                return false;
        }
    }


}
