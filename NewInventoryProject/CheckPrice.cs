/* Title:           Check Price
 * Date:            5-15-16
 * Author:          Terry Holmes
 *
 * Description:     This form will check the price for a null value */

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
using PartNumberDLL;
using DataValidationDLL;
using EventLogDLL;

namespace NewInventoryProject
{
    public partial class CheckPrice : Form
    {
        //setting up the classes
        MessagesClass TheMessagesClass = new MessagesClass();
        PartNumberClass ThePartNumberClass = new PartNumberClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        PleaseWait PleaseWait = new PleaseWait();

        //setting up the data variables
        PartNumbersDataSet ThePartNumbersDataSet;

        //setting structure
        struct PartNumberStructure
        {
            public int mintPartID;
            public string mstrPartNumber;
            public string mstrDescription;
            public double mdouPrice;
        }

        //setting the structure variables
        PartNumberStructure[] ThePartNumbers;
        int mintPartCounter;
        int mintPartUpperLimit;

        public CheckPrice()
        {
            InitializeComponent();
        }

        private void btnMainMenu_Click(object sender, EventArgs e)
        {
            //this will display the main menu
            MainMenu MainMenu = new MainMenu();
            MainMenu.Show();
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //this will close the application
            TheMessagesClass.CloseTheProgram();
        }

        private void CheckPrice_Load(object sender, EventArgs e)
        {
            //setting local variables
            int intCounter;
            int intNumberOfRecords;
            string strPriceFromTable;
            bool blnNotADouble;

            PleaseWait.Show();

            try
            {
                //loading the data set
                ThePartNumbersDataSet = ThePartNumberClass.GetPartNumbersInfo();

                //getting ready for the loop
                intNumberOfRecords = ThePartNumbersDataSet.partnumbers.Rows.Count - 1;
                ThePartNumbers = new PartNumberStructure[intNumberOfRecords + 1];
                mintPartCounter = 0;

                //beginning loop
                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    //getting the price
                    strPriceFromTable = Convert.ToString(ThePartNumbersDataSet.partnumbers.Rows[intCounter][6]);

                    //checking to see if the value is a double
                    blnNotADouble = TheDataValidationClass.VerifyDoubleData(strPriceFromTable);

                    if(blnNotADouble == true)
                    {
                        //loading the structure
                        ThePartNumbers[mintPartCounter].mintPartID = Convert.ToInt32(ThePartNumbersDataSet.partnumbers.Rows[intCounter][0]);
                        ThePartNumbers[mintPartCounter].mdouPrice = 0.00;
                        ThePartNumbers[mintPartCounter].mstrDescription = Convert.ToString(ThePartNumbersDataSet.partnumbers.Rows[intCounter][2]).ToUpper();
                        ThePartNumbers[mintPartCounter].mstrPartNumber = Convert.ToString(ThePartNumbersDataSet.partnumbers.Rows[intCounter][1]).ToUpper();
                        mintPartUpperLimit = mintPartCounter;
                        mintPartCounter++;
                    }
                }

                mintPartCounter = 0;
            }
            catch (Exception Ex)
            {
                //message to user
                TheMessagesClass.ErrorMessage(Ex.Message);

                //creating Event Log entry
                TheEventLogClass.CreateEventLogEntry("Check Prices " + Ex.Message);
            }

            PleaseWait.Hide();
        }
    }
}
