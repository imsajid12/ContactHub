using ContactHub.ContactHubClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ContactHub
{
    public partial class frmContactHub : Form
    {
        public frmContactHub()
        {
            InitializeComponent();
        }

        ContactClass cc = new ContactClass();
        static string MyConnString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

        //Method to clear fields
        public void Clear()
        {
            tbContactID.Text = "";
            tbFirstName.Text = "";
            tbLastName.Text = "";
            tbEmailId.Text = "";
            tbContactNo.Text = "";
            tbAddress.Text = "";
            cmbGender.Text = "";
        }

        private void dgvContactList_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Extract data from Data Grid View and to load it to respective fields
            //Indentify row index
            int RowIndex = e.RowIndex;
            tbContactID.Text = dgvContactList.Rows[RowIndex].Cells[0].Value.ToString();
            tbFirstName.Text = dgvContactList.Rows[RowIndex].Cells[1].Value.ToString();
            tbLastName.Text = dgvContactList.Rows[RowIndex].Cells[2].Value.ToString();
            tbEmailId.Text = dgvContactList.Rows[RowIndex].Cells[3].Value.ToString();
            tbContactNo.Text = dgvContactList.Rows[RowIndex].Cells[4].Value.ToString();
            tbAddress.Text = dgvContactList.Rows[RowIndex].Cells[5].Value.ToString();
            cmbGender.Text = dgvContactList.Rows[RowIndex].Cells[6].Value.ToString();
        }

        private void frmContactHub_Load(object sender, EventArgs e)
        {
            //Load Data in data grid view
            DataTable Dt = cc.Select();
            dgvContactList.DataSource = Dt;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Get the values from input field
            cc.FirstName = tbFirstName.Text;
            cc.LastName = tbLastName.Text;
            cc.EmailId = tbEmailId.Text;
            cc.ContactNo = tbContactNo.Text;
            cc.Address = tbAddress.Text;
            cc.Gender = cmbGender.Text;

            //Inserting data in database using Insert() method.
            bool Success = cc.Insert(cc);
            if (Success == true)
            {
                MessageBox.Show("New contact successfully inserted", "Added New Contact", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed to insert contact, Please try again", "Failed to Add", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //Load Data in data grid view
            DataTable Dt = cc.Select();
            dgvContactList.DataSource = Dt;

            //Call Method to clear fields
            Clear();

            //Set focus on FrstName textbox
            tbFirstName.Focus();

        }     

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                //Getting data from textboxes
                cc.ContactID = int.Parse(tbContactID.Text);
                cc.FirstName = tbFirstName.Text;
                cc.LastName = tbLastName.Text;
                cc.EmailId = tbEmailId.Text;
                cc.ContactNo = tbContactNo.Text;
                cc.Address = tbAddress.Text;
                cc.Gender = cmbGender.Text;

                //Updating data in database using Update() method.
                bool Success = cc.Update(cc);
                if (Success == true)
                {
                    MessageBox.Show("Contact has been successfully updated","Contact Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Load Data in data grid view
                    DataTable Dt = cc.Select();
                    dgvContactList.DataSource = Dt;
                }
                else
                {
                    MessageBox.Show("Failed to update contact, Please try again", "Error in Updation", MessageBoxButtons.OK, MessageBoxIcon.Error   );
                }

                //Call Method to clear fields
                Clear();

                //Set focus on FrstName textbox
                tbFirstName.Focus();
            }
            catch(Exception)
            {
                MessageBox.Show("To update a contact, You need to select it from the contact list", "Select Contact", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult ShouldDelete = MessageBox.Show("Do You Want To Delete The Selected Contact?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if(ShouldDelete == DialogResult.Yes)
                {
                    //Getting data from textboxes
                    cc.ContactID = int.Parse(tbContactID.Text);

                    //Deleting data from database using Delete() method.
                    bool Success = cc.Delete(cc);
                    if (Success == true)
                    {
                        MessageBox.Show("Contact has been successfully deleted", "Contact Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //Load Data in data grid view
                        DataTable Dt = cc.Select();
                        dgvContactList.DataSource = Dt;
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete contact, Please try again", "Failed to Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    //Call Method to clear fields
                    Clear();

                    //Set focus on FrstName textbox
                    tbFirstName.Focus();
                }
                else
                {
                    //Set focus on FrstName textbox
                    tbFirstName.Focus();
                }
            }
            catch(Exception)
            {
                MessageBox.Show("To delete a contact, You need to select it from contact list", "Select Contact", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
            tbFirstName.Focus();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            //Get Data from textbox
            string keyword = tbSearch.Text;

            //Connecting to database
            SqlConnection Conn = new SqlConnection(MyConnString);
            SqlDataAdapter SqlDataAdapter = new SqlDataAdapter("SELECT * FROM Contacts WHERE Contact_ID LIKE '%"+keyword+"%' OR First_Name LIKE '%"+keyword+"%' OR Last_Name LIKE '%"+keyword+"%' OR Email_Id LIKE '%"+keyword+"%' OR Contact_No LIKE '%"+keyword+"%' OR Address LIKE '%"+keyword+"%' OR Gender LIKE '%"+keyword+"%'", Conn);
            DataTable Dt = new DataTable();
            SqlDataAdapter.Fill(Dt);
            dgvContactList.DataSource = Dt;
        }

        private void tbContactNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
                MessageBox.Show("Contact number does not have alphabets and special characters, So kindly enter numbers", "Only numbers to be entered", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbContactID_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
