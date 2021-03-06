﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Accounts_Pos.Helpers;
using Accounts_Pos.View.Customer;
using Newtonsoft.Json;
using System.ComponentModel;
using Accounts_Pos.Model;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Accounts_Pos.View.Supplier;


namespace Accounts_Pos.ViewModel.Supplier
{
    class SupplierDocumentViewModel : DependencyObject, INotifyPropertyChanged, IModalService
    {
        // Import C++ dll
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHELLEXECUTEINFO
        {
            public int cbSize;
            public uint fMask;
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpVerb;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpParameters;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }

        private const int SW_SHOW = 5;
        private const uint SEE_MASK_INVOKEIDLIST = 12;
        public static bool ShowFileProperties(string Filename)
        {
            SHELLEXECUTEINFO info = new SHELLEXECUTEINFO();
            info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(info);
            info.lpVerb = "properties";
            info.lpFile = Filename;
            info.nShow = SW_SHOW;
            info.fMask = SEE_MASK_INVOKEIDLIST;
            return ShellExecuteEx(ref info);
        }

        //


        public HttpResponseMessage response;
        DocumentModel[] data = null;

        ObservableCollection<DocumentModel> _ListGrid_Temp = new ObservableCollection<DocumentModel>();

        public SupplierDocumentViewModel()
        {
            selectSupplierDocument = new DocumentModel();
            //SUPPLIER_CODE=App.Current.Properties["Supplier_Code"].ToString();
            //SUPPLIER_NAME=App.Current.Properties["Supplier_Name"].ToString();
            //IS_CUSTOMER=Convert.ToBoolean(App.Current.Properties["IS_CUSTOMER"]);
            //string SupplierCode = App.Current.Properties["Supplier_Code"].ToString();
            string SupplierCode = "mmm";
            GetDocuments(SupplierCode);
        }

        private DocumentModel _selectSupplierDocument;
        public DocumentModel selectSupplierDocument
        {
            get { return _selectSupplierDocument; }
            set
            {
                if (_selectSupplierDocument != value)
                {
                    _selectSupplierDocument = value;
                    OnPropertyChanged("selectSupplierDocument");
                }
            }
        }

