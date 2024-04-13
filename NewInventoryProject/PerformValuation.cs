/* Title:           Perform Valuation
 * Date:            5-14-16
 * Author:          Terry Holmes
 *
 * Description:     This is the form that does the valuation */

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
using EventLogDLL;
using InventoryValuationDLL;
using EmployeeDLL;
using KeyWordDLL;
using CSVFileDLL;

namespace NewInventoryProject
{
    public partial class PerformValuation : Form
    {
        //setting the classes
        MessagesClass TheMessagesClass = new MessagesClass();
        PartNumberClass ThePartNumberClass = new PartNumberClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        PleaseWait PleaseWait = new PleaseWait();
        InventoryValuationClass TheInventoryValuationClass = new InventoryValuationClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();
        KeyWordClass TheKeyWordClass = new KeyWordClass();
        

        //setting the data
        PartNumbersDataSet ThePartNumberDataSet;
        ClevelandCableDataSet TheClevelandCableDataSet;
        ClevelandMaterialDataSet TheClevelandMaterialDataSet;
        JamestownCableDataSet TheJamestownCableDataSet;
        JamestownMaterialDataSet TheJamestownMaterialDataSet;
        KentuckyCableDataSet TheKentuckyCableDataSet;
        KentuckyMaterialDataSet TheKentuckyMaterialDataSet;
        MilwaukeeCableDataSet TheMilwaukeeCableDataSet;
        MilwaukeeMaterialDataSet TheMilwaukeeMaterialDataSet;
        CostEstimatorDataSet TheCostEstimatorDataSet;
        EmployeeDataSet TheEmployeeDataSet;

        struct PartNumbers
        {
            public int mintPartID;
            public string mstrPartNumber;
            public string mstrDescription;
            public double mdouPrice;
        }

        PartNumbers[] ThePartNumbers;
        int mintPartUpperLimit;

        struct InventoryTables
        {
            public int mintQuantity;
            public string mstrPartNumber;
            public string mstrDescription;
            public double mdouPrice;
        }

        InventoryTables[] InventoryForValuation;
        int mintValuationUpperLimit;

        InventoryTables[] CostEstimator;
        int mintCostUpperLimit;

        struct Warehouses
        {
            public int mintWarehouseID;
            public string mstrWarehouseName;
        }

        Warehouses[] TheWarehouses;
        int mintWarehouseCounter;
        int mintWarehouseUpperLimit;

        struct SearchResults
        {
            public int mintWarehouseID;
            public string mstrWarehouseName;
            public double mdouPrice;
        }

        SearchResults[] TheSearchResults;
        int mintResultCounter;
        int mintResultUpperLimit;

        public PerformValuation()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //this will close the application
            TheMessagesClass.CloseTheProgram();
        }

        private void btnMainMenu_Click(object sender, EventArgs e)
        {
            //this will display the main menu
            MainMenu MainMenu = new MainMenu();
            MainMenu.Show();
            this.Close();
        }

