using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContactHub.ContactHubClasses
{
    class ContactClass
    {
        //Getter & Setter Properties
        //Act as a data carrier in our application

        public int ContactID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }

        //add the connection string that was added in aap.config file
        //For Configuration Manager you had to first add it from the reference section by click on "Add reference" and search for config check it and click Ok then
        //write "using System.Configuration"
        static string MyConnString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

        //Selecting data from database
        public DataTable Select()
        {
            //Step 1: Database Connection
            SqlConnection Conn = new SqlConnection(MyConnString);
            DataTable Dt = new DataTable();

            try
            {
                //Writing sql query
                string Sql = "SELECT * FROM Contacts";
                
                //Creating cmd using Sql and Conn
                SqlCommand SqlCmd = new SqlCommand(Sql, Conn);
                
                //Creating Sql DataAdapter using SqlCmd
                SqlDataAdapter SqlDataAdapter = new SqlDataAdapter(SqlCmd);
                
                //Opening Connection
                Conn.Open();
                
                //Filling DataAdapter with Datatable Dt
                SqlDataAdapter.Fill(Dt);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //Closing connection
                Conn.Close();
            }

            return Dt;
        }

        //Inserting data in database
        public bool Insert(ContactClass cc)
        {
            //Creating a default return type and initializing it to false
            bool IsSuccess = false;
            
            //Step 1: Database Connection
            SqlConnection Conn = new SqlConnection(MyConnString);

            try
            {
                //Writing sql query
                string Sql = "INSERT INTO Contacts(First_Name, Last_Name, Email_Id, Contact_No, Address, Gender) VALUES(@FirstName, @LastName, @EmailId, @ContactNo, @Address, @Gender)";
                
                //Creating cmd using Sql and Conn
                SqlCommand SqlCmd = new SqlCommand(Sql, Conn);
                
                //Create Parameters to add data
                SqlCmd.Parameters.AddWithValue("@FirstName", cc.FirstName);
                SqlCmd.Parameters.AddWithValue("@LastName", cc.LastName);
                SqlCmd.Parameters.AddWithValue("@EmailId", cc.EmailId);
                SqlCmd.Parameters.AddWithValue("@ContactNo", cc.ContactNo);
                SqlCmd.Parameters.AddWithValue("@Address", cc.Address);
                SqlCmd.Parameters.AddWithValue("@Gender", cc.Gender);

                //Opening Connection
                Conn.Open();
                if(cc.FirstName == "" || cc.LastName == "" || cc.EmailId == "" || cc.ContactNo == "" || cc.Address == "" || cc.Gender == "")
                {
                    MessageBox.Show("Please fill all the fields to add new contact", "Empty Fields Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if(!ValidateEmail()) {
                    MessageBox.Show("Enter a valid email address", "Invalid email address", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    int Rows = SqlCmd.ExecuteNonQuery();
                    //if query runs successfully value of rows will be greater than 0 else less than 0
                    if (Rows > 0)
                    {
                        IsSuccess = true;
                    }
                    else
                    {
                        IsSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //Closing connection
                Conn.Close();
            }
            return IsSuccess;
        }

        //Method to update data in Database
        public bool Update(ContactClass cc)
        {
            //Creating a default return type and initializing it to false
            bool IsSuccess = false;
            
            //Step 1: Database Connection
            SqlConnection Conn = new SqlConnection(MyConnString);
            try
            {
                //Writing sql query
                string Sql = "UPDATE Contacts SET First_Name=@FirstName, Last_Name=@LastName, Email_Id=@EmailId, Contact_No=@ContactNo, Address=@Address, Gender=@Gender WHERE Contact_ID=@ContactID";
                
                //Creating cmd using Sql and Conn
                SqlCommand SqlCmd = new SqlCommand(Sql, Conn);
                
                //Create Parameters to add data
                SqlCmd.Parameters.AddWithValue("@FirstName", cc.FirstName);
                SqlCmd.Parameters.AddWithValue("@LastName", cc.LastName);
                SqlCmd.Parameters.AddWithValue("@EmailId", cc.EmailId);
                SqlCmd.Parameters.AddWithValue("@ContactNo", cc.ContactNo);
                SqlCmd.Parameters.AddWithValue("@Address", cc.Address);
                SqlCmd.Parameters.AddWithValue("@Gender", cc.Gender);
                SqlCmd.Parameters.AddWithValue("@ContactID", cc.ContactID);

                //Opening Connection
                Conn.Open();

                if (cc.FirstName == "" || cc.LastName == "" || cc.EmailId == "" || cc.ContactNo == "" || cc.Address == "" || cc.Gender == "")
                {
                    MessageBox.Show("Please fill all the fields to add new contact", "Empty Fields Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (!ValidateEmail())
                {
                    MessageBox.Show("Enter a valid email address", "Invalid email address", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    int Rows = SqlCmd.ExecuteNonQuery();
                    //if query runs successfully value of rows will be greater than 0 else less than 0
                    if (Rows > 0)
                    {
                        IsSuccess = true;
                    }
                    else
                    {
                        IsSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //Closing connection
                Conn.Close();
            }
            return IsSuccess;
        }

        //Method to delete data from database
        public bool Delete(ContactClass cc)
        {
            //Creating a default return type and initializing it to false
            bool IsSuccess = false;

            //Step 1: Database Connection
            SqlConnection Conn = new SqlConnection(MyConnString);
            try
            {
                //Writing sql query
                string Sql = "DELETE FROM Contacts WHERE Contact_ID=@ContactID";

                //Creating cmd using Sql and Conn
                SqlCommand SqlCmd = new SqlCommand(Sql, Conn);
                SqlCmd.Parameters.AddWithValue("@ContactID", cc.ContactID);

                //Opening Connection
                Conn.Open();

                int Rows = SqlCmd.ExecuteNonQuery();
                //if query runs successfully value of rows will be greater than 0 else less than 0
                if (Rows > 0)
                {
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //Closing connection
                Conn.Close();
            }
            return IsSuccess;
        }

        private bool ValidateEmail()
        {
            bool IsValid = false;
            string email = EmailId;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
            {
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }
            return IsValid;
        }
    }
}