        #region Document Upload
        public ICommand _DocumentUpload;
        public ICommand DocumentUpload
        {
            get
            {
                if (_DocumentUpload == null)
                {
                    _DocumentUpload = new DelegateCommand(Document_Upload);
                }
                return _DocumentUpload;
            }
        }
        public async void Document_Upload()
        {
            string SupplierCode = App.Current.Properties["Supplier_Code"].ToString();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {

                if (File.Exists(openFileDialog.FileName))
                {
                    string docfilepath = openFileDialog.FileName.ToString();
                    var docFile = new System.IO.FileInfo(docfilepath);
                    string file = docFile.Name;
                    var applicationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);


                    // get your 'Uploaded' folder

                    var dir = new System.IO.DirectoryInfo(System.IO.Path.Combine(applicationPath, "uploaded"));
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }
                    // Copy file to your folder
                    docFile.CopyTo(System.IO.Path.Combine(dir.FullName + "\\", file), true);

                    //Prepare  Document Model
                    DocumentModel doc = new DocumentModel();
                    doc.FILE_NAME = file;
                    doc.TYPE_ID_DOC = docFile.Extension;
                    doc.SIZE = docFile.Length.ToString() + " bytes";
                    doc.DATE = docFile.LastWriteTime.ToString();
                    doc.SUPPLIER_CODE = SupplierCode;

                    //Insert data
                    try
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
                        client.BaseAddress = new Uri(GlobalData.gblApiAdress);
                        var response = await client.PostAsJsonAsync("api/DocumentAPI/CreateSupplierDocument/", doc);


                        //////
                        //Refresh List
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    GetDocuments(SupplierCode);
                }
            }


        }
        #endregion

        #region Document Open
        public ICommand _DocumentOpen;
        public ICommand DocumentOpen
        {
            get
            {
                if (_DocumentOpen == null)
                {
                    _DocumentOpen = new DelegateCommand(Document_Open);
                }
                return _DocumentOpen;
            }
        }
        public void Document_Open()
        {
            if (selectSupplierDocument.DOCUMENT_ID > 0)
            {
                try
                {
                    string fileName = selectSupplierDocument.FILE_NAME;
                    var applicationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    var dir = new System.IO.DirectoryInfo(System.IO.Path.Combine(applicationPath, "uploaded"));
                    var filePath = System.IO.Path.Combine(dir.FullName + "\\", fileName);

                    ProcessStartInfo pi = new ProcessStartInfo(filePath);
                    pi.Arguments = Path.GetFileName(filePath);
                    pi.UseShellExecute = true;
                    pi.WorkingDirectory = Path.GetDirectoryName(filePath);
                    pi.FileName = filePath;
                    pi.Verb = "OPEN";
                    Process.Start(pi);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                MessageBox.Show("Select Document");
            }
        }

        #endregion

        #region Document Properties
        public ICommand _DocumentProperties;
        public ICommand DocumentProperties
        {
            get
            {
                if (_DocumentProperties == null)
                {
                    _DocumentProperties = new DelegateCommand(Document_Properties);
                }
                return _DocumentProperties;
            }
        }
        public void Document_Properties()
        {
            if (selectSupplierDocument.DOCUMENT_ID > 0)
            {
                try
                {
                    string fileName = selectSupplierDocument.FILE_NAME;
                    var applicationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    var dir = new System.IO.DirectoryInfo(System.IO.Path.Combine(applicationPath, "uploaded"));
                    var filePath = System.IO.Path.Combine(dir.FullName + "\\", fileName);

                    ShowFileProperties(filePath);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                MessageBox.Show("Select Document");
            }
        }
        #endregion

        #region Document Delete

        public ICommand _DocumentDelete;
        public ICommand DocumentDelete
        {
            get
            {
                if (_DocumentDelete == null)
                {
                    _DocumentDelete = new DelegateCommand(Document_Delete);
                }
                return _DocumentDelete;
            }
        }
        public async void Document_Delete()
        {
            string SupplierCode = App.Current.Properties["Supplier_Code"].ToString();
            if (selectSupplierDocument.DOCUMENT_ID > 0)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure to delete this document  " + selectSupplierDocument.FILE_NAME + "?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {

                    var id = selectSupplierDocument.DOCUMENT_ID;
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    client.BaseAddress = new Uri(GlobalData.gblApiAdress);
                    HttpResponseMessage response = client.GetAsync("api/DocumentAPI/DeleteSupplierDocument?id=" + id + "").Result;
                    if (response.StatusCode.ToString() == "OK")
                    {
                        MessageBox.Show("Document Deleted Successfully");
                        //Refresh List

                        GetDocuments(SupplierCode);

                    }
                }

            }
            else
            {
                MessageBox.Show("Select Document");
            }
        }

        #endregion




        private string _FILE_NAME;
        public string FILE_NAME
        {
            get
            {
                return selectSupplierDocument.FILE_NAME;
            }
            set
            {
                selectSupplierDocument.FILE_NAME = value;
                OnPropertyChanged("FILE_NAME");

            }
        }

        private string _DATE;
        public string DATE
        {
            get
            {
                return selectSupplierDocument.DATE;
            }
            set
            {
                selectSupplierDocument.DATE = value;
                OnPropertyChanged("DATE");

            }
        }

        private string _TYPE_ID_DOC;
        public string TYPE_ID_DOC
        {
            get
            {
                return selectSupplierDocument.TYPE_ID_DOC;
            }
            set
            {
                selectSupplierDocument.TYPE_ID_DOC = value;
                OnPropertyChanged("TYPE_ID_DOC");

            }
        }

        private string _SIZE;
        public string SIZE
        {
            get
            {
                return selectSupplierDocument.SIZE;
            }
            set
            {
                selectSupplierDocument.SIZE = value;
                OnPropertyChanged("SIZE");

            }
        }

        private string _TAG;
        public string TAG
        {
            get
            {
                return selectSupplierDocument.TAG;
            }
            set
            {
                selectSupplierDocument.TAG = value;
                OnPropertyChanged("TAG");

            }
        }

        private string _CUSTOMER_CODE;
        public string CUSTOMER_CODE
        {
            get
            {
                return selectSupplierDocument.CUSTOMER_CODE;
            }
            set
            {
                selectSupplierDocument.CUSTOMER_CODE = value;
                OnPropertyChanged("CUSTOMER_CODE");

            }
        }

        private string _DOCUMENT_ID;
        public string DOCUMENT_ID
        {
            get
            {
                return selectSupplierDocument.DOCUMENT_ID.ToString();
            }
            set
            {
                selectSupplierDocument.DOCUMENT_ID = Convert.ToInt32(value);
                OnPropertyChanged("DOCUMENT_ID");

            }
        }

        public ObservableCollection<DocumentModel> _ListGrid { get; set; }
        public ObservableCollection<DocumentModel> ListGrid
        {
            get
            {
                return _ListGrid;
            }
            set
            {
                this._ListGrid = value;
                OnPropertyChanged("ListGrid");
            }
        }

        public async Task<ObservableCollection<DocumentModel>> GetDocuments(string SupplierCode)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(GlobalData.gblApiAdress);
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                response = client.GetAsync("api/DocumentAPI/SupplierDocumentList?SupplierCode=" + SupplierCode + "").Result;
                _ListGrid_Temp.Clear();
                if (response.IsSuccessStatusCode)
                {
                    data = JsonConvert.DeserializeObject<DocumentModel[]>(await response.Content.ReadAsStringAsync());
                    // EmployeeData = new List<EmployeeModel>();
                    int x = 0;

                    for (int i = 0; i < data.Length; i++)
                    {
                        x++;
                        string date = null;
                        if (data[i].DATE != null)
                        {
                            date = data[i].DATE.ToString();
                        }
                        _ListGrid_Temp.Add(new DocumentModel
                        {

                            FILE_NAME = data[i].FILE_NAME,
                            DATE = date,
                            TYPE_ID_DOC = data[i].TYPE_ID_DOC,
                            SIZE = data[i].SIZE,
                            TAG = data[i].TAG,
                            SUPPLIER_CODE = data[i].SUPPLIER_CODE,
                            DOCUMENT_ID = data[i].DOCUMENT_ID

                        });
                    }

                }

                ListGrid = _ListGrid_Temp;
                return new ObservableCollection<DocumentModel>(_ListGrid_Temp);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #region Insert Data
        public ICommand _InsertData;
        public ICommand InsertData
        {
            get
            {
                if (_InsertData == null)
                {
                    _InsertData = new DelegateCommand(Insert_Data);
                }
                return _InsertData;
            }
        }


        public async void Insert_Data()
        {
            Cancel_Supplier();
            MessageBox.Show("Supplier Documents Added Successfully...");
            SupplierListing _SL = new SupplierListing();
            _SL.ShowDialog();

        }

        #endregion


        #region Cancel Data

        public ICommand _Cancel;
        public ICommand Cancel
        {
            get
            {
                if (_Cancel == null)
                {
                    _Cancel = new DelegateCommand(Cancel_Supplier);
                }
                return _Cancel;
            }
        }



        public void Cancel_Supplier()
        {
            foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                }
            }

        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private Stack<BackNavigationEventHandler> _backFunctions = new Stack<BackNavigationEventHandler>();

        void IModalService.NavigateTo(Window uc, BackNavigationEventHandler backFromDialog)
        {

        }

        void IModalService.GoBackward(bool dialogReturnValue)
        {
        }


        public static IModalService ModalService
        {
            get { return (IModalService)Application.Current.MainWindow; }
        }

    }
}
