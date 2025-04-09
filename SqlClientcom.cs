//https://qiita.com/ymasaoka/items/944e8a5f1987cc9e0d37
//注意
//form1.cs：ソースコード、リソースコード[デザイン]
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace WindowsFormsGithub
{
    public partial class SqlClientcom : Form
    {
        public SqlClientcom()
        {
            InitializeComponent();
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            try
            {
//                Console.WriteLine("SQL Server に接続し、Create、Read、Update、Delete 操作のデモを行います。");
                txtConsole.Text = "SQL Server に接続し、Create、Read、Update、Delete 操作のデモを行います。";

                // 接続文字列を構築
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "localhost";   // 接続先の SQL Server インスタンス
                builder.UserID = "sa";              // 接続ユーザー名
                builder.Password = "your_password"; // 接続パスワード
                builder.InitialCatalog = "master";  // 接続するデータベース(ここは変えないでください)
                // builder.ConnectTimeout = 60000;  // 接続タイムアウトの秒数(ms) デフォルトは 15 秒

                // SQL Server に接続
//                Console.Write("SQL Server に接続しています... ");
                txtConsole.Text = "SQL Server に接続しています... ";
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
//                    Console.WriteLine("接続成功。");
                    txtConsole.Text = "接続成功。";

                    // サンプルデータベースの作成
//                    Console.Write("既に作成されている SampleDB データベースを削除し、再作成します... ");
                    txtConsole.Text = "既に作成されている SampleDB データベースを削除し、再作成します... ";
                    String sql = "DROP DATABASE IF EXISTS [SampleDB]; CREATE DATABASE [SampleDB]";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 60000; // コマンドがタイムアウトする場合は秒数を変更(ms) デフォルトは 30秒
                        command.ExecuteNonQuery();
//                        Console.WriteLine("SampleDB データベースを作成しました。");
                        txtConsole.Text = "SampleDB データベースを作成しました。";
                    }

                    // テーブルを作成しサンプルデータを登録
//                    Console.Write("サンプルテーブルを作成しデータを登録します。任意のキーを押して続行します...");
//                    Console.ReadKey(true);
                    txtConsole.Text = "サンプルテーブルを作成しデータを登録します。";
                    StringBuilder sb = new StringBuilder();
                    sb.Append("USE SampleDB; ");
                    sb.Append("CREATE TABLE Employees ( ");
                    sb.Append(" Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY, ");
                    sb.Append(" Name NVARCHAR(50), ");
                    sb.Append(" Location NVARCHAR(50) ");
                    sb.Append("); ");
                    sb.Append("INSERT INTO Employees (Name, Location) VALUES ");
                    sb.Append("(N'Jared', N'Australia'), ");
                    sb.Append("(N'Nikita', N'India'), ");
                    sb.Append("(N'Tom', N'Germany'); ");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 60000; // コマンドがタイムアウトする場合は秒数を変更(ms) デフォルトは 30秒
                        command.ExecuteNonQuery();
//                        Console.WriteLine("作成完了。");
                        txtConsole.Text = "作成完了。";
                    }

                    // INSERT デモ
//                    Console.Write("テーブルに新しい行を挿入するには、任意のキーを押して続行します...");
//                    Console.ReadKey(true);
                    txtConsole.Text = "テーブルに新しい行を挿入";
                    sb.Clear();
                    sb.Append("INSERT Employees (Name, Location) ");
                    sb.Append("VALUES (@name, @location);");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 60000; // コマンドがタイムアウトする場合は秒数を変更(ms) デフォルトは 30秒
                        command.Parameters.AddWithValue("@name", "Jake");
                        command.Parameters.AddWithValue("@location", "United States");
                        int rowsAffected = command.ExecuteNonQuery();
//                        Console.WriteLine(rowsAffected + " 行 挿入されました。");
                        txtConsole.Text = rowsAffected.ToString() + " 行 挿入されました。";
                    }

                    // UPDATE デモ
                    String userToUpdate = "Nikita";
                    Console.Write("ユーザー名 '" + userToUpdate + "' の 'Location' を更新中です。任意のキーを押して処理を続行します...");
                    Console.ReadKey(true);
                    txtConsole.Text = "ユーザー名 '" + userToUpdate + "' の 'Location' を更新中です。";
                    sb.Clear();
                    sb.Append("UPDATE Employees SET Location = N'United States' WHERE Name = @name");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 60000; // コマンドがタイムアウトする場合は秒数を変更(ms) デフォルトは 30秒
                        command.Parameters.AddWithValue("@name", userToUpdate);
                        int rowsAffected = command.ExecuteNonQuery();
//                        Console.WriteLine(rowsAffected + " 行 更新されました。");
                        txtConsole.Text = rowsAffected.ToString() + " 行 更新されました。";
                    }

                    // DELETE デモ
                    String userToDelete = "Jared";
//                    Console.Write("ユーザー名 '" + userToDelete + "' を削除中です。任意のキーを押して処理を続行します...");
//                    Console.ReadKey(true);
                    txtConsole.Text = "ユーザー名 '" + userToDelete + "' を削除中です。";
                    sb.Clear();
                    sb.Append("DELETE FROM Employees WHERE Name = @name;");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 60000; // コマンドがタイムアウトする場合は秒数を変更(ms) デフォルトは 30秒
                        command.Parameters.AddWithValue("@name", userToDelete);
                        int rowsAffected = command.ExecuteNonQuery();
                        //                        Console.WriteLine(rowsAffected + " 行 削除されました。");
                        txtConsole.Text = rowsAffected.ToString() + " 行 削除されました。";
                    }

                    // READ デモ
//                    Console.WriteLine("テーブルからデータを読み取り中です。任意のキーを押して処理を続行します。...");
//                    Console.ReadKey(true);
                    txtConsole.Text = "テーブルからデータを読み取り中です。";
                    sql = "SELECT Id, Name, Location FROM Employees;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //                                Console.WriteLine("{0} {1} {2}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                                txtConsole.Text = reader.GetInt32(0).ToString() + reader.GetString(1).ToString() + reader.GetString(2);
                            }
                        }
                    }
                }
            }
//            catch (SqlException e)
//            {
//                Console.WriteLine(e.ToString());
//                txtConsole.Text = e.ToString();
//            }
            catch (SqlException error)
            {
                //                Console.WriteLine(e.ToString());
                txtConsole.Text = error.ToString();
            }

            //            Console.WriteLine("全て完了しました。任意のキーを押して終了します...");
            //            Console.ReadKey(true);
            txtConsole.Text = "全て完了しました。";

        }
    }
}
