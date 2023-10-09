using Bai2_BT2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Bai2_BT2
{
    public partial class frmMain : Form
    {
        private List<Faculty> listFaculties = new List<Faculty>();
        string pathImage;
        public frmMain()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dgvSinhVien);
                StudentContextDB context = new StudentContextDB();
                List<Faculty> listFaculties = context.Faculties.ToList();
                List<Student> listStudents = context.Students.ToList();
                FillFacultyCombobox(listFaculties);
                BindGrid(listStudents);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BindGrid(List<Student> listStudents)
        {
            dgvSinhVien.Rows.Clear();
            foreach (var item in listStudents)
            {
                int index = dgvSinhVien.Rows.Add();
                dgvSinhVien.Rows[index].Cells["MaSV"].Value = item.StudentID;
                dgvSinhVien.Rows[index].Cells["HoTen"].Value = item.FullName;
                if (item.Faculty != null)
                    dgvSinhVien.Rows[index].Cells["Khoa"].Value = item.Faculty.FacultyName;

                dgvSinhVien.Rows[index].Cells["DiemTB"].Value = item.AverageScore + "";
                if (item.MajorID != null)
                    dgvSinhVien.Rows[index].Cells["ChuyenNganh"].Value = item.Major.Name + "";
                ShowAvatar(item.Avatar);

            }
        }

        private void ShowAvatar(string ImageName)
        {
            if (string.IsNullOrEmpty(ImageName))
            {
                picAvatar.Image = null;
            }
            else
            {
                string parentDirectory =
               Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName
               ;
                string imagePath = Path.Combine(parentDirectory, "Image",
               ImageName);
                picAvatar.Image = System.Drawing.Image.FromFile(imagePath);
                picAvatar.Refresh();
            }
        }

        public void setGridViewStyle(DataGridView dgview)
        {
            dgview.BorderStyle = BorderStyle.None;
            dgview.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgview.CellBorderStyle =
           DataGridViewCellBorderStyle.SingleHorizontal;
            dgview.BackgroundColor = Color.White;
            dgview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void FillFacultyCombobox(List<Faculty> listFaculties)
        {
            this.cmbChuyenNganh.DataSource = listFaculties;
            this.cmbChuyenNganh.DisplayMember = "FacultyName";
            this.cmbChuyenNganh.ValueMember = "FacultyID";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {

            StudentContextDB db = new StudentContextDB();
            Student dbupdate = db.Students.FirstOrDefault(p => p.StudentID == txtMaSV.Text);
            if (dbupdate != null)
            {
                dbupdate.FullName = txtHoTen.Text;
                dbupdate.AverageScore = double.Parse(txtDiemTB.Text);
                dbupdate.FacultyID = int.Parse(cmbChuyenNganh.SelectedValue.ToString());
                if (!string.IsNullOrEmpty(pathImage) && File.Exists(pathImage))
                {
                    string imageName = txtMaSV.Text.Trim().ToString() + "." + Path.GetExtension(pathImage).TrimStart('.');
                    string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                    string imagepath = Path.Combine(parentDirectory, "Image");
                    if (!Directory.Exists(imagepath))
                    {
                        Directory.CreateDirectory(imagepath);
                    }
                    string imageDestinationPath = Path.Combine(imagepath, imageName);
                    File.Copy(pathImage, imageDestinationPath, true);
                    dbupdate.Avatar = imageName;
                }
            }
            else
            {
                string imageName = "";
                if (!string.IsNullOrEmpty(pathImage) && File.Exists(pathImage))
                {
                    imageName = txtMaSV.Text.Trim().ToString() + "." + Path.GetExtension(pathImage).TrimStart('.');
                    string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                    string imagepath = Path.Combine(parentDirectory, "Image");
                    if (!Directory.Exists(imagepath))
                    {
                        Directory.CreateDirectory(imagepath);
                    }
                    string imageDestinationPath = Path.Combine(imagepath, imageName);
                    File.Copy(pathImage, imageDestinationPath, true);
                    Student s = new Student()
                    {
                        StudentID = txtMaSV.Text,
                        FullName = txtHoTen.Text,
                        AverageScore = double.Parse(txtDiemTB.Text),
                        FacultyID = int.Parse(cmbChuyenNganh.SelectedValue.ToString()),
                        Avatar = imageName
                    };
                    db.Students.Add(s);
                }
                
            }
            db.SaveChanges();
            List<Student> liststudents = db.Students.ToList();
            BindGrid(liststudents);
        }


        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            StudentContextDB db = new StudentContextDB();
            if (e.RowIndex >= 0)
            {
                txtHoTen.Text = dgvSinhVien.Rows[e.RowIndex].Cells["HoTen"].Value.ToString();
                txtMaSV.Text = dgvSinhVien.Rows[e.RowIndex].Cells["MaSV"].Value.ToString();
                txtDiemTB.Text = dgvSinhVien.Rows[e.RowIndex].Cells["DiemTB"].Value.ToString();
                cmbChuyenNganh.Text = dgvSinhVien.Rows[e.RowIndex].Cells["Khoa"].Value.ToString();
                var av = db.Students.SingleOrDefault(c => c.StudentID.Equals(txtMaSV.Text));
                ShowAvatar(av.Avatar);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            StudentContextDB db = new StudentContextDB();

            Student deleteStudent = db.Students.SingleOrDefault(c => c.StudentID.Equals(txtMaSV.Text));

            if (deleteStudent != null)
            {
                db.Students.Remove(deleteStudent);
                db.SaveChanges();
            }
            BindGrid(db.Students.ToList());
        }


        private void btnDong_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void chkUnregisterMajor_CheckedChanged(object sender, EventArgs e)
        {
            var listStudents = new List<Student>();
            StudentContextDB context = new StudentContextDB();
            if (this.chkUnregisterMajor.Checked)
                listStudents = context.Students.Where(p=>p.MajorID == null).ToList();
            else
                listStudents = context.Students.ToList();
            BindGrid(listStudents);
        }

        private void a(object sender, EventArgs e)
        {

        }

        private void thêmChuyênNgànhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRegister frmRegister = new frmRegister();
            frmRegister.ShowDialog();   
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String imagelocation = "";
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Title = "Chọn Ảnh Sinh Viên";
                op.Filter = "Hình Ảnh (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|ALL files (*.*)|*.*";
                if (op.ShowDialog() == DialogResult.OK)
                {
                    imagelocation = op.FileName;
                    picAvatar.Image = System.Drawing.Image.FromFile(imagelocation);
                    pathImage = imagelocation;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("lỗi không thể uplload file ảnh", "Lỗi!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
