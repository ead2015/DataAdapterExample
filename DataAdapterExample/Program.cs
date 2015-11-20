using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAdapterExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Shuja\DotNetSamples\F10\Ado.Net\ConnectedModel\ConnectedModel\MyDB.mdf;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            Console.WriteLine("Reading from DB");
            string query = "select * from users;";
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            //filling the datatable
            DataTable targetTable = new DataTable();

            da.Fill(targetTable);

            foreach (DataRow row in targetTable.Rows)
            {
                Console.WriteLine("data = " + row[0] + "; " + row[1]);
            }

           //updating the datatable and sent changes back.

            //insert command
             query = "insert into Users (userName, password) values(@u, @p);";
           // string userName = "new User";
            //string password = "new Password";
            SqlCommand insertcmd = new SqlCommand(query, connection);
            SqlParameter p1 = new SqlParameter("u",SqlDbType.VarChar,50 ,"UserName");
            SqlParameter p2 = new SqlParameter("p",SqlDbType.VarChar,50, "Password");
            insertcmd.Parameters.Add(p1);
            insertcmd.Parameters.Add(p2);

            da.InsertCommand = insertcmd;

            //update command
            query = "update  Users set  password= @p where userName= @u";
            
            SqlCommand updatecmd = new SqlCommand(query, connection);
            SqlParameter p3 = new SqlParameter("u", SqlDbType.VarChar, 50, "UserName");
            SqlParameter p4 = new SqlParameter("p", SqlDbType.VarChar, 50, "Password");
            updatecmd.Parameters.Add(p3);
            updatecmd.Parameters.Add(p4);

            da.UpdateCommand = updatecmd;
            //delete command
            query = "delete from   Users  where userName= @u";
            
            SqlCommand deletecmd = new SqlCommand(query, connection);
            SqlParameter p5 = new SqlParameter("u", SqlDbType.VarChar, 50, "UserName");

            deletecmd.Parameters.Add(p5);

            da.DeleteCommand = deletecmd;

            //New Row
            DataRow newRow = targetTable.NewRow();
            newRow["UserName"] = "afnan";
            newRow["Password"] = "myNewPassword";
            targetTable.Rows.Add(newRow);

            //Update Some Row
            DataRow rowToModify = targetTable.Rows[4];
            rowToModify["UserName"] = "UserName";
            rowToModify["Password"] = "MyPassword";

            //Delete Some Row

            DataRow rowToDelete = targetTable.Rows[0];
            rowToDelete.Delete();
            //the difference b/w delete and remove is that delete will mark this row need to delete when we call update method
            //but remove will immediatly delete this row from the table and hence no footprint left and database row will not
            // remove from the external database.

            //targetTable.Rows.Remove(rowToDelete);

            da.Update(targetTable);
        }
    }
}