        private void PerformValuation_Load(object sender, EventArgs e)
        {
            //setting local variables
            bool blnFatalError = false;

            PleaseWait.Show();

            blnFatalError = LoadPartStructure();
            if (blnFatalError == false)
                blnFatalError = LoadCostEstimatorStructure();
            if (blnFatalError == false)
                blnFatalError = LoadWarehouseStructure();

            dgvSearchResults.ColumnCount = 3;
            dgvSearchResults.Columns[0].Name = "Warehouse ID";
            dgvSearchResults.Columns[0].Width = 150;
            dgvSearchResults.Columns[1].Name = "Warehouse Name";
            dgvSearchResults.Columns[1].Width = 150;
            dgvSearchResults.Columns[2].Name = "Valuation";
            dgvSearchResults.Columns[2].Width = 200;
            
            PleaseWait.Hide();

            //message to user if there is a failure
            if(blnFatalError == true)
            {
                TheMessagesClass.ErrorMessage(Logon.mstrErrorMessage);

                //log entry
                TheEventLogClass.CreateEventLogEntry(Logon.mstrErrorMessage);
            }
        }
        public bool LoadWarehouseStructure()
        {
            //setting local variables
            bool blnFatalError = false;
            int intCounter;
            int intNumberOfRecords;
            string strKeyWordForSearch;
            string strKeywordFromTable;
            string strEmployeeLastName;
            bool blnKeyWordFound = true;

            try
            {
                //loading the data set
                TheEmployeeDataSet = TheEmployeeClass.GetEmployeeInfo();

                //getting ready for the loop
                intNumberOfRecords = TheEmployeeDataSet.employees.Rows.Count - 1;
                TheWarehouses = new Warehouses[intNumberOfRecords + 1];
                TheSearchResults = new SearchResults[intNumberOfRecords + 1];
                mintWarehouseCounter = 0;

                //for loop
                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    //getting the last name
                    strEmployeeLastName = Convert.ToString(TheEmployeeDataSet.employees.Rows[intCounter][2]).ToUpper();

                    if(strEmployeeLastName == "PARTS")
                    {
                        strKeyWordForSearch = "JH";
                        strKeywordFromTable = Convert.ToString(TheEmployeeDataSet.employees.Rows[intCounter][1]).ToUpper();

                        blnKeyWordFound = TheKeyWordClass.FindKeyWord(strKeyWordForSearch, strKeywordFromTable);

                        if(blnKeyWordFound == false)
                        {
                            TheWarehouses[mintWarehouseCounter].mstrWarehouseName = strKeywordFromTable;
                            TheWarehouses[mintWarehouseCounter].mintWarehouseID = Convert.ToInt32(TheEmployeeDataSet.employees.Rows[intCounter][0]);
                            mintWarehouseUpperLimit = mintWarehouseCounter;
                            mintWarehouseCounter++;
                        }
                    }
                }

                mintWarehouseCounter = 0;

            }
            catch (Exception Ex)
            {
                Logon.mstrErrorMessage = Ex.Message;

                TheEventLogClass.CreateEventLogEntry("Proform Valuation " + Ex.Message);
            }

