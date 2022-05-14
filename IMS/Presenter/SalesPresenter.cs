﻿using IMS.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Models;
using IMS._Repositories;
using System.Windows.Forms;
using System.Data;

namespace IMS.Presenter
{
    public class SalesPresenter
    {
        private ISalesView view;
        private ISalesRepository repository;
        private BindingSource salesBindingSource;
        private BindingSource prodductBindindSource;
        private DataTable table;

        private IEnumerable<SalesModel> salesList;

        //Constructor
        public SalesPresenter(ISalesView view, ISalesRepository repository)
        {
            this.salesBindingSource = new BindingSource();
            this.prodductBindindSource = new BindingSource();
            this.view = view;
            this.repository = repository;

            table = new DataTable();
            table.Columns.Add("ID");
            table.Columns.Add("Quantity");
            table.Columns.Add("Price");

            //Subscribe event handler method to view events
            this.view.SearchEvent += SearchSale;
            this.view.AddNewEvent += AddProductToCart;
            this.view.UpdateEvent += UpdateCart;
            this.view.ProcessEvent += ProcessSales;
            this.view.CancelEvent += CancelAction;

            //Set product binding source
            this.view.SetSalesListBindingSource(salesBindingSource);
            this.view.SetCartProductsBindingSource(prodductBindindSource);

            //Load data to the product list
            LoadAllSalesList();

            //Show View
            this.view.Show();
        }

        private void LoadAllSalesList()
        {
            salesList = repository.GetAll();
            salesBindingSource.DataSource = salesList;  //Set data source.
        }

        private void SearchSale(object sender, EventArgs e)
        {
            bool emptyValue = string.IsNullOrWhiteSpace(this.view.SearchValue);
            if (emptyValue == false)
                salesList = repository.GetByValue(this.view.SearchValue);
            else salesList = repository.GetAll();
            salesBindingSource.DataSource = salesList;
        }

        private void AddProductToCart(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            int product_id = Convert.ToInt32(view.Id);
            int quantity = Convert.ToInt32(view.Quantity);
            int price = Convert.ToInt32(view.Selling_Price);
            table.Rows.Add(product_id, quantity, price);
            prodductBindindSource.DataSource = table;
        }

        private void UpdateCart(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ProcessSales(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            string phone = (string)view.PhoneNo;
            string received_amount = (string)view.ReceivedAmount;
            int isSuccessfull=repository.ProcessSale(table, phone, received_amount);
            if (isSuccessfull == 1)
            {
                table.Clear();
                view.PhoneNo = "Phone No";
                view.ReceivedAmount = "Received Amount";
            }
            
                
        }

        private void CancelAction(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            table.Clear();
            view.PhoneNo = "Phone No";
            view.ReceivedAmount = "Received Amount";
        }
    }
}
