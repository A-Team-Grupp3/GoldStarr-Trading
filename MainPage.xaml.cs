﻿using GalaSoft.MvvmLight.Command;
using GoldStarr_Trading.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps.Guidance;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace GoldStarr_Trading
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        #region Properties
        public ICommand AddButtonCommand { get; set; }
        #endregion

        #region Collections
        ObservableCollection<CustomerClass> CustomerList { get; set; }  //= new List<CustomerClass>();
        ObservableCollection<StockClass> StockList { get; set; }
        //public ObservableCollection<CustomerClass> CustomerList;

        #endregion

        public MainPage()
        {

            this.InitializeComponent();

            DataContext = this;

            StoreClass store = new StoreClass();

            InStockList.ItemsSource = store.GetCurrentStockList();
            StockToAddList.ItemsSource = store.GetCurrentDeliverysList();
            CustomerList = new ObservableCollection<CustomerClass>(store.GetCurrentCustomerList());
            StockList = new ObservableCollection<StockClass>(store.GetCurrentStockList());

            #region OLD            
            //this.CreateOrderTabCustomersComboBox.ItemsSource = store.GetCurrentStockList();
            //InStockList.ItemsSource = dataSets.GetDefaultStockList();
            //StockToAddList.ItemsSource = dataSets.GetDefaultDeliverysList();

            //CustomerList = store.GetCurrentCustomerList();
            //CustomerList = dataSets.GetDefaultCustomerList();
            #endregion

            #region OLD
            //PopulateCustomerComboBox(store);
            //PopulateCreateOrderComboBox(store);
            #endregion


        }



        #region OLD Methods
        //private void PopulateCustomerComboBox(StoreClass store)
        //{
        //    List<string> customers = new List<string>();


        //    foreach (var item in store.Customer)
        //    {
        //        customers.Add(item.Name);
        //    }

        //    //DataContext = customers;

        //    this.CustomersTabComboBox.ItemsSource = customers;
        //    this.CreateOrderTabCustomersComboBox.ItemsSource = customers;
        //}

        //private void PopulateCreateOrderComboBox(StoreClass store)
        //{
        //    List<string> merchandise = new List<string>();


        //    foreach (var item in store.Stock)
        //    {
        //        merchandise.Add(item.ItemName);
        //    }

        //    this.CreateOrderTabItemComboBox.ItemsSource = merchandise;
        //}
        #endregion


        #region Events
        private async void BtnAddDeliveredMerchandise_Click(object sender, RoutedEventArgs e)
        {
             

            var parent = (sender as Button).Parent;
            //string customerName = (string)e.OriginalSource;
            TextBox value = parent.GetChildrenOfType<TextBox>().First(x => x.Name == "TxtBoxAddQty");
            //TextBlock value2 = parent.GetChildrenOfType<TextBlock>().First(x => x.Name == "TxtBoxAddQty");


            int valueToAdd = Convert.ToInt32(value.Text);



            //var parent2 = (sender as TextBlock).Parent;
            TextBlock value2 = parent.GetChildrenOfType<TextBlock>().First(x => x.Name == "ItemName");


            var message = new MessageDialog($"You have added: {value.Text} {value2.Text} to the list");
            await message.ShowAsync();


            Debug.WriteLine(value.Text);
        }
        private void CustomersTabComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            string customerName = e.AddedItems[0].ToString();

            CustomerClass newCustomer = CustomerList.First(x => x.CustomerName == customerName);
            CustomerName.Text = newCustomer.CustomerName;
            CustomerPhoneNumber.Text = newCustomer.CustomerPhone;
            CustomerAddress.Text = newCustomer.CustomerAddress;
            CustomerZipCode.Text = newCustomer.CustomerZipCode;
            CustomerCity.Text = newCustomer.CustomerCity;



            #region OLD
            //switch (customerName)
            //{
            //    case "Lisa Underwood":
            //        //var message = new MessageDialog(DataContextProperty.ToString());
            //        var message = new MessageDialog("CustomersTab ComboBox Changed");
            //        await message.ShowAsync();
            //        break;
            //}

            #endregion
        }

        private void CreateOrderTabCustomersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string customerName = e.AddedItems[0].ToString();

            #region OLD
            //switch (customerName)
            //{
            //    case "Lisa Underwood":
            //        //var message = new MessageDialog(DataContextProperty.ToString());
            //        var message = new MessageDialog("CreateOrders Tab ComboBox Changed");
            //        await message.ShowAsync();
            //        break;
            //   }
            #endregion
        }

        private void CreateOrderTabItemComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        #endregion

        #region PropertyChangedEventHandler
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion


    }

    public static class Extensions
    {
        public static IEnumerable<T> GetChildrenOfType<T>(this DependencyObject start) where T : class
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var item = queue.Dequeue();

                var realItem = item as T;
                if (realItem != null)
                {
                    yield return realItem;
                }

                int count = VisualTreeHelper.GetChildrenCount(item);
                for (int i = 0; i < count; i++)
                {
                    queue.Enqueue(VisualTreeHelper.GetChild(item, i));
                }
            }
        }
    }
}