            //returnign value
            return blnFatalError;
        }
        private bool LoadCostEstimatorStructure()
        {
            //setting local variables
            bool blnFatalError = false;
            int intCounter;
            int intNumberOfRecords;

            try
            {
                TheCostEstimatorDataSet = TheInventoryValuationClass.GetCostEstimatorInfo();

                //setting up for the loop
                intNumberOfRecords = TheCostEstimatorDataSet.costestimatorpartnumbers.Rows.Count - 1;
                CostEstimator = new InventoryTables[intNumberOfRecords + 1];
                mintCostUpperLimit = intNumberOfRecords;

                //loop
                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    CostEstimator[intCounter].mdouPrice = Convert.ToDouble(TheCostEstimatorDataSet.costestimatorpartnumbers.Rows[intCounter][3]);
                    CostEstimator[intCounter].mstrDescription = Convert.ToString(TheCostEstimatorDataSet.costestimatorpartnumbers.Rows[intCounter][2]).ToUpper();
                    CostEstimator[intCounter].mstrPartNumber = Convert.ToString(TheCostEstimatorDataSet.costestimatorpartnumbers.Rows[intCounter][1]).ToUpper();
                }

            }
            catch (Exception Ex)
            {
                Logon.mstrErrorMessage = "Perform Valuation " + Ex.Message;

                blnFatalError = true;
            }

            return blnFatalError;
        }
        private bool LoadMilwaukeeMaterialStructure()
        {
            //setting local variables
            bool blnFatalError = false;
            int intCounter;
            int intNumberOfRecords;

            try
            {
                TheMilwaukeeMaterialDataSet = TheInventoryValuationClass.GetMilwaukeeMaterialInfo();

                //getting ready for the loop
                intNumberOfRecords = TheMilwaukeeMaterialDataSet.milwaukeematerial.Rows.Count - 1;
                InventoryForValuation = new InventoryTables[intNumberOfRecords + 1];
                mintValuationUpperLimit = intNumberOfRecords;

                //preforming loop
                for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    InventoryForValuation[intCounter].mintQuantity = Convert.ToInt32(TheMilwaukeeMaterialDataSet.milwaukeematerial.Rows[intCounter][1]);
                    InventoryForValuation[intCounter].mstrDescription = Convert.ToString(TheMilwaukeeMaterialDataSet.milwaukeematerial.Rows[intCounter][2]).ToUpper();
                    InventoryForValuation[intCounter].mstrPartNumber = Convert.ToString(TheMilwaukeeMaterialDataSet.milwaukeematerial.Rows[intCounter][3]).ToUpper();
                    InventoryForValuation[intCounter].mdouPrice = Convert.ToDouble(TheMilwaukeeMaterialDataSet.milwaukeematerial.Rows[intCounter][4]);
                }

            }
            catch (Exception Ex)
            {
                Logon.mstrErrorMessage = "Perform Valuation " + Ex.Message;

                blnFatalError = true;
            }

            //returning value
            return blnFatalError;
        }
        private bool LoadMilwaukeeCableStructure()
        {
            //setting local variables
            bool blnFatalError = false;
            int intCounter;
            int intNumberOfRecords;

            try
            {
                TheMilwaukeeCableDataSet = TheInventoryValuationClass.GetMilwaukeeCableInfo();

                //getting ready for the loop
                intNumberOfRecords = TheMilwaukeeCableDataSet.milwaukeecable.Rows.Count - 1;
                InventoryForValuation = new InventoryTables[intNumberOfRecords + 1];
                mintValuationUpperLimit = intNumberOfRecords;

                //for loop
                for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    InventoryForValuation[intCounter].mintQuantity = Convert.ToInt32(TheMilwaukeeCableDataSet.milwaukeecable.Rows[intCounter][1]);
                    InventoryForValuation[intCounter].mstrDescription = Convert.ToString(TheMilwaukeeCableDataSet.milwaukeecable.Rows[intCounter][2]).ToUpper();
                    InventoryForValuation[intCounter].mstrPartNumber = Convert.ToString(TheMilwaukeeCableDataSet.milwaukeecable.Rows[intCounter][3]).ToUpper();
                    InventoryForValuation[intCounter].mdouPrice = Convert.ToDouble(TheMilwaukeeCableDataSet.milwaukeecable.Rows[intCounter][4]);
                }

            }
            catch (Exception Ex)
            {
                Logon.mstrErrorMessage = "Perform Valuation " + Ex.Message;

                blnFatalError = true;
            }

            //returning value
            return blnFatalError;
        }
        private bool LoadKentuckyMaterialStructure()
        {
            //setting local variables
            bool blnFatalError = false;
            int intCounter;
            int intNumberOfRecords;

            try
            {
                TheKentuckyMaterialDataSet = TheInventoryValuationClass.GetKentuckyMaterialInfo();

                //getting ready for the loop
                intNumberOfRecords = TheKentuckyMaterialDataSet.kentuckymaterial.Rows.Count - 1;
                InventoryForValuation = new InventoryTables[intNumberOfRecords + 1];
                mintValuationUpperLimit = intNumberOfRecords;

                //preforming loop
                for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    InventoryForValuation[intCounter].mintQuantity = Convert.ToInt32(TheKentuckyMaterialDataSet.kentuckymaterial.Rows[intCounter][1]);
                    InventoryForValuation[intCounter].mstrDescription = Convert.ToString(TheKentuckyMaterialDataSet.kentuckymaterial.Rows[intCounter][2]).ToUpper();
                    InventoryForValuation[intCounter].mstrPartNumber = Convert.ToString(TheKentuckyMaterialDataSet.kentuckymaterial.Rows[intCounter][3]).ToUpper();
                    InventoryForValuation[intCounter].mdouPrice = Convert.ToDouble(TheKentuckyMaterialDataSet.kentuckymaterial.Rows[intCounter][4]);
                }

            }
            catch (Exception Ex)
            {
                Logon.mstrErrorMessage = "Perform Valuation " + Ex.Message;

                blnFatalError = true;
            }

            //returning value
            return blnFatalError;
        }
        private bool LoadKentuckyCableStructure()
        {
            //setting local variables
            bool blnFatalError = false;
            int intCounter;
            int intNumberOfRecords;

            try
            {
                TheKentuckyCableDataSet = TheInventoryValuationClass.GetKentuckyCableInfo();

                //getting ready for the loop
                intNumberOfRecords = TheKentuckyCableDataSet.kentuckycable.Rows.Count - 1;
                InventoryForValuation = new InventoryTables[intNumberOfRecords + 1];
                mintValuationUpperLimit = intNumberOfRecords;

                //for loop
                for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    InventoryForValuation[intCounter].mintQuantity = Convert.ToInt32(TheKentuckyCableDataSet.kentuckycable.Rows[intCounter][1]);
                    InventoryForValuation[intCounter].mstrDescription = Convert.ToString(TheKentuckyCableDataSet.kentuckycable.Rows[intCounter][2]).ToUpper();
                    InventoryForValuation[intCounter].mstrPartNumber = Convert.ToString(TheKentuckyCableDataSet.kentuckycable.Rows[intCounter][3]).ToUpper();
                    InventoryForValuation[intCounter].mdouPrice = Convert.ToDouble(TheKentuckyCableDataSet.kentuckycable.Rows[intCounter][4]);
                }

            }
            catch (Exception Ex)
            {
                Logon.mstrErrorMessage = "Perform Valuation " + Ex.Message;

                blnFatalError = true;
            }

            //returning value
            return blnFatalError;
        }
        private bool LoadJamestownMaterialStructure()
        {
            //setting local variables
            bool blnFatalError = false;
            int intCounter;
            int intNumberOfRecords;

            try
            {
                TheJamestownMaterialDataSet = TheInventoryValuationClass.GetJamestownMaterialInfo();

                //getting ready for the loop
                intNumberOfRecords = TheJamestownMaterialDataSet.jamestownmaterial.Rows.Count - 1;
                InventoryForValuation = new InventoryTables[intNumberOfRecords + 1];
                mintValuationUpperLimit = intNumberOfRecords;

                //preforming loop
                for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    InventoryForValuation[intCounter].mintQuantity = Convert.ToInt32(TheJamestownMaterialDataSet.jamestownmaterial.Rows[intCounter][1]);
                    InventoryForValuation[intCounter].mstrDescription = Convert.ToString(TheJamestownMaterialDataSet.jamestownmaterial.Rows[intCounter][2]).ToUpper();
                    InventoryForValuation[intCounter].mstrPartNumber = Convert.ToString(TheJamestownMaterialDataSet.jamestownmaterial.Rows[intCounter][3]).ToUpper();
                    InventoryForValuation[intCounter].mdouPrice = Convert.ToDouble(TheJamestownMaterialDataSet.jamestownmaterial.Rows[intCounter][4]);
                }

            }
            catch (Exception Ex)
            {
                Logon.mstrErrorMessage = "Perform Valuation " + Ex.Message;

                blnFatalError = true;
            }

            //returning value
            return blnFatalError;
        }
        private bool LoadJamestownCableStructure()
        {
            //setting local variables
            bool blnFatalError = false;
            int intCounter;
            int intNumberOfRecords;

            try
            {
                TheJamestownCableDataSet = TheInventoryValuationClass.GetJamestownCableInfo();

                //getting ready for the loop
                intNumberOfRecords = TheJamestownCableDataSet.jamestowncable.Rows.Count - 1;
                InventoryForValuation = new InventoryTables[intNumberOfRecords + 1];
                mintValuationUpperLimit = intNumberOfRecords;

                //for loop
                for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    InventoryForValuation[intCounter].mintQuantity = Convert.ToInt32(TheJamestownCableDataSet.jamestowncable.Rows[intCounter][1]);
                    InventoryForValuation[intCounter].mstrDescription = Convert.ToString(TheJamestownCableDataSet.jamestowncable.Rows[intCounter][2]).ToUpper();
                    InventoryForValuation[intCounter].mstrPartNumber = Convert.ToString(TheJamestownCableDataSet.jamestowncable.Rows[intCounter][3]).ToUpper();
                    InventoryForValuation[intCounter].mdouPrice = Convert.ToDouble(TheJamestownCableDataSet.jamestowncable.Rows[intCounter][4]);
                }

            }
            catch (Exception Ex)
            {
                Logon.mstrErrorMessage = "Perform Valuation " + Ex.Message;

                blnFatalError = true;
            }

            //returning value
            return blnFatalError;
        }
        private bool LoadClevelandMaterialStructure()
        {
            //setting local variables
            bool blnFatalError = false;
            int intCounter;
            int intNumberOfRecords;

            try
            {
                TheClevelandMaterialDataSet = TheInventoryValuationClass.GetClevelandMaterialInfo();

                //getting ready for the loop
                intNumberOfRecords = TheClevelandMaterialDataSet.clevelandmaterial.Rows.Count - 1;
                InventoryForValuation = new InventoryTables[intNumberOfRecords + 1];
                mintValuationUpperLimit = intNumberOfRecords;

                //preforming loop
                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    InventoryForValuation[intCounter].mintQuantity = Convert.ToInt32(TheClevelandMaterialDataSet.clevelandmaterial.Rows[intCounter][1]);
                    InventoryForValuation[intCounter].mstrDescription = Convert.ToString(TheClevelandMaterialDataSet.clevelandmaterial.Rows[intCounter][2]).ToUpper();
                    InventoryForValuation[intCounter].mstrPartNumber = Convert.ToString(TheClevelandMaterialDataSet.clevelandmaterial.Rows[intCounter][3]).ToUpper();
                    InventoryForValuation[intCounter].mdouPrice = Convert.ToDouble(TheClevelandMaterialDataSet.clevelandmaterial.Rows[intCounter][4]);
                }

            }
            catch (Exception Ex)
            {
                Logon.mstrErrorMessage = "Perform Valuation " + Ex.Message;

                blnFatalError = true;
            }

            //returning value
            return blnFatalError;
        }
        private bool LoadClevelandCableStructure()
        {
            //setting local variables
            bool blnFatalError = false;
            int intCounter;
            int intNumberOfRecords;

            try
            {
                TheClevelandCableDataSet = TheInventoryValuationClass.GetClevelandCableInfo();

                //getting ready for the loop
                intNumberOfRecords = TheClevelandCableDataSet.clevelandcable.Rows.Count - 1;
                InventoryForValuation = new InventoryTables[intNumberOfRecords + 1];
                mintValuationUpperLimit = intNumberOfRecords;

                //for loop
                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    InventoryForValuation[intCounter].mintQuantity = Convert.ToInt32(TheClevelandCableDataSet.clevelandcable.Rows[intCounter][1]);
                    InventoryForValuation[intCounter].mstrDescription = Convert.ToString(TheClevelandCableDataSet.clevelandcable.Rows[intCounter][2]).ToUpper();
                    InventoryForValuation[intCounter].mstrPartNumber = Convert.ToString(TheClevelandCableDataSet.clevelandcable.Rows[intCounter][3]).ToUpper();
                    InventoryForValuation[intCounter].mdouPrice = Convert.ToDouble(TheClevelandCableDataSet.clevelandcable.Rows[intCounter][4]);
                }

            }
            catch (Exception Ex)
            {
                Logon.mstrErrorMessage = "Perform Valuation " + Ex.Message;

                blnFatalError = true;
            }

            //returning value
            return blnFatalError;
        }

        private bool LoadPartStructure()
        {
            //setting local variables
            bool blnFatalError = false;
            int intCounter;
            int intNumberOfRecords;

            try
            {
                //filling the part number
                ThePartNumberDataSet = ThePartNumberClass.GetPartNumbersInfo();

                //setting up the for the loop
                intNumberOfRecords = ThePartNumberDataSet.partnumbers.Rows.Count - 1;
                ThePartNumbers = new PartNumbers[intNumberOfRecords + 1];
                mintPartUpperLimit = intNumberOfRecords;

                //beginning loop
                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    ThePartNumbers[intCounter].mdouPrice = Convert.ToDouble(ThePartNumberDataSet.partnumbers.Rows[intCounter][6]);
                    ThePartNumbers[intCounter].mintPartID = Convert.ToInt32(ThePartNumberDataSet.partnumbers.Rows[intCounter][0]);
                    ThePartNumbers[intCounter].mstrDescription = Convert.ToString(ThePartNumberDataSet.partnumbers.Rows[intCounter][2]).ToUpper();
                    ThePartNumbers[intCounter].mstrPartNumber = Convert.ToString(ThePartNumberDataSet.partnumbers.Rows[intCounter][1]).ToUpper();
                 }
                
            }
            catch (Exception Ex)
            {
                Logon.mstrErrorMessage = "Perform Valuation " + Ex.Message;

                blnFatalError = true;
            }

            //returning value
            return blnFatalError;
        }
        private void btnProcess_Click(object sender, EventArgs e)
        {
            //setting local variables
            bool blnFatalError = false;
            int intWarehouseCounter;

            PleaseWait.Show();

            //getting calling the routine
            mintResultCounter = 0;
            blnFatalError = LoadClevelandCableStructure();
            if(blnFatalError == false)
            {
                TheSearchResults[mintResultCounter].mstrWarehouseName = "CLEVELAND CABLE";

                //getting the id
                for(intWarehouseCounter = 0; intWarehouseCounter <= mintWarehouseUpperLimit; intWarehouseCounter++)
                {
                    if(TheWarehouses[intWarehouseCounter].mstrWarehouseName == "CLEVELAND-JH")
                    {
                        TheSearchResults[mintResultCounter].mintWarehouseID = TheWarehouses[intWarehouseCounter].mintWarehouseID;
                        TheSearchResults[mintResultCounter].mdouPrice = 0.0;
                        GetValuation();
                    }
                }
            }
            blnFatalError = LoadClevelandMaterialStructure();
            if (blnFatalError == false)
            {
                TheSearchResults[mintResultCounter].mstrWarehouseName = "CLEVELAND MATERIAL";

                //getting the id
                for (intWarehouseCounter = 0; intWarehouseCounter <= mintWarehouseUpperLimit; intWarehouseCounter++)
                {
                    if (TheWarehouses[intWarehouseCounter].mstrWarehouseName == "CLEVELAND-JH")
                    {
                        TheSearchResults[mintResultCounter].mintWarehouseID = TheWarehouses[intWarehouseCounter].mintWarehouseID;
                        TheSearchResults[mintResultCounter].mdouPrice = 0.0;
                        GetValuation();
                    }
                }
            }
            blnFatalError = LoadJamestownCableStructure();
            if (blnFatalError == false)
            {
                TheSearchResults[mintResultCounter].mstrWarehouseName = "JAMESTOWN CABLE";

                //getting the id
                for (intWarehouseCounter = 0; intWarehouseCounter <= mintWarehouseUpperLimit; intWarehouseCounter++)
                {
                    if (TheWarehouses[intWarehouseCounter].mstrWarehouseName == "JAMESTOWN-JH")
                    {
                        TheSearchResults[mintResultCounter].mintWarehouseID = TheWarehouses[intWarehouseCounter].mintWarehouseID;
                        TheSearchResults[mintResultCounter].mdouPrice = 0.0;
                        GetValuation();
                    }
                }
            }
            blnFatalError = LoadJamestownMaterialStructure();
            if (blnFatalError == false)
            {
                TheSearchResults[mintResultCounter].mstrWarehouseName = "JAMESTOWN MATERIAL";

                //getting the id
                for (intWarehouseCounter = 0; intWarehouseCounter <= mintWarehouseUpperLimit; intWarehouseCounter++)
                {
                    if (TheWarehouses[intWarehouseCounter].mstrWarehouseName == "JAMESTOWN-JH")
                    {
                        TheSearchResults[mintResultCounter].mintWarehouseID = TheWarehouses[intWarehouseCounter].mintWarehouseID;
                        TheSearchResults[mintResultCounter].mdouPrice = 0.0;
                        GetValuation();
                    }
                }
            }
            blnFatalError = LoadMilwaukeeCableStructure();
            if (blnFatalError == false)
            {
                TheSearchResults[mintResultCounter].mstrWarehouseName = "MILWAUKEE CABLE";

                //getting the id
                for (intWarehouseCounter = 0; intWarehouseCounter <= mintWarehouseUpperLimit; intWarehouseCounter++)
                {
                    if (TheWarehouses[intWarehouseCounter].mstrWarehouseName == "MILWAUKEE-JH")
                    {
                        TheSearchResults[mintResultCounter].mintWarehouseID = TheWarehouses[intWarehouseCounter].mintWarehouseID;
                        TheSearchResults[mintResultCounter].mdouPrice = 0.0;
                        GetValuation();
                    }
                }
            }
            blnFatalError = LoadMilwaukeeMaterialStructure();
            if (blnFatalError == false)
            {
                TheSearchResults[mintResultCounter].mstrWarehouseName = "MILWAUKEE MATERIALS";

                //getting the id
                for (intWarehouseCounter = 0; intWarehouseCounter <= mintWarehouseUpperLimit; intWarehouseCounter++)
                {
                    if (TheWarehouses[intWarehouseCounter].mstrWarehouseName == "MILWAUKEE-JH")
                    {
                        TheSearchResults[mintResultCounter].mintWarehouseID = TheWarehouses[intWarehouseCounter].mintWarehouseID;
                        TheSearchResults[mintResultCounter].mdouPrice = 0.0;
                        GetValuation();
                    }
                }
            }
            blnFatalError = LoadKentuckyCableStructure();
            if (blnFatalError == false)
            {
                TheSearchResults[mintResultCounter].mstrWarehouseName = "KENTUCKY CABLE";

                //getting the id
                for (intWarehouseCounter = 0; intWarehouseCounter <= mintWarehouseUpperLimit; intWarehouseCounter++)
                {
                    if (TheWarehouses[intWarehouseCounter].mstrWarehouseName == "KENTUCKY-JH")
                    {
                        TheSearchResults[mintResultCounter].mintWarehouseID = TheWarehouses[intWarehouseCounter].mintWarehouseID;
                        TheSearchResults[mintResultCounter].mdouPrice = 0.0;
                        GetValuation();
                    }
                }
            }
            blnFatalError = LoadKentuckyMaterialStructure();
            if (blnFatalError == false)
            {
                TheSearchResults[mintResultCounter].mstrWarehouseName = "KENTUCKY MATERIAL";

                //getting the id
                for (intWarehouseCounter = 0; intWarehouseCounter <= mintWarehouseUpperLimit; intWarehouseCounter++)
                {
                    if (TheWarehouses[intWarehouseCounter].mstrWarehouseName == "KENTUCKY-JH")
                    {
                        TheSearchResults[mintResultCounter].mintWarehouseID = TheWarehouses[intWarehouseCounter].mintWarehouseID;
                        TheSearchResults[mintResultCounter].mdouPrice = 0.0;
                        GetValuation();
                    }
                }
            }
            LoadGrid();

            PleaseWait.Hide();

            if(blnFatalError == true)
            {
                TheMessagesClass.ErrorMessage(Logon.mstrErrorMessage);
            }

        }
        private void GetValuation()
        {
            //setting local variables
            int intValuationCounter;
            int intPartCounter;
            int intCostCounter;
            string strPartNumberForSearch;
            string strPartDescriptionForSearch;
            bool blnItemFound;
            bool blnKeyWordNotFound;

            //valuation counter
            for(intValuationCounter = 0; intValuationCounter <= mintValuationUpperLimit; intValuationCounter++)
            {
                blnItemFound = false;

                if(InventoryForValuation[intValuationCounter].mdouPrice != 0)
                {
                    TheSearchResults[mintResultCounter].mdouPrice = TheSearchResults[mintResultCounter].mdouPrice + (InventoryForValuation[intValuationCounter].mdouPrice * InventoryForValuation[intValuationCounter].mintQuantity);
                    blnItemFound = true;
                }
                else
                {
                    strPartDescriptionForSearch = InventoryForValuation[intValuationCounter].mstrDescription;
                    strPartNumberForSearch = InventoryForValuation[intValuationCounter].mstrPartNumber;

                    //checking the part number
                    for(intPartCounter =0; intPartCounter <= mintPartUpperLimit; intPartCounter++)
                    {
                        //blnKeyWordNotFound = TheKeyWordClass.FindKeyWord(strPartDescriptionForSearch, ThePartNumbers[intPartCounter].mstrDescription);

                        if(strPartNumberForSearch == ThePartNumbers[intPartCounter].mstrPartNumber)
                        {
                            TheSearchResults[mintResultCounter].mdouPrice = TheSearchResults[mintResultCounter].mdouPrice + (InventoryForValuation[intValuationCounter].mdouPrice * InventoryForValuation[intValuationCounter].mintQuantity);
                            blnItemFound = true;
                        }
                        else if(strPartDescriptionForSearch == ThePartNumbers[intPartCounter].mstrDescription)
                        {
                            TheSearchResults[mintResultCounter].mdouPrice = TheSearchResults[mintResultCounter].mdouPrice + (InventoryForValuation[intValuationCounter].mdouPrice * InventoryForValuation[intValuationCounter].mintQuantity);
                            blnItemFound = true;
                        }
                    }
                    if(blnItemFound == false)
                    {
                        for(intCostCounter = 0; intCostCounter <= mintCostUpperLimit; intCostCounter++)
                        {
                            if (strPartNumberForSearch == CostEstimator[intCostCounter].mstrPartNumber)
                            {
                                TheSearchResults[mintResultCounter].mdouPrice = TheSearchResults[mintResultCounter].mdouPrice + (InventoryForValuation[intValuationCounter].mdouPrice * InventoryForValuation[intValuationCounter].mintQuantity);
                                blnItemFound = true;
                            }
                            else if (strPartDescriptionForSearch == CostEstimator[intCostCounter].mstrDescription)
                            {
                                TheSearchResults[mintResultCounter].mdouPrice = TheSearchResults[mintResultCounter].mdouPrice + (InventoryForValuation[intValuationCounter].mdouPrice * InventoryForValuation[intValuationCounter].mintQuantity);
                                blnItemFound = true;
                            }
                        }
                    }
                }
            }

            mintResultUpperLimit = mintResultCounter;
            mintResultCounter++;
        }
        private void LoadGrid()
        {
            //setting local variables
            int intCounter;
            string[] NewGridRow;

            dgvSearchResults.Rows.Clear();

            for(intCounter = 0; intCounter <= mintResultUpperLimit; intCounter++)
            {
                NewGridRow = new string[] { Convert.ToString(TheSearchResults[intCounter].mintWarehouseID), TheSearchResults[intCounter].mstrWarehouseName, Convert.ToString(TheSearchResults[intCounter].mdouPrice) };
                dgvSearchResults.Rows.Add(NewGridRow);
            }
        }

        private void btnExportToCSV_Click(object sender, EventArgs e)
        {
            //setting local variables
            int intCounter;

            //try catch for exceptions
            try
            {
                //creating the file writer
                ReadWirteCSV.CsvFileWriter TheValuationCSV = new ReadWirteCSV.CsvFileWriter("E:\\valuation.CSV");

                //calling the writer
                using (TheValuationCSV)
                {
                    for (intCounter = 0; intCounter <= mintResultUpperLimit; intCounter++)
                    {
                        //creating a new row
                        ReadWirteCSV.CsvRow NewCSVRow = new ReadWirteCSV.CsvRow();

                        NewCSVRow.Add(Convert.ToString(TheSearchResults[intCounter].mintWarehouseID));
                        NewCSVRow.Add(TheSearchResults[intCounter].mstrWarehouseName);
                        NewCSVRow.Add(Convert.ToString(TheSearchResults[intCounter].mdouPrice));
                       
                        //writing the new row
                        TheValuationCSV.WriteRow(NewCSVRow);
                    }
                }

                //output to user
                TheMessagesClass.InformationMessage("The File Has Been Created And Saved Under\n WHSETrac folder contained in the Warehouse Folder");
            }
            catch (Exception Ex)
            {
                //message to user
                TheMessagesClass.ErrorMessage(Ex.Message);

                //event log entry
                TheEventLogClass.CreateEventLogEntry("Exporting CSV File " + Ex.Message);
            }
        }

        private void btnRunReport_Click(object sender, EventArgs e)
        {
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.PrinterSettings = printDialog1.PrinterSettings;
                
                printDocument1.Print();

            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int intCounter;
            Font PrintHeaderFont = new Font("Arial", 14, FontStyle.Bold);
            Font PrintItemFont = new Font("Arial", 10, FontStyle.Regular);
            float PrintX = e.MarginBounds.Left;
            float PrintY = e.MarginBounds.Top;
            float HeadingLineHeight = PrintHeaderFont.GetHeight() + 18;
            int intStartingPageCounter;
            float fltHeadingLineHeight = PrintHeaderFont.GetHeight() + 10;
            float fltItemLineHeight = PrintItemFont.GetHeight() + 5;
            double douTotals = 0;

            PrintX = 200;
            PrintY = 100;
            intStartingPageCounter = 0;

            //setting up the header
            e.Graphics.DrawString("Blue Jay Communications Inventory Valuation", PrintHeaderFont, Brushes.Black, PrintX, PrintY);
            PrintY = PrintY + fltHeadingLineHeight + 10;

            PrintX = 100;
            e.Graphics.DrawString("Warehouse ID", PrintHeaderFont, Brushes.Black, PrintX, PrintY);
            PrintX = 350;
            e.Graphics.DrawString("Warehouse Name", PrintHeaderFont, Brushes.Black, PrintX, PrintY);
            PrintX = 700;
            e.Graphics.DrawString("Value", PrintHeaderFont, Brushes.Black, PrintX, PrintY);
            PrintX = 525;
            PrintY = PrintY + fltHeadingLineHeight + 10;

            for (intCounter = intStartingPageCounter; intCounter <= mintResultUpperLimit; intCounter++)
            {
                PrintX = 100;
                e.Graphics.DrawString(Convert.ToString(TheSearchResults[intCounter].mintWarehouseID), PrintItemFont, Brushes.Black, PrintX, PrintY);
                PrintX = 350;
                e.Graphics.DrawString(TheSearchResults[intCounter].mstrWarehouseName, PrintItemFont, Brushes.Black, PrintX, PrintY);
                PrintX = 700;
                e.Graphics.DrawString(Convert.ToString(TheSearchResults[intCounter].mdouPrice), PrintItemFont, Brushes.Black, PrintX, PrintY);
                douTotals = douTotals + TheSearchResults[intCounter].mdouPrice;
                PrintY = PrintY + fltItemLineHeight + 5;
            }

            PrintY = PrintY + fltHeadingLineHeight + 10;
            PrintY = PrintY + fltHeadingLineHeight + 10;
            PrintX = 350;
            e.Graphics.DrawString("Total Valuation " + Convert.ToString(douTotals), PrintHeaderFont, Brushes.Black, PrintX, PrintY);
        }
    }
}
