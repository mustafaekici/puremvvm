using Elasticsearch.Net;
using Nest;
using ServiceLibrary;
using System;

namespace WcfService
{
    public class ElasticConnection
    {

        public static ElasticClient EsClient()
        {
            ConnectionSettings connectionSettings;
            ElasticClient elasticClient;
            StaticConnectionPool connectionPool;

            //Connection string for Elasticsearch
            /*connectionSettings = new ConnectionSettings(new Uri("http://localhost:9200/")); //local PC
            elasticClient = new ElasticClient(connectionSettings);*/

            //Multiple node for fail over (cluster addresses)
            var nodes = new Uri[]
            {
                new Uri("http://localhost:9200/"),
                //new Uri("Add server 2 address")   //Add cluster addresses here
                //new Uri("Add server 3 address")
            };

            connectionPool = new StaticConnectionPool(nodes);
            connectionSettings = new ConnectionSettings(connectionPool).DefaultIndex("tickets");
          
        
            connectionSettings.DisableDirectStreaming();
            
            elasticClient = new ElasticClient(connectionSettings);
            elasticClient.CreateIndex("tickets", c => c
           .Mappings(m => m
               .Map<Ticket>(mm => mm
                   .AutoMap()
               )
           )
       );
            return elasticClient;
        }
    }
}