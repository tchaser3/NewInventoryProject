/* Title:           Logon
 * Date:            5-11-16
 * Author:          Terry Holmes
 *
 * Description:     This is the logon form for the application */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessagesDLL;
using DataValidationDLL;
using EmployeeDLL;
using EventLogDLL;
using LastTransactionDLL;


namespace NewInventoryProject
{
    public partial class Logon : Form
    {
        //setting up the classes
        MessagesClass TheMessagesClass = new MessagesClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        LastTransactionClass TheLastTransactionClass = new LastTransactionClass();

        //setting the global variables
        public static string mstrErrorMessage;
        public static int mintWarehouseEmployeeID;
        public static int mintEmployeeID;
        public static int mintPartsWarehouseID;
        public static string mstrLastTransactionSummary;
        public static string mstrSelectedButton;
        public static int mintInternalProjectID;
        public static string mstrTWCProjectID;
        public static string mstrPartNumber;
        public static string mintQuantity;
        public static string mstrMSRNumber;
        public static string mstrWarehouse;
        public static DateTime mdatTransactionDate;
        int mintNumberOfMisses;
        public static string mstrEmployeeGroup;
        public static string mstrLastName;
        public static string mstrFirstName;

        public Logon()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //this will close the program
            TheMessagesClass.CloseTheProgram();
        }

        private void btnLogon_Click(object sender, EventArgs e)
        {
            //setting the local variables
            bool blnFatalError = false;
            bool blnThereIsAProblem = false;
            string strValueForValidation;
            bool blnVerifyLogonIn;

            //beginning data validation
            strValueForValidation = txtEmployeeID.Text;
            blnFatalError = TheDataValidationClass.VerifyIntegerData(strValueForValidation);
            if(blnFatalError == true)
            {
                blnThereIsAProblem = true;
                mstrErrorMessage = mstrErrorMessage + "The Employee ID is not an Integer\n";
            }
            else
            {
                mintEmployeeID = Convert.ToInt32(strValueForValidation);
            }
            mstrLastName = txtLogonLastName.Text;
            blnFatalError = TheDataValidationClass.VerifyTextData(mstrLastName);
            if(blnFatalError == true)
            {
                blnThereIsAProblem = true;
                mstrErrorMessage = mstrErrorMessage + "The Last Name Was Not Entered\n";
            }

            //message to user if validation fails
            if(blnThereIsAProblem == true)
            {
                TheMessagesClass.ErrorMessage(mstrErrorMessage);
                return;
            }

            //verifying employee
            blnVerifyLogonIn = TheEmployeeClass.VerifyLogon(mintEmployeeID, mstrLastName);

            if(blnVerifyLogonIn == false)
            {
                //calling sub routine
                LogonFailed();
            }
            else
            {
                //getting the group
                mstrEmployeeGroup = TheEmployeeClass.FindEmployeeGroup(mintEmployeeID);

                //determining if user is an administrator
                if (mstrEmployeeGroup != "ADMIN")
                {
                    LogonFailed();
                }
                else
                {
                    //getting the first name
                    mstrFirstName = TheEmployeeClass.FindEmployeeFirstNamewithID(mintEmployeeID);

                    //message to user
                    mstrErrorMessage = mstrFirstName + " " + mstrLastName + " Has Successfully Logged In";

                    TheMessagesClass.InformationMessage(mstrErrorMessage);

                    //lastransaction entry
                    TheLastTransactionClass.CreateLastTransactionEntry(mintEmployeeID, mstrErrorMessage);

                    MainMenu MainMenu = new MainMenu();
                    MainMenu.Show();
                    this.Hide();
                }
            }
        }

        private void Logon_Load(object sender, EventArgs e)
        {
            //setting variables
            mintNumberOfMisses = 0;
            mstrErrorMessage = "";
        }
        private void LogonFailed()
        {
            //message to user
            TheMessagesClass.InformationMessage("Either The Logon Information Does Not Match\n Or You Are Not An Administrator");

            //incrementing the number of missess
            mintNumberOfMisses++;

            //clearing the controls
            txtEmployeeID.Text = "";
            txtLogonLastName.Text = "";

            //if statements
            if(mintNumberOfMisses == 3)
            {
                //Messsage to user
                TheMessagesClass.ErrorMessage("There Have Been Three Attempts To Log In, The Application Will Close");

                //event log entry
                TheEventLogClass.CreateEventLogEntry("There Have Been Three Attempts To Log Into the New Inventory Program, The Application Closed");
                
                Application.Exit();
            }
        }
    }
}
