/*--------------------------------------------
 * 
 *   docmaker.net Ver.2
 *
 *   Producer: Hiroyuki Kawaguchi (Hiro KAWAGUCHI Labo.) 
 *   Author:Kenji Takanashi (HL Will Co.,Ltd.)
 *   Since:2013/10/1
 *   
 *--------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using ExcelLibrary.BinaryDrawingFormat;
using ExcelLibrary.BinaryFileFormat;
using ExcelLibrary.CompoundDocumentFormat;
using ExcelLibrary.SpreadSheet;

namespace net.docmaker.Classes {
    //コンバートするファイルの種類
    internal enum XmlType : int {
        Index,
        TaskList,
        Master,
        Task,
        Dpr
    }

    //エクセルファイルから、xmlファイルにコンバートする為のクラス
    internal class XMLConverter {
        private XmlType xml_type;
        private string e_fname;
        private string xml_fname;
        private string dir;
        private Workbook book;
        private DataSet data;

        internal XMLConverter(XmlType type, string excel_file) {
            xml_type = type;
            e_fname = excel_file;
            dir =(excel_file==null || excel_file=="")? "":Directory.GetParent(e_fname).FullName+"\\";
            switch (type) {
                case XmlType.Index:
                    xml_fname = "index.xml";
                    break;
                case XmlType.TaskList:
                    xml_fname = "task.xml";
                    break;
                case XmlType.Master:
                    xml_fname = "master.xml";
                    break;
                case XmlType.Dpr:
                    xml_fname = Path.GetFileNameWithoutExtension(e_fname) + ".dprx";
                    break;
            }
            data = new DataSet();
            if (e_fname == null || e_fname == "") return;
            data.DataSetName = "dockmaker.net";
            try {
                book = Workbook.Open(e_fname);
            } catch (Exception ex) {
                throw ex;
            }
        }

        //コンバート処理
        internal void Convert() {
            if (e_fname == null || e_fname == "") return;
            switch (xml_type) {
                case XmlType.Index:
                    ConvertIndex();
                    break;
                case XmlType.TaskList:
                    ConvertTask();
                    break;
                case XmlType.Master:
                    ConvertMaster();
                    break;
            }
        
        }

        //コンバート処理、結果をDataSetに渡す
        internal void Convert(out DataSet ds) {
            switch (xml_type) {
                case XmlType.Task:
                    ds = ConvertTaskDetail();
                    break;
                case XmlType.Dpr:
                    ds=ConvertDpr();
                    break;
                default:
                    ds = new DataSet();
                    break;
            }
        }

        internal DataSet CreateNewDataSet() {
            if (xml_type == XmlType.Index) {
                data = new DataSet();
                DataTable dt = data.Tables.Add("ProjectData");
                string[] pd_cols = new string[8] { "項番", "案件名", "フォルダ名", "担当者名", "作成日", "期限", "次回更新日", "状態" };
                for (int i = 0; i < 8; i++) {
                    dt.Columns.Add(new DataColumn(pd_cols[i], Type.GetType("System.String")));
                }
                dt = data.Tables.Add("担当者");
                dt.Columns.Add(new DataColumn("Name", Type.GetType("System.String")));
                dt = data.Tables.Add("状態");
                dt.Columns.Add(new DataColumn("Status", Type.GetType("System.String")));
                dt = data.Tables.Add("タスク種別");
                dt.Columns.Add(new DataColumn("TaskType", Type.GetType("System.String")));
                string[] of_cols = new string[5] {"Name","Zip","Address","TEL","FAX" };
                for (int i = 0; i < 5; i++) {
                    dt.Columns.Add(new DataColumn(of_cols[i], Type.GetType("System.String"))); 
                }
            }
            return data;
        }

        //index.xmlへのコンバート
        private void ConvertIndex() {
            Worksheet sheet1 = book.Worksheets[0];
            int idx = 0;
            DataTable table1 = data.Tables.Add(sheet1.Name);
            DataRow dr;
            while (sheet1.Cells[0, idx].Value!= null) {
                table1.Columns.Add(new DataColumn(sheet1.Cells[0,idx].Value.ToString(), Type.GetType("System.String")));
                idx++;
            }

            idx = 1;
            object val;
            float float_v = 0f;
            string str_val;
            DateTime dt;
            while (sheet1.Cells[idx, 0].Value!=null) {
                dr = table1.NewRow();
                for (int i = 0; i < table1.Columns.Count; i++) {
                    str_val="";
                    val= sheet1.Cells[idx, i].Value;
                    if (val == null)
                        str_val = "";
                    else if ((i==4 || i==5 || i==6) && float.TryParse(val.ToString(),out float_v )){
                        dt = DateTime.FromOADate((double)float_v);
                        str_val = dt.Year.ToString() + "/" + dt.Month.ToString() + "/" + dt.Day.ToString();
                        if (float_v - (int)float_v != 0.0f)
                            str_val += " " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString();
                    }else
                        str_val=val.ToString();
                    dr[i] = str_val;    
                }
                table1.Rows.Add(dr);
                idx++;
            }

            Worksheet sheet2 = book.Worksheets[1];
            idx = 1;
            DataTable table2 = data.Tables.Add(sheet2.Cells[0,0].Value.ToString());
            table2.Columns.Add(new DataColumn("Name"));
            while (sheet2.Cells[idx, 0].Value!=null) {
                dr = table2.NewRow();
                dr[0] = sheet2.Cells[idx, 0].Value.ToString();
                table2.Rows.Add(dr);
                idx++;
            }

            table2 = data.Tables.Add(sheet2.Cells[0,2].Value.ToString());
            idx = 1;
            table2.Columns.Add(new DataColumn("Status"));
            while (sheet2.Cells[idx, 2].Value!=null) {
                dr = table2.NewRow();
                dr[0] = sheet2.Cells[idx, 2].Value.ToString();
                table2.Rows.Add(dr);
                idx++;
            }

            table2 = data.Tables.Add(sheet2.Cells[0,4].Value.ToString());
            idx = 1;
            table2.Columns.Add(new DataColumn("TaskType"));
            while (sheet2.Cells[idx, 4].Value!=null) {
                dr = table2.NewRow();
                dr[0] = sheet2.Cells[idx, 4].Value.ToString();
                table2.Rows.Add(dr);
                idx++;
            }

            table2 = data.Tables.Add(sheet2.Cells[0,6].Value.ToString());
            table2.Columns.Add(new DataColumn("Name", Type.GetType("System.String")));
            table2.Columns.Add(new DataColumn("Zip", Type.GetType("System.String")));
            table2.Columns.Add(new DataColumn("Address", Type.GetType("System.String")));
            table2.Columns.Add(new DataColumn("TEL", Type.GetType("System.String")));
            table2.Columns.Add(new DataColumn("FAX", Type.GetType("System.String")));
            dr = table2.NewRow();
            for (int i = 0; i < 5; i++) {
                dr[i] = sheet2.Cells[i+1, 6].Value == null ? "" : sheet2.Cells[i+1, 6].Value.ToString();
            }
            table2.Rows.Add(dr);

            StreamWriter sw = new StreamWriter(dir + xml_fname, false);
            data.WriteXml(sw, XmlWriteMode.WriteSchema);
            sw.Close();
        }

        //task.xmlへのコンバート
        private void ConvertTask() {
            data = ConvertToDataSet();
            StreamWriter sw = new StreamWriter(dir + xml_fname, false);
            data.WriteXml(sw, XmlWriteMode.WriteSchema);
            sw.Close();
        }

        private DataSet ConvertTaskDetail() {
            return ConvertToDataSet();
        }

        private DataSet ConvertToDataSet() {
            Worksheet sheet1 = book.Worksheets[0];
            int idx = 0;
            DataSet ds = new DataSet();
            ds.DataSetName = "docmaker.net";
            DataTable table1 = ds.Tables.Add("task");
            DataRow dr;
            string[] types = { 
                                 "System.Int32", 
                                 "System.String", 
                                 "System.String", 
                                 "System.String", 
                                 "System.String", 
                                 "System.String", 
                                 "System.String", 
                                 "System.String", 
                                 "System.Int32", 
                                 "System.Int32", 
                                 "System.Int32", 
                                 "System.Int32", 
                                 "System.Int32", 
                                 "System.Int32",
                                 "System.Int32",
                                 "System.Int32",
                                 "System.Int32",
                                 "System.Int32"
                             };
            while (sheet1.Cells[0, idx].Value != null) {
                table1.Columns.Add(new DataColumn(sheet1.Cells[0, idx].Value.ToString(), Type.GetType(types[idx])));
                idx++;
            }

            idx = 1;
            object val;
            float float_v = 0f;
            string str_val;
            int int_val;
            DateTime dt;
            while (sheet1.Cells[idx, 0].Value != null) {
                dr = table1.NewRow();
                for (int i = 0; i < table1.Columns.Count; i++) {
                    str_val = "";
                    int_val = 0;
                    val = sheet1.Cells[idx, i].Value;
                    if (val == null) {
                        str_val = "";
                        int_val = 0;
                    } else if ((i == 5 || i == 6) && float.TryParse(val.ToString(), out float_v)) {
                        dt = DateTime.FromOADate((double)float_v);
                        str_val = dt.Year.ToString() + "/" + dt.Month.ToString() + "/" + dt.Day.ToString();
                        if (float_v - (int)float_v != 0.0f)
                            str_val += " " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString();
                    } else {
                        str_val = val.ToString();
                        if (!int.TryParse(val.ToString(), out int_val)) int_val = 0;
                    }
                    dr[i] = types[i].IndexOf("String") > 0 ? (object)str_val : (object)int_val;
                }
                table1.Rows.Add(dr);
                idx++;
            }

            Worksheet sheet2 = book.Worksheets[1];
            idx = 0;
            DataTable table2 = ds.Tables.Add("Company");
            while (sheet2.Cells[idx, 0].Value != null) {
                table2.Columns.Add(new DataColumn(sheet2.Cells[idx, 0].Value.ToString(), Type.GetType("System.String")));
                idx++;
            }
            dr = table2.NewRow();
            for (int i = 0; i < table2.Columns.Count; i++) {
                dr[i] = sheet2.Cells[i, 1].Value == null ? "" : sheet2.Cells[i, 1].Value.ToString();
            }
            table2.Rows.Add(dr);
            return ds;
        }

        //master.xmlへのコンバート
        private void ConvertMaster() {
            Worksheet sheet1 = book.Worksheets[0];
            int idx = 0;
            DataTable table1 = data.Tables.Add(sheet1.Name);
            DataRow dr;
            while (sheet1.Cells[0, idx].Value != null) {
                if (idx!=2)
                    table1.Columns.Add(new DataColumn(sheet1.Cells[0, idx].Value.ToString(), Type.GetType("System.String")));
                else
                    table1.Columns.Add(new DataColumn(sheet1.Cells[0, idx].Value.ToString(), Type.GetType("System.Int32")));
                idx++;
            }

            idx = 1;
            object val;
            int int_val=0;
            while (sheet1.Cells[idx, 0].Value != null) {
                dr = table1.NewRow();
                for (int i = 0; i < table1.Columns.Count; i++) {
                    val = sheet1.Cells[idx, i].Value;
                    if (i == 2){
                        if (int.TryParse(val.ToString(), out int_val))
                            dr[i]=int_val;
                        else 
                            dr[i]=0;
                    } else
                        dr[i] =val==null? "":val.ToString();
                }
                table1.Rows.Add(dr);
                idx++;
            }

            Worksheet sheet2 = book.Worksheets[1];
            idx = 0;
            DataTable table2 = data.Tables.Add(sheet2.Name);
            while (sheet2.Cells[0, idx].Value != null) {
                if (idx==0)
                    table2.Columns.Add(new DataColumn(sheet2.Cells[0, idx].Value.ToString(), Type.GetType("System.Int32")));
                else
                    table2.Columns.Add(new DataColumn(sheet2.Cells[0, idx].Value.ToString(), Type.GetType("System.String")));
                idx++;
            }

            idx = 1;            
            while (sheet2.Cells[idx, 0].Value != null) {
                dr = table2.NewRow();
                for (int i = 0; i < table2.Columns.Count; i++) {
                    val = sheet2.Cells[idx, i].Value;
                    if (i == 0) {
                        if (int.TryParse(val.ToString(), out int_val))
                            dr[i] = int_val;
                        else
                            dr[i] = 0;
                    } else
                        dr[i] = val == null ? "" : val.ToString();
                }
                table2.Rows.Add(dr);
                idx++;
            }

            int sheet_no = 2;
            while (book.Worksheets.Count > sheet_no) {
                Worksheet sheet3 = book.Worksheets[sheet_no];
                idx = 0;
                DataTable table3 = data.Tables.Add(sheet3.Name);
                while (sheet3.Cells[0, idx].Value != null) {
                    table3.Columns.Add(new DataColumn(sheet3.Cells[0, idx].Value.ToString(), Type.GetType("System.String")));
                    idx++;
                }
                idx = 1;
                while (sheet3.Cells[idx, 1].Value != null) {
                    dr = table3.NewRow();
                    for (int i = 0; i < table3.Columns.Count; i++) {
                        val = sheet3.Cells[idx, i].Value;
                        dr[i] = val == null ? "" : val.ToString();
                    }
                    table3.Rows.Add(dr);
                    idx++;
                }
                sheet_no++;
            }
            StreamWriter sw = new StreamWriter(dir + xml_fname, false);
            data.WriteXml(sw, XmlWriteMode.WriteSchema);
            sw.Close();
        
        }

        //dprxファイルへのコンバート
        private DataSet ConvertDpr() {
            Worksheet sheet = book.Worksheets[0];
            int idx = 0;
            DataTable table = data.Tables.Add(sheet.Name);
            DataRow dr;
            while (sheet.Cells[0, idx].Value != null) {
                table.Columns.Add(new DataColumn(sheet.Cells[0, idx].Value.ToString(), Type.GetType("System.String")));
                idx++;
            }

            idx = 1;
            object val;
            float float_v = 0f;
            string str_val;
            DateTime dt;
            while (sheet.Cells[idx, 0].Value != null) {
                dr = table.NewRow();
                for (int i = 0; i < table.Columns.Count; i++) {
                    str_val = "";
                    val = sheet.Cells[idx, i].Value;
                    if (val == null)
                        str_val = "";
                    else if ((i == 4 || i == 5 || i == 6) && float.TryParse(val.ToString(), out float_v)) {
                        dt = DateTime.FromOADate((double)float_v);
                        str_val = dt.Year.ToString() + "/" + dt.Month.ToString() + "/" + dt.Day.ToString();
                        if (float_v - (int)float_v != 0.0f)
                            str_val += " " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString();
                    } else
                        str_val = val.ToString();
                    dr[i] = str_val;
                }
                table.Rows.Add(dr);
                idx++;
            }

            StreamWriter sw = new StreamWriter(dir + xml_fname, false);
            data.WriteXml(sw, XmlWriteMode.WriteSchema);
            sw.Close();
            return data;

        }
    }
}
