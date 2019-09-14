using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KarmasisDomain
{
    //Bir Domain objesidir, DomainModelBase classından türeyerek INotifyPropertyChanged yeteneğini propertylerine kazandırır,
    //DataTransfer objesi olarak görev yapar.
    public class Ticket : DomainModelBase
    {

        int id;
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                if (id != value)
                {
                    int temp = id;
                    id = value;
                    RaisePropertyChanged("ID", temp, value);
                }
            }
        }
        string subject;
        public string Subject
        {
            get
            {
                return subject;
            }
            set
            {
                if (subject != value)
                {
                    string temp = subject;
                    subject = value;
                    RaisePropertyChanged("Subject", temp, value);
                }
            }
        }

        string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (description != value)
                {
                    string temp = description;
                    description = value;
                    RaisePropertyChanged("Description", temp, value);
                }
            }
        }

        string customerName;
        public string CustomerName
        {
            get
            {
                return customerName;
            }
            set
            {
                if (customerName != value)
                {
                    string temp = customerName;
                    customerName = value;
                    RaisePropertyChanged("CustomerName", temp, value);
                }
            }
        }

        string productName;
        public string ProductName
        {
            get
            {
                return productName;
            }
            set
            {
                if (productName != value)
                {
                    string temp = productName;
                    productName = value;
                    RaisePropertyChanged("ProductName", temp, value);
                }
            }
        }

        string solvedBy;
        public string SolvedBy
        {
            get
            {
                return solvedBy;
            }
            set
            {
                if (solvedBy != value)
                {
                    string temp = solvedBy;
                    solvedBy = value;
                    RaisePropertyChanged("SolvedBy", temp, value);
                }
            }
        }

        bool solved;
        public bool Solved
        {
            get
            {
                return solved;
            }
            set
            {
                if (solved != value)
                {
                    bool temp = solved;
                    solved = value;
                    RaisePropertyChanged("Solved", temp, value);
                }
            }
        }

        DateTime timeCreated;
        public DateTime TimeCreated
        {
            get
            {
                return timeCreated;
            }
            set
            {
                if (timeCreated != value)
                {
                    DateTime temp = timeCreated;
                    timeCreated = value;
                    RaisePropertyChanged("TimeCreated", temp, value);
                }
            }
        }

    }
}
