using pukhoApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pukhoApp.Dashboard
{
    public partial class adminProductdetails : MetroSet_UI.Forms.MetroSetForm
    {
        pukhoappTestDBEntities db = new pukhoappTestDBEntities();

        public adminProductdetails()
        {
            InitializeComponent();
            
            //categoriesBindingSource.DataSource = db.Categories.ToList();
        }
        private person LoginPerson { get; set; }

        public adminProductdetails(string selectedproduct, person loginPerson)
        {
            LoginPerson=loginPerson;
            this.selectedproduct = selectedproduct;
            InitializeComponent();
            txtProductname.Text = selectedproduct;
            FillFormElements(selectedproduct);  
        }

        private void FillFormElements(string selectedproduct)
        {
            if (LoginPerson.roleid == 1)
            {
                if (selectedproduct != null)
                {
                    pukhoappTestDBEntities db = new pukhoappTestDBEntities();
                    Products kayitproduct = db.Products.FirstOrDefault(r => r.Name == txtProductname.Text);
                    categoriesBindingSource.DataSource = db.Categories.ToList();
                    txtProductname.Text = kayitproduct.Name;
                    txtProductdesc.Text = kayitproduct.Prodesc;
                    cmbAdminproductdetailscat.SelectedValue = kayitproduct.Categories.Catname;
                    txtProductaddedby.Text = kayitproduct.person.username;
                    txtProductdate.Text = kayitproduct.Createdate.Date.ToString();
                    btnUpdate.Visible = true;
                    btnAddproduct.Visible = false;
                }
                else
                {
                    this.Close();
                    MessageBox.Show("Bir hata var seçilen ürün boş");
                }
            }
            else
            {
                if (selectedproduct != null)
                {
                    pukhoappTestDBEntities db = new pukhoappTestDBEntities();
                    Products kayitproduct = db.Products.FirstOrDefault(r => r.Name == txtProductname.Text);
                    categoriesBindingSource.DataSource = db.Categories.ToList();
                    txtProductname.Text = kayitproduct.Name;
                    txtProductdesc.Text = kayitproduct.Prodesc;
                    cmbAdminproductdetailscat.SelectedValue = kayitproduct.Categories.Catname;
                    txtProductaddedby.Text = kayitproduct.person.username;
                    txtProductdate.Text = kayitproduct.Createdate.Date.ToString();
                    txtProductname.Enabled = false;
                    txtProductdesc.Enabled = false;
                    txtProductdate.Enabled = false;
                    cmbAdminproductdetailscat.Enabled = false;
                    txtProductaddedby.Enabled = false;
                    btnUpdate.Visible = false;
                    btnAddproduct.Visible = true;
                    Carts newCart = new Carts();
                    Products selectedProduct = db.Products.FirstOrDefault(r => r.Name == txtProductname.Text);
                    newCart.UserID = LoginPerson.ID;
                    newCart.ProductID = selectedProduct.ID;
                    Carts selectedProductfirst = db.Carts.FirstOrDefault(r => r.ProductID == newCart.ProductID && newCart.UserID == LoginPerson.ID);
                    if (selectedProductfirst != null)
                    {
                        btnAddproduct.Visible = false;
                        btnDeleteCart.Visible = true;
                    }


                }
                else
                {
                    this.Close();
                    MessageBox.Show("Bir hata var seçilen ürün boş");
                }

            }

           
        }
        private void LockTxtBasedRole(person loginPerson)
        {

            if (!Models.Constants.Helpers.checkuserrole(loginPerson.username))
            {
                txtProductname.Enabled = false;
            }
            else
            {
            }
        }
        public string selectedproduct { get;}

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            pukhoappTestDBEntities db = new pukhoappTestDBEntities();
            Products guncellenen = db.Products.FirstOrDefault(r => r.Name == selectedproduct);
            //person guncelleyen = db.person.FirstOrDefault(r => r.username == LoginPerson.username);
            guncellenen.Name = txtProductname.Text;
            guncellenen.Createdate = DateTime.Now;
            guncellenen.Createdby = LoginPerson.ID;
            guncellenen.Prodesc = txtProductdesc.Text;
            Categories guncellenencat = db.Categories.FirstOrDefault(r => r.Catname == cmbAdminproductdetailscat.SelectedValue.ToString());
            guncellenen.Procatid = guncellenencat.ID;
            //guncellenen.Createdby = guncelleyen.ID;
            if (db.SaveChanges() > 0)
            {
                MessageBox.Show("Succesfull");
                List<Form> activeforms = new List<Form>();
                foreach (Form f in Application.OpenForms)
                    if (f.Name == "adminDashboard")
                        activeforms.Add(f);
                foreach (Form f in activeforms)
                    f.Close();
                adminDashboard newform = new adminDashboard(LoginPerson);
                newform.Show();
                List<Form> activeforms2 = new List<Form>();
                foreach (Form f in Application.OpenForms)
                    if (f.Name == "adminProductdetails")
                        activeforms.Add(f);
                foreach (Form f in activeforms)
                    f.Close();

            }
            else
            {
                MessageBox.Show("Not successful");
            }
        }

        private void btnAddproduct_Click(object sender, EventArgs e)
        {
            Carts newCart = new Carts();
            Products selectedProduct = db.Products.FirstOrDefault(r => r.Name == txtProductname.Text);
            newCart.UserID = LoginPerson.ID;
            newCart.ProductID = selectedProduct.ID;
            Carts selectedProductfirst = db.Carts.FirstOrDefault(r => r.ProductID == newCart.ProductID && newCart.UserID == LoginPerson.ID);
            if (selectedProductfirst == null)
            {
                db.Carts.Add(newCart);
                if (db.SaveChanges() > 0)
                {
                    MessageBox.Show("Product successfully added to shopping card");
                    List<Form> activeforms2 = new List<Form>();
                    foreach (Form f in Application.OpenForms)
                        if (f.Name == "adminProductdetails" || f.Name == "userDashboard")
                            activeforms2.Add(f);
                    foreach (Form f in activeforms2)
                        f.Close();
                    userDashboard newform = new userDashboard(LoginPerson);
                    newform.Show();
                }
                else
                {
                    MessageBox.Show("Not successful");
                }

            }
            else
            {
                MessageBox.Show("Already in user shopping card");
            }

        }

        private void btnDeleteCart_Click_1(object sender, EventArgs e)
        {
                Carts newCart = new Carts();
                Products selectedProduct = db.Products.FirstOrDefault(r => r.Name == txtProductname.Text);
                newCart.UserID = LoginPerson.ID;
                newCart.ProductID = selectedProduct.ID;
                Carts selectedProductfirst = db.Carts.FirstOrDefault(r => r.ProductID == newCart.ProductID && newCart.UserID == LoginPerson.ID);
                if (selectedProductfirst != null)
                {
                    db.Carts.Remove(selectedProductfirst);
                    if (db.SaveChanges() > 0)
                    {
                        MessageBox.Show("Product successfully deleted from shopping card");
                    List<Form> activeforms = new List<Form>();
                    foreach (Form f in Application.OpenForms)
                        if (f.Name == "userDashboard")
                            activeforms.Add(f);
                    foreach (Form f in activeforms)
                        f.Close();
                    List<Form> activeforms2 = new List<Form>();
                    foreach (Form f in Application.OpenForms)
                        if (f.Name == "adminProductdetails")
                            activeforms2.Add(f);
                    foreach (Form f in activeforms2)
                        f.Close();
                    userDashboard newform = new userDashboard(LoginPerson);
                    newform.Show();

                }
                    else
                    {
                        MessageBox.Show("Not successful");
                    }

                }
                else
                {
                    MessageBox.Show("Error");
                }
            }

        }
    }

