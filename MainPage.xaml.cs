﻿using GoldStarr_Trading.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


namespace GoldStarr_Trading
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        StoreClass store;
        private App _app;

        public MainPage()
        {
            this.InitializeComponent();
            store = new StoreClass();
            _app = (App)App.Current;

            #region OLD
            //DataContext = this;

            //CustomerOrders = new ObservableCollection<CustomerOrderClass>();

            //InStockList.ItemsSource = store.GetCurrentStockList();
            //StockToAddList.ItemsSource = store.GetCurrentDeliverysList();
            //CustomersTabComboBoxCustomerName.ItemsSource = store.GetCurrentCustomerList();
            //CustomerView.ItemsSource = store.GetCurrentCustomerList();
            //CustomerList = new ObservableCollection<CustomerClass>(store.GetCurrentCustomerList());

            //StockList = new ObservableCollection<StockClass>(store.GetCurrentStockList());
            //CustomerOrders = new ObservableCollection<CustomerOrderClass>(store.GetCurrentCustomerOrdersList());
            #endregion

        }
        #region Events

        private void AddOrderContent_Click(object sender, RoutedEventArgs e)
        {
            var parent = (sender as Button).Parent;
            CustomerClass customerOrderer = null;
            StockClass stockOrder = null;
            DateTime orderDate = DateTime.UtcNow;

            string orderQuantity = OrderQuantity.Text;
            int.TryParse(orderQuantity, out int amount);
            customerOrderer = (CustomerClass)CreateOrderTabCustomersComboBox.SelectedValue;
            stockOrder = (StockClass)CreateOrderTabItemComboBox.SelectedValue;

            if (customerOrderer == null || stockOrder == null)
            {
                MessageToUser("You must choose a customer and an item");
            }
            else if (orderQuantity == "" || orderQuantity == "" || amount == 0)
            {

                MessageToUser("You must enter an integer");
            }
            else
            {

                if (orderQuantity != "" && stockOrder.Qty - amount >= 0)
                {
                    // if no orders are present, simply add an order to the collection.
                    if (_app.GetDefaultCustomerOrdersList().Count == 0)
                    {
                        //stockOrder.Qty -= amount;
                        store.RemoveFromStock(stockOrder, amount);

                        StockClass order = new StockClass(stockOrder.ItemName, stockOrder.Supplier, amount);

                        //_app.GetDefaultCustomerOrdersList().Add(new CustomerOrderClass(customerOrderer, order, orderDate));
                        store.CreateOrder(customerOrderer, order);

                        //MessageToUser($"You have successfully created a new Customer order for: \n{customerOrderer.CustomerName} with {amount} {stockOrder.ItemName} in it");
                        MessageToUser($"You have successfully created a new Customer order \n\nCustomer: {customerOrderer.CustomerName} \nItem: {order.ItemName} \nAmount: {order.Qty} \nOrderdate: {orderDate.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")}");

                        CreateOrderTabCustomersComboBox.SelectedIndex = -1;
                        CreateOrderTabItemComboBox.SelectedIndex = -1;
                        OrderQuantity.Text = "";



                    }

                    // Otherwise create a new order object, prepared for future functionality
                    else
                    {
                        //stockOrder.Qty -= amount;
                        store.RemoveFromStock(stockOrder, amount);

                        StockClass order = new StockClass(stockOrder.ItemName, stockOrder.Supplier, amount);

                        _app.GetDefaultCustomerOrdersList().Add(new CustomerOrderClass(customerOrderer, order, orderDate));

                        //MessageToUser($"You have successfully created a new Customer order for: \n{customerOrderer.CustomerName} with {amount} {stockOrder.ItemName} in it");
                        MessageToUser($"You have successfully created a new Customer order \n\nCustomer: {customerOrderer.CustomerName} \nItem: {order.ItemName} \nAmount: {order.Qty} \nOrderdate: {orderDate.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")}");

                        CreateOrderTabCustomersComboBox.SelectedIndex = -1;
                        CreateOrderTabItemComboBox.SelectedIndex = -1;
                        OrderQuantity.Text = "";

                    }

                    #region Code for Release 2
                    //for (int i = 0; i < CustomerOrders.Count; ++i)
                    //{
                    //if (CustomerOrders[i].Customer == customerOrderer)
                    //{
                    //    stockOrder.Qty -= amount;
                    //    StockClass orderToUpdate = new StockClass(stockOrder.ItemName, stockOrder.Supplier, amount);
                    //    CustomerOrders.Add(new CustomerOrderClass(customerOrderer, stockClass));

                    //    //StoreClass.RemoveFromStock(orderToUpdate, amount);

                    //    //MessageToUser($"You have successfully created a new Customer order for: \n{customerOrderer.CustomerName} with {amount} {stockOrder.ItemName} in it");
                    //    MessageToUser($"You have successfully created a new Customer order \n\nCustomer: {customerOrderer.CustomerName} \nItem: {stockOrder.ItemName} \nAmount: {amount}");

                    //    OrderQuantity.Text = "";
                    //}
                    //else
                    //{
                    //stockOrder.Qty -= amount;
                    //StockClass orderToAdd = new StockClass(stockOrder.ItemName, stockOrder.Supplier, amount);
                    //stockClass.Add(orderToAdd);

                    //CustomerOrders.Add(new CustomerOrderClass(customerOrderer, stockClass));

                    ////MessageToUser($"You have successfully created a new Customer order for: \n{customerOrderer.CustomerName} with {amount} {stockOrder.ItemName} in it");
                    //MessageToUser($"You have successfully created a new Customer order \n\nCustomer:{customerOrderer.CustomerName} \nItem: {orderToAdd.ItemName} \nAmount: {orderToAdd.Qty}");

                    //OrderQuantity.Text = "";
                    //break;
                    //}
                    //}
                    #endregion
                }
                else
                {
                    int currQ = _app.QueuedOrders.Count + 1;
                    store.CreateOrder(customerOrderer, stockOrder, amount, currQ );
                    MessageToUser($"You have successfully created a new Customer order \n\nCustomer: {customerOrderer.CustomerName} \nItem: {stockOrder.ItemName} \nAmount: {amount} \nOrderdate: {orderDate.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")}" +
                        $"\nYour order is placed at number {currQ} in the queue.");
                    OrderQuantity.Text = "";
                }
            }



        }

        private void BtnAddDeliveredMerchandise_Click(object sender, RoutedEventArgs e)
        {
            var parent = (sender as Button).Parent;


            TextBox valueToAdd = parent.GetChildrenOfType<TextBox>().First(x => x.Name == "TxtBoxAddQty");
            TextBlock valueToCheck = parent.GetChildrenOfType<TextBlock>().First(x => x.Name == "QTY");
            TextBlock itemToAdd = parent.GetChildrenOfType<TextBlock>().First(x => x.Name == "ItemName");

            string toConvert = valueToAdd.Text;
            int intValueToAdd = 0;
            int intValueToCheck = Convert.ToInt32(valueToCheck.Text);

            if (int.TryParse(toConvert, out intValueToAdd))
            {
                if (intValueToAdd > intValueToCheck)
                {
                    MessageToUser($"Enter the correct number of stock to submit, maximum number to submit is: {intValueToCheck} ");
                    valueToAdd.Text = "";
                }
                else
                {
                    StockClass merch = null;

                    foreach (var item in _app.GetDefaultStockList())
                    {
                        if (item.ItemName == itemToAdd.Text)
                        {
                            merch = item;
                        }
                    }

                    store.AddToStock(merch, intValueToAdd);
                    


                    MessageToUser($"You have added: {valueToAdd.Text} {itemToAdd.Text} to your stock");
                    valueToAdd.Text = "";

                }
            }
            else
            {
                MessageToUser("You must enter an integer");
            }


            #region For Debug
            Debug.WriteLine(valueToAdd.Text);
            Debug.WriteLine(valueToCheck.Text);
            #endregion

        }

        private void CustomersTabComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string customerName = e.AddedItems[0].ToString();

            CustomerClass newCustomer = _app.GetDefaultCustomerList().First(x => x.CustomerName == customerName);
            CustomerName.Text = newCustomer.CustomerName;
            CustomerPhoneNumber.Text = newCustomer.CustomerPhone;
            CustomerAddress.Text = newCustomer.CustomerAddress;
            CustomerZipCode.Text = newCustomer.CustomerZipCode;
            CustomerCity.Text = newCustomer.CustomerCity;
        }


        private void CustomerAddButton_Click(object sender, RoutedEventArgs e)
        {

            if (AddNewCustomerName.Text == "" || AddNewCustomerName.Text == " " || AddNewCustomerPhoneNumber.Text == "" || AddNewCustomerPhoneNumber.Text == " " || AddNewCustomerAddress.Text == "" || AddNewCustomerAddress.Text == " " || AddNewCustomerZipCode.Text == "" || AddNewCustomerZipCode.Text == "" || AddNewCustomerCity.Text == "" || AddNewCustomerCity.Text == " ")
            {
                MessageToUser("All textboxes must be filled in");

            }
            else
            {
                #region Variables
                string name = AddNewCustomerName.Text;
                string phone = AddNewCustomerPhoneNumber.Text;
                string address = AddNewCustomerAddress.Text;
                string zipCode = AddNewCustomerZipCode.Text;
                string city = AddNewCustomerCity.Text;
                #endregion

                #region Regex
                Regex regexToCheckName = new Regex(@"^([A-ZÅÄÖ]\w*[a-zåäö]+\s[A-ZÅÄÖ]\w*[a-zåäö]+)$");                                              //Firstname and Lastname must start with capitol letters
                Regex regexToCheckPhone = new Regex(@"^(\+?\d{2}\-?\s?)?\d{4}\-?\s?\d{3}\-?\s?\d{3}$");                                             //Must be in these formats +46 0707-123 456, +46 0707-123456, +46 0707-123-456, 0707 123 456
                Regex regexToCheckAddress = new Regex(@"^(([A-ZÅÄÖ]\w*[a-zåäö]+|[A-ZÅÄÖ]\w*[a-zåäö]+\s[a-zA-ZåäöÅÄÖ]\w*[a-zåäö]+)+\s?\d{0,3})+$");  //Adress must start with capitol letter with optional second part and digits at end
                Regex regexToCheckZipCode = new Regex(@"^\d{3}\s?\d{2}$");                                                                          //Must be in the format xxx xx
                Regex regexToCheckCity = new Regex(@"^([A-ZÅÄÖ]\w*[a-zåäö]+|[A-ZÅÄÖ]\w*[a-zåäö]+\s[a-zA-ZåäöÅÄÖ]+)$");                              //Must start with capitol letter and can have a optional second part
                #endregion

                #region Input Validation
                if (!regexToCheckName.IsMatch(name))
                {
                    MessageToUser("Enter first and last name in the correct format names starting with capitol letters: \n\nEx: Firstname Lastname");
                    return;
                }
                if (!regexToCheckPhone.IsMatch(phone))
                {
                    MessageToUser("Enter phone number in the correct format: \n\nEx: +46 0707-123 456, +46 0707-123456, +46 0707-123-456, 0707-123 456, 0707 123 456");
                    return;
                }
                if (!regexToCheckAddress.IsMatch(address))
                {
                    MessageToUser("Enter address in the correct format. Every word must start with a capitol letter: \n\nEx: Street + no, Street, Two Word Street: Capitol Road + no, Two Word Street: Capitol Road");
                    return;
                }
                if (!regexToCheckZipCode.IsMatch(zipCode))
                {
                    MessageToUser("Enter zipcode in the correct format: \n\nEx: 123 45");
                    return;
                }
                if (!regexToCheckCity.IsMatch(city))
                {
                    MessageToUser("Enter city name in the correct format. Every word must start with a capitol letter: \n\nEx: City, Two Word City: Capitol City");
                    return;
                }
                #endregion


                MessageToUser($"You have successfully added a new customer to your customer list \n\nCustomer name: {name}");

                _app.GetDefaultCustomerList().Add(new CustomerClass(name, address, zipCode, city, phone));

                
                #region Reset TextBoxes
                AddNewCustomerName.Text = "";
                AddNewCustomerPhoneNumber.Text = "";
                AddNewCustomerAddress.Text = "";
                AddNewCustomerZipCode.Text = "";
                AddNewCustomerCity.Text = "";
                #endregion
            }

        }

        private void PendingOrdersBtnSend_Click(object sender, RoutedEventArgs e)
        {
            var parent = (sender as Button).Parent;
            TextBlock cn = parent.GetChildrenOfType<TextBlock>().First(x => x.Name == "PendingOrdersCustomerName");
            QueuedOrder queuedOrder = store.FindQueued(cn.Text);
            store.SendOrder(queuedOrder);
            MessageToUser("Sent order!");

        }

        #endregion


        #region Methods

        public static async void MessageToUser(string inputMessage)
        {
            var message = new MessageDialog(inputMessage);
            await message.ShowAsync();
        }

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

        #endregion

    }

    #region Help Class
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
    #endregion

}
